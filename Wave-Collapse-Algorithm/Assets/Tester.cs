using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tester : MonoBehaviour
{
    #region Active
    int[][] input;
    private void Start()
    {
        StartCoroutine (Test());
    }

    IEnumerator Test()
    {
        input = GenerateInput();
        Debug.Log(ReadArray(input));
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
        lists.Add(new int[5] { 0, 0, 0, 0, 0 });
        lists.Add(new int[5] { 0, 0, 0, 0, 0 });
        lists.Add(new int[5] { 0, 0, 0, 0, 0 });
        lists.Add(new int[5] { 1, 1, 1, 1, 1 });
        lists.Add(new int[5] { 0, 0, 0, 0, 0 });
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
            log += "\n";
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
        return log;
    }

    #endregion
}
