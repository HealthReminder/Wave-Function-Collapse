using System.Collections;
using System.Linq;
using UnityEngine;
using WaveFunctionCollapse;

public class UnitTestsInput : UnitTestsBase
{
    //Delayed testing
    public float order = 0;
    //Show individual unit results
    bool display_internal_results = true;
    void Start()
    {
        StartCoroutine(TestInput(display_internal_results));
    }

    IEnumerator TestInput(bool display_internal)
    {
        Debug.LogWarning("Skipped input test module.");

        yield break;

        yield return new WaitForSeconds(order);
        //The WFC algorithm basically works with
        //The flow of the collapsing array
        //The identification of patterns


        //Test Constraint Array Extraction
        bool result_module = true;
        bool result_unit = true;

        //1,1,0,0
        //1,1,0,0
        //1,1,0,0
        result_unit = GetConstraintArray_VerticalSlice_ProperPatternIndex(display_internal);
        result_unit = (result_module) ? result_unit : false;

        if (result_unit)
            Debug.Log("Result of constraint testing: <color=green>True</color>");
        else
            Debug.LogError("Result of constraint testing: <color=red>False</color>");

        yield break;
    }
    #region Input Tests
    bool GetConstraintArray_VerticalSlice_ProperPatternIndex(bool debug)
    {
        Tile[] input_tiles = IntArrayToTileArray(new int[]
        {
            1,1,0,0,
            1,1,0,0,
            1,1,0,0,
            1,1,0,0,
        });
        int[] output_expected = (new int[]
        {
            0,0,1,1,
            0,0,1,1,
            0,0,1,1,
            0,0,1,1,
        });

        bool result = true;
        int[][] output_square = InputReader.GetConstraintArray(input_tiles);
        int[] output_linear = SquareArrayToLinear(output_square);
        result = CompareLinearArrays(output_linear, output_expected);

        //if (debug) Debug.Log("<color=blue> GetOffsetArray_PatternSizeOne_NoPadding: " + result + "</color>\n" +
            //ReadIntArrayLinear(input_linear) + "\n" + ReadIntArrayLinear(output_linear) + "\n" + ReadIntArrayLinear(output_expected));

        return result;
    }
    Tile[] IntArrayToTileArray(int[] input_array)
    {
        int side_length = input_array.Length;
        int perimeter_length = side_length * side_length;
        Tile[] output_array = new Tile[side_length];
        for (int i = 0; i < side_length; i++)
        {
            output_array[i] = new GameObject().AddComponent<Tile>();
            output_array[i].type = input_array[i];

        }

        return output_array;
    }
    #endregion
}
