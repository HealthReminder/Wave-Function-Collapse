using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WaveFunctionCollapse
{
    public static class ReadInput
    {
        #region Pattern
        public static int[][] GetPatternArray(int[][] offset, int pattern_size)
        {

            //Get sizes and setup result array
            int size_x = offset.Length;
            int size_y = offset[0].Length;
            int[][] result = new int[size_x][];

            for (int x = 0; x < size_x; x++)
                result[x] = new int[size_x];
            for (int x = 0; x < size_x; x++)
                for (int y = 0; y < size_y; y++)
                    result[x][y] = offset[x][y];

            //This array is used to store the unique pattern indexes
            List<int[][]>  unique_patterns = new List<int[][]>();

            //This array will contain the indexes of the patterns each cell belongs to
            int[][] current_pattern = new int[pattern_size][];
            for (int i = 0; i < pattern_size; i++)
                current_pattern[i] = new int[pattern_size];

            for (int x = 0; x < size_x; x++)
            {
                for (int y = 0; y < size_y; y++)
                {
                    bool is_out_of_bounds = false;
                    //Get the next pattern if not out of bounds
                    if (x + pattern_size - 1 < size_x && y + pattern_size - 1 < size_y)
                    {
                        for (int b = 0; b < pattern_size; b++)
                        {
                            for (int a = 0; a < pattern_size; a++)
                            {
                                current_pattern[a][b] = result[x + a][y + b];
                            }
                        }
                    } else
                    {
                        is_out_of_bounds = true;
                    }

                    if (!is_out_of_bounds)
                    {
                        //Compare it to other patterns in the unique pattern list
                        int unique_id = unique_patterns.Count;
                        for (int i = 0; i < unique_patterns.Count; i++)
                            if (CompareArrays(unique_patterns[i], current_pattern))
                            {
                                unique_id = i;
                                i = unique_patterns.Count;
                            }

                        //If this condition is true it means that the current pattern
                        //Is unique and must add it to the unique pattern list
                        if (unique_id == unique_patterns.Count)
                        {
                            int[][] new_pattern = new int[pattern_size][];
                            for (int i = 0; i < pattern_size; i++)
                                new_pattern[i] = new int[pattern_size];

                            for (int o = 0; o < pattern_size; o++)
                                for (int i = 0; i < pattern_size; i++)
                                    new_pattern[i][o] = current_pattern[i][o];
                            unique_patterns.Add(new_pattern);
                        }

                        //Finally modify the pattern array to contain the indexes of the patterns
                        result[x][y] = unique_id;
                    } else
                    {
                        result[x][y] = -1;
                    }
                }
            }

            //Debug.Log("Generated offset array with " + unique_patterns.Count + " unique patterns.");
            return (result);
        }
        static bool CompareArrays(int[][] a, int[][] b) 
        {
            if (a == null || b == null)
                Debug.LogError("Cannot compare null arrays.");
            else if (a.Length == 0 || b.Length == 0)
                Debug.Log("Cannot compare empty arrays.");

            for (int y = 0; y < a.Length; y++)
                for (int x = 0; x < a[0].Length; x++)
                    if (a[x][y] != b[x][y])
                        return false;
            return true;
        }
        #endregion
        #region Offset
        public static int[][] GetOffsetArray (int[][] input, int padding)
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
        #endregion
    }
}
