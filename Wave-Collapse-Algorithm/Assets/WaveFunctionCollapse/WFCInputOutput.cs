using System;
using System.Collections.Generic;
using UnityEngine;

namespace WaveFunctionCollapse
{
    public static class WFCInputOutput
    {
        public static int[][] GetOffsetArray(int[][] input, int padding)
        {
            //Debug
            if (input == null)
                Debug.LogError("Cannot get offset array for null input array.");
            else if (padding < 0)
                Debug.LogError("Padding should not be a negative number.");
            else if (input[0] == null)
                Debug.LogError("Cannot get offset array for invalid input array");

            //Get input and output side sizes
            int input_x, input_y, output_x, output_y;
            input_x = input.Length;
            input_y = input[0].Length;
            output_x = input_x + padding * 1;
            output_y = input_y + padding * 1;
            int[][] result = new int[output_x][];

            //Fill each cell of the output array
            for (int x = 0; x < output_x; x++)
            {
                result[x] = new int[output_x];
                for (int y = 0; y < output_y; y++)
                {
                    //For cells which fit in the input array just copy the number
                    if (y < input_y && x < input_x)
                        result[x][y] = input[x][y];
                    //For cells outside the bounds get the right coordinates
                    else
                    {
                        int new_x = x;
                        while (new_x >= input_x)
                            new_x -= input_x;
                        int new_y = y;
                        while (new_y >= input_y)
                            new_y -= input_y;
                        //Log the new x and y positions that are now inside the input array Debug.Log(new_x + " " + new_y);
                        // Debug.Log(input_array[new_x][new_y]);
                        result[x][y] = input[new_x][new_y];
                    }
                }
            }
            return result;
        }
        public static int[][] GetOutputArray(List<int>[][] coll, List<Pattern> patterns, int pattern_size)
        {
            //Get useful variables
            int result_size = coll.Length * pattern_size;
            //Setup result array
            int[][] result;
            result = new int[result_size][];
            for (int i = 0; i < result.Length; i++)
                result[i] = new int[result_size];

            for (int i = 0; i < result.Length; i++)
            {
                for (int o = 0; o < result[i].Length; o++)
                {
                    result[i][o] = -1;
                }
            }
            //Go on every pattern that will be in resulting array (therefore skipping by pattern size)
            for (int y = 0; y < result_size; y += pattern_size)
            {
                for (int x = 0; x < result_size; x += pattern_size)
                {
                    //X and Y represents the "pivot" of the current pattern being transcribed
                    //Divide it by pattern size to cached the respective pattern
                    List<int> possible_solutions = coll[x / pattern_size][y / pattern_size];

                    //for each cell of the pattern fill it with the appropriate value
                    for (int b = 0; b < pattern_size; b++)
                    {
                        for (int a = 0; a < pattern_size; a++)
                        {
                            //Check if cell of pattern is within bounds of resulting array
                            if (x + a >= 0 && x + a < result_size && y + b >= 0 && y + b < result_size)
                            {
                                //This cell of this pattern has not been observed
                                if (possible_solutions == null)
                                    result[x+a][y+b] = -2;
                                //This cell of this pattern has been observed but it is overlapping
                                else if (possible_solutions.Count != 1)
                                    result[x + a][y + b] = -1;
                                //This cell of this pattern has been collapsed already
                                else
                                {
                                    Pattern current_pattern = patterns[possible_solutions[0]];
                                    //Assign value using coordinates
                                    result[x + a][y + b] = current_pattern.values[a][b];

                                    //Save computations by finding offset right away
                                    //Since patterns are stored clockwise
                                    //But the output array is stored cartesi anally


                                    int offset_x = a + 1;
                                    if (offset_x >= pattern_size)
                                        offset_x = 0;
                                    int offset_y = b + 1;
                                    if (offset_y >= pattern_size)
                                        offset_y = 0;


                                    if (y % 2 == 0)
                                    {
                                        if (x % 2 == 0)
                                            result[x][y] = current_pattern.values[offset_x][b];
                                        else
                                            result[x][y] = current_pattern.values[a][b];
                                    }
                                    else
                                    {
                                        if (x % 2 != 0)
                                            result[x][y] = current_pattern.values[a][offset_y];
                                        else
                                            result[x][y] = current_pattern.values[offset_x][offset_y];

                                    }

                                }
                            }
                        }
                    }
                }
            }
            return result;
        }
    }
}
