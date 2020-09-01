using System;
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
    private void Start()
    {
        StartCoroutine(Test());
    }

    IEnumerator Test()
    {
        input = GenerateInput();
        Debug.Log("<color=yellow> Generated input: \n</color> " + ReadArray(input));
        offset = ReadInput.GetOffsetArray(input, pattern_size);
        Debug.Log("<color=yellow> Offset grid output: \n</color> " + ReadArray(offset));
        pattern = ReadInput.GetPatternArray(offset, pattern_size);
        Debug.Log("<color=yellow> Pattern grid output: \n</color> " + ReadArray(pattern));
        Debug.Log("Finished test routine.");
        yield break;
    }
    #endregion
    #region Input
    int[][] GenerateInput()
    {
        //Creates a list so the numbers can be changed on the fly
        int[][] result;
        List<int[]> lists = new List<int[]>();
        lists.Add(new int[6] { 0, 0, 0, 0, 0, 0 });
        lists.Add(new int[6] { 0, 0, 0, 0, 0, 0 });
        lists.Add(new int[6] { 0, 0, 0, 0, 0, 0 });
        lists.Add(new int[6] { 8, 8, 8, 8, 8, 8 });
        lists.Add(new int[6] { 8, 8, 8, 8, 8, 8 });
        lists.Add(new int[6] { 8, 8, 8, 8, 8, 8 });
        result = lists.ToArray();
        return result;
    }
    #endregion
    #region Debug
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
