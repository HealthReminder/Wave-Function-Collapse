using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace WaveFunctionCollapse
{
    public static class WFCCollapse
    {
        //COMECA A TRANSFORMAR ESSA COROTINA NA ROTINA DO TESTER
        //AQUI SO TERA FUNCOES QUE RETORNAM TIPOS E NO ROUTINAS
        public static void CollapseCell(List<int> possible_patterns, List<Pattern> patterns, int collapse_into = -1)
        {
            if (possible_patterns == null)
                Debug.LogError("Cannot collapse a non-initialized cell");
            else if (possible_patterns.Count == 0)
                Debug.LogError("Cannot collapse a cell with no possible solutions");
            else if (possible_patterns.Count == 1)
                Debug.LogWarning("Should not collapse cell with already one solution");

            if (collapse_into == -1)
            {
                possible_patterns.Clear();
                possible_patterns.Add(collapse_into);
            }
            else
            {
                int c = possible_patterns.Count;
                List<int> r_indexes = new List<int>();
                int current_pattern;
                for (int i = 0; i < c; i++)
                {
                    current_pattern = possible_patterns[i];
                    for (int o = 0; o < patterns[current_pattern].frequency; o++)
                        r_indexes.Insert(Random.Range(0, r_indexes.Count), current_pattern);
                }

                //string log = "Cell with "+c+" possible indexes generated frequency list of: ";
                //for (int h = 0; h < r_indexes.Count; h++)
                //    log += r_indexes[h] + " ";
                //Debug.Log(log);

                int chosen_solution = r_indexes[Random.Range(0, r_indexes.Count)];
                //Debug.Log("Collapsed cell into "+chosen_solution);
                possible_patterns.Clear();
                possible_patterns.Add(chosen_solution);
                //Debug.Log("Collapsed cell");

            }
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
            arr[(int)hyperstates[index].x][(int)hyperstates[index].y] = GetHyperstate(patterns);
            return hyperstates[index];
        }
        public static List<int> GetHyperstate(List<Pattern> patterns)
        {
            List<int> p = new List<int>();
            for (int i = 0; i < patterns.Count; i++)
                p.Add(i);
            return p;
        }

        public static void CollapseMostProbable(List<int>[][] arr, List<Vector2> hyperstates, List<Pattern> patterns)
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
            if (lowest_entropy == 0)
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
            //Debug.Log("Chosen cell " + chosen_cell + " within cells of entropy of " + lowest_entropy);
            //Debug.Log(chosen_cell);
            //Debug.Log(patterns[chosen_cell.possible_patterns[0]].DebugGetNeighbors());
            CollapseCell(chosen_cell, patterns, chosen_cell[Random.Range(0, chosen_cell.Count)]);
            //Debug.Log(DebugCells(cells));
            //Debug.log(cells, false);
            //Before propagation get valid neighbors
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
            //Debug.Log(DebugCells(cells));
            //Debug.Log(DebugCells(cells, false));
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
            int lowest_entropy = 0;
            for (int y = 0; y < c; y++)
                for (int x = 0; x < c; x++)

                    if (arr[x][y] != null && arr[x][y].Count > 1)

                        if (arr[x][y].Count < lowest_entropy && arr[x][y].Count > 1)
                            lowest_entropy = arr[x][y].Count;

            return lowest_entropy;
        }
    }
}
