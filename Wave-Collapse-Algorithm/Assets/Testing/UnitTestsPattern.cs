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
        StartCoroutine(TestPatterns(display_internal_results));
    }

    IEnumerator TestPatterns(bool display_internal)
    {
        yield return new WaitForSeconds(order);

        bool result_module = true;
        bool result_unit = true;

        ///////////////////////////////////////////////////////////////////////////////// PATTERN LIST TESTS

        result_module = GetPatternList_PatternSizeTwo_AllFourMatch(display_internal);
        result_unit = (result_module) ? result_unit : false;

        Debug.LogWarning("May be innacurate.");

        ////////////////////////////////////////////////////////////////////////////// PATTERN ARRAY TESTS

        result_module = GetPatternArray_ChessPatternOffsetPatternSizeTwo_PatternIdentificationMatches(display_internal);
        result_unit = (result_module) ? result_unit : false;

        result_module = GetPatternArray_ComplexOffsetPatternSizeTwo_PatternIdentificationMatches(display_internal);
        result_unit = (result_module) ? result_unit : false;

        result_module = GetPatternArray_ComplexOffsetPatternSizeThree_PatternIdentificationMatches(display_internal);
        result_unit = (result_module) ? result_unit : false;

        result_module = GetPatternArray_ComplexOffsetPatternSizeFour_PatternIdentificationMatches(display_internal);
        result_unit = (result_module) ? result_unit : false;

        result_module = GetPatternArray_ComplexOffsetPatternSizeFive_PatternIdentificationMatches(display_internal);
        result_unit = (result_module) ? result_unit : false;

        result_module = GetPatternArray_RealOffsetPatternSizeFour_PatternIdentificationMatches(display_internal);
        result_unit = (result_module) ? result_unit : false;

        

        ////////////////////////////////////////////////////////////////////////////////// NEIGHBOR TESTS

        result_module = GetNeighbors_ChessPatternPatternSizeTwo_FirstLastMatches(display_internal);
        result_unit = (result_module) ? result_unit : false;

        result_module = GetNeighbors_VerticalLinesPatternSizeTwo_FirstLastMatches(display_internal);
        result_unit = (result_module) ? result_unit : false;

        result_module = GetNeighbors_HorizontalLinesPatternSizeTwo_FirstLastMatches(display_internal);
        result_unit = (result_module) ? result_unit : false;

        result_module = GetNeighbors_ComplexPatternSizeTwoDoubleOffset_FirstLastMatches(display_internal);
        result_unit = (result_module) ? result_unit : false;

        result_module = GetNeighbors_CrossingPatternPatternSizeTwoDoubleOffset_FirstLastMatches(display_internal);
        result_unit = (result_module) ? result_unit : false;

        result_module = GetNeighbors_CrossingPatternPatternSizeTwoBiggerOffset_FirstLastMatches(display_internal);
        result_unit = (result_module) ? result_unit : false;


        



        if (result_unit)
            Debug.Log("<color=#191919> Result of pattern testing: </color><color=green>True</color>");
        else
            Debug.Log("<color=#191919> Result of pattern testing: </color><color=red>False</color>");


        yield break;
    }
    #region Neighboring
    
    bool GetNeighbors_HorizontalLinesPatternSizeTwo_FirstLastMatches(bool debug)
    {
        ////////////////////////////////////////////////////      INPUT
        int pattern_size = 2;
        //This is the offset array to get unique pattern list
        int[] input_offset_linear = new int[]
        {
            0,0,0,0,
            1,1,1,1,
            0,0,0,0,
            1,1,1,1,
        };
        int[][] offset_array = LinearArrayToSquare(input_offset_linear, (int)Mathf.Sqrt(input_offset_linear.Length));
        List<Pattern> pattern_list = WFCPattern.GetUniquePatterns(offset_array, pattern_size);

        //This is the pattern index array used to find the neighbors
        int[] input_pattern_linear = new int[]
        {
            0,0,0,-1,
            1,1,1,-1,
            0,0,0,-1,
            -1,-1,-1,-1,
        };
        int[][] pattern_array = LinearArrayToSquare(input_pattern_linear, (int)Mathf.Sqrt(input_offset_linear.Length));
        List<Pattern> output_neighbor_list = WFCPattern.GetNeighbors(pattern_array, pattern_size, pattern_list);

        /////////////////////////////////////////////////////     OUTPUT
        int[][] output_first_pattern_neighbors = ListIntToArray(output_neighbor_list[0].possible_neighbors);
        int[][] output_last_pattern_neighbors = ListIntToArray(output_neighbor_list[output_neighbor_list.Count - 1].possible_neighbors);

        int[][] expected_first_neighbors = new int[4][];  //0
        expected_first_neighbors[0] = new int[] { 1 }; //Top
        expected_first_neighbors[1] = new int[] { 0 }; //Right
        expected_first_neighbors[2] = new int[] { 1 }; //Bot
        expected_first_neighbors[3] = new int[] { 0 }; //Left

        int[][] expected_last_neighbors = new int[4][];  //1
        expected_last_neighbors[0] = new int[] { 0 }; //Top
        expected_last_neighbors[1] = new int[] { 1 }; //Right
        expected_last_neighbors[2] = new int[] { 0 }; //Bot
        expected_last_neighbors[3] = new int[] { 1 }; //Left

        ///////////////     COMPARISON
        bool result = true;
        if (!CompareArrays(output_first_pattern_neighbors, expected_first_neighbors))
            result = false;
        if (!CompareArrays(output_last_pattern_neighbors, expected_last_neighbors))
            result = false;

        if (debug) Debug.Log(string.Format("<color=red> GetNeighbors_HorizontalLinesPatternSizeTwo_FirstLastMatches: {0} </color> \n" +
            "Offset linear array: \n" + ReadIntArrayLinear(input_offset_linear) + "\n" +
            "Pattern linear array: \n" + ReadIntArrayLinear(input_pattern_linear) + "\n" +
            "Expected neighbors of first pattern : \n{2}" +
            "Output neighbors of first pattern: \n{3}{4}" +
            "Expected neighbors of last pattern: \n{5}" +
            "Output neighbors of last pattern: \n{6}{7}"
            , result, ReadIntArrayLinear(input_offset_linear),
             ReadIntArraySquare(expected_first_neighbors), ReadIntArraySquare(output_neighbor_list[0].values), ReadIntArraySquare(output_first_pattern_neighbors),
             ReadIntArraySquare(expected_last_neighbors), ReadIntArraySquare(output_neighbor_list[output_neighbor_list.Count - 1].values), ReadIntArraySquare(output_last_pattern_neighbors)
            ));

        return result;
    } 
    bool GetNeighbors_VerticalLinesPatternSizeTwo_FirstLastMatches(bool debug)
    {
        ////////////////////////////////////////////////////      INPUT
        int pattern_size = 2;
        //This is the offset array to get unique pattern list
        int[] input_offset_linear = new int[]
        {
            0,1,0,1,
            0,1,0,1,
            0,1,0,1,
            0,1,0,1,
        };
        int[][] offset_array = LinearArrayToSquare(input_offset_linear, (int)Mathf.Sqrt(input_offset_linear.Length));
        List<Pattern> pattern_list = WFCPattern.GetUniquePatterns(offset_array, pattern_size);

        //This is the pattern index array used to find the neighbors
        int[] input_pattern_linear = new int[]
        {
            0,1,0,-1,
            0,1,0,-1,
            0,1,0,-1,
            -1,-1,-1,-1,
        };
        int[][] pattern_array = LinearArrayToSquare(input_pattern_linear, (int)Mathf.Sqrt(input_offset_linear.Length));
        List<Pattern> output_neighbor_list = WFCPattern.GetNeighbors(pattern_array, pattern_size, pattern_list);

        /////////////////////////////////////////////////////     OUTPUT
        int[][] output_first_pattern_neighbors = ListIntToArray(output_neighbor_list[0].possible_neighbors);
        int[][] output_last_pattern_neighbors = ListIntToArray(output_neighbor_list[output_neighbor_list.Count - 1].possible_neighbors);

        int[][] expected_first_neighbors = new int[4][];  //0
        expected_first_neighbors[0] = new int[] { 0 }; //Top
        expected_first_neighbors[1] = new int[] { 1 }; //Right
        expected_first_neighbors[2] = new int[] { 0 }; //Bot
        expected_first_neighbors[3] = new int[] { 1 }; //Left

        int[][] expected_last_neighbors = new int[4][];  //1
        expected_last_neighbors[0] = new int[] { 1 }; //Top
        expected_last_neighbors[1] = new int[] { 0 }; //Right
        expected_last_neighbors[2] = new int[] { 1 }; //Bot
        expected_last_neighbors[3] = new int[] { 0 }; //Left

        ///////////////     COMPARISON
        bool result = true;
        if (!CompareArrays(output_first_pattern_neighbors, expected_first_neighbors))
            result = false;
        if (!CompareArrays(output_last_pattern_neighbors, expected_last_neighbors))
            result = false;

        if (debug) Debug.Log(string.Format("<color=red> GetNeighbors_VerticalLinesPatternSizeTwo_FirstLastMatches: {0} </color> \n" +
            "Offset linear array: \n" + ReadIntArrayLinear(input_offset_linear) + "\n" +
            "Pattern linear array: \n" + ReadIntArrayLinear(input_pattern_linear) + "\n" +
            "Expected neighbors of first pattern : \n{2}" +
            "Output neighbors of first pattern: \n{3}{4}" +
            "Expected neighbors of last pattern: \n{5}" +
            "Output neighbors of last pattern: \n{6}{7}"
            , result, ReadIntArrayLinear(input_offset_linear),
             ReadIntArraySquare(expected_first_neighbors), ReadIntArraySquare(output_neighbor_list[0].values), ReadIntArraySquare(output_first_pattern_neighbors),
             ReadIntArraySquare(expected_last_neighbors), ReadIntArraySquare(output_neighbor_list[output_neighbor_list.Count - 1].values), ReadIntArraySquare(output_last_pattern_neighbors)
            ));

        return result;
    }
    bool GetNeighbors_ChessPatternPatternSizeTwo_FirstLastMatches(bool debug)
    {
        ////////////////////////////////////////////////////      INPUT
        int pattern_size = 2;
        //This is the offset array to get unique pattern list
        int[] input_offset_linear = new int[]
        {
            0,1,0,1,
            1,0,1,0,
            0,1,0,1,
            1,0,1,0,
        };
        int[][] offset_array = LinearArrayToSquare(input_offset_linear, (int)Mathf.Sqrt(input_offset_linear.Length));
        List<Pattern> pattern_list = WFCPattern.GetUniquePatterns(offset_array, pattern_size);

        //This is the pattern index array used to find the neighbors
        int[] input_pattern_linear = new int[]
        {
            0,1,0,-1,
            1,0,1,-1,
            0,1,0,-1,
            -1,-1,-1,-1,
        };
        int[][] pattern_array = LinearArrayToSquare(input_pattern_linear, (int)Mathf.Sqrt(input_offset_linear.Length));
        List<Pattern> output_neighbor_list = WFCPattern.GetNeighbors(pattern_array, pattern_size, pattern_list);

        /////////////////////////////////////////////////////     OUTPUT
        int[][] output_first_pattern_neighbors = ListIntToArray(output_neighbor_list[0].possible_neighbors);
        int[][] output_last_pattern_neighbors = ListIntToArray(output_neighbor_list[output_neighbor_list.Count - 1].possible_neighbors);

        int[][] expected_first_neighbors = new int[4][];
        expected_first_neighbors[0] = new int[] { 1 }; //Top
        expected_first_neighbors[1] = new int[] { 1 }; //Right
        expected_first_neighbors[2] = new int[] { 1 }; //Bot
        expected_first_neighbors[3] = new int[] { 1 }; //Left

        int[][] expected_last_neighbors = new int[4][];
        expected_last_neighbors[0] = new int[] { 0 }; //Top
        expected_last_neighbors[1] = new int[] { 0 }; //Right
        expected_last_neighbors[2] = new int[] { 0 }; //Bot
        expected_last_neighbors[3] = new int[] { 0 }; //Left


        ///////////////     COMPARISON
        bool result = true;
        if (!CompareArrays(output_first_pattern_neighbors, expected_first_neighbors))
            result = false;
        if (!CompareArrays(output_last_pattern_neighbors, expected_last_neighbors))
            result = false;

        if (debug) Debug.Log("<color=red> GetNeighbors_ChessPatternPatternSizeTwo_FirstLastMatches: " +
            result + "</color>\n" +
            "Offset linear array: \n" + ReadIntArrayLinear(input_offset_linear) + "\n" +
            "Pattern linear array: \n" + ReadIntArrayLinear(input_pattern_linear) + "\n" +
            "Expected neighbors of first pattern: \n" + ReadIntArraySquare((expected_first_neighbors)) + "\n" +
            "Output neighbors of first pattern: \n" + ReadIntArraySquare((output_first_pattern_neighbors)) + "\n" +
            "Expected neighbors of last pattern: \n" + ReadIntArraySquare((expected_last_neighbors)) + "\n" +
            "Output neighbors of last pattern: \n" + ReadIntArraySquare((output_last_pattern_neighbors)) + "\n"
            );

        return result;
    }
    bool GetNeighbors_ComplexPatternSizeTwoDoubleOffset_FirstLastMatches(bool debug)
    {
        ////////////////////////////////////////////////////      INPUT
        int pattern_size = 2;
        //This is the offset array to get unique pattern list
        int[] input_offset_linear = new int[]
        {
            4,5,4,5,
            7,6,7,6,
            4,5,4,5,
            7,6,7,6,
        };
        int[][] offset_array = LinearArrayToSquare(input_offset_linear, (int)Mathf.Sqrt(input_offset_linear.Length));
        List<Pattern> pattern_list = WFCPattern.GetUniquePatterns(offset_array, pattern_size);

        //This is the pattern index array used to find the neighbors
        int[] input_pattern_linear = new int[]
        {
            0,1,0,-1,
            2,3,2,-1,
            0,1,0,-1,
            -1,-1,-1,-1,
        };
        int[][] pattern_array = LinearArrayToSquare(input_pattern_linear, (int)Mathf.Sqrt(input_offset_linear.Length));
        List<Pattern> output_neighbor_list = WFCPattern.GetNeighbors(pattern_array, pattern_size, pattern_list);
        //  0,1 ->0     1,0 ->1     3,2 ->2     2,3 ->3
        //  3,2         2,3         0,1         1,0

        /////////////////////////////////////////////////////     OUTPUT
        int[][] output_first_pattern_neighbors = ListIntToArray(output_neighbor_list[0].possible_neighbors);
        int[][] output_last_pattern_neighbors = ListIntToArray(output_neighbor_list[output_neighbor_list.Count - 1].possible_neighbors);

        int[][] expected_first_neighbors = new int[4][];  //0
        expected_first_neighbors[0] = new int[] { 2 }; //Top
        expected_first_neighbors[1] = new int[] { 1 }; //Right
        expected_first_neighbors[2] = new int[] { 2 }; //Bot
        expected_first_neighbors[3] = new int[] { 1 }; //Left

        int[][] expected_last_neighbors = new int[4][];  //1
        expected_last_neighbors[0] = new int[] { 1 }; //Top
        expected_last_neighbors[1] = new int[] { 2 }; //Right
        expected_last_neighbors[2] = new int[] { 1 }; //Bot
        expected_last_neighbors[3] = new int[] { 2 }; //Left

        ///////////////     COMPARISON
        bool result = true;
        if (!CompareArrays(output_first_pattern_neighbors, expected_first_neighbors))
            result = false;
        if (!CompareArrays(output_last_pattern_neighbors, expected_last_neighbors))
            result = false;

        if (debug) Debug.Log(string.Format("<color=red> GetNeighbors_ComplexPatternSizeTwoDoubleOffset_FirstLastMatches: {0} </color> \n" +
            "Offset linear array: \n" + ReadIntArrayLinear(input_offset_linear) + "\n" +
            "Pattern linear array: \n" + ReadIntArrayLinear(input_pattern_linear) + "\n" +
            "Expected neighbors of first pattern : \n{2}" +
            "Output neighbors of first pattern: \n{3}{4}" +
            "Expected neighbors of last pattern: \n{5}" +
            "Output neighbors of last pattern: \n{6}{7}"
            , result, ReadIntArrayLinear(input_offset_linear),
             ReadIntArraySquare(expected_first_neighbors), ReadIntArraySquare(output_neighbor_list[0].values), ReadIntArraySquare(output_first_pattern_neighbors),
             ReadIntArraySquare(expected_last_neighbors), ReadIntArraySquare(output_neighbor_list[output_neighbor_list.Count - 1].values), ReadIntArraySquare(output_last_pattern_neighbors)
            ));

        return result;
    }
    bool GetNeighbors_CrossingPatternPatternSizeTwoDoubleOffset_FirstLastMatches(bool debug)
    {
        ////////////////////////////////////////////////////      INPUT
        /// 0,0,1,0,
        /// 0,0,1,0,
        /// 1,1,1,1,
        /// 0,0,1,0,
        
        int pattern_size = 2;
        int[] input_offset_linear = new int[]
        {
            0,0,1,0,0,0,
            0,0,1,0,0,0,
            1,1,1,1,1,1,
            0,0,1,0,0,0,
            0,0,1,0,0,0,
            0,0,1,0,0,0,
        };
        int[][] offset_array = LinearArrayToSquare(input_offset_linear, (int)Mathf.Sqrt(input_offset_linear.Length));
        List<Pattern> pattern_list = WFCPattern.GetUniquePatterns(offset_array, pattern_size);

        //This is the pattern index array used to find the neighbors
        int[] input_pattern_linear = new int[]
        {
            0,1,2,0,0,-1,
            3,4,5,3,3,-1,
            6,7,8,6,6,-1,
            0,1,2,0,0,-1,
            0,1,2,0,0,-1,
            -1,-1,-1,-1,-1,-1,
        };
        int[][] pattern_array = LinearArrayToSquare(input_pattern_linear, (int)Mathf.Sqrt(input_offset_linear.Length));
        List<Pattern> output_neighbor_list = WFCPattern.GetNeighbors(pattern_array, pattern_size, pattern_list);

        /////////////////////////////////////////////////////     OUTPUT
        int[][] output_first_pattern_neighbors = ListIntToArray(output_neighbor_list[0].possible_neighbors);
        int[][] output_last_pattern_neighbors = ListIntToArray(output_neighbor_list[output_neighbor_list.Count - 1].possible_neighbors);

        int[][] expected_first_neighbors = new int[4][];
        expected_first_neighbors[0] = new int[] { 6,0 }; //Top
        expected_first_neighbors[1] = new int[] { 1,0 }; //Right
        expected_first_neighbors[2] = new int[] { 3,0 }; //Bot
        expected_first_neighbors[3] = new int[] { 2,0 }; //Left

        int[][] expected_last_neighbors = new int[4][];
        expected_last_neighbors[0] = new int[] { 5 }; //Top
        expected_last_neighbors[1] = new int[] { 6 }; //Right
        expected_last_neighbors[2] = new int[] { 2 }; //Bot
        expected_last_neighbors[3] = new int[] { 7 }; //Left


        ///////////////     COMPARISON
        bool result = true;
        if (!CompareArrays(output_first_pattern_neighbors, expected_first_neighbors))
            result = false;
        if (!CompareArrays(output_last_pattern_neighbors, expected_last_neighbors))
            result = false;

        if (debug) Debug.Log("<color=red> GetNeighbors_CrossingPatternPatternSizeTwoDoubleOffset_FirstLastMatches: " +
            result + "</color>\n" +
            "Offset linear array: \n" + ReadIntArrayLinear(input_offset_linear) + "\n" +
            "Pattern linear array: \n" + ReadIntArrayLinear(input_pattern_linear) + "\n" +
            "Expected neighbors of first pattern: \n" + ReadIntArraySquare((expected_first_neighbors)) + "\n" +
            "Output neighbors of first pattern: \n" + ReadIntArraySquare((output_first_pattern_neighbors)) + "\n" +
            "Expected neighbors of last pattern: \n" + ReadIntArraySquare((expected_last_neighbors)) + "\n" +
            "Output neighbors of last pattern: \n" + ReadIntArraySquare((output_last_pattern_neighbors)) + "\n"
            );

        return result;
    }
    bool GetNeighbors_CrossingPatternPatternSizeTwoBiggerOffset_FirstLastMatches(bool debug)
    {
        Debug.LogWarning("Algorithm does not work on Odd Offsets");
        return false;
        ////////////////////////////////////////////////////      INPUT
        int pattern_size = 2;
        int[] input_offset_linear = new int[]
        {
            0,0,1,0,0,0,0,
            0,0,1,0,0,0,0,
            1,1,1,1,1,1,1,
            0,0,1,0,0,0,0,
            0,0,1,0,0,0,0,
            0,0,1,0,0,0,0,
            0,0,1,0,0,0,0,
        };  
        int[][] offset_array = LinearArrayToSquare(input_offset_linear, (int)Mathf.Sqrt(input_offset_linear.Length));
        List<Pattern> pattern_list = WFCPattern.GetUniquePatterns(offset_array, pattern_size);

        //This is the pattern index array used to find the neighbors
        int[] input_pattern_linear = new int[]
        {
            0,1,2,0,0,0,-1,
            3,4,5,3,3,3,-1,
            6,7,8,6,6,6,-1,
            0,1,2,0,0,0,-1,
            0,1,2,0,0,0,-1,
            0,1,2,0,0,0,-1,
            -1,-1,-1,-1,-1,-1,
        };
        int[][] pattern_array = LinearArrayToSquare(input_pattern_linear, (int)Mathf.Sqrt(input_offset_linear.Length));
        List<Pattern> output_neighbor_list = WFCPattern.GetNeighbors(pattern_array, pattern_size, pattern_list);

        /////////////////////////////////////////////////////     OUTPUT
        int[][] output_first_pattern_neighbors = ListIntToArray(output_neighbor_list[0].possible_neighbors);
        int[][] output_last_pattern_neighbors = ListIntToArray(output_neighbor_list[output_neighbor_list.Count - 1].possible_neighbors);

        int[][] expected_first_neighbors = new int[4][];
        expected_first_neighbors[0] = new int[] { 6, 0 }; //Top
        expected_first_neighbors[1] = new int[] { 1, 0 }; //Right
        expected_first_neighbors[2] = new int[] { 3, 0 }; //Bot
        expected_first_neighbors[3] = new int[] { 2, 0 }; //Left

        int[][] expected_last_neighbors = new int[4][];
        expected_last_neighbors[0] = new int[] { 5 }; //Top
        expected_last_neighbors[1] = new int[] { 6 }; //Right
        expected_last_neighbors[2] = new int[] { 2 }; //Bot
        expected_last_neighbors[3] = new int[] { 7 }; //Left


        ///////////////     COMPARISON
        bool result = true;
        if (!CompareArrays(output_first_pattern_neighbors, expected_first_neighbors))
            result = false;
        if (!CompareArrays(output_last_pattern_neighbors, expected_last_neighbors))
            result = false;

        if (debug) Debug.Log("<color=red> GetNeighbors_CrossingPatternPatternSizeTwoBiggerOffset_FirstLastMatches: " +
            result + "</color>\n" +
            "Offset linear array: \n" + ReadIntArrayLinear(input_offset_linear) + "\n" +
            "Pattern linear array: \n" + ReadIntArrayLinear(input_pattern_linear) + "\n" +
            "Expected neighbors of first pattern: \n" + ReadIntArraySquare((expected_first_neighbors)) + "\n" +
            "Output neighbors of first pattern: \n" + ReadIntArraySquare((output_first_pattern_neighbors)) + "\n" +
            "Expected neighbors of last pattern: \n" + ReadIntArraySquare((expected_last_neighbors)) + "\n" +
            "Output neighbors of last pattern: \n" + ReadIntArraySquare((output_last_pattern_neighbors)) + "\n"
            );

        return result;
    }

    #endregion
    #region Pattern Array 
    bool GetPatternArray_ChessPatternOffsetPatternSizeTwo_PatternIdentificationMatches(bool debug)
    {
        int pattern_size = 2;
        int[] offset_linear = new int[]
        {
            0,1,0,1,
            1,0,1,0,
            0,1,0,1,
            1,0,1,0,
        };
        int[][] offset_array = LinearArrayToSquare(offset_linear, (int)Mathf.Sqrt(offset_linear.Length));

        List<Pattern> unique_patterns = WFCPattern.GetUniquePatterns(offset_array, pattern_size);
        //////////////////////////////////////////////

        int[] output_expected = new int[]
       {
            0,1,0,-1,
            1,0,1,-1,
            0,1,0,-1,
            -1,-1,-1,-1,

       };

        //////////////////////////////////////////////
        int[][] output_square = WFCPattern.GetPatternArray(offset_array, pattern_size, unique_patterns);
        int[] output_linear = SquareArrayToLinear(output_square);
        bool result = CompareLinearArrays(output_linear, output_expected);
        if (debug) Debug.Log("<color=magenta> GetPatternArray_ChessPatternOffsetPatternSizeTwo_PatternIdentificationMatches: " + result + "</color>\n" +
           ReadIntArrayLinear(offset_linear) + "\n" + ReadIntArrayLinear((output_expected)) + "\n" + ReadIntArrayLinear(SquareArrayToLinear(output_square)));

        return result;
    }
    bool GetPatternArray_ComplexOffsetPatternSizeTwo_PatternIdentificationMatches(bool debug)
    {
        int pattern_size = 2;
        int[] offset_linear = new int[]
        {
            4,5,4,5,
            7,6,7,6,
            4,5,4,5,
            7,6,7,6
        };
        int[][] offset_array = LinearArrayToSquare(offset_linear, (int)Mathf.Sqrt(offset_linear.Length));

        List<Pattern> unique_patterns = WFCPattern.GetUniquePatterns(offset_array, pattern_size);
        //////////////////////////////////////////////

        int[] output_expected = new int[]
       {
            0,1,0,-1,
            2,3,2,-1,
            0,1,0,-1,
            -1,-1,-1,-1,

       };

        //////////////////////////////////////////////
        int[][] output_square = WFCPattern.GetPatternArray(offset_array, pattern_size, unique_patterns);
        int[] output_linear = SquareArrayToLinear(output_square);
        bool result = CompareLinearArrays(output_linear, output_expected);
        if (debug) Debug.Log("<color=magenta> GetPatternArray_ComplexOffsetPatternSizeTwo_PatternIdentificationMatches: " + result + "</color>\n" +
           ReadIntArrayLinear(offset_linear) + "\n" + ReadIntArrayLinear((output_expected)) + "\n" + ReadIntArrayLinear(SquareArrayToLinear(output_square)));

        return result;
    }
    bool GetPatternArray_ComplexOffsetPatternSizeThree_PatternIdentificationMatches(bool debug)
    {
        int pattern_size = 3;
        int[] offset_linear = new int[]
        {
            4,5,4,5,4,5,4,
            7,6,7,6,7,6,7,
            4,5,4,5,4,5,4,
            7,6,7,6,7,6,7,
            4,5,4,5,4,5,4,
            7,6,7,6,7,6,7,
            4,5,4,5,4,5,4,
        };
        int[][] offset_array = LinearArrayToSquare(offset_linear, (int)Mathf.Sqrt(offset_linear.Length));

        List<Pattern> unique_patterns = WFCPattern.GetUniquePatterns(offset_array, pattern_size);
        //////////////////////////////////////////////

        int[] output_expected = new int[]
       {
            0,1,0,1,0,-1,-1,
            2,3,2,3,2,-1,-1,
            0,1,0,1,0,-1,-1,
            2,3,2,3,2,-1,-1,
            0,1,0,1,0,-1,-1,
            -1,-1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,-1,

       };

        //////////////////////////////////////////////
        int[][] output_square = WFCPattern.GetPatternArray(offset_array, pattern_size, unique_patterns);
        int[] output_linear = SquareArrayToLinear(output_square);
        bool result = CompareLinearArrays(output_linear, output_expected);
        if (debug) Debug.Log("<color=magenta> GetPatternArray_ComplexOffsetPatternSizeThree_PatternIdentificationMatches: " + result + "</color>\n" +
           ReadIntArrayLinear(offset_linear) + "\n" + ReadIntArrayLinear((output_expected)) + "\n" + ReadIntArrayLinear(SquareArrayToLinear(output_square)));

        return result;
    }
    bool GetPatternArray_ComplexOffsetPatternSizeFour_PatternIdentificationMatches(bool debug)
    {
        int pattern_size = 4;
        int[] offset_linear = new int[]
        {
            4,5,4,5,4,5,4,5,
            7,6,7,6,7,6,7,6,
            4,5,4,5,4,5,4,5,
            7,6,7,6,7,6,7,6,
            4,5,4,5,4,5,4,5,
            7,6,7,6,7,6,7,6,
            4,5,4,5,4,5,4,5,
            7,6,7,6,7,6,7,6,
        };
        int[][] offset_array = LinearArrayToSquare(offset_linear, (int)Mathf.Sqrt(offset_linear.Length));

        List<Pattern> unique_patterns = WFCPattern.GetUniquePatterns(offset_array, pattern_size);
        //////////////////////////////////////////////

        int[] output_expected = new int[]
       {
            0,1,0,1,0,-1,-1,-1,
            2,3,2,3,2,-1,-1,-1,
            0,1,0,1,0,-1,-1,-1,
            2,3,2,3,2,-1,-1,-1,
            0,1,0,1,0,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,-1,-1

       };

        //////////////////////////////////////////////
        int[][] output_square = WFCPattern.GetPatternArray(offset_array, pattern_size, unique_patterns);
        int[] output_linear = SquareArrayToLinear(output_square);
        bool result = CompareLinearArrays(output_linear, output_expected);
        if (debug) Debug.Log("<color=magenta> GetPatternArray_ComplexOffsetPatternSizeFour_PatternIdentificationMatches: " + result + "</color>\n" +
           ReadIntArrayLinear(offset_linear) + "\n" + ReadIntArrayLinear((output_expected)) + "\n" + ReadIntArrayLinear(SquareArrayToLinear(output_square)));

        return result;
    }
    bool GetPatternArray_ComplexOffsetPatternSizeFive_PatternIdentificationMatches(bool debug)
    {
        int pattern_size = 4;
        int[] offset_linear = new int[]
        {
            4,5,4,5,4,5,4,5,4,5,
            7,6,7,6,7,6,7,6,7,6,
            4,5,4,5,4,5,4,5,4,5,
            7,6,7,6,7,6,7,6,7,6,
            4,5,4,5,4,5,4,5,4,5,
            7,6,7,6,7,6,7,6,7,6,
            4,5,4,5,4,5,4,5,4,5,
            7,6,7,6,7,6,7,6,7,6,
            4,5,4,5,4,5,4,5,4,5,
            7,6,7,6,7,6,7,6,7,6,
        };
        int[][] offset_array = LinearArrayToSquare(offset_linear, (int)Mathf.Sqrt(offset_linear.Length));

        List<Pattern> unique_patterns = WFCPattern.GetUniquePatterns(offset_array, pattern_size);
        //////////////////////////////////////////////

        int[] output_expected = new int[]
       {
            4,5,4,5,4,5,-1,-1,-1,-1,
            7,6,7,6,7,6,-1,-1,-1,-1,
            4,5,4,5,4,5,-1,-1,-1,-1,
            7,6,7,6,7,6,-1,-1,-1,-1,
            4,5,4,5,4,5,-1,-1,-1,-1,
            7,6,7,6,7,6,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,-1,-1,



       };

        //////////////////////////////////////////////
        int[][] output_square = WFCPattern.GetPatternArray(offset_array, pattern_size, unique_patterns);
        int[] output_linear = SquareArrayToLinear(output_square);
        bool result = CompareLinearArrays(output_linear, output_expected);
        if (debug) Debug.Log("<color=magenta> GetPatternArray_ComplexOffsetPatternSizeFive_PatternIdentificationMatches: " + result + "</color>\n" +
           ReadIntArrayLinear(offset_linear) + "\n" + ReadIntArrayLinear((output_expected)) + "\n" + ReadIntArrayLinear(SquareArrayToLinear(output_square)));

        return result;
    }
    bool GetPatternArray_RealOffsetPatternSizeFour_PatternIdentificationMatches(bool debug)
    {
        int pattern_size = 4;
        int[] offset_linear = new int[]
        {
            8,8,0,0,8,8,0,0,
            8,8,0,0,8,8,0,0,
            7,7,6,6,7,7,6,6,
            7,7,6,6,7,7,6,6,
            8,8,0,0,8,8,0,0,
            8,8,0,0,8,8,0,0,
            7,7,6,6,7,7,6,6,
            7,7,6,6,7,7,6,6
        };
        int[][] offset_array = LinearArrayToSquare(offset_linear, (int)Mathf.Sqrt(offset_linear.Length));

        List<Pattern> unique_patterns = WFCPattern.GetUniquePatterns(offset_array, pattern_size);
        //////////////////////////////////////////////

        int[] output_expected = new int[]
       {
            0,1,2,3,0,-1,-1,-1,
            4,5,6,7,4,-1,-1,-1,
            8,9,10,11,8,-1,-1,-1,
            12,13,14,15,12,-1,-1,-1,
             0,1,2,3,0,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,-1,-1,
            -1,-1,-1,-1,-1,-1,-1,-1

       };

        //////////////////////////////////////////////
        int[][] output_square = WFCPattern.GetPatternArray(offset_array, pattern_size, unique_patterns);
        int[] output_linear = SquareArrayToLinear(output_square);
        bool result = CompareLinearArrays(output_linear, output_expected);
        if (debug) Debug.Log("<color=magenta> GetPatternArray_RealOffsetPatternSizeFour_PatternIdentificationMatches: " + result + "</color>\n" +
           ReadIntArrayLinear(offset_linear) + "\n" + ReadIntArrayLinear((output_expected)) + "\n" + ReadIntArrayLinear(SquareArrayToLinear(output_square)));

        return result;
    }

    #endregion
    #region Pattern List

    bool GetPatternList_PatternSizeThree_AllFourMatch(bool debug)
    {
        int pattern_size = 3;
        int[] offset_linear = new int[]
        {
            4,5,4,5,
            7,6,7,6,
            4,5,4,5,
            7,6,7,6
        };
        int[][] offset_array = LinearArrayToSquare(offset_linear, (int)Mathf.Sqrt(offset_linear.Length));

        int[][] output_expected = new int[4][];
        output_expected[0] = new int[]
        {
            1,2,1,
            4,3,4,
            1,2,1
        };
        output_expected[1] = new int[]
        {
            2,1,2,
            3,4,3,
            2,1,2
        };
        output_expected[2] = new int[]
        {
            4,3,4,
            1,2,1,
            4,3,4
        };
        output_expected[3] = new int[]
        {
            3,4,3,
            2,1,2,
            3,4,3
        };
        List<Pattern> unique_patterns = WFCPattern.GetUniquePatterns(offset_array, pattern_size);
        int[][] pattern_list_to_array = PatternListToArray(unique_patterns);

        bool result = true;
        result = CompareSquareArrays(pattern_list_to_array, output_expected);

        //Print the result if required
        if (debug) Debug.Log("<color=purple> GetPatternList_PatternSizeThree_AllFourMatch: " + result + "</color>\n" +
            ReadIntArrayLinear(offset_linear) + "\n" + ReadIntArrayLinear(SquareArrayToLinear(pattern_list_to_array)) + "\n" + ReadIntArrayLinear(SquareArrayToLinear(output_expected)));

        return result;
    }

    bool GetPatternList_PatternSizeTwo_AllFourMatch(bool debug)
    {

        int pattern_size = 2;
        int[] offset_linear = new int[]
        {
            4,5,4,5,
            7,6,7,6,
            4,5,4,5,
            7,6,7,6
        };
        int[][] offset_array = LinearArrayToSquare(offset_linear, (int)Mathf.Sqrt(offset_linear.Length));

        int[][] output_expected = new int[4][];
        output_expected[0] = new int[]
        {
            4,5,
            7,6
        };
        output_expected[1] = new int[]
        {
            5,4,
            6,7
        };
        output_expected[2] = new int[]
        {
            7,6,
            4,5
        };
        output_expected[3] = new int[]
        {
            6,7,
            5,4
        };
        List<Pattern> unique_patterns = WFCPattern.GetUniquePatterns(offset_array, pattern_size);
        int[][] pattern_list_to_array = PatternListToArray(unique_patterns);

        bool result = true;
        result = CompareSquareArrays(pattern_list_to_array, output_expected);

        //Print the result if required
        if (debug) Debug.Log("<color=purple> GetPatternList_PatternSizeTwo_AllFourMatch: " + result + "</color>\n" +
            ReadIntArrayLinear(offset_linear) + "\n" + ReadIntArrayLinear(SquareArrayToLinear(pattern_list_to_array)) + "\n" + ReadIntArrayLinear(SquareArrayToLinear(output_expected)));

        return result;
    }
    #endregion
}
