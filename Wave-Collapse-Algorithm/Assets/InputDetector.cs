using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace WaveFunctionCollapse
{
    public class InputDetector : MonoBehaviour
    {
        //Dataset size is the size of the side of the square 
        //Or grid that represents the dataset
        //This script only works on square matrix
        public Tile[] dataset;
        //These unique objects must be cached by the main routine
        //It is a list that composes the tileset
        public Tile[] tileset;

        private void Start()
        {
            GetInput(dataset);
        }

        public int[][] GetInput(Tile[] dataset)
        {
            int total = dataset.Length;
            float sqr = Mathf.Sqrt(total);
            if (!Mathf.Approximately(sqr, (int)sqr))
            {
                Debug.LogError("Input dataset has invalid length. Grid must be a square matrix.");
                return null;
            }
            int side = (int)sqr;

            //Cache output and reset it
            int[][] output = SetupOutput(side);

            //Order received input by position
            List<Tile> tiles = new List<Tile>();
            for (int i = 0; i < total; i++)
                tiles.Add(dataset[i]);

            //Sort by Y
            //for (int i = 0; i < total; i++)
            //objs.OrderBy(o => o.transform.position.y);

            //Sort by X
            List<Vector3> ordered_pos = new List<Vector3>();
            for (int i = 0; i < total; i++)
                ordered_pos.Add(dataset[i].transform.position);
            ordered_pos.Sort(CompareVector3);
            for (int i = 0; i < total; i++)
            {
                Debug.Log(ordered_pos[i]);
            }

            tiles.OrderBy(o => o.transform.position);

           
            //Label tilest
            for (int i = 0; i < total; i++)
            {

                tiles[i].gameObject.name = ((int)tiles[i].transform.position.x).ToString() + "/" + ((int)tiles[i].transform.position.z).ToString();
            }

            

            //Populate output array
            for (int x = 0; x < side; x++)
            {
                for (int y = 0; y < side; y++)
                {
                    //Find the index in the tileset
                    bool found = false;
                    int r = -1;
                    for (int i = 0; i < tileset.Length; i++)
                    {
                        if (tiles[x + y * side].type == tileset[i].type)
                        {
                            r = i;
                            found = true;
                        }
                    }
                    output[x][y] = r;
                    if (!found)
                        Debug.LogWarning("Did not find object in tileset. Defaulting to -1.");
                }
            }
            
            Debug.Log(ReadArrayInt(output));
            return (output);
        }
        public static int CompareVector3(Vector3 v1, Vector3 v2)
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
            return v1.z < v2.z ? -1 : 1;
        }
        string ReadArrayInt(int[][] arr)
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
        int[][] SetupOutput(int size)
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
    }
}
