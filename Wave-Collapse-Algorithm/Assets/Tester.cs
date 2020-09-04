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


        //yield return CollapseArray(12, unique);
        Debug.Log("Finished test routine.");
        yield break;
    }
    IEnumerator CollapseArray (int output_size, List<Pattern> all_patterns)
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
        for (int i = 0; i < output_size; i++)
        {
            collapsing_array[i] = new List<int>[output_size];
            for (int o = 0; o < output_size; o++)
            {
                collapsing_array[o][i] = new List<int>();
                infinite_cells.Add(new Vector2(o, i));
            }
        }
        
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
        lists.Add(new int[4] { 4, 3, 4, 3});
        lists.Add(new int[4] { 1, 2, 1, 2 });
        lists.Add(new int[4] { 4, 3, 4, 3 });
        result = lists.ToArray();
        return result;
    }
    #endregion
    #region Debug
    string ReadList (List<int> list)
    {
        string log = "";
        for (int i = 0; i < list.Count; i++)
            log += list[i] +" | ";
        return log;
    }
    string ReadArrayList(List<int[][]> listarray)
    {
        string log = "";
        for (int i = 0; i < listarray.Count; i++)
        {
            int s = listarray[i].Length;
            for (int y = 0; y < s; y++)
            {
                for (int x = 0; x < s; x++)
                {
                    log += listarray[i][x][y];
                }
                log += "\n";

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
