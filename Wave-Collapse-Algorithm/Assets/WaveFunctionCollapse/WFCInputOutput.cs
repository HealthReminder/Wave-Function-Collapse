using System;
using System.Collections.Generic;
using UnityEngine;

namespace WaveFunctionCollapse
{
    public static class WFCInputOutput
    {
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
        public static int[][] GetOutputArray(List<int>[][] coll, List<Pattern> patterns, int pattern_size)
        {
            //Get useful variables
            int coll_size = coll.Length;
            //Setup result array
            int[][] result;
            result = new int[coll_size][];
            for (int i = 0; i < result.Length; i++)
                result[i] = new int[coll_size];

            for (int i = 0; i < result.Length; i++)
            {
                for (int o = 0; o < result[i].Length; o++)
                {
                    result[i][o] = -1;
                }
            }
            //This UVMap coordinates to read the patterns
            int pattern_x = 0;
            int pattern_y = 0;
            //Go around the collapsed array
            for (int y = 0; y < coll_size; y++)
            {
                for (int x = 0; x < coll_size; x++)
                {
                    List<int> list_value = coll[x][y];
                    if (list_value == null)
                    {
                        //This pattern has not been observed
                        result[x][y] = -2;
                        //FillArray(result ,-2, patterns,new Vector2(x,y));
                    }
                    else if (list_value.Count != 1)
                    {
                        result[x][y] = -1;

                        //This pattern has been observed but it is overlapping
                        //FillArray(result, -1, patterns, new Vector2(x, y));

                    }
                    else
                    {
                        //Find the pattern
                        int pattern_index = coll[x][y][0];
                        //Assign value using coordinates
                        result[x][y] = patterns[pattern_index].values[pattern_x][pattern_y];

                        //Save computations by finding offset right away
                        //Since patterns are stored clockwise
                        //But the output array is stored cartesi anally


                        int offset_x = pattern_x + 1;
                        if (offset_x >= pattern_size)
                            offset_x = 0;
                        int offset_y = pattern_y + 1;
                        if (offset_y >= pattern_size)
                            offset_y = 0;


                        if (y % 2 == 0)
                        {
                            if (x % 2 == 0)
                                result[x][y] = patterns[pattern_index].values[offset_x][pattern_y];
                            else
                                result[x][y] = patterns[pattern_index].values[pattern_x][pattern_y]; 
                        } else
                        {
                            if (x % 2 != 0)
                                result[x][y] = patterns[pattern_index].values[pattern_x][offset_y];
                            else
                                result[x][y] = patterns[pattern_index].values[offset_x][offset_y];

                        }
                        //result[x][y] = pattern_index;

                        //This pattern has been observed and has collapsed
                        //FillArray(result, list_value[0], patterns, new Vector2(x, y));

                    }
                    //Get the coordinates in the pattern boundaries
                    pattern_x++;
                    if (pattern_x >= pattern_size)
                        pattern_x = 0;

                }
                pattern_y++;
                if (pattern_y >= pattern_size)
                    pattern_y = 0;
            }
            return result;
        }
    }
}
