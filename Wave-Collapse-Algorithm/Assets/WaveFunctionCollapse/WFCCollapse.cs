using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace WaveFunctionCollapse
{
    public static class WFCCollapse
    {
        //COMECA A TRANSFORMAR ESSA COROTINA NA ROTINA DO TESTER
        //AQUI SO TERA FUNCOES QUE RETORNAM TIPOS E NO ROUTINAS
        public static void CollapseCell(List<int>[][] arr, Vector2 coords, List<Pattern> patterns, int collapse_into = -1)
        {
            List<int> cell = arr[(int)coords.x][(int)coords.y];
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

                //string log = "Cell with "+c+" possible indexes generated frequency list of: ";
                //for (int h = 0; h < r_indexes.Count; h++)
                //    log += r_indexes[h] + " ";
                //Debug.Log(log);

                int chosen_solution = r_indexes[Random.Range(0, r_indexes.Count)];
                //Debug.Log("Collapsed cell into "+chosen_solution);
                cell.Clear();
                cell.Add(chosen_solution);
                //Debug.Log("Collapsed cell");

            }
            
            //Propagate to neihgbors
            Propagate(arr, patterns, coords);
        }
        public static List<int>[][] SetupCollapseArray(List<int>[][] arr, int s)
        {
            //This function sets up the resulting array, the initial cells and
            //Return a list of coords in the grid
            //Each cell points to with infinite possibilities
            arr = new List<int>[s][];
            for (int y = 0; y < s; y++)
            {
                arr[y] = new List<int>[s];
                for (int x = 0; x < s; x++)
                    arr[y][x] = new List<int>();
                
            }
            /* List<Vector2> coords = new List<Vector2>();
            arr = new List<int>[s][];
            for (int x = 0; x < s; x++)
            {
                arr[x] = new List<int>[s];
                for (int y = 0; y < s; y++)
                {
                    arr[x][y] = new List<int>();
                    coords.Add(new Vector2(x, y));
                }
            }*/
            return arr;
        }
        public static Vector2 CollapseRandomCell(List<int>[][] arr, List<Vector2> hyperstates, List<Pattern> patterns)
        {
            //If this condition is met it means that no cells were instantiated yet
            //Ideally this should be called only once
            if (hyperstates.Count <= 0)
                Debug.LogError("No cell of infinite solutions in list. Cannot collapse.");
            int index = Random.Range(0, hyperstates.Count);
            //Put the random cell in hyperstate and return it
            int x = (int)hyperstates[index].x; int y = (int)hyperstates[index].y;
            arr[x][y] = GetHyperstate(patterns);
            CollapseCell(arr,hyperstates[index], patterns);
            //Debug.Log("Collapsed random cell of coordinates: " + x + "," + y + " into "+arr[x][y][0]+" from " + patterns.Count + " patterns with a resulting infinite list of length " + hyperstates.Count);

            return hyperstates[index];
        }
        public static List<int> GetHyperstate(List<Pattern> patterns)
        {
            List<int> p = new List<int>();
            for (int i = 0; i < patterns.Count; i++)
                p.Add(i);
            return p;
        }

        public static Vector2 CollapseMostProbable(List<int>[][] arr, List<Vector2> hyperstates, List<Pattern> patterns)
        {
            if (arr == null)
                Debug.LogError("Cannot calculate lowest entropy of null array.");
            else if (arr[0] == null)
                Debug.LogError("Cannot calculate lowest entropy of null rows.");
            else if (arr[0][0] == null)
                Debug.LogError("Cannot calculate lowest entropy of null columns.");
            else if (arr.Length <= 0 || arr[0].Length <= 0)
                Debug.LogError("Cannot calculate lowest entropy of empty array,");
            else if (hyperstates == null)
                Debug.LogError("Cannot collapse most probable cell using null hyperstates list.");
            else if (patterns == null)
                Debug.LogError("Cannot collapse most probable cell using null pattern list.");

            int s = arr.Length;
            //Store the most probable cell in these
            Vector2 chosen_coords = Vector2.zero;
            List<int> chosen_cell;

            int lowest_entropy = GetLowestEntropy(arr);
            if (lowest_entropy == 9999)
            {
                chosen_coords = CollapseRandomCell(arr, hyperstates, patterns);
            }
            else
            {
                List<Vector2> possible_cells = new List<Vector2>();
                for (int y = 0; y < s; y++)
                    for (int x = 0; x < s; x++)
                        if (arr[x][y] != null)
                            if (arr[x][y].Count == lowest_entropy)
                                possible_cells.Add(new Vector2(x, y));
                chosen_coords = possible_cells[Random.Range(0, possible_cells.Count)];
            }
            chosen_cell = arr[(int)chosen_coords.x][(int)chosen_coords.y];
            Debug.Log("Chosen cell " + chosen_cell[0] + " within cells of entropy of " + lowest_entropy);
            //Debug.Log(chosen_cell);
            //Debug.Log(patterns[chosen_cell.possible_patterns[0]].DebugGetNeighbors());
            CollapseCell(arr,chosen_coords, patterns, chosen_cell[Random.Range(0, chosen_cell.Count)]);
            //Debug.Log(DebugCells(cells));
            //Debug.log(cells, false);
            //Before propagation get valid neighbors
<<<<<<< HEAD

=======
            List<int> north, east, south, west;
            north = east = south = west = null;
            //You can only propagate cells that exist obviously
            int cx, cy;
            cx = (int) chosen_coords.x; cy = (int) chosen_coords.y;
            if (chosen_coords.y - 1 >= 0)
                north = arr[cx][cy - 1];
            if (chosen_coords.x + 1 < arr.Length)
                east = arr[cx + 1][cy];
            if (chosen_coords.y + 1 < arr[0].Length)
                south = arr[cx][cy + 1];
            if (chosen_coords.x - 1 >= 0)
                west = arr[cx - 1][cy];
            //Propagate to neihgbors
            //Propagate(chosen_cell, patterns, north, east, south, west);
>>>>>>> parent of 71a1766... Early propagation
            //Debug.Log(DebugCells(cells));
            //Debug.Log(DebugCells(cells, false));
            //Scout the infinite list to remove extra coordinates
            for (int i = 0; i < hyperstates.Count; i++)
                if(hyperstates[i] == chosen_coords)
                {
                    hyperstates.RemoveAt(i);
                    break;
                }
            return chosen_coords;
        }

        static int GetLowestEntropy(List<int>[][] arr)
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
            int lowest_entropy = 9999;
            for (int y = 0; y < c; y++)
                for (int x = 0; x < c; x++)

                    if (arr[x][y] != null && arr[x][y].Count > 1)

                        if (arr[x][y].Count < lowest_entropy && arr[x][y].Count > 1)
                            lowest_entropy = arr[x][y].Count;

            return lowest_entropy;
        }
<<<<<<< HEAD
        static void Propagate(List<int>[][] arr, List<Pattern> patterns, Vector2 main_coord)
        {
            List<int> cell = arr[(int)main_coord.x][(int)main_coord.y];
            if (cell != null)
                if (cell.Count > 1)
                    Debug.LogWarning("Propagating cell with more than one solution: " + cell.Count);

            //Get possible neighbors
            List<int>[] neighbors = new List<int>[4];
            neighbors[0] = neighbors[1] = neighbors[2] = neighbors[3] = null;
            Vector2[] neighbors_coords = new Vector2[4];
            //You can only propagate cells that exist obviously
            int cx, cy;
            cx = (int)main_coord.x; cy = (int)main_coord.y;
            if (main_coord.y - 1 >= 0) { 
                neighbors[0] = arr[cx][cy - 1];
                neighbors_coords[0] = new Vector2(cx, cy - 1);
                }
            if (main_coord.x + 1 < arr.Length)
            {
                neighbors[1] = arr[cx + 1][cy];
                neighbors_coords[1] = new Vector2(cx + 1, cy);

            }
            if (main_coord.y + 1 < arr[0].Length)
            {
                neighbors[2] = arr[cx][cy + 1];
                neighbors_coords[2] = new Vector2(cx, cy + 1);

            }
            if (main_coord.x - 1 >= 0)
            {
                neighbors[3] = arr[cx - 1][cy];
                neighbors_coords[3] = new Vector2(cx - 1, cy);

            }

            //Setup infinite neighbors
            if (neighbors[0] != null)
                if (neighbors[0] == null)
                    neighbors[0] = GetHyperstate(patterns);
            if (neighbors[1] != null)
                if (neighbors[1] == null)
                    neighbors[1] = GetHyperstate(patterns);
            if (neighbors[2] != null)
                if (neighbors[2] == null)
                    neighbors[2] = GetHyperstate(patterns); 
            if (neighbors[3] != null)
                if (neighbors[3] == null)
                    neighbors[3] = GetHyperstate(patterns);

            //All the neighbors of the main cell will be restricted to its possible neighbors.
            //For the neighbors that collapse in consequence of propagation, collapse it to propagate it again
            List<Vector2> consequently_collapsed = new List<Vector2>();
            for (int i = 0; i < cell.Count; i++)
            {
                //Debug.Log("There is "+patterns.Count+" existing patterns");
                //Debug.Log("Cell pattern "+i+" is: "+cell.possible_patterns[i]); 

                List<int>[] n = patterns[cell[i]].possible_neighbors;

                for (int s = 0; s < 4; s++)
                {
                    if (neighbors[s] != null)
                    {
                        if (neighbors[s].Count != 1)
                            for (int o = 0; o < n[0].Count; o++)
                                neighbors[s].Add(n[0][o]);
                        if (neighbors[s].Count == 1)
                            consequently_collapsed.Add(neighbors_coords[s]);
                    }
                }
            }
            Debug.Log(consequently_collapsed.Count + " neighbors affected.");
            for (int i = 0; i < consequently_collapsed.Count; i++)
            {
                CollapseCell(arr, consequently_collapsed[i],patterns);
            }
        }
=======
>>>>>>> parent of 71a1766... Early propagation
    }
}
