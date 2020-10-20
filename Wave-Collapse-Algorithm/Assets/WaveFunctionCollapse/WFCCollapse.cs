using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace WaveFunctionCollapse

{
    public static class WFCCollapse
    {
        //COMECA A TRANSFORMAR ESSA COROTINA NA ROTINA DO TESTER
        //AQUI SO TERA FUNCOES QUE RETORNAM TIPOS E NO ROUTINAS
        public static void CollapseCell(List<int>[][] coll, int[][] entr, Vector2 coords, List<Pattern> patterns, Vector2 first_collapsed, int collapse_into = -1)
        {
            List<int> cell = coll[(int)coords.x][(int)coords.y];
            if (cell == null)
                Debug.LogError("Cannot collapse a non-initialized cell");
            else if (cell.Count == 0)
                Debug.LogError("Cannot collapse a cell with no possible solutions");
            else if (cell.Count == -1)
                Debug.LogWarning("Should not collapse cell that was already collapsed");

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
                //Debug.Log("Collapsed cell into "+chosen_solution);
            }
            entr[(int)coords.x][(int)coords.y] = -1;

            //Propagate to neihgbors
            Propagate(coll, entr, coords, patterns, first_collapsed);
        }
        public static Vector2 CollapseHyperCell(List<int>[][] coll, int[][] entr, List<Pattern> patterns, Vector2 first_collapsed)
        {
            //This function should be called only once, ideally
            //Get a list of hyper cells
            List<Vector2> hyperstates = new List<Vector2>();
            for (int y = 0; y < entr.Length; y++)
                for (int x = 0; x < entr[0].Length; x++)
                    if (entr[x][y] == 0)
                        hyperstates.Add(new Vector2(x, y));
            if (hyperstates.Count <= 0)
                Debug.LogError("No cell of infinite solutions in list. Cannot collapse.");

            //Get a random hyper cell and collapse it.
            int index = Random.Range(0, hyperstates.Count);
            int i = (int)hyperstates[index].x; int o = (int)hyperstates[index].y;
            coll[i][o] = GetHyperstate(patterns);
            CollapseCell(coll, entr, new Vector2(i, o), patterns,first_collapsed);

            //Debug.Log("Collapsed random cell of coordinates: " + x + "," + y + " into "+arr[x][y][0]+" from " + patterns.Count + " patterns with a resulting infinite list of length " + hyperstates.Count);
            return hyperstates[index];
        }
        public static void CollapseMostProbable(List<int>[][] coll, int[][] entr, List<Pattern> patterns, Vector2 first_collapsed)
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
                CollapseHyperCell(coll, entr, patterns, first_collapsed);
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
                CollapseCell(coll, entr, r_cell, patterns,first_collapsed);

            }
            //Debug.Log("Chosen cell " + chosen_cell + " within cells of entropy of " + lowest_entropy);
            //Debug.Log(chosen_cell);
            //Debug.Log(patterns[chosen_cell.possible_patterns[0]].DebugGetNeighbors());
            //Debug.Log(DebugCells(cells));
            //Debug.log(cells, false);

            //Debug.Log(DebugCells(cells));
            //Debug.Log(DebugCells(cells, false));
        }
        static void Propagate(List<int>[][] coll, int[][] entr, Vector2 coords, List<Pattern> patterns, Vector2 first_collapsed)
        {
            int cx, cy;
            cx = (int)coords.x; cy = (int)coords.y;
            List<int> epicenter = coll[cx][cy];
            if (epicenter != null)
                if (epicenter.Count > 1)
                    Debug.LogWarning("Propagating cell with more than one solution: " + epicenter.Count);
            //Debug.Log("Propagating");
            //Find neighbors
            //Before propagation get valid neighbors
            Vector2[] neighbors = new Vector2[4];
            //You can only propagate cells that exist obviously
            for (int i = 0; i < 4; i++)
                neighbors[i] = new Vector2(-1, -1);

            if (coords.y - 1 >= 0)
                neighbors[0] = (new Vector2(cx, cy - 1));
            if (coords.x + 1 < coll.Length)
                neighbors[1] = (new Vector2(cx + 1, cy));
            if (coords.y + 1 < coll[0].Length)
                neighbors[2] = (new Vector2(cx, cy + 1));
            if (coords.x - 1 >= 0)
                neighbors[3] = (new Vector2(cx - 1, cy));

            //Debug.Log(neighbors[0] + " / " + neighbors[1] + " / " + neighbors[2] + " / " + neighbors[3]);
            bool is_valid = true;
            //All the neighbors of the main cell will be restricted to its possible neighbors
            for (int side = 0; side < neighbors.Length; side++)
            {
                if (neighbors[side] != new Vector2(-1, -1))
                {

                    int entropy = entr[(int)neighbors[side].x][(int)neighbors[side].y];
                    List<int> new_possibilities = patterns[epicenter[0]].possible_neighbors[side];
                    //Check if this "neighbor" has infinite solutions
                    if (entropy == 0)
                    {
                        //Debug.Log("This neighbor should have the total possibilities of its epicenter.");
                        //If it does set its solutions to the possible neighbors from the epicenter
                        int nx = (int)neighbors[side].x;
                        int ny = (int)neighbors[side].y;
                        coll[nx][ny].Clear();
                        foreach (int p in new_possibilities)
                            coll[nx][ny].Add(p);
                    }
                    //If the "neighbor" does have solutions already and it is not collapsed
                    else if (entropy != -1)
                    {
                        //Debug.Log("This neighbor should have set solutions already.");
                        //Go through the current solutions 
                        //Record solutions that must be removed
                        List<int> original_cell = coll[(int)neighbors[side].x][(int)neighbors[side].y];
                        List<int> current_possibilities = new List<int>();
                        for (int i = 0; i < original_cell.Count; i++)
                            current_possibilities.Add(original_cell[i]);
                        //Debug.Log(current_possibilities.Count);
                        //Debug.Log("Working on cell with " + current_possibilities.Count + " solutions and entropy of "+entropy);
                        //ERROR HERE
                        int iterations = current_possibilities.Count - 1;
                        bool is_contained;
                        //Of current possibilities
                        for (int c = iterations; c >= 0; c--)
                        {
                            //Flag the ones that are no valid
                            is_contained = false;
                            foreach (int p in new_possibilities)
                                if (current_possibilities[c] == p)
                                    is_contained = true;
                            //If there is no solution leave the last solution as the only possible
                            if (!is_contained && current_possibilities.Count > 1)
                                current_possibilities.RemoveAt(c);
                            else if (!is_contained && current_possibilities.Count == 1)
                            {
                                is_valid = false;
                                //This is a conflicting result
                                //The cell must collapse from the last state of the list
                                //The cell is flagged to be collapsed immediatelly
                                current_possibilities.Clear();
                                //current_possibilities = original_cell;
                                //Or maybe it can only collapse in the most probable solution on the board
                                for (int i = 0; i < original_cell.Count; i++)
                                    current_possibilities.Add(original_cell[i]);
                                for (int i = 0; i < new_possibilities.Count; i++)
                                    current_possibilities.Add(new_possibilities[i]);
                                Debug.LogWarning("Invalid!");
                            }
                        }
                        coll[(int)neighbors[side].x][(int)neighbors[side].y] = current_possibilities;
                        //Debug.Log("Neighbor has " + current_possibilities.Count + " current possibilities");

                    }
                }
            }

            //Update entropy array
            for (int i = 0; i < neighbors.Length; i++)
            {
                if (neighbors[i] != new Vector2(-1, -1))
                {
                    int x = (int)neighbors[i].x;
                    int y = (int)neighbors[i].y;
                    //If this neighbor was collapsed make it 9* so it must collapse after
                    if (entr[x][y] != -1)
                    {
                        //int dist_epicenter = (int)Vector2.Distance(first_collapsed, neighbors[i]);

                        //int count = coll[x][y].Count; count *= count;
                        //entr[x][y] = (count * priority)*10 / (dist_epicenter);

                        

                        int modifier = GetCollapsedNeighborCount(x, y, entr) + 1;
                        entr[x][y] = modifier;

                        if (!is_valid)
                            entr[x][y] *= 3;
                        if (coll[x][y].Count == 1)
                            entr[x][y] *= 2;

                        //Debug.Log(entr[x][y]);
                    }
                }
            }
            //Debug.Log("Finished propagating");
        }
        static int GetCollapsedNeighborCount(int x, int y, int[][] entr)
        {
            int count = 0;
            if (y - 1 >= 0)
            {
                if (entr[x][y - 1] == -1)
                    count += 2;
            }
            else
                count++;

            if (x + 1 < entr.Length)
            {
                if (entr[x + 1][y] == -1)
                    count += 2;
            }
            else
                count++;


            if (y + 1 < entr[0].Length)
            {
                if (entr[x][y + 1] == -1)
                    count += 2;
            }
            else
                count++;


            if (x - 1 >= 0)
            {
                if (entr[x - 1][y] == -1)
                    count+=2;
            }
            else
                count++;


            //Debug.Log("Cell has " + count + "collapsed neighbors");
            return (count*count);
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
