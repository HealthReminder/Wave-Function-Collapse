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
        public int[][] sides;
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
        public string GetSides()
        {
            string log = "";
            if (sides == null)
                return ("NULL SIDES");
            else
            {
                for (int i = 0; i < sides.Length; i++)
                {
                    for (int o = 0; o < sides[i].Length; o++)
                    {
                        log += sides[i][o];
                    }
                    log += "\n";

                }

            }
            return log;
        }
        public string GetNeighbors(List<Pattern> pattern_list)
        {
            string log = "";
            if (possible_neighbors == null)
                return ("NO POSSIBLE NEIGHBORS");
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    log += "For side: " + i + "\n";
                    int[][] l = values;
                    int s = values.Length;

                    for (int o = 0; o < possible_neighbors[i].Count; o++)
                    {
                        for (int y = 0; y < s; y++)
                        {
                            for (int x = 0; x < s; x++)
                            {
                                log += pattern_list[possible_neighbors[i][o]].values[x][y];
                            }
                            log += "\n";

                        }
                        log += "\n";

                    }
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
            pattern_list = FillPatterns(pattern_list);
            return (pattern_array, pattern_list);
        }

        static List<Pattern> FillPatterns(List<Pattern> pattern_list)
        {
            //This function assume that the unique patterns were already cached in the pattern list
            int pattern_size = pattern_list[0].values.Length;
            int c = pattern_list.Count;
            if (pattern_size <= 0)
                Debug.LogError("Cannot fill patterns with less than one size.");
            else if (pattern_list == null)
                Debug.LogError("Cannot fill patterns with null pattern array.");
            else if (pattern_list.Count <= 0)
                Debug.LogError("Cannot fill patterns with empty pattern array.");

            //Assign the sides of the pattern to compared later
            for (int i = 0; i < c; i++)
                pattern_list[i].sides = GetPatternSides(pattern_list[i]);
            //Debug.Log("Got pattern sides for pattern: " + patterns[i].DebugGetValues() + " with the resulting of: " + patterns[i].DebugGetSides());

            //Get the neighbors of the patterns
            for (int i = 0; i < c; i++)
                pattern_list[i].possible_neighbors = GetNeighbors(pattern_list[i], pattern_list);

           // for (int i = 0; i < c; i++)
               //Debug.Log("Logging pattern: " + pattern_list[i].GetValues() + " with sides: " + pattern_list[i].GetSides() + " of neighbors: " + pattern_list[i].GetNeighbors());

            return (pattern_list);
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
        static List<int>[] GetNeighbors(Pattern pat, List<Pattern> patterns)
        {
            List<int>[] cache = new List<int>[4];
            int c = patterns.Count;
            //Each side of the pattern will be compared to the respective side of the pattern list
            for (int side_index = 0; side_index < 4; side_index++)
            {
                cache[side_index] = new List<int>();
                for (int o = 0; o < c; o++)
                {
                    if (CompareSides(pat, patterns[o], side_index))
                    {
                        cache[side_index].Add(o);
                    }
                }
            }
            return (cache);
        }
        static bool CompareSides(Pattern pat, Pattern comparingTo, int side)
        {
            if (pat.sides == null || comparingTo.sides == null)
                Debug.LogError("Input cannot compare null sides of patterns.");

            //int comparing_index = side+2;
            //if (comparing_index > side)
            //    comparing_index -= side+1;
            int comparing_index = 0;
            if (side == 0)
                comparing_index = 2;
            else if (side == 1)
                comparing_index = 3;
            else if (side == 2)
                comparing_index = 0;
            else if (side == 3)
                comparing_index = 1;

            bool result = true;
            int[] s = pat.sides[side];
            int[] c = ArrayInvert(comparingTo.sides[comparing_index]);
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] != c[i])
                    result = false;
            }
            //Debug.Log(result + " result for neighbor \n"+comparingTo.GetValues()+ " of \n" + pat.GetValues() + " for side: \n"+side);
            return result;
        }
        static int[][] GetPatternSides(Pattern pat)
        {
            //Tested
            //This variable will store the values of the cells of each of the four sides starting by the top side
            int[][] result = new int[4][];
            int size = pat.values.Length;

            List<int> cache = new List<int>();

            //Get up side
            for (int i = 0; i < size; i++)
                cache.Add(pat.values[i][0]);
            result[0] = cache.ToArray();

            //Get right side
            cache.Clear();
            for (int i = 0; i < size; i++)
                cache.Add(pat.values[size - 1][i]);
            result[1] = cache.ToArray();

            //Get down side
            cache.Clear();
            for (int i = 0; i < size; i++)
                cache.Add(pat.values[i][size - 1]);
            result[2] = ArrayInvert(cache.ToArray());

            //Get left side
            cache.Clear();
            for (int i = 0; i < size; i++)
                cache.Add(pat.values[0][i]);

            result[3] = ArrayInvert(cache.ToArray());

            return result;
        }
        static int[] ArrayInvert(int[] original)
        {
            //Tested
            List<int> result = new List<int>();
            for (int i = original.Length - 1; i >= 0; i--)
            {
                result.Add(original[i]);
            }
            return result.ToArray();
        }
       
    }
}
