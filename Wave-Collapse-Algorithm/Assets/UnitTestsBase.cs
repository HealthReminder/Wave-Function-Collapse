using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitTestsBase : MonoBehaviour
{
    #region Array Calculations
    public int[][] LinearArrayToSquare(int[] input, int side_length)
    {
        if (side_length * side_length != input.Length)
            Debug.LogError("Cannot convert square array to linear array with invalid side length.");

        int[][] output = new int[side_length][];
        for (int y = 0; y < side_length; y++)
            output[y] = new int[side_length];

        for (int y = 0; y < side_length; y++)
            for (int x = 0; x < side_length; x++)
                output[y][x] = input[x + y * side_length];

        return output;
    }

    public int[] SquareArrayToLinear(int[][] input)
    {
        int side_length = input.Length;
        int[] output = new int[side_length * side_length];
        for (int y = 0; y < side_length; y++)
            for (int x = 0; x < side_length; x++)
                output[x + y * side_length] = input[y][x];
        return output;
    }
    public bool CompareLinearArrays(int[] output, int[] expected)
    {
        //Returns true if arrays are equal to each other
        if (output.Length != expected.Length)
            return false;
        else
        {
            int length = output.Length;
            for (int i = 0; i < length; i++)
                if (output[i] != expected[i])
                    return false;
        }
        return true;
    }
    #endregion

    #region Debug Prints
    public string ReadIntArrayLinear(int[] arr)
    {
        int length = (int)Mathf.Sqrt(arr.Length);
        string log = "";
        if (arr == null)
            log += ("NULL INT ARRAY");
        else
        {
            for (int y = 0; y < length; y++)
            {
                for (int x = 0; x < length; x++)
                {
                    log += arr[x + y * length];
                }
                log += "\n";

            }
            log += "\n";
        }

        log += "\n";
        return log;
    }
    public string ReadIntArraySquare(int[][] arr)
    {
        string log = "";
        if (arr == null)
            log += ("NULL INT ARRAY");
        else
        {
            for (int y = 0; y < arr.Length; y++)
            {
                for (int x = 0; x < arr[y].Length; x++)
                {
                    log += arr[y][x];
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
