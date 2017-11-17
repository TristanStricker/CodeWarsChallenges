using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeWars
{
    /*
     Your task is to find the path between two points inside a square maze. 
     The mazes are generated using Eller's algorithm, so there is only one possible path between any two points.
     Mazes look like this:

    #######
    # #   #
    # # ###      S - start point
    #    G#      G - goal point
    ### # #      # - walls of the maze
    #S  # #
    #######

    Implement the FindPath method.
    FindPath returns an array of traversed indices to reach the goal point from the start point.

    Available input data:

    maze - flat 1D array where True shows a passable space and False means a wall
    size - length of one side of the maze including borders (maze is square)
    startIndex - the index of the cell in the maze array to start your path from
    goalIndex - the index of the cell in the maze array to reach
    
    In the maze above S has index 36 and G has index 26 in the maze array. 
    The correct path would be through these cells:  { 36, 37, 38, 31, 24, 25, 26 }
   */

    public class Cell
    {
        public bool Left { get; set; }
        public bool Top { get; set; }
        public bool Right { get; set; }
        public bool Below { get; set; }
    }

    public interface IMaze
    {
       int Rows { get; }
       int Columns { get; }
       Cell[,] Cells { get; }
    }

    public class PerfectMaze : IMaze
    {
        public PerfectMaze(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
            Generate();
        }

        private void Generate()
        {
            Cells = new Cell[Rows,Columns];
            for(var row = 0; row < Rows; row++)  
            {
                for (var column = 0; column < Columns; column++)
                {
                    Cells[row, column] = GenerateCell(row, column);
                }
            }
        }

        private Cell GenerateCell(int row, int column)
        {
            var setLeft = false;
            var setTop = false;
            var setRight = false;
            var setBottom = false;
            
            if (column == 0)
            {
                setLeft = true;
            }

            if (row == 0)
            {
                setTop = true;
            }

            if (column == Columns - 1)
            {
                setRight = true;
            }

            if (row == Rows - 1)
            {
                setBottom = true;
            }

            return new Cell
            {
                Below = setBottom,
                Top = setTop,
                Left = setLeft,
                Right = setRight
            };
        }


        public int Rows { get; }
        public int Columns { get; }
        public Cell[,] Cells { get; private set;}
    }


    public static class MazeFinder
    {

        public static int[] FindPath(bool[] maze, int size, int startIndex, int goalIndex)
        {
            // Implement path finding here
            return null;
        }
    }

    public static class EllerAlgorithm
    {
        public static bool[] GenerateMaze(int v1, int v2)
        {
            return null;
        }
    }
}
