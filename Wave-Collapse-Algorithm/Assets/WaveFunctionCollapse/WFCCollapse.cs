using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace WaveFunctionCollapse

{
    public static class WFCCollapse
    {
        //COMECA A TRANSFORMAR ESSA COROTINA NA ROTINA DO TESTER
        //AQUI SO TERA FUNCOES QUE RETORNAM TIPOS E NO ROUTINAS
        public static void CollapseCell(List<int>[][] coll, int[][] entr, Vector2 coords, List<Pattern> patterns, int collapse_into = -1)
        {
            List<int> cell = coll[(int)coords.x][(int)coords.y];
            if (cell == null)
                Debug.LogError("Cannot collapse a non-initialized cell");
            else if (cell.Count == 0)
                Debug.LogError("Cannot collapse a cell with no possible solutions");
            else if (cell.Count == 1)
                Debug.LogWarning("Should not collapse cell with already one solution");

            if (collapse_into != -1)
            {
                cell.Clear();
                cell.Add(collapse_into);
            }
            else
            {
                int c = cell.Count;
                List<int> r_indexes = new List<int>();
                int current_pattern;
                for (int i = 0; i < c; i++)
                {
                    current_pattern = cell[i];
                    for (int o = 0; o < patterns[current_pattern].frequency; o++)
                        r_indexes.Insert(Random.Range(0, r_indexes.Count), current_pattern);
                }
                int chosen_solution = r_indexes[Random.Range(0, r_indexes.Count)];
                cell.Clear();
                cell.Add(chosen_solution);
                entr[(int)coords.x][(int)coords.y] = -1;
                //Debug.Log("Collapsed cell into "+chosen_solution);

            }

            //Propagate to neihgbors
            Propagate(coll, entr, coords, patterns);
        }
        public static Vector2 CollapseHyperCell(List<int>[][] coll, int[][] entr, List<Pattern> patterns)
        {
            //This function should be called only once, ideally
            //Get a list of hyper cells
            List<Vector2> hyperstates = new List<Vector2>();
            for (int y = 0; y < entr.Length; y++)
                for (int x = 0; x < entr[0].Length; x++)
                    if(entr[x][y] == 0)
                        hyperstates.Add(new Vector2(x, y));
            if (hyperstates.Count <= 0)
                Debug.LogError("No cell of infinite solutions in list. Cannot collapse.");

            //Get a random hyper cell and collapse it.
            int index = Random.Range(0, hyperstates.Count);
            int i = (int)hyperstates[index].x; int o = (int)hyperstates[index].y;
            coll[i][o] = GetHyperstate(patterns);
            CollapseCell(coll,entr, new Vector2(i,o), patterns);

            //Debug.Log("Collapsed random cell of coordinates: " + x + "," + y + " into "+arr[x][y][0]+" from " + patterns.Count + " patterns with a resulting infinite list of length " + hyperstates.Count);
            return hyperstates[index];
        }
        public static void CollapseMostProbable(List<int>[][] coll, int[][] entr, List<Pattern> patterns)
        {
            if (coll == null)
                Debug.LogError("Cannot calculate lowest entropy of null array.");
            else if (coll[0] == null)
                Debug.LogError("Cannot calculate lowest entropy of null rows.");
            else if (coll[0][0] == null)
                Debug.LogError("Cannot calculate lowest entropy of null columns.");
            else if (coll.Length <= 0 || coll[0].Length <= 0)
                Debug.LogError("Cannot calculate lowest entropy of empty array,");
            else if (patterns == null)
                Debug.LogError("Cannot collapse most probable cell using null pattern list.");

            //Get a list of hyper cells
            List<Vector2> hyperstates = new List<Vector2>();
            for (int y = 0; y < entr.Length; y++)
                for (int x = 0; x < entr[0].Length; x++)
                    if (entr[x][y] == 0)
                        hyperstates.Add(new Vector2(x, y));

            int s = coll.Length;
            //Store the most probable cell in these
            List<int> epicenter_cell = new List<int>();
            int highest_entropy = GetHighestEntropy(entr);
            if (highest_entropy == 0)
            {
                Debug.LogWarning("Ideally this should not be called?");
                CollapseHyperCell(coll, entr, patterns);
            }
            else
            {
                List<Vector2> possible_cells = new List<Vector2>();
                for (int y = 0; y < s; y++)
                    for (int x = 0; x < s; x++)
                            if (entr[x][y] == highest_entropy)
                                possible_cells.Add(new Vector2(x, y));
                //0 will always be the cell in the middle. The others are its neighbors if any
                Vector2 r_cell = possible_cells[Random.Range(0, possible_cells.Count)];
                CollapseCell(coll, entr, r_cell, patterns);

            }
            //Debug.Log("Chosen cell " + chosen_cell + " within cells of entropy of " + lowest_entropy);
            //Debug.Log(chosen_cell);
            //Debug.Log(patterns[chosen_cell.possible_patterns[0]].DebugGetNeighbors());
            //Debug.Log(DebugCells(cells));
            //Debug.log(cells, false);
            
            //Debug.Log(DebugCells(cells));
            //Debug.Log(DebugCells(cells, false));
        }
        static void Propagate(List<int>[][] coll, int[][] entr, Vector2 coords, List<Pattern> patterns)
        {
            int cx, cy;
            cx = (int)coords.x; cy = (int)coords.y;
            List<int> cell = coll[cx][cy];
            if (cell != null)
                if (cell.Count > 1)
                    Debug.LogWarning("Propagating cell with more than one solution: " + cell.Count);

            //Find neighbors
            //Before propagation get valid neighbors
            List<Vector2> neighbors = new List<Vector2>();
            //You can only propagate cells that exist obviously
            
            if (coords.y - 1 >= 0)
                neighbors.Add(new Vector2(cx,cy - 1));
            if (coords.x + 1 < coll.Length)
                neighbors.Add(new Vector2(cx + 1, cy));
            if (coords.y + 1 < coll[0].Length)
                neighbors.Add(new Vector2(cx, cy + 1));
            if (coords.x - 1 >= 0)
                neighbors.Add(new Vector2(cx - 1, cy));

            for (int i = 0; i < neighbors.Count; i++)
                if(coll[(int)neighbors[i].x][(int)neighbors[i].y] == null)
                    coll[(int)neighbors[i].x][(int)neighbors[i].y] = GetHyperstate(patterns);

            //All the neighbors of the main cell will be restricted to its possible neighbors.
            //This is WRONG
            for (int i = 0; i < cell.Count; i++)
            {

                List<int>[] n = patterns[cell[i]].possible_neighbors;

                for (int k = 0; k < neighbors.Count; k++)
                {
                    List<int> c = coll[(int)neighbors[k].x][(int)neighbors[k].y];
                    if (c.Count != 1)
                            for (int o = 0; o < n[0].Count; o++)
                                c.Add(n[0][o]);
                    
                }
            }
            //Update entropy array
            for (int i = 0; i < neighbors.Count; i++)
            {
                int x = (int)neighbors[i].x;
                int y = (int)neighbors[i].y;
                //If this neighbor was collapsed make it 999 so it must collapse after
                if (entr[x][y] != -1) {
                    if (coll[x][y].Count == 1)
                        entr[x][y] = 9;
                    else
                        entr[x][y] = coll[x][y].Count * (GetCollapsedNeighborCount(x, y, entr) + 1);
                }
            }
            //Debug.Log("Finished propagating");
        }
        static int GetCollapsedNeighborCount(int x, int y, int[][] entr) 
        {
            int count = 0;
            if (y - 1 >= 0)
                if (entr[x][y - 1] == -1)
                    count++;
            if (x + 1 < entr.Length)
                if (entr[x + 1][y] == -1)
                    count++;
            if (y + 1 < entr[0].Length)
                if (entr[x][y + 1] == -1)
                    count++;
            if (x - 1 >= 0)
                if (entr[x - 1][y] == -1)
                    count++;

            //Debug.Log("Cell has " + count + "collapsed neighbors");
            return (count);
        }
        public static List<int> GetHyperstate(List<Pattern> patterns)
        {
            List<int> p = new List<int>();
            for (int i = 0; i < patterns.Count; i++)
                p.Add(i);
            return p;
        }

        static int GetHighestEntropy(int[][] arr)
        {
            //This function is used to get the lowest number of possible combinations in the board

            if (arr == null)
                Debug.LogError("Cannot calculate lowest entropy of null array.");
            else if (arr[0] == null)
                Debug.LogError("Cannot calculate lowest entropy of null rows.");
            else if (arr[0][0] == null)
                Debug.LogError("Cannot calculate lowest entropy of null columns.");
            else if (arr.Length <= 0 || arr[0].Length <= 0)
                Debug.LogError("Cannot calculate lowest entropy of empty array");

            int c = arr[0].Length;
            int highest_entropy = -9999;
            for (int y = 0; y < c; y++)
                for (int x = 0; x < c; x++)
                    if (arr[x][y] != -1)
                        if (arr[x][y] > highest_entropy)
                            highest_entropy = arr[x][y];

            if (highest_entropy == -9999)
                Debug.LogError("There is no collapsable cell int entropy array.");

            return highest_entropy;
        }
    }
}
