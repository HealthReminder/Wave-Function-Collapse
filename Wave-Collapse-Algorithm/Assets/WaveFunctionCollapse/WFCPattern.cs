using System.Collections.Generic;
using UnityEngine;
namespace WaveFunctionCollapse
{
    public static class WFCPattern
    {
        public static (int[][] pattern_array, List<int[][]> pattern_list, List<int> frequency_list) GetPatternInformation(int[][] offset, int pattern_size)
        {
            //Get sizes and setup result array
            int size_x = offset.Length;
            int size_y = offset[0].Length;
            int[][] pattern_array = new int[size_x][];

            for (int x = 0; x < size_x; x++)
                pattern_array[x] = new int[size_x];
            for (int x = 0; x < size_x; x++)
                for (int y = 0; y < size_y; y++)
                    pattern_array[x][y] = offset[x][y];

            //This array is used to store the unique pattern indexes
            List<int[][]> pattern_list = new List<int[][]>();
            List<int> frequency_list = new List<int>();

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
                                //For some reason b must be in place of a
                                //Otherwise pattern identification invert axis
                                current_pattern[b][a] = pattern_array[x + a][y + b];
                            }
                        }
                    }
                    else
                    {
                        is_out_of_bounds = true;
                    }

                    if (!is_out_of_bounds)
                    {
                        //Compare it to other patterns in the unique pattern list
                        int unique_id = pattern_list.Count;
                        for (int i = 0; i < pattern_list.Count; i++)
                            if (CompareArrays(pattern_list[i], current_pattern))
                            {
                                //Debug.Log(current_pattern[0][0] + "|" + current_pattern[1][0]);
                                frequency_list[i]++;
                                unique_id = i;
                                i = pattern_list.Count;
                            }

                        //If this condition is true it means that the current pattern
                        //Is unique and must add it to the unique pattern list
                        if (unique_id == pattern_list.Count)
                        {
                            //Instantiate new pattern
                            int[][] new_pattern = new int[pattern_size][];
                            for (int i = 0; i < pattern_size; i++)
                                new_pattern[i] = new int[pattern_size];
                            for (int o = 0; o < pattern_size; o++)
                                for (int i = 0; i < pattern_size; i++)
                                    new_pattern[i][o] = current_pattern[i][o];
                            //Create an instance in pattern list and set frequency
                            pattern_list.Add(new_pattern);
                            frequency_list.Add(1);
                        }
                        //Finally modify the pattern array to contain the indexes of the patterns
                        pattern_array[x][y] = unique_id;
                    }
                    else
                    {
                        pattern_array[x][y] = -1;
                    }
                }
            }

            if (pattern_list.Count != frequency_list.Count)
                Debug.LogError("Pattern list and frequency list counts does not match.");

            //Debug.Log("Generated offset array with " + unique_patterns.Count + " unique patterns.");
            return (pattern_array, pattern_list, frequency_list);
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
    }
}
