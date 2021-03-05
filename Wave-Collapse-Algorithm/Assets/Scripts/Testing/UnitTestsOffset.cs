using System.Collections;
using UnityEngine;
using WaveFunctionCollapse;

public class UnitTestsOffset : UnitTestsBase
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
        StartCoroutine(TestOffset(display_internal_results));
    }

    IEnumerator TestOffset(bool display_internal)
    {
        yield return new WaitForSeconds(order);

        bool result_module = true;
        bool result_unit = true;

        ///////////////////////////////////////////////////////////////////////////////// PATTERN SIZE TESTS

        result_module = GetOffsetArray_PatternSizeOne_NoPadding(display_internal);
        result_unit = (result_module) ? result_unit:false;

        result_module = GetOffsetArray_PatternSizeEvenArrayOdd_OnePadding(display_internal);
        result_unit = (result_module) ? result_unit : false;

        result_module = GetOffsetArray_PatternSizeEvenArrayEven_OnePadding(display_internal);
        result_unit = (result_module) ? result_unit : false;

        result_module = GetOffsetArray_PatternSizeOddArrayOdd_OnePadding(display_internal);
        result_unit = (result_module) ? result_unit : false;

        result_module = GetOffsetArray_PatternSizeOddArrayEven_OnePadding(display_internal);
        result_unit = (result_module) ? result_unit : false;

        result_module = GetOffsetArray_PatternSizeEqual_PaddingDoubleOriginalSizeMinusOne(display_internal);
        result_unit = (result_module) ? result_unit : false;

        result_module = GetOffsetArray_PatternSizeBigger_PaddingDoubleOriginalMinusOnePlusExtraLength(display_internal);
        result_unit = (result_module) ? result_unit : false;

        ////////////////////////////////////////////////////////////////////////////// ARRAY TRANSCRIPTION TESTS

        result_module = GetOffsetArray_ComplexPattern_OnePadding(display_internal);
        result_unit = (result_module) ? result_unit : false;
        


        if (result_unit)
            Debug.Log("<color=#191919> Result of offset testing: </color><color=green>True</color>");
        else
            Debug.Log("<color=#191919> Result of offset testing: </color><color=red>False</color>");


        yield break;
    }
    #region Pattern Complexity
    bool GetOffsetArray_ComplexPattern_OnePadding(bool debug)
    {

        int pattern_size = 2;
        int[] input_linear = new int[]
        {
            0,1,0,1,
            3,2,3,2,
            0,1,0,1,
            3,2,3,2
        };
        int[] output_expected = new int[]
        {
            0,1,0,1,0,
            3,2,3,2,3,
            0,1,0,1,0,
            3,2,3,2,3,
            0,1,0,1,0,

        };

        int[][] input_array = LinearArrayToSquare(input_linear, (int)Mathf.Sqrt(input_linear.Length));
        bool result = true;
        int[][] output_square = WFCInputOutput.GetOffsetArray(input_array, pattern_size);
        int[] output_linear = SquareArrayToLinear(output_square);
        result = CompareLinearArrays(output_linear, output_expected);

        //Print the result if required
        if (debug) Debug.Log("<color=blue> GetOffsetArray_ComplexPattern_OnePadding: " + result + "</color>\n" +
            ReadIntArrayLinear(input_linear) + "\n" + ReadIntArrayLinear(output_linear) + "\n" + ReadIntArrayLinear(output_expected));

        return result;
    }
    #endregion
    #region Pattern Size
    bool GetOffsetArray_PatternSizeBigger_PaddingDoubleOriginalMinusOnePlusExtraLength(bool debug)
    {

        int pattern_size = 4;
        int[] input_linear = new int[]
        {
            1,1,0,
            1,1,0,
            1,1,0,
        };
        int[] output_expected = new int[]
        {
            1,1,0,1,1,0,1,
            1,1,0,1,1,0,1,
            1,1,0,1,1,0,1, 
            1,1,0,1,1,0,1,
            1,1,0,1,1,0,1,
            1,1,0,1,1,0,1,
            1,1,0,1,1,0,1
        };

        int[][] input_array = LinearArrayToSquare(input_linear, (int)Mathf.Sqrt(input_linear.Length));
        bool result = true;
        int[][] output_square = WFCInputOutput.GetOffsetArray(input_array, pattern_size);
        int[] output_linear = SquareArrayToLinear(output_square);
        result = CompareLinearArrays(output_linear, output_expected);

        //Print the result if required
        if (debug) Debug.Log("<color=blue> GetOffsetArray_PatternSizeBigger_PaddingDoubleOriginalMinusOnePlusExtraLength: " + result + "</color>\n" +
            ReadIntArrayLinear(input_linear) + "\n" + ReadIntArrayLinear(output_linear) + "\n" + ReadIntArrayLinear(output_expected));

        return result;
    }
    bool GetOffsetArray_PatternSizeEqual_PaddingDoubleOriginalSizeMinusOne(bool debug)
    {

        int pattern_size = 3;
        int[] input_linear = new int[]
        {
            1,1,0,
            1,1,0,
            1,1,0,

        };
        int[] output_expected = new int[]
        {
            1,1,0,1,1,
            1,1,0,1,1,
            1,1,0,1,1,
            1,1,0,1,1,
            1,1,0,1,1,

        };

        int[][] input_array = LinearArrayToSquare(input_linear, (int)Mathf.Sqrt(input_linear.Length));
        bool result = true;
        int[][] output_square = WFCInputOutput.GetOffsetArray(input_array, pattern_size);
        int[] output_linear = SquareArrayToLinear(output_square);
        result = CompareLinearArrays(output_linear, output_expected);

        //Print the result if required
        if (debug) Debug.Log("<color=blue> GetOffsetArray_PatternSizeEqual_PaddingDoubleOriginalSizeMinusOne: " + result + "</color>\n" +
            ReadIntArrayLinear(input_linear) + "\n" + ReadIntArrayLinear(output_linear) + "\n" + ReadIntArrayLinear(output_expected));

        return result;
    }
    bool GetOffsetArray_PatternSizeOne_NoPadding(bool debug)
    {

        int pattern_size = 1;
        int[] input_linear = new int[]
        {
            1,1,0,
            1,1,0,
            1,1,0,
        };
        int[] output_expected = new int[]
        {
            1,1,0,
            1,1,0,
            1,1,0,
        };

        int[][] input_array = LinearArrayToSquare(input_linear, (int)Mathf.Sqrt(input_linear.Length));
        bool result = true;
        int[][] output_square = WFCInputOutput.GetOffsetArray(input_array, pattern_size);
        int[] output_linear = SquareArrayToLinear(output_square);
        result = CompareLinearArrays(output_linear, output_expected);

        //Print the result if required
        if (debug) Debug.Log("<color=blue> GetOffsetArray_PatternSizeOne_NoPadding: " + result + "</color>\n" +
            ReadIntArrayLinear(input_linear) + "\n" + ReadIntArrayLinear(output_linear) + "\n" + ReadIntArrayLinear(output_expected));

        return result;
    }
    bool GetOffsetArray_PatternSizeOddArrayEven_OnePadding(bool debug)
    {

        int pattern_size = 3;
        int[] input_linear = new int[]
        {
            1,1,0,0,
            1,1,0,0,
            1,1,0,0,
            1,1,0,0,
        };
        int[] output_expected = new int[]
        {
            1,1,0,0,1,1,
            1,1,0,0,1,1,
            1,1,0,0,1,1,
            1,1,0,0,1,1,
            1,1,0,0,1,1,
            1,1,0,0,1,1,
        };

        int[][] input_array = LinearArrayToSquare(input_linear, (int)Mathf.Sqrt(input_linear.Length));
        bool result = true;
        int[][] output_square = WFCInputOutput.GetOffsetArray(input_array, pattern_size);
        int[] output_linear = SquareArrayToLinear(output_square);
        result = CompareLinearArrays(output_linear, output_expected);

        //Print the result if required
        if (debug) Debug.Log("<color=blue> GetOffsetArray_PatternSizeOddArrayEven_OnePadding: " + result + "</color>\n" +
            ReadIntArrayLinear(input_linear) + "\n" + ReadIntArrayLinear(output_linear) + "\n" + ReadIntArrayLinear(output_expected));

        return result;
    }

    bool GetOffsetArray_PatternSizeOddArrayOdd_OnePadding(bool debug)
    {

        int pattern_size = 3;
        int[] input_linear = new int[]
        {
            1,1,0,0,1,
            1,1,0,0,1,
            1,1,0,0,1,
            1,1,0,0,1,
            1,1,0,0,1
        };
        int[] output_expected = new int[]
        {
            1,1,0,0,1,1,1,
            1,1,0,0,1,1,1,
            1,1,0,0,1,1,1,
            1,1,0,0,1,1,1,
            1,1,0,0,1,1,1,
            1,1,0,0,1,1,1,
            1,1,0,0,1,1,1,
        };

        int[][] input_array = LinearArrayToSquare(input_linear, (int)Mathf.Sqrt(input_linear.Length));
        bool result = true;
        int[][] output_square = WFCInputOutput.GetOffsetArray(input_array, pattern_size);
        int[] output_linear = SquareArrayToLinear(output_square);
        result = CompareLinearArrays(output_linear, output_expected);

        //Print the result if required
        if (debug) Debug.Log("<color=blue> GetOffsetArray_PatternSizeOddArrayOdd_OnePadding: " + result + "</color>\n" +
            ReadIntArrayLinear(input_linear) + "\n" + ReadIntArrayLinear(output_linear) + "\n" + ReadIntArrayLinear(output_expected));

        return result;
    }
    bool GetOffsetArray_PatternSizeEvenArrayOdd_OnePadding(bool debug)
    {

        int pattern_size = 2;
        int[] input_linear = new int[]
        {
            1,1,0,
            1,1,0,
            1,1,0,
        };
        int[] output_expected = new int[]
        {
            1,1,0,1,
            1,1,0,1,
            1,1,0,1,
            1,1,0,1,
        };

        int[][] input_array = LinearArrayToSquare(input_linear, (int)Mathf.Sqrt(input_linear.Length));
        bool result = true;
        int[][] output_square = WFCInputOutput.GetOffsetArray(input_array, pattern_size);
        int[] output_linear = SquareArrayToLinear(output_square);
        result = CompareLinearArrays(output_linear, output_expected);

        //Print the result if required
        if (debug) Debug.Log("<color=blue> GetOffsetArray_PatternSizeEvenArrayOdd_OnePadding: " + result + "</color>\n" +
            ReadIntArrayLinear(input_linear) + "\n" + ReadIntArrayLinear(output_linear) + "\n" + ReadIntArrayLinear(output_expected));

        return result;
    }
    bool GetOffsetArray_PatternSizeEvenArrayEven_OnePadding(bool debug)
    {

        int pattern_size = 2;
        int[] input_linear = new int[]
        {
            1,1,0,0,
            1,1,0,0,
            1,1,0,0,
            1,1,0,0
        };
        int[] output_expected = new int[]
        {
            1,1,0,0,1,
            1,1,0,0,1,
            1,1,0,0,1,
            1,1,0,0,1,
            1,1,0,0,1,
        };

        int[][] input_array = LinearArrayToSquare(input_linear, (int)Mathf.Sqrt(input_linear.Length));
        bool result = true;
        int[][] output_square = WFCInputOutput.GetOffsetArray(input_array, pattern_size);
        int[] output_linear = SquareArrayToLinear(output_square);
        result = CompareLinearArrays(output_linear, output_expected);

        //Print the result if required
        if (debug) Debug.Log("<color=blue> GetOffsetArray_PatternSizeEvenArrayEven_OnePadding: " + result + "</color>\n" +
            ReadIntArrayLinear(input_linear) + "\n" + ReadIntArrayLinear(output_linear) + "\n" + ReadIntArrayLinear(output_expected));

        return result;
    }


    #endregion
}
