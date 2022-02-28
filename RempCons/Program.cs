using System;
using System.Collections.Generic;

namespace RempCons
{
    class Program
    {
        static void displayTab(int[,] tabPixel, int lenght, int height)
        {
            int i, j;
            string line = "+-"; //For a nice display

            for (i = 0; i < lenght * 2; i++)
            {
                line += "-";
            }
            line += "+";

            Console.WriteLine(line);

            for (i = 0; i < lenght; i++)
            {
                Console.Write("| ");
                for (j = 0; j < height; j++)
                {
                    Console.Write($"{tabPixel[i, j]} ");
                }
                Console.WriteLine("|");
            }

            Console.WriteLine(line);
        }

        static void fill(int[,] tabPixel, int width, int height, int pixelLine, int pixelColumn, int targetColor, int finalColor, int sensibility)
        {
            Coordinate xy; 
            Stack<Coordinate> stack = new Stack<Coordinate>();

            int west, east, line, i; 
            int targetMin = targetColor - sensibility; 
            int targetMax = targetColor + sensibility;

            stack.Push(new Coordinate(pixelColumn, pixelLine));

            while (stack.Count > 0)
            {
                xy = stack.Pop();

                //Initialise west and east borders for the zone to recolor
                west = xy.X;
                east = xy.X;
                line = xy.Y;

                //West frontier calculation
                while ((west > 0) &&
                        (tabPixel[line, west - 1] >= targetMin) &&
                        (tabPixel[line,  west - 1] <= targetMax)){
                    west--;
                }

                //East frontier calculation
                while ((east < width-1) &&
                        (tabPixel[line, east + 1] >= targetMin) &&
                        (tabPixel[line, east + 1] <= targetMax))
                {
                    east++;
                }

                for (i = west; i <= east; i++)
                {
                    tabPixel[line, i] = finalColor; //colorisation

                    //Pixel at north
                    if ((line != 0) &&
                        (tabPixel[line - 1, i] != finalColor) && //needed to avoid snowball effect when sensibily > 0
                        (tabPixel[line - 1, i] >= targetMin) &&
                        (tabPixel[line - 1, i] <= targetMax))
                    {
                        stack.Push(new Coordinate(i, line - 1));
                    }
                    //Pixel at south
                    if ((line != height-1) &&
                        (tabPixel[line + 1, i] != finalColor) &&
                        (tabPixel[line + 1, i] >= targetMin) &&
                        (tabPixel[line + 1, i] <= targetMax))
                    {
                        stack.Push(new Coordinate(i, line + 1));
                    }
                }
            }
        }

        static void Main(string[] args)
        {
            int width = 5;
            int height = 5;
            int[,] tabPixel = new int[,] { { 0, 0, 1, 2, 1 }, 
                                           { 1, 5, 0, 1, 1 },
                                           { 1, 1, 1, 1, 1 },
                                           { 3, 0, 1, 6, 6 },
                                           { 0, 1, 1, 0, 1 } };

            Console.WriteLine("Image initiale :");
            displayTab(tabPixel, width, height);

            fill(tabPixel, width, height, 2, 2, 1, 1, 2);

            Console.WriteLine("Image finale :");
            displayTab(tabPixel, width, height);
        }
    }
}
