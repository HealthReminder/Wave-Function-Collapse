using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WaveFunctionCollapse;

public class Tester : MonoBehaviour
{
    public int pattern_size = 2;
    #region Active
    int[][] input;
    int[][] offset;
    int[][] pattern;
    List<Pattern> unique;
    List<int>[][] collapsing_array;

    private void Start()
    {
        StartCoroutine(Test());
    }

    IEnumerator Test()
    {
        input = GenerateInput();
        Debug.Log("<color=yellow> Generated input: \n</color> " + ReadArray(input));
        offset = WFCInput.GetOffsetArray(input, pattern_size);
        Debug.Log("<color=yellow> Offset grid output: \n</color> " + ReadArray(offset));


        var pattern_info = WFCPattern.GetPatternInformation(offset, pattern_size);
        pattern = pattern_info.pattern_array;
        Debug.Log("<color=orange> Pattern grid output: \n</color> " + ReadArray(pattern));
        unique = pattern_info.pattern_list;

        string log = "";
        for (int i = 0; i < unique.Count; i++)
            log += unique[i].GetValues() + "\n";
        Debug.Log("<color=orange> Unique patterns: \n</color> " + log);

        log = "";
        for (int i = 0; i < unique.Count; i++)
            log += unique[i].GetFrequency() + "\n";
        Debug.Log("<color=red> Frequency of unique patterns: \n</color> " + log);

        log = "";
        for (int i = 0; i < unique.Count; i++)
            log += unique[i].GetSides() + "\n";
        Debug.Log("<color=red> Sides of unique patterns: \n</color> " + log);

        log = "";
        for (int i = 0; i < unique.Count; i++)
            log += "For pattern \n" + unique[i].GetValues() + "\n" + unique[i].GetNeighbors(unique) + "\n-----------\n";
        Debug.Log("<color=red> Neighbors of patterns: </color> " + log);

        //Output an array of patterns of X size according to a pattern list
        //Optional parameters to include a preset first cell
        yield return CollapseArray(pattern_size*3, unique, 1);

        log = "";
        for (int y = 0; y < collapsing_array.Length; y++)
        {
            for (int x = 0; x < collapsing_array[0].Length; x++)
            {

            }
        }
        Debug.Log("<color=green> Collapsed grid: </color> " + log);

        Debug.Log("Finished test routine.");
        yield break;
    }
    IEnumerator CollapseArray(int output_size, List<Pattern> all_patterns, int initial_pattern = -1, int initial_x = -1, int initial_y = -1)
    {
        if (all_patterns == null)
            Debug.LogError("Cannot collapse array with null patterns.");
        else if (all_patterns.Count <= 0)
            Debug.LogError("Cannot collapse array with no patterns.");

        //Setup useful lists and variables
        collapsing_array = WFCCollapse.SetupCollapseArray(collapsing_array, output_size);

        List<Vector2> infinite_cells = new List<Vector2>();
        for (int y = 0; y < output_size; y++)
            for (int x = 0; x < output_size; x++)
                infinite_cells.Add(new Vector2(x, y));

        #region WFC Algorithm
        //FOR DEBUG ONLY
        initial_pattern = -1;

        //FIRST CELL COLLAPSE --------------------------------------------------------------------
        //Store the collapsed cell as coordinates to be removed from the infinite list
        Vector2 collapsed_cell;
        if (initial_pattern != -1)
        {
            if (initial_x == -1)
                initial_x = Random.Range(0, output_size);
            if (initial_y == -1)
                initial_y = Random.Range(0, output_size);
            collapsing_array[initial_x][initial_y] = WFCCollapse.GetHyperstate(all_patterns);
            collapsed_cell = new Vector2(initial_x, initial_y);
            WFCCollapse.CollapseCell(collapsing_array[initial_x][initial_y], all_patterns, initial_pattern);
            //After collapse, remove from infinite list
            Debug.Log("Collapsed first cell of coordinates: " + initial_x + "," + initial_y + " from "+all_patterns.Count+" with a resulting infinite list of length " + infinite_cells.Count);
        } else
        {
            collapsed_cell = WFCCollapse.CollapseRandomCell(collapsing_array, infinite_cells, all_patterns);
        }

        //Remove the collapsed cell
        for (int i = 0; i < infinite_cells.Count; i++)
            if (infinite_cells[i] == collapsed_cell)
            {
                infinite_cells.RemoveAt(i);
                //Debug.Log("Found collapsed cell in infinite list");
                break;
            }

        string log = "";
        log += ReadArrayList(collapsing_array);
        Debug.Log("<color=cyan> Initial collapse: </color> \n" + log);

        //LOOP COLLAPSE --------------------------------------------------------------------
        //Loop until no left cells and result is valid
        bool is_valid = false;
        while (!is_valid)
        {
            for (int t = 0; t < 999; t++)
            {
                collapsed_cell = WFCCollapse.CollapseMostProbable(collapsing_array, infinite_cells, all_patterns);
                //Remove the collapsed cell
                for (int i = 0; i < infinite_cells.Count; i++)
                    if (infinite_cells[i] == collapsed_cell)
                    {
                        infinite_cells.RemoveAt(i);
                        //Debug.Log("Found collapsed cell in infinite list");
                        break;
                    }
                log = "";
                log += ReadArrayList(collapsing_array);
                Debug.Log("<color=cyan> " + t + " collapse: </color> \n" + log);
                if (CheckValidity(collapsing_array, output_size))
                    t = 9999;
            }
            is_valid = CheckValidity(collapsing_array, output_size);
            //Debug.LogWarning("Invalid result. Looping.");
            yield return null;
        }
        #endregion
        yield break;
    }
    bool CheckValidity(List<int>[][] cells, int length)
    {
        for (int y = 0; y < length; y++)
        {
            for (int x = 0; x < length; x++)
            {
                if (cells[x][y] == null)
                    return false;
                else if (cells[x][y] == null)
                    return false;
                else if (cells[x][y].Count != 1)
                    return false;
            }
        }
        return true;
    }
    #endregion
    #region Input
    int[][] GenerateInput()
    {
        //Creates a list so the numbers can be changed on the fly
        int[][] result;
        List<int[]> lists = new List<int[]>();
        lists.Add(new int[4] { 0, 0, 0, 0 });
        lists.Add(new int[4] { 0, 0, 0, 0 });
        lists.Add(new int[4] { 1, 1, 1, 1 });
        lists.Add(new int[4] { 1, 1, 1, 1 });
        result = lists.ToArray();
        return result;
    }
    #endregion
    #region Debug
    string ReadList(List<int> list)
    {
        string log = "";
        for (int i = 0; i < list.Count; i++)
            log += list[i] + " | ";
        return log;
    }
    string ReadArrayList(List<int>[][] arraylist)
    {
        string log = "";

        for (int y = 0; y < arraylist.Length; y++)
        {
            int s = arraylist[y].Length;
            for (int x = 0; x < s; x++)
            {
                if (arraylist[y][x].Count <= 0)
                    log += "X";
                else if (arraylist[y][x].Count > 1)
                    log += "O";
                else if (arraylist[y][x].Count == 1)
                    log += arraylist[y][x][0];
                }
            log += "\n";
        }
        return log;
    }
    string ReadArray(int[][] array)
    {
        string log = "";
        if (array == null)
            log += ("NULL INT ARRAY");
        else
        {
            for (int i = 0; i < array.Length; i++)
            {
                for (int o = 0; o < array[i].Length; o++)
                {
                    log += array[i][o];
                }
                log += "\n";

            }
            log += "\n";
        }

        log += "\n";
        return log;
    }

    #endregion
}
