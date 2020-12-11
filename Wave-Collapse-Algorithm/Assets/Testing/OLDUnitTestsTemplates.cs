using System.Collections;
using System.Linq;
using UnityEngine;
using WaveFunctionCollapse;

public class OLDUnitTestsTemplate: MonoBehaviour
{
    //Delayed testing
    float order = 0;
    //Show individual unit results
    bool display_internal_results = true;
    void Start()
    {
        StartCoroutine(TestWFCNamespace(display_internal_results));
    }

    IEnumerator TestWFCNamespace(bool display_internal)
    {
        yield return new WaitForSeconds(order);
        //The WFC algorithm basically works with
        //The flow of the collapsing array
        //The identification of patterns


        //Test Constraint Array Extraction
        bool result_constraint = ConstraintsTest(display_internal);
        if (result_constraint)
            Debug.Log("<color=#000000> Result of constraint testing: </color><color=green>True</color>");
        else
            Debug.LogError("<color=#000000> Result of constraint testing: </color><color=red>False</color>");

        yield break;

        //Test Offset Array Generation
        bool result_offset = true;
        if (result_offset)
            Debug.Log("<color=#191919> Result of offset testing: </color><color=green>True</color>");
        else
            Debug.LogError("<color=#191919> Result of offset testing: </color><color=red>False</color>");

        //Test Unique Pattern Extraction
        bool result_unique = true;
        if (result_unique)
            Debug.Log("<color=#333333> Result of unique pattern testing: </color><color=green>True</color>");
        else
            Debug.LogError("<color=#333333> Result of unique pattern testing: </color><color=red>False</color>");

        //Test Pattern Array Generation
        bool result_pattern = true;
        if (result_pattern)
            Debug.Log("<color=#4c4c4c> Result of pattern array testing: </color><color=green>True</color>");
        else
            Debug.LogError("<color=#4c4c4c> Result of pattern array testing: </color><color=red>False</color>");

        //Test Array Collapse
        bool result_collapse = true;
        if (result_collapse)
            Debug.Log("<color=#666666> Result of collapsed array testing: </color><color=green>True</color>");
        else
            Debug.LogError("<color=#666666> Result of collapsed array testing: </color><color=red>False</color>");


        yield break;
    }
    #region Input Tests
    bool ConstraintsTest(bool display_results)
    {
        bool result = false;

        result = GetConstraintArray_ProperIdentification_VerticalSlice();
        if (display_results)
            Debug.Log("<color=blue> GetConstraintArray_ProperIdentification_VerticalSlice: " + result+"</color>");

        return result;
    }
    bool GetConstraintArray_ProperIdentification_VerticalSlice()
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
        return result;
    }
    bool CompareLinearArrays(int[] output, int[] expected)
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
    int[] SquareArrayToLinear(int[][] input)
    {
        int side_length = input.Length;
        int[] output = new int[side_length * side_length];
        for (int y = 0; y < side_length; y++)
            for (int x = 0; x < side_length; x++)
                output[x + y * side_length] = input[y][x];
        return output;
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
