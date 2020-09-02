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
    List<int[][]> unique;
    List<int> frequencies;
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
        Debug.Log("<color=orange> Unique patterns list: \n</color> " + ReadArrayList(unique));
        frequencies = pattern_info.frequency_list;
        Debug.Log("<color=orange> Unique patterns list: \n</color> " + ReadList(frequencies));


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
