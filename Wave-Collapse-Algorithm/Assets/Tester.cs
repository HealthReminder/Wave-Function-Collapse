using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WaveFunctionCollapse;

public class Tester : MonoBehaviour
{
    public int pattern_size = 3;
    #region Active
    int[][] input;
    int[][] offset;
    int[][] pattern;
    List<Pattern> unique;
    List<int>[][] collapsing;
    int[][] entropy;
    int[][] output;


    private void Start()
    {
        StartCoroutine(Test());
    }

    IEnumerator Test()
    {
        input = GenerateInput();
        Debug.Log("<color=yellow> Generated input: \n</color> " + ReadArray(input));
        offset = WFCInputOutput.GetOffsetArray(input, pattern_size);
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
        //Setup useful lists and variables
        SetupCollapseArrays(output_size);

        if (all_patterns == null)
            Debug.LogError("Cannot collapse array with null patterns.");
        else if (all_patterns.Count <= 0)
            Debug.LogError("Cannot collapse array with no patterns.");
        else if (output_size <= 0)
            Debug.LogError("Cannot output array of invalid size.");
        else if (collapsing == null)
            Debug.LogError("Cannot collapse null collapsing array.");
        else if (entropy == null)
            Debug.LogError("Cannot collapse null entropy array.");


        #region WFC Algorithm
        //FOR DEBUG ONLY
        initial_pattern = -1;

        //FIRST CELL COLLAPSE --------------------------------------------------------------------
        if (initial_pattern != -1)
        {
            if (initial_x == -1)
                initial_x = Random.Range(0, output_size);
            if (initial_y == -1)
                initial_y = Random.Range(0, output_size);
            Vector2 v = new Vector2(initial_x, initial_y);
            collapsing[initial_x][initial_y] = WFCCollapse.GetHyperstate(all_patterns);
            WFCCollapse.CollapseCell(collapsing, entropy, v, all_patterns, initial_pattern);
            //After collapse, remove from infinite list
            Debug.Log("Collapsed first cell of coordinates: " + initial_x + "," + initial_y + " from "+all_patterns.Count);
        } else
            WFCCollapse.CollapseHyperCell(collapsing, entropy, all_patterns);
        
        string log = ReadArrayList(collapsing);
        Debug.Log("<color=cyan> Initial solution collapse: </color> \n" + log);

        log = ReadArray(entropy);
        Debug.Log("<color=blue> Initial entropy collapse: </color> \n" + log);

        //READ OUTPUT
        output = WFCInputOutput.GetOutputArray(collapsing, unique, pattern_size);
        log = ReadArray(output);
        Debug.Log("<color=magenta> " + "Initial output array" + " collapse: </color> \n" + log);

        //LOOP COLLAPSE --------------------------------------------------------------------
        //Loop until no left cells and result is valid
        bool is_valid = false;
        while (!is_valid)
        {
            for (int t = 0; t < 999; t++)
            {
                WFCCollapse.CollapseMostProbable(collapsing, entropy, all_patterns);

                //READ COLLAPSING
                //log = ReadArrayList(collapsing);
                //Debug.Log("<color=cyan> " + "Test" + " collapse: </color> \n" + log);

                //READ ENTROPY
                //log = ReadArray(entropy);
                //Debug.Log("<color=blue> " + "Test" + " collapse: </color> \n" + log);

                //READ OUTPUT
                output = WFCInputOutput.GetOutputArray(collapsing, unique, pattern_size);
                log = ReadArray(output);
                Debug.Log("<color=magenta> " + "Output array" + " collapse: </color> \n" + log);

                if (CheckValidity(entropy, output_size))
                    break;
            }
            is_valid = CheckValidity(entropy,output_size);
        }
        //Creation was successful!
        //Generate the output
        output = WFCInputOutput.GetOutputArray(collapsing, unique, pattern_size);
        log = ReadArray(output);
        Debug.Log("<color=magenta> " + "Output array" + " collapse: </color> \n" + log);
        #endregion
        yield break;
    }
    void SetupCollapseArrays(int s)
    {
        collapsing = new List<int>[s][];
        for (int y = 0; y < s; y++)
        {
            collapsing[y] = new List<int>[s];
            for (int x = 0; x < s; x++)
                collapsing[y][x] = new List<int>();

        }
        entropy = new int[s][];
        for (int i = 0; i < s; i++)
        {
            entropy[i] = new int[s];
            for (int o = 0; o < s; o++)
            {
                entropy[i][o] = 0;
            }
        }

    }
    bool CheckValidity(int[][] entr, int length)
    {
        for (int y = 0; y < length; y++)
        {
            for (int x = 0; x < length; x++)
            {
                if (entr[x][y] != -1)
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
        lists.Add(new int[4] { 0, 0, 0, 0 });
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
                    log += "▓";
                else if (arraylist[y][x].Count > 1)
                    log += "▒";
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
