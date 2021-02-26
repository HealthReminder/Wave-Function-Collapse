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
        //This is a list of possible neighbors
        //In the future this might be a list of an array of int
        //The first index of the list will be the pattern index
        //The second index of the list will be the pattern frequency
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
                        log += values[x][y];
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
                for (int i = 0; i < 4; i++)
                {
                    log += "For side: " + i + "\n";
                    int[][] l = values;
                    int pattern_size = values.Length;

                    for (int o = 0; o < possible_neighbors[i].Count; o++)
                    {
                        for (int y = 0; y < pattern_size; y++)
                        {
                            for (int x = 0; x < pattern_size; x++)
                                log += pattern_list[possible_neighbors[i][o]].values[y][x];
                            log += "\n";
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
        /*
        public static (int[][] pattern_array, List<Pattern> pattern_list) GetPatternArray(int[][] offset, int pattern_size)
        {
            //The pattern array starts off with the size and values of the offset array
            int size_y = offset.Length;
            int size_x = offset[0].Length;
            int[][] pattern_array = new int[size_y][];
            for (int y = 0; y < size_y; y++)
                pattern_array[y] = new int[size_y];

            for (int y = 0; y < size_y; y++)
                for (int x = 0; x < size_x; x++)
                    pattern_array[y][x] = offset[y][x];

            //This array will contain the pattern cell values
            int[][] current_pattern = new int[pattern_size][];
            for (int i = 0; i < pattern_size; i++)
                current_pattern[i] = new int[pattern_size];


            for (int y = 0; y < size_x; y++)
            {
                for (int x = 0; x < size_y; x++)
                {
                    bool is_out_of_bounds = false;
                    //Get the next pattern if not out of bounds
                    if (x + pattern_size - 1 < size_y && y + pattern_size - 1 < size_x)
                    {
                        for (int b = 0; b < pattern_size; b++)
                        {
                            for (int a = 0; a < pattern_size; a++)
                            {
                                //For some reason b must be in place of a
                                //Otherwise pattern identification invert axis
                                current_pattern[a][b] = pattern_array[x + a][y + b];
                            }
                        }
                    }
                    else
                    {
                        is_out_of_bounds = true;
                    }
                    // && x%pattern_size == 0 && y%pattern_size == 0
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
            GetNeighbors(pattern_array, pattern_size, pattern_list);
            return (pattern_array, pattern_list);
        }
        */

        public static List<Pattern> GetUniquePatterns(int[][] offset, int pattern_size)
        {
            //Get sizes
            int size_x = offset[0].Length;
            int size_y = offset.Length;

            //This array is used to store the unique pattern indexes
            List<Pattern> pattern_list = new List<Pattern>();

            //This array will hold the values of each pattern analyzed
            int[][] current_pattern = new int[pattern_size][];
            for (int i = 0; i < pattern_size; i++)
                current_pattern[i] = new int[pattern_size];

            for (int y = 0; y < size_y; y++)
            {
                for (int x = 0; x < size_x; x++)
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
                                current_pattern[b][a] = offset[y + b][x + a];
                            }
                        }
                    }
                    else
                        is_out_of_bounds = true;
                    
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

                            //Fill pattern cells
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
                    }
                }
            }
            //for (int i = 0; i < pattern_list.Count; i++)
                //for (int o = 0; o < pattern_list[i].values.Length; o++)
                    //for (int k = 0; k < pattern_list[i].values[o].Length; k++)
                        //Debug.Log(pattern_list[i].values[o][k]);

            return (pattern_list);
        }

        public static int[][] GetPatternArray(int[][] offset, int pattern_size, List<Pattern> unique_patterns)
        {
            //Get sizes and setup result array
            int size_y = offset.Length;
            int size_x = offset[0].Length;

            int[][] pattern_array = new int[size_x][];
            for (int y = 0; y < size_y; y++)
                pattern_array[y] = new int[size_x];

            for (int y = 0; y < size_y; y++)
                for (int x = 0; x < size_x; x++)
                    pattern_array[y][x] = offset[y][x];

            //This array will contain the indexes of the patterns each cell belongs to
            int[][] current_pattern = new int[pattern_size][];
            for (int i = 0; i < pattern_size; i++)
                current_pattern[i] = new int[pattern_size];

            for (int y = 0; y < size_y; y++)
            {
                for (int x = 0; x < size_x; x++)
                {
                    bool is_out_of_bounds = false;
                    //Get the next pattern if not out of bounds
                    if (x + pattern_size - 1 < size_x && y + pattern_size - 1 < size_y)
                    {
                        for (int b = 0; b < pattern_size; b++)
                        {
                            for (int a = 0; a < pattern_size; a++)
                            {
                                current_pattern[b][a] = pattern_array[y + b][x + a];
                            }
                        }
                    }
                    else
                        is_out_of_bounds = true;
                    
                    if (!is_out_of_bounds)
                    {
                        //Compare it to other patterns in the unique pattern list
                        for (int i = 0; i < unique_patterns.Count; i++)
                            if (CompareArrays(unique_patterns[i].values, current_pattern))
                            {
                                pattern_array[y][x] = i;
                            }                       
                    }
                    else
                    {
                        pattern_array[y][x] = -1;
                    }
                }
            }
            //Debug.Log("Generated offset array with " + unique_patterns.Count + " unique patterns.");
            //GetNeighbors(pattern_array, pattern_size, unique_patterns);
            return (pattern_array);
        }

        /*public static (int[][] pattern_array, List<Pattern> pattern_list) GetPatternInformation(int[][] offset, int pattern_size)
        {
            //Get sizes and setup result array
            int size_x = offset.Length;
            int size_y = offset[0].Length;
            int[][] pattern_array = new int[size_x][];
            for (int x = 0; x < size_x; x++)
                pattern_array[x] = new int[size_x];

            for (int y = 0; y < size_y; y++)
                for (int x = 0; x < size_x; x++)
                    pattern_array[x][y] = offset[x][y];

            //This array is used to store the unique pattern indexes
            List<Pattern> pattern_list = new List<Pattern>();

            //This array will contain the indexes of the patterns each cell belongs to
            int[][] current_pattern = new int[pattern_size][];
            for (int i = 0; i < pattern_size; i++)
                current_pattern[i] = new int[pattern_size];

            for (int y = 0; y < size_y; y++)
            {
                for (int x = 0; x < size_x; x++)
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
                                current_pattern[a][b] = pattern_array[x + a][y + b];
                            }
                        }
                    }
                    else
                    {
                        is_out_of_bounds = true;
                    }
                    // && x%pattern_size == 0 && y%pattern_size == 0
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
            GetNeighbors(pattern_array, pattern_size, pattern_list);
            return (pattern_array, pattern_list);
        }*/
        public static List<Pattern> GetNeighbors(int[][] pat_array, int pat_size, List<Pattern> pat_list)
        {
            //This function assume that the unique patterns were already cached in the pattern list
            int pattern_size = pat_list[0].values.Length;
            int c = pat_list.Count;
            if (pattern_size <= 0)
                Debug.LogError("Cannot fill patterns with less than one size.");
            else if (pat_list == null)
                Debug.LogError("Cannot fill patterns with null pattern array.");
            else if (pat_list.Count <= 0)
                Debug.LogError("Cannot fill patterns with empty pattern array.");

            //Instantiate possible neighbors array in each pattern found in the offset array
            for (int i = 0; i < pat_list.Count; i++)
            {
                pat_list[i].possible_neighbors = new List<int>[4];
                for (int o = 0; o < 4; o++)
                    pat_list[i].possible_neighbors[o] = new List<int>();
            }
            //Go through the pattern array and get neighbors
            int l = pat_array.Length;

            for (int y = 0; y < l; y++)
            {
                for (int x = 0; x < l; x++)
                {
                    //Get the pattern from list
                    int index = pat_array[y][x];

                    if (index != -1)
                    {
                        Pattern current_pattern = pat_list[pat_array[y][x]];

                        //Cache all exisiting neighbors plus the new ones found
                        List<int>[] all_possible_neighbors = new List<int>[4];
                        for (int i = 0; i < 4; i++)
                            all_possible_neighbors[i] = current_pattern.possible_neighbors[i];


                        //Check if neighbor is valid
                        if (y - 1 >= 0)
                            if (pat_array[y - 1][x] != -1)
                            {
                                //If this is not a neighbor already, add it
                                if (!current_pattern.possible_neighbors[0].Contains(pat_array[y - 1][x]))
                                    all_possible_neighbors[0].Add(pat_array[y - 1][x]);
                                //Debug.Log("For pattern: "+pat_array[y][x]+" found top neighbor of index: "+pat_array[y - 1][x]);
                            }

                        if (x + 1 < l)
                            if (pat_array[y][x + 1] != -1)
                            {
                                if (!current_pattern.possible_neighbors[1].Contains(pat_array[y][x + 1]))
                                    all_possible_neighbors[1].Add(pat_array[y][x + 1]);
                                //Debug.Log("For pattern: " + pat_array[y][x] + " found right neighbor of index: " + pat_array[y][x+1]);

                            }
                        if (y + 1 < l)
                            if (pat_array[y + 1][x] != -1)
                            {
                                if (!current_pattern.possible_neighbors[2].Contains(pat_array[y + 1][x]))
                                    all_possible_neighbors[2].Add(pat_array[y + 1][x]);
                                //Debug.Log("For pattern: " + pat_array[y][x] + " found bottom neighbor of index: " + pat_array[y + 1][x]);

                            }
                        if (x - 1 >= 0)
                            if (pat_array[y][x - 1] != -1)
                            {
                                if (!current_pattern.possible_neighbors[3].Contains(pat_array[y][x - 1]))
                                    all_possible_neighbors[3].Add(pat_array[y][x - 1]);
                                //Debug.Log("For pattern: " + pat_array[y][x] + " found left neighbor of index: " + pat_array[y][x - 1]);

                            }

                        //Cache all neighbors, skip duplicates
                        List<int>[] unique_possible_neighbors = new List<int>[4];
                        for (int i = 0; i < 4; i++)
                            unique_possible_neighbors[i] = new List<int>();

                        //For all 4 sides of the pattern, go over all the neighbors and cache the unique ones
                        //Debug.Log("Future reference: You may add neighbor side frequency here.");
                        for (int i = 0; i < 4; i++)
                        {
                            for (int j = 0; j < all_possible_neighbors[i].Count; j++)
                            {
                                bool is_contained = false;
                                for (int k = 0; k < unique_possible_neighbors[i].Count; k++)
                                {
                                    if (all_possible_neighbors[i][j] == unique_possible_neighbors[i][k])
                                        is_contained = true;
                                }

                                if (!is_contained)
                                    unique_possible_neighbors[i].Add(all_possible_neighbors[i][j]);
                            }
                        }
                        current_pattern.possible_neighbors = all_possible_neighbors;
                        Debug.Log("For pattern "+ pat_array[y][x] + "\n"+ ReadArrayListInt(all_possible_neighbors));

                    }
                }
            }
            return pat_list;
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

        #region Debug
        static string ReadArrayListInt(List<int>[] arraylistint)
        {
            string result = "";
            if (arraylistint == null)
                return ("Neighbors array is null");
            else
                for (int i = 0; i < arraylistint.Length; i++)
                {
                    if (arraylistint[i] == null)
                        return ("Neighbors list for side " + i + " is null");
                    else
                    {
                        result += "For side " + i + ": ";
                        for (int o = 0; o < arraylistint[i].Count; o++)
                        {
                            result += arraylistint[i][o] + " ";
                        }
                        result += "\n";

                    }
                }
            return result;
        }
        #endregion

    }

}
