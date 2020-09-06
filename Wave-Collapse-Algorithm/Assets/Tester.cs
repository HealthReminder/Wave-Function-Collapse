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
        List<Vector2> infinite_cells = new List<Vector2>();
        int qtd_patterns = all_patterns.Count;

        //Setup the resulting array
        //Flag initial cells as cells with infinite possibilities
        collapsing_array = new List<int>[output_size][];
        for (int y = 0; y < output_size; y++)
        {
            collapsing_array[y] = new List<int>[output_size];
            for (int x = 0; x < output_size; x++)
            {
                collapsing_array[y][x] = new List<int>();
                infinite_cells.Add(new Vector2(y, x));
            }
        }

        #region WFC Algorithm

        //Collapse first cell
        if (initial_pattern != -1)
        {
            if (initial_x == -1)
                initial_x = Random.Range(0, output_size);
            if (initial_y == -1)
                initial_y = Random.Range(0, output_size);
            collapsing_array[initial_x][initial_y] = WFCCollapse.SetupHyperposition(all_patterns);
            WFCCollapse.CollapseCell(collapsing_array[initial_x][initial_y], all_patterns, initial_pattern);
            Debug.Log("Collapse first cell.");
        }

        Debug.Log(collapsing_array.Length);
        Debug.Log(collapsing_array[0].Length);
        Debug.Log(collapsing_array[0][0].Count);


        string log = "";
        log += ReadArrayList(collapsing_array);
        Debug.Log("<color=cyan> Collapsed list: </color> \n" + log);

        #endregion
        yield break;
    }
    #endregion
    #region Input
    int[][] GenerateInput()
    {
        //Creates a list so the numbers can be changed on the fly
        int[][] result;
        List<int[]> lists = new List<int[]>();
        lists.Add(new int[4] { 1, 2, 1, 2 });
        lists.Add(new int[4] { 4, 3, 4, 3 });
        lists.Add(new int[4] { 1, 2, 1, 2 });
        lists.Add(new int[4] { 4, 3, 4, 3 });
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
