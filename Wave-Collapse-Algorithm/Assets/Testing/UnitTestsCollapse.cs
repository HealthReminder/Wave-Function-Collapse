using System.Collections;
using UnityEngine;
using WaveFunctionCollapse;

public class UnitTestsCollapse : UnitTestsBase
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
        StartCoroutine(TestCollapse(display_internal_results));
    }

    IEnumerator TestCollapse(bool display_internal)
    {
        yield return new WaitForSeconds(order);

        bool result_module = true;
        bool result_unit = true;

        ///////////////////////////////////////////////////////////////////////////////// INITIAL PATTERN COLLAPSE TESTS

        result_module = TestTemplate(display_internal);
        result_unit = (result_module) ? result_unit:false;

        


        if (result_unit)
            Debug.Log("<color=#191919> Result of collapse testing: </color><color=green>True</color>");
        else
            Debug.Log("<color=#191919> Result of collapse testing: </color><color=red>False</color>");


        yield break;
    }
    
    #region Pattern Size

    bool TestTemplate(bool debug)
    {
        return true;
        ///////////////////////////////////////////////         Input
        int pattern_size = 2;
        int[] input_linear = new int[]
        {
            1,1,0,0,
            1,1,0,0,
            1,1,0,0,
            1,1,0,0
        };
        int[][] input_array = LinearArrayToSquare(input_linear, (int)Mathf.Sqrt(input_linear.Length));
        int[][] output_square = WFCInputOutput.GetOffsetArray(input_array, pattern_size);

        /////////////////////////////////////////               Expected output
        int[] output_expected = new int[]
        {
            1,1,0,0,1,
            1,1,0,0,1,
            1,1,0,0,1,
            1,1,0,0,1,
            1,1,0,0,1,
        };
        int[] output_linear = SquareArrayToLinear(output_square);

        //////////////////////////////////////                  Comparision
        bool result = true;
        result = CompareLinearArrays(output_linear, output_expected);

        //Print the result if required
        if (debug) Debug.Log("<color=blue> GetCollapseArray_CollapseInitialCell_X: " + result + "</color>\n" +
            ReadIntArrayLinear(input_linear) + "\n" + ReadIntArrayLinear(output_linear) + "\n" + ReadIntArrayLinear(output_expected));

        return result;
    }


    #endregion
}
