using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeWars.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            IMaze maze = new PerfectMaze(10,10);

            PrintMaze(maze);
        }

        private static void PrintMaze(IMaze maze)
        {
            for (var i = 0; i < maze.Cells.GetLength(0); i++)
            {
                var tops = string.Empty;
                var sides = string.Empty;
                var bottoms = string.Empty;

                //tops
                for (var j = 0; j < maze.Cells.GetLength(1); j++)
                {
                    var cell = maze.Cells[i, j];

                    if (cell.Top)
                    {
                        tops += " --- ";
                    }

                    var c = string.Empty;

                    c += cell.Left ? "|" : " ";

                    c += "   ";

                    c += cell.Right ? "|" : " ";


                    sides += c;

                    if (cell.Below)
                    {
                        bottoms += " --- ";
                    }
                }

                System.Console.WriteLine(tops);
                System.Console.WriteLine(sides);
                System.Console.WriteLine(bottoms);
            }
        }
    }
}
