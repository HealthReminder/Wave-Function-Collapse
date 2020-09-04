using System.Collections.Generic;
using UnityEngine;
namespace WaveFunctionCollapse
{
    public class Pattern
    {
        //The indexes of the cells of the modified grid 
        //That compose the pattern with set size
        public int[][] values;
        //Amount of times this pattern appears in the input
        public int frequency;
        //Sides from up and clockwise (length 4)
        public float[][] sides;
        //Neighbors from north and clockwise (length 4)
        public List<int>[] possible_neighbors;

        public string GetValues()
        {
            string log = "";
            if (values == null)
                return ("NULL PATTERN VALUES");
            else
            {
                int s = values.Length;
                for (int y = 0; y < s; y++)
                {
                    for (int x = 0; x < s; x++)
                    {
                        log += values[x][y];
                    }
                    log += "\n";

                }
            }
            return log;
        }
        public string GetFrequency()
        {
            return (frequency.ToString());
        }
    }
    public static class WFCPattern
    {

        public static (int[][] pattern_array, List<Pattern> pattern_list) GetPatternInformation(int[][] offset, int pattern_size)
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
            List<Pattern> pattern_list = new List<Pattern>();

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
                            if (CompareArrays(pattern_list[i].values, current_pattern))
                            {
                                //Debug.Log(current_pattern[0][0] + "|" + current_pattern[1][0]);
                                pattern_list[i].frequency++;
                                unique_id = i;
                                i = pattern_list.Count;
                            }

                        //If this condition is true it means that the current pattern
                        //Is unique and must add it to the unique pattern list
                        if (unique_id == pattern_list.Count)
                        {
                            //Instantiate new pattern
                            Pattern new_pattern = new Pattern();
                            new_pattern.values = new int[pattern_size][];
                            for (int i = 0; i < pattern_size; i++)
                                new_pattern.values[i] = new int[pattern_size];

                            for (int o = 0; o < pattern_size; o++)
                                for (int i = 0; i < pattern_size; i++)
                                    new_pattern.values[i][o] = current_pattern[i][o];
                            //Create an instance in pattern list and set frequency
                            new_pattern.frequency = 1;
                            pattern_list.Add(new_pattern);
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

            //Debug.Log("Generated offset array with " + unique_patterns.Count + " unique patterns.");
            return (pattern_array, pattern_list);
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
