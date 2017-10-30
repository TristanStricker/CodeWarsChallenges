using System;
using NUnit.Framework;

namespace PokerHand.Tests
{
    public class PokerHandTests
    {
        [TestCase("Highest straight flush wins", Result.Loss, "2H 3H 4H 5H 6H", "KS AS TS QS JS")]
        [TestCase("Straight flush wins of 4 of a kind", Result.Win, "2H 3H 4H 5H 6H", "AS AD AC AH JD")]
        [TestCase("Highest 4 of a kind wins", Result.Win, "AS AH 2H AD AC", "JS JD JC JH 3D")]
        [TestCase("4 Of a kind wins of full house", Result.Loss, "2S AH 2H AS AC", "JS JD JC JH AD")]
        [TestCase("Full house wins of flush", Result.Win, "2S AH 2H AS AC", "2H 3H 5H 6H 7H")]
        [TestCase("Highest flush wins", Result.Win, "AS 3S 4S 8S 2S", "2H 3H 5H 6H 7H")]
        [TestCase("Flush wins of straight", Result.Win, "2H 3H 5H 6H 7H", "2S 3H 4H 5S 6C")]
        [TestCase("Equal straight is tie", Result.Tie, "2S 3H 4H 5S 6C", "3D 4C 5H 6H 2S")]
        [TestCase("Straight wins of three of a kind", Result.Win, "2S 3H 4H 5S 6C", "AH AC 5H 6H AS")]
        [TestCase("3 Of a kind wins of two pair", Result.Loss, "2S 2H 4H 5S 4C", "AH AC 5H 6H AS")]
        [TestCase("2 Pair wins of pair", Result.Win, "2S 2H 4H 5S 4C", "AH AC 5H 6H 7S")]
        [TestCase("Highest pair wins", Result.Loss, "6S AD 7H 4S AS", "AH AC 5H 6H 7S")]
        [TestCase("Pair wins of nothing", Result.Loss, "2S AH 4H 5S KC", "AH AC 5H 6H 7S")]
        [TestCase("Highest card loses", Result.Loss, "2S 3H 6H 7S 9C", "7H 3C TH 6H 9S")]
        [TestCase("Highest card wins", Result.Win, "4S 5H 6H TS AC", "3S 5H 6H TS AC")]
        [TestCase("Equal cards is tie", Result.Tie, "2S AH 4H 5S 6C", "AD 4C 5H 6H 2C")]
        [TestCase("Highest pair wins", Result.Win, "8C 4S KH JS 4D", "KC 4H KS 2H 8D")]
        [TestCase("Random Test", Result.Loss, "JH 8S TH AH QH", "TS KS 5S 9S AC")]
        public void PokerHandTest(string description, Result expected, string hand, string opponentHand)
        {
            Assert.AreEqual(expected, new PokerHand(hand).CompareWith(new PokerHand(opponentHand)), description);
        }

        [TestCase("Only five cards allowed in a hand but 6 cards dealt", "2H 3H 4H 5H 6H 6H")]
        [TestCase("Card 1 does not meet format RS, whare (R)ank, (S)uit", "2KK 3H 4H 5H 6H")]
        [TestCase("Unknown rank 'L' for card 1", "LH 3H 4H 5H 6H")]
        [TestCase("Unknown suit 'B' for card 1", "2B 3H 4H 5H 6H")]
        public void PokerHand_Contructed_With_Invalid_Arguments_Throws(
            string exceptionMessage, 
            string hand)
        {
            Action custructor = () => new PokerHand(hand);

            Assert.That(() => custructor(),
                 Throws.TypeOf<ArgumentException>()
                     .With.Message.EqualTo(exceptionMessage));
        }

    }
}
