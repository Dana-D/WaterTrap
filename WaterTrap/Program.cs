using System;
using System.Collections.Generic;

namespace WaterTrap
{
    class Program
    {
        static void Main(string[] args)
        {
            string input = @"5
4
7 4 0 9
3
6 9 9
6
10 0 0 10 0 10
5
6 0 7 1 4
7
2 0 4 0 4 0 2";

            /**
             * Expected output:
             * 10
             * 0
             * 30
             * 9
             * 8
             */

            input = input.Replace("\r","");
            Test[] tests = parseInput(input);
            foreach(Test T in tests)
            {
                T.print();
                int volume = T.getVolume(0);
                Console.WriteLine("Volume: " + volume);
            }
        }

        static Test[] parseInput(string input)
        {
            string[] inputs = input.Split("\n");
            int num_of_tests = Int32.Parse(inputs[0]);
            Test[] tests = new Test[num_of_tests];
            int position = 0;
            Test t = null;

            for (int i = 1; i < inputs.Length; i++)
            {
                if(i%2 == 1)//odd
                {
                    t = new Test(Int32.Parse(inputs[i]));
                }
                else
                {
                    string[] values = inputs[i].Split(" ");
                    for (int j = 0; j < values.Length; j++)
                    {
                        t.trap[j] = Int32.Parse(values[j]);
                    }
                    tests[position] = t;
                    position++;
                }
            }

            return tests;
        }
    }

    class Test
    {
        public int[] trap;

        public Test(int size)
        {
            trap = new int[size];
        }

        public int getVolume(int start)
        {
            int volume = 0;
            int[] walls = getNextWalls(start);

            int length = Math.Abs(walls[0] - walls[2]) - 1; //the -1 is to prevent counting a wall
            int shorterWall;
            if(walls[1] < walls[3])
            {
                shorterWall = walls[1];
            }
            else
            {
                shorterWall = walls[3];
            }
            volume = shorterWall * length; //volume between walls

            //Remove submerged blocks from volume count
            for (int i = walls[0] + 1; i < walls[2]; i++)
            {
                volume -= trap[i];
            }

            if (walls[2] != trap.Length - 1)
            {
                volume += getVolume(walls[2]);
            }

            if(volume < 0)
            {
                volume = 0;
            }

            return volume;
        }

        /**
         * Position 0: Position of wall
         * Position 1: Height of wall
         * Position 2: Position of second wall
         * Position 3: Height of second wall
         */
        public int[] getNextWalls(int start)
        {
            int[] walls = { 0, 0, 0, 0 };
            for (int i = start; i < trap.Length; i++)
            {
                if(trap[i] != 0 && walls[1] == 0)
                {
                    walls[0] = i;
                    walls[1] = trap[i];
                }
                else if(trap[i] >= walls[1] || i == trap.Length - 1)
                {
                    walls[2] = i;
                    walls[3] = trap[i];
                    return walls;
                }
            }
            return walls;
        }

        public void print()
        {
            Console.Write(trap[0]);
            for(int i = 1; i < trap.Length; i++)
            {
                Console.Write(" " + trap[i]);
            }
            Console.WriteLine();
        }
    }
}
