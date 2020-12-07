using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace WaveFunctionCollapse
{
    public static class InputReader 
    {
    
        //This function reads the input dataset
        public static int[][] GetConstraintArray(Tile[] dataset)
        {
            //This function reads the dataset array and outputs an organized map of the tileset grid that can be iterated through
            int squared_size = dataset.Length;
            float sqr = Mathf.Sqrt(squared_size);
            if (!Mathf.Approximately(sqr, (int)sqr))
            {
                Debug.LogError("Input dataset has invalid length. Grid must be a square matrix.");
                return null;
            }
            int size = (int)sqr;

            //Cache output and reset it
            int[][] output = SetupOutput(size);

            //Order received input by position
            List<Tile> unordered = new List<Tile>();
            for (int i = 0; i < squared_size; i++)
                unordered.Add(dataset[i]);

            //Sort by distance
            List<Vector3> positions = new List<Vector3>();
            for (int i = 0; i < squared_size; i++)
                positions.Add(dataset[i].transform.position);
            positions.Sort(CompareVector3);

            //Populate tiles again
            List<Tile> ordered = new List<Tile>();
            for (int i = 0; i < positions.Count; i++)
            {
                int matching_index = 0;
                for (int o = 0; o < unordered.Count; o++)
                {
                    if (unordered[o].transform.position == positions[i])
                        matching_index = o;
                }
                ordered.Add(unordered[matching_index]);
            }
            if (ordered.Count < squared_size)
                Debug.LogError("Missed tile in ordering routine.");

            //Label tilest
            for (int i = 0; i < squared_size; i++)
            {

                ordered[i].gameObject.name = ((int)ordered[i].transform.position.x).ToString() + "/" + ((int)ordered[i].transform.position.z).ToString();
                //Move dataset for debug purposes
                //ordered[i].transform.position += new Vector3(0, i*0.1f, 0);
            }

            //Populate output array
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    //Find the index in the tileset
                    int r = -1;
                    //Goes by each x and add the row using the side -1 to keep it in boundaries
                    r = ordered[x + y * (size - 1) + y].type;
                    output[x][y] = r;
                    if (r == -1)
                        Debug.LogWarning("Did not find object in tileset. Defaulting to -1.");
                }
            }


            Debug.Log(ReadArrayInt(output));
            return (output);
        }
        static int CompareVector3(Vector3 v1, Vector3 v2)
        {
            // Comparing two vectors this way is fine
            // Unity has overloaded the == operator
            // So as to avoid floating point imprecision
            if (v1 == v2) return 0;

            if (Mathf.Approximately(v1.z, v2.z))
            {
                if (Mathf.Approximately(v1.x, v2.x))
                    return v1.y < v2.y ? -1 : 1;
                else
                    return v1.x < v2.x ? -1 : 1;
            }
            return v1.z > v2.z ? -1 : 1;
        }
        static int[][] SetupOutput(int size)
        {
            //Cache output and reset it
            int[][] r = new int[size][];
            for (int i = 0; i < size; i++)
                r[i] = new int[size];

            for (int y = 0; y < size; y++)
                for (int x = 0; x < size; x++)
                    r[x][y] = -1;

            return r;
        }
        static string ReadArrayInt(int[][] arr)
        {
            string log = "";
            if (arr == null)
                log += ("NULL INT ARRAY");
            else
            {
                for (int y = 0; y < arr.Length; y++)
                {
                    for (int x = 0; x < arr[y].Length; x++)
                    {
                        log += arr[x][y];
                    }
                    log += "\n";

                }
                log += "\n";
            }

            log += "\n";
            return log;
        }
    }
}
