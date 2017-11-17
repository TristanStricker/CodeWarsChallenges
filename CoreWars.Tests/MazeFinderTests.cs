using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeWars;
using NUnit.Framework;

namespace CoreWars.Tests
{
    public class MazeFinderTests
    {
        [Test]
        public void StaticMaze1()
        {
            int[] path = { 36, 37, 38, 31, 24, 25, 26 };
            CollectionAssert.AreEqual(path, MazeFinder.FindPath(EllerAlgorithm.GenerateMaze(3, 2), 7, 36, 26));
        }

        [Test]
        public void StaticMaze2()
        {
            int[] path = { 78, 89, 100, 101, 102, 103, 104, 105, 106, 107, 108 };
            CollectionAssert.AreEqual(path, MazeFinder.FindPath(EllerAlgorithm.GenerateMaze(5, 0), 11, 78, 108));
        }

        [Test]
        public void StaticMaze3()
        {
            int[] path = { 46, 61, 76, 91, 106, 121, 136, 151, 166, 167, 168, 183, 198, 199, 200, 201, 202, 203, 204, 205, 206, 191, 176, 161, 146, 147, 148, 133, 118, 103, 88, 73, 58, 43, 28 };
            CollectionAssert.AreEqual(path, MazeFinder.FindPath(EllerAlgorithm.GenerateMaze(7, 1), 15, 46, 28));
        }
    }
}
