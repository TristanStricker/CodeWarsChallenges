using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace PokerHand
{
    public class PokerHand
    {
        private class Card
        {
            public Card(Rank rank, Suit suit)
            {
                this.Rank = rank;
                this.Suit = suit;
            }

            public Rank Rank { get; }
            public Suit Suit { get; }
        }

        private readonly Dictionary<char, Rank> RankLookup = new Dictionary<char, Rank>
        {
            {'2', Rank.Two  },
            {'3', Rank.Three},
            {'4', Rank.Four },
            {'5', Rank.Five },
            {'6', Rank.Six  },
            {'7', Rank.Seven},
            {'8', Rank.Eight},
            {'9', Rank.Nine },
            {'T', Rank.Ten  },
            {'J', Rank.Jack },
            {'Q', Rank.Queen},
            {'K', Rank.King },
            {'A', Rank.Ace  }
        };

        Dictionary<char, Suit> SuitLookup = new Dictionary<char, Suit>
        {
            { 'S', Suit.Spades },
            { 'H', Suit.Hearts },
            { 'D', Suit.Diamonds },
            { 'C', Suit.Clubs }
        };

        private enum Rank
        {
            Two,
            Three,
            Four,
            Five,
            Six,
            Seven,
            Eight,
            Nine,
            Ten,
            Jack,
            Queen,
            King,
            Ace
        }

        private enum Suit
        {
            Spades,
            Hearts,
            Diamonds,
            Clubs
        }

        private enum HandClassification
        {
            HighHand,
            Pair,
            TwoPair,
            ThreeOfAKind,
            Straight,
            Flush,
            FullHouse,
            FourOfAKind,
            StraightFlush,
            RoyalFlush,
        }

        private List<Card> Hand { get; } = new List<Card>(5);

        private HandClassification Classification { get; }
        private List<IGrouping<Suit, Card>> SuitGroups { get; set; }
        private List<IGrouping<Rank, Card>> RankGroups { get; set; }

        public PokerHand(string dealtCards)
        {
            var cards = dealtCards.Split(null);

            if (cards.Length != 5)
            {
                throw new ArgumentException($"Only five cards allowed in a hand but {cards.Length} cards dealt");
            }

            Hand = cards
                .Select((card, index) => CreateCard(card, index))
                .OrderByDescending(card => card.Rank)
                .ToList();

            Classification = Classify(Hand);
        } 

        private HandClassification Classify(List<Card> hand)
        {
            SuitGroups = hand
                .GroupBy(card => card.Suit).ToList();

            RankGroups = hand
                .GroupBy(card => card.Rank).ToList();

            // Pinched from StackOverflow!
            var isHandInMonotonicDecreasingRankOrder = 
                hand
                .Zip(hand.Skip(1), (curr, next) => curr.Rank == next.Rank + 1)
                .All(x => x);

            var firstCard = hand.First();

            if (firstCard.Rank == Rank.Ace &&
                isHandInMonotonicDecreasingRankOrder &&
                SuitGroups.Count == 1)
            {
                return HandClassification.RoyalFlush;
            }

            if (isHandInMonotonicDecreasingRankOrder &&
                SuitGroups.Count == 1)
            {
                return HandClassification.StraightFlush;
            }

            if (RankGroups.Any(group => group.Count() == 4))
            {
                return HandClassification.FourOfAKind;
            }

            if (RankGroups.Count == 2)
            {
                return HandClassification.FullHouse;
            }

            if (SuitGroups.Count == 1 &&
                isHandInMonotonicDecreasingRankOrder == false)
            { 
                return HandClassification.Flush;
            }

            if (SuitGroups.Count != 1 
                && isHandInMonotonicDecreasingRankOrder == true)
            {
                return HandClassification.Straight;
            }

            if (RankGroups.Any(group => group.Count() == 3))
            {
                return HandClassification.ThreeOfAKind;
            }

            if (RankGroups.Count == 3 &&
                RankGroups.Any(group => group.Count() == 2))
            {
                return HandClassification.TwoPair;
            }

            if (RankGroups.Count == 4)
            {
                return HandClassification.Pair;
            }

            return HandClassification.HighHand;
        }

        private Card CreateCard(string card, int index)
        {
            if(card.Length != 2)
            {
                throw new ArgumentException($"Card {card} at position {index + 1}, does not meet format RS, whare (R)ank, (S)uit");
            }

            if (!RankLookup.TryGetValue(card[0], out Rank rank))
            {
                throw new ArgumentException($"Unknown rank '{card[0]}' for card {index + 1}");
            }

            if (!SuitLookup.TryGetValue(card[1], out Suit suit))
            {
                throw new ArgumentException($"Unknown suit '{card[1]}' for card {index + 1}");
            }

            return new Card(rank, suit);
        }

        public Result CompareWith(PokerHand other)
        {
            //int compared;
         
            if (this.Classification == other.Classification)
            {
                var skip = 0;
                var take = 0;
                
                switch (Classification)
                {
                    case HandClassification.HighHand:
                    {
                        var thisSideCards = this.Hand.Select(x => x.Rank).ToList();
                        var otherSideCards = other.Hand.Select(x => x.Rank).ToList();

                        return CompareSideCards(thisSideCards, otherSideCards);
                    }
                    case HandClassification.Pair:
                    {
                        var thisPairRank = this.RankGroups[0].Key;
                        var otherPairRank = other.RankGroups[0].Key;
                        if (thisPairRank != otherPairRank)
                        {
                            return Decide(thisPairRank.CompareTo(otherPairRank));
                        }

                        var thisSideCards = this.Hand.Skip(2).Select(x => x.Rank).ToList();
                        var otherSideCards = other.Hand.Skip(2).Select(x => x.Rank).ToList();
                        return CompareSideCards(thisSideCards, otherSideCards);
                    }
                    case HandClassification.TwoPair:
                    {
                        take = 1;
                        // First pair are same rank
                        if (this.RankGroups[0].Key == other.RankGroups[0].Key)
                        {
                            // Second pair are same rank
                            if (this.RankGroups[1].Key == other.RankGroups[1].Key)
                            {
                                skip = 4;
                                break;
                            }

                            skip = 2;
                            break;                               
                        }
                        break;
                    }
                    case HandClassification.ThreeOfAKind:
                    {
                        take = 3;
                        break;
                    }
                    case HandClassification.Straight:
                    {
                        take = 1;
                        break;
                    }
                    case HandClassification.Flush:
                    {
                        take = 5;
                        break;
                    }
                    case HandClassification.FullHouse:
                    {
                        take = 3;
                        break;
                    }
                    case HandClassification.FourOfAKind:
                    {
                        take = 4;
                        break;
                    }
                    case HandClassification.StraightFlush:
                    {
                        take = 5;
                        break;
                    }
                    case HandClassification.RoyalFlush:
                    {
                        return Result.Tie;
                    }
                }

                var thisSum = this.Hand.Skip(skip).Take(take).Sum(card => (int)card.Rank);
                var otherSum = other.Hand.Skip(skip).Take(take).Sum(card => (int)card.Rank);
                return Decide(thisSum.CompareTo(otherSum));
            }

            return Decide(this.Classification.CompareTo(other.Classification));
        }

        private static Result CompareSideCards(List<Rank> thisSideCards, List<Rank> otherSideCards)
        {
            for (int index = 0; index < thisSideCards.Count; index++)
            {
                if (thisSideCards[index] != otherSideCards[index])
                {
                    return Decide(thisSideCards[index].CompareTo(otherSideCards[index]));
                }
            }

            return Result.Tie;
        }

        private static Result Decide(int comparedRank)
        {
            if (comparedRank < 0)
            {
                return Result.Loss;
            }

            return comparedRank == 0 ? Result.Tie : Result.Win;
        }
    }
}
