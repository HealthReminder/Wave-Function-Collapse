using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WaveFunctionCollapse;
public class IntegrationTests : UnitTestsBase
{
    public float order = 0;
    private void Start()
    {
        StartCoroutine(Test());
    }

    IEnumerator Test()
    {
        yield return new WaitForSeconds(order);
        //This testing suite is focused on testing the WFC functions working as one
        //Initially it will not present different scenarios, only the complete result to evaluate 
        //The work of the separate unit test suites
        int pattern_size = 2;
        int[] input_linear =
        {
            4,5,4,5,
            7,6,7,6,
            4,5,4,5,
            7,6,7,6
        };
        int[][] input_array = LinearArrayToSquare(input_linear,4);
        Debug.Log("<color=yellow> Input grid: </color>\n " + ReadIntArraySquare(input_array));


        int[][] offset = WFCInputOutput.GetOffsetArray(input_array, pattern_size);
        Debug.Log("<color=orange> Offset grid: </color>\n " + ReadIntArraySquare(offset));

        List<Pattern> unique = WFCPattern.GetUniquePatterns(offset, pattern_size);
        string unique_pattern_log = "" ;
        for (int i = 0; i < unique.Count; i++)
            unique_pattern_log += unique[i].GetValues() + "\n";
        Debug.Log("<color=red> Unique patterns: </color> \n" + unique_pattern_log);

        yield break;
    }
}
