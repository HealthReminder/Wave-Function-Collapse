using System;
using System.Collections.Generic;
using UnityEngine;

namespace WaveFunctionCollapse
{
    public static class WFCInputOutput
    {
        public static int[][] GetOffsetArray(int[][] input, int padding)
        {
            //It seems that doubling instead of padding generates less errors
            if (input == null)
                Debug.LogError("Cannot get offset array for null input array.");
            else if (padding < 0)
                Debug.LogError("Padding should not be a negative number.");
            else if (input[0] == null)
                Debug.LogError("Cannot get offset array for invalid input array");

            int extra_padding = 0;
            //This is an edge case
            if (padding > input.Length)
            {
                //Debug.LogWarning("Ideally offset padding should not be bigger than dataset size. It means that the pattern size is actually bigger than the dataset.");
                extra_padding = padding - input.Length;
            }

            //Get input and output side sizes
            int input_y, input_x, output_y, output_x;
            input_y = input.Length;
            input_x = input[0].Length;
            output_y = input_y + padding  + extra_padding;
            output_x = input_x + padding  + extra_padding;
            int[][] result = new int[output_y][];

            //Fill each cell of the output array
            for (int y = 0; y < output_y; y++)
            {
                result[y] = new int[output_y];
                for (int x = 0; x < output_x; x++)
                {
                    //For cells which fit in the input array just copy the number
                    if (x < input_x && y < input_y)
                        result[y][x] = input[y][x];
                    //For cells outside the bounds get the right coordinates
                    else
                    {
                        int new_y = y;
                        while (new_y >= input_y)
                            new_y -= input_y;
                        int new_x = x;
                        while (new_x >= input_x)
                            new_x -= input_x;
                        //Log the new x and y positions that are now inside the input array Debug.Log(new_x + " " + new_y);
                        // Debug.Log(input_array[new_x][new_y]);
                        result[y][x] = input[new_y][new_x];
                    }
                }
            }
            return result;
        }
        public static int[][] GetOutputArray(List<int>[][] coll, List<Pattern> patterns, int pattern_size)
        {
            //Get useful variables
            int result_size = coll.Length;
            //Setup result array by multiplying the pattern array by the pattern size
            int[][] result;
            result = new int[result_size][];
            for (int i = 0; i < result.Length; i++)
                result[i] = new int[result_size];

            for (int i = 0; i < result.Length; i++)
                for (int o = 0; o < result[i].Length; o++)
                    result[i][o] = -1;


            //Go on every pattern that will be in resulting array (therefore skipping by pattern size)
            for (int y = 0; y < result_size; y += 1)
            {
                for (int x = 0; x < result_size; x += 1)
                {
                    //X and Y represents the "pivot" of the current pattern being transcribed
                    //Divide it by pattern size to cached the respective pattern
                    List<int> possible_solutions = coll[y][x];

                    //for each cell of the pattern fill it with the appropriate value
                    for (int b = 0; b < pattern_size; b++)
                    {
                        for (int a = 0; a < pattern_size; a++)
                        {
                            //Check if cell of pattern is within bounds of resulting array
                            if (x + a >= 0 && x + a < result_size && y + b >= 0 && y + b < result_size)
                            {
                                if (possible_solutions == null)
                                    //This cell has not been observed yet
                                    result[y + b][x + a] = -2;
                                else if (possible_solutions.Count != 1)
                                    //This cell of this pattern has been observed but has more than one solution
                                    result[y + b][x + a] = -1;
                                else
                                {
                                    //This cell has been collapsed already
                                    Pattern current_pattern = patterns[possible_solutions[0]];

                                    //For SOME REASON? The rotation of the pattern change when creating the output
                                    //Therefore we have to rotate. This rotation was calculated manually

                                    result[y + b][x + a] = current_pattern.values[b][a];
                                    //int off_a = a + off_x;
                                    int off_a = a;
                                    while (off_a >= pattern_size)
                                        off_a -= pattern_size;

                                    //int off_b = b + off_y;
                                    int off_b = b;
                                    while (off_b >= pattern_size)
                                        off_b -= pattern_size;

                                    result[y + b][x + a] = current_pattern.values[off_b][off_a];

                                    //This codes lets you figure out the location of the pattern (topleft top right bot right bot left)
                                    //if (y / pattern_size % 2 == 0 && x / pattern_size % 2 == 0)
                                    //result[x + a][y + b] = 0;

                                    //result[x + a][y + b] = y/pattern_size;
                                }

                            }
                        }
                    }
                }
            }
            return result;
            /* //Get useful variables
            int result_size = coll.Length * pattern_size;
            //Setup result array by multiplying the pattern array by the pattern size
            int[][] result;
            result = new int[result_size][];
            for (int i = 0; i < result.Length; i++)
                result[i] = new int[result_size];

            for (int i = 0; i < result.Length; i++)
                for (int o = 0; o < result[i].Length; o++)
                    result[i][o] = -1;


            //Go on every pattern that will be in resulting array (therefore skipping by pattern size)
            for (int y = 0; y < result_size; y += pattern_size)
            {
                for (int x = 0; x < result_size; x += pattern_size)
                {
                    //X and Y represents the "pivot" of the current pattern being transcribed
                    //Divide it by pattern size to cached the respective pattern
                    List<int> possible_solutions = coll[y / pattern_size][x / pattern_size];

                    //for each cell of the pattern fill it with the appropriate value
                    for (int b = 0; b < pattern_size; b++)
                    {
                        for (int a = 0; a < pattern_size; a++)
                        {
                            //Check if cell of pattern is within bounds of resulting array
                            if (x + a >= 0 && x + a < result_size && y + b >= 0 && y + b < result_size)
                            {
                                if (possible_solutions == null)
                                    //This cell has not been observed yet
                                    result[y + b][x + a] = -2;
                                else if (possible_solutions.Count != 1)
                                    //This cell of this pattern has been observed but has more than one solution
                                    result[y + b][x + a] = -1;
                                else
                                {
                                    //This cell has been collapsed already
                                    Pattern current_pattern = patterns[possible_solutions[0]];

                                    //For SOME REASON? The rotation of the pattern change when creating the output
                                    //Therefore we have to rotate. This rotation was calculated manually

                                    result[y + b][x + a] = current_pattern.values[b][a];
                                    //int off_a = a + off_x;
                                    int off_a = a;
                                    while (off_a >= pattern_size)
                                        off_a -= pattern_size;

                                    //int off_b = b + off_y;
                                    int off_b = b;
                                    while (off_b >= pattern_size)
                                        off_b -= pattern_size;

                                    result[y + b][x + a] = current_pattern.values[off_b][off_a];

                                    //This codes lets you figure out the location of the pattern (topleft top right bot right bot left)
                                    //if (y / pattern_size % 2 == 0 && x / pattern_size % 2 == 0)
                                    //result[x + a][y + b] = 0;

                                    //result[x + a][y + b] = y/pattern_size;
                                }

                            }
                        }
                    }
                }
            }
            return result;*/
        }
    }
}
