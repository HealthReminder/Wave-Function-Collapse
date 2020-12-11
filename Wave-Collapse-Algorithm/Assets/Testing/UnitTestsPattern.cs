using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WaveFunctionCollapse;

public class UnitTestsPattern : UnitTestsBase
{
    //Delayed testing
    public float order = 0;
    //Show individual unit results
    bool display_internal_results = true;
    void Start()
    {
        //This module uses input simulating the results
        //Of the resulting array from the input read
        //Each cell of the input array must be a unique type of tiles
        StartCoroutine(TestUniquePatterns(display_internal_results));
    }

    IEnumerator TestUniquePatterns(bool display_internal)
    {
        yield return new WaitForSeconds(order);

        bool result_module = true;
        bool result_unit = true;

        ///////////////////////////////////////////////////////////////////////////////// PATTERN SIZE TESTS

        result_module = GetPatternList_PatternSizeTwo_AllFourMatch(display_internal);
        result_unit = (result_module) ? result_unit:false;

        ////////////////////////////////////////////////////////////////////////////// ARRAY TRANSCRIPTION TESTS


        if (result_unit)
            Debug.Log("<color=#191919> Result of pattern testing: </color><color=green>True</color>");
        else
            Debug.Log("<color=#191919> Result of pattern testing: </color><color=red>False</color>");


        yield break;
    }
    #region Pattern Size
    bool GetPatternList_PatternSizeTwo_AllFourMatch(bool debug)
    {

        int pattern_size = 2;
        int[] offset_linear = new int[]
        {
            1,2,1,2,
            4,3,4,3,
            1,2,1,2,
            4,3,4,3
        };
        int[][] output_expected = new int[4][];
        output_expected[0] = new int[]
        {
            1,2,
            4,3
        };
        output_expected[1] = new int[]
        {
            2,1,
            3,4
        };
        output_expected[2] = new int[]
        {
            4,3,
            1,2
        };
        output_expected[3] = new int[]
        {
            3,4,
            2,1
        };
        bool result = true;
        int[][] offset_array = LinearArrayToSquare(offset_linear, (int)Mathf.Sqrt(offset_linear.Length));
        List<Pattern> unique_patterns = WFCPattern.GetUniquePatterns(offset_array, pattern_size);
        int[][] pattern_list_to_array = PatternListToArray(unique_patterns);
        result = CompareSquareArrays(pattern_list_to_array, output_expected);

        //Print the result if required
        if (debug) Debug.Log("<color=blue> GetPatternList_PatternSizeTwo_AllFourMatch: " + result + "</color>\n" +
            ReadIntArrayLinear(offset_linear) + "\n" + ReadIntArrayLinear(SquareArrayToLinear(pattern_list_to_array)) + "\n" + ReadIntArrayLinear(SquareArrayToLinear(output_expected)));

        return result;
    }
    #endregion
}
