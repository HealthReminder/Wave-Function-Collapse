using System.Collections;
using System.Linq;
using UnityEngine;
using WaveFunctionCollapse;

public class UnitTestsInput : MonoBehaviour
{
    //Delayed testing
    float order = 0;
    //Show individual unit results
    bool display_internal_results = true;
    void Start()
    {
        StartCoroutine(TestInput(display_internal_results));
    }

    IEnumerator TestInput(bool display_internal)
    {
        Debug.LogWarning("Input testing is deprecated for now.");
        yield break;

        yield return new WaitForSeconds(order);
        //The WFC algorithm basically works with
        //The flow of the collapsing array
        //The identification of patterns


        //Test Constraint Array Extraction
        bool result_constraint = true;
        bool unit_result = true;

        //1,1,0,0
        //1,1,0,0
        //1,1,0,0
        unit_result = GetConstraintArray_ProperIdentification_VerticalSlice();
        //Print the result if required
        if (display_internal) Debug.Log("<color=blue> GetConstraintArray_ProperIdentification_VerticalSlice: " + unit_result + "</color>");
        //Check if routine has failed
        result_constraint = (unit_result) ? result_constraint:false);


        if (result_constraint)
            Debug.Log("<color=#000000> Result of constraint testing: </color><color=green>True</color>");
        else
            Debug.LogError("<color=#000000> Result of constraint testing: </color><color=red>False</color>");

        yield break;
    }
    #region Input Tests
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
