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
            var suitGroups = hand
                .GroupBy(card => card.Suit);

            var rankGroups = hand
                .GroupBy(card => card.Rank);

            // Pinched from StackOverflow!
            var isHandInMonotonicDecreasingRankOrder = 
                hand
                .Zip(hand.Skip(1), (curr, next) => curr.Rank == next.Rank + 1)
                .All(x => x);

            var firstCard = hand.First();

            if (firstCard.Rank == Rank.Ace &&
                isHandInMonotonicDecreasingRankOrder && 
                suitGroups.Count() == 1)
            {
                return HandClassification.RoyalFlush;
            }

            if (isHandInMonotonicDecreasingRankOrder &&
                suitGroups.Count() == 1)
            {
                return HandClassification.StraightFlush;
            }

            if (rankGroups.Any(group => group.Count() == 4))
                return HandClassification.FourOfAKind;

            if (rankGroups.Count() == 2)
            {
                return HandClassification.FullHouse;
            }

            if (suitGroups.Count() == 1 &&
                isHandInMonotonicDecreasingRankOrder == false)
            { 
                return HandClassification.Flush;
            }

            if (suitGroups.Count() != 1 
                && isHandInMonotonicDecreasingRankOrder == true)
            {
                return HandClassification.Straight;
            }

            if (rankGroups.Any(group => group.Count() == 3))
            {
                return HandClassification.ThreeOfAKind;
            }

            if (rankGroups.Count() == 3 &&
                rankGroups.Any(group => group.Count() == 2))
            {
                return HandClassification.TwoPair;
            }

            if (rankGroups.Count() == 4)
            {
                return HandClassification.Pair;
            }

            return HandClassification.HighHand;
        }

        private Card CreateCard(string card, int index)
        {
            if(card.Length != 2)
            {
                throw new ArgumentException($"Card {index + 1} does not meet format RS, whare (R)ank, (S)uit");
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
            int compared;

            Debug.WriteLine($"This classification {this.Classification}");
            Debug.WriteLine($"Other classification {other.Classification}");

            if (this.Classification == other.Classification)
            {
                var thisSum = this.Hand.Sum(card => (int)card.Rank);
                var otherSum = other.Hand.Sum(card => (int)card.Rank);

                compared = thisSum.CompareTo(otherSum);
            }
            else
            {
                compared = this.Classification.CompareTo(other.Classification);
            }

            if (compared < 0)
                return Result.Loss;
            if (compared == 0)
            {
                return Result.Tie;
            }

            return Result.Win;
        }
    }
}
