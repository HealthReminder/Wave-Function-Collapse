using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using WaveFunctionCollapse;

public class Tester : MonoBehaviour
{
    #region Input
    //These unique objects must be cached by the main routine
    //It is a list that composes the tileset
    public Transform tileset_transform;
    Tile[] tiles;
    //Dataset size is the size of the side of the square 
    //Or grid that represents the dataset
    //This script only works on square matrix
    public Transform dataset_transform;
    Tile[] dataset;
    int[][] output_for_display;
    #endregion
    #region WFC
    public int pattern_size = 3;
    public int output_size = 30;
    public Texture2D input_map;
    #region Active
    int[][] input;
    int[][] offset;
    int[][] pattern;
    List<Pattern> unique;
    List<int>[][] collapsing;
    int[][] entropy;
    int[][] output;
    char[][] result;
    #endregion
    #endregion

    private void Start()
    {
        StartCoroutine(Test());
        StartCoroutine(ReloadScene(3));
    }
    IEnumerator ReloadScene(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    IEnumerator Test()
    {
        //Get the tileset and dataset from the respective transforms
        tiles = tileset_transform.GetComponentsInChildren<Tile>();
        dataset = dataset_transform.GetComponentsInChildren<Tile>();
        Debug.Log("<color=green> Got tileset of size: " + tiles.Length + ". Got dataset of size: " + dataset.Length + "\n</color> ");


        input = InputReader.GetInput(dataset);
        Debug.Log("<color=yellow> Generated input: \n</color> " + ReadArrayInt(input));
        offset = WFCInputOutput.GetOffsetArray(input, pattern_size);
        Debug.Log("<color=yellow> Offset grid output: \n</color> " + ReadArrayInt(offset));


        var pattern_info = WFCPattern.GetPatternInformation(offset, pattern_size);
        pattern = pattern_info.pattern_array;
        Debug.Log("<color=orange> Pattern grid output: \n</color> " + ReadArrayInt(pattern));
        unique = pattern_info.pattern_list;

        string log = "";
        for (int i = 0; i < unique.Count; i++)
            log += unique[i].GetValues() + "\n";
        Debug.Log("<color=orange> Unique patterns: \n</color> " + log);

        log = "";
        for (int i = 0; i < unique.Count; i++)
            log += unique[i].GetFrequency() + "\n";
        Debug.Log("<color=red> Frequency of unique patterns: \n</color> " + log);

        //log = "";
        //for (int i = 0; i < unique.Count; i++)
        //    log += unique[i].GetSides() + "\n";
        //Debug.Log("<color=red> Sides of unique patterns: \n</color> " + log);

        log = "";
        for (int i = 0; i < unique.Count; i++)
            log += "For pattern \n" + unique[i].GetValues() + "\n" + unique[i].GetNeighbors(unique) + "\n-----------\n";
        Debug.Log("<color=red> Neighbors of patterns: </color> " + log);

        //Output an array of patterns of X size according to a pattern list
        //Optional parameters to include a preset first cell
        yield return CollapseArray(output_size, unique, 1);

        StartCoroutine(InstantiateOutput(output, tiles));

        Debug.Log("Finished test routine.");
        yield break;
    }
    public static IEnumerator InstantiateOutput(int[][] indexes, Tile[] tiles)
    {
        int side = indexes.Length;
        int l = tiles.Length;
        for (int y = 0; y < side; y++)
        {
            for (int x = 0; x < side; x++)
            {
                int index = -1;
                int type = indexes[x][y];
                for (int i = 0; i < l; i++)
                {
                    if (tiles[i].type == type)
                        index = i;
                }

                if (index == -1)
                    Debug.LogError("Could not find matching tile in tileset to instantiate output.");

                Instantiate(tiles[index], new Vector3(x, -10, -y), Quaternion.identity);
            }
        }

        yield return null;
    }
    IEnumerator CollapseArray(int output_size, List<Pattern> all_patterns, int initial_pattern = -1, int initial_x = -1, int initial_y = -1)
    {
        //Setup useful lists and variables
        SetupCollapseArrays(output_size);

        if (all_patterns == null)
            Debug.LogError("Cannot collapse array with null patterns.");
        else if (all_patterns.Count <= 0)
            Debug.LogError("Cannot collapse array with no patterns.");
        else if (output_size <= 0)
            Debug.LogError("Cannot output array of invalid size.");
        else if (collapsing == null)
            Debug.LogError("Cannot collapse null collapsing array.");
        else if (entropy == null)
            Debug.LogError("Cannot collapse null entropy array.");


        #region WFC Algorithm
        //FOR DEBUG ONLY
        initial_pattern = 1;
        Vector2 initial_collapse;
        //FIRST CELL COLLAPSE --------------------------------------------------------------------
        if (initial_pattern != -1)
        {
            if (initial_x == -1)
                initial_x = 2;//Random.Range(0, output_size);
            if (initial_y == -1)
                initial_y = 2;//Random.Range(0, output_size);
            initial_collapse = new Vector2(initial_x, initial_y);
            collapsing[initial_x][initial_y] = WFCCollapse.GetHyperstate(all_patterns);
            WFCCollapse.CollapseCell(collapsing, entropy, initial_collapse, all_patterns, initial_collapse, initial_pattern);
            //After collapse, remove from infinite list
            Debug.Log("Collapsed first cell of coordinates: " + initial_x + "," + initial_y + " from " + all_patterns.Count);
        } else
            initial_collapse = WFCCollapse.CollapseHyperCell(collapsing, entropy, all_patterns, new Vector2(-1, -1));

        string log = ReadArrayList(collapsing);
        Debug.Log("<color=cyan> Initial solution collapse: </color> \n" + log);


        Debug.Log("<color=yellow> " + "Interpreted input array" + " collapse: </color> \n" + ReadArrayChar(InterpretOutput(input)));

        //READ OUTPUT
        output = WFCInputOutput.GetOutputArray(collapsing, unique, pattern_size);
        //log = ReadArray(output);
        //Debug.Log("<color=magenta> " + "Initial output array" + " collapse: </color> \n" + log);
        result = InterpretOutput(output);
        log = ReadArrayChar(result);
        Debug.Log("<color=green> " + "Initial interpreted output array" + " collapse: </color> \n" + log);

        //LOOP COLLAPSE --------------------------------------------------------------------
        //Loop until no left cells and result is valid
        bool is_valid = false;
        while (!is_valid)
        {
            for (int t = 0; t < 999; t++)
            {
                WFCCollapse.CollapseMostProbable(collapsing, entropy, all_patterns, initial_collapse);

                //READ COLLAPSING
                //log = ReadArrayList(collapsing);
                //Debug.Log("<color=cyan> " + "Test" + " collapse: </color> \n" + log);

                //READ ENTROPY
                //log = ReadArray(entropy);
                // Debug.Log("<color=blue> " + "Test" + " collapse: </color> \n" + log);

                //READ OUTPUT
                output = WFCInputOutput.GetOutputArray(collapsing, unique, pattern_size);
                log = ReadArrayInt(output);
                Debug.Log("<color=magenta> " + "Initial output array" + " collapse: </color> \n" + log);
                //result = InterpretOutput(output);
                //log = ReadArrayChar(result);
                //Debug.Log("<color=green> " + t +" interpreted output array" + " collapse: </color> \n" + log);

                if (CheckValidity(entropy, output_size))
                    break;
            }
            is_valid = CheckValidity(entropy, output_size);
        }
        //Creation was successful!
        //Generate the output
        output = WFCInputOutput.GetOutputArray(collapsing, unique, pattern_size);
        log = ReadArrayInt(output);
        Debug.Log("<color=magenta> " + "Output array" + " collapse: </color> \n" + log);
        #endregion
        yield break;
    }
    char[][] InterpretOutput(int[][] arr)
    {
        //This should be provided by the reading of the input tileset
        char char_invalid = '?';
        List<char> char_list = new List<char>() { '░', '▒', '▒', '▓', '▓', '▒', '░', '▒', '▓', '▓' };

        //Setup result array
        int h = arr.Length;
        int w = arr[0].Length;
        char[][] result = new char[h][];
        for (int y = 0; y < h; y++)
            result[y] = new char[w];

        //Convert index into chars
        //Or tiles
        for (int y = 0; y < h; y++)
        {
            for (int x = 0; x < w; x++)
            {
                result[x][y] = char_invalid;
                //Debug.Log("1");
                int i = arr[x][y];
                //Debug.Log(i);
                if (i >= 0 && i < char_list.Count)
                    result[x][y] = char_list[i];
                //Debug.Log("3");

            }
        }
        return result;
    }
    void SetupCollapseArrays(int s)
    {
        collapsing = new List<int>[s][];
        for (int y = 0; y < s; y++)
            collapsing[y] = new List<int>[s];

        for (int y = 0; y < s; y++)
            for (int x = 0; x < s; x++)
                collapsing[x][y] = new List<int>();


        entropy = new int[s][];
        for (int i = 0; i < s; i++)
            entropy[i] = new int[s];
        for (int i = 0; i < s; i++)
            for (int o = 0; o < s; o++)
                entropy[o][i] = 0;

    }
    bool CheckValidity(int[][] entr, int length)
    {
        for (int y = 0; y < length; y++)
        {
            for (int x = 0; x < length; x++)
            {
                if (entr[x][y] != -1)
                    return false;
            }
        }
        return true;
    }
    #region Input
    /*int[][] GenerateInput()
    {
        //Creates a list so the numbers can be changed on the fly
        int[][] result;
        result = new int[4][];
        for (int x = 0; x < 4; x++)
            result[x] = new int[4];

        

        //Results in
        // 1 2
        // 4 3
        for (int y = 0; y < 4; y++)
        {
            for (int x = 0; x < 4; x++)
            {
                if (y % 2 == 0)
                {
                    if (x % 2 == 0)
                        result[x][y] = 1;
                    else
                        result[x][y] = 2;
                }
                else
                {
                    if (x % 2 == 0)
                        result[x][y] = 4;
                    else
                        result[x][y] = 3;
                }
            }
        }

        //Results in a line
        // 0 0
        // 1 1
        for (int y = 0; y < 4; y++)
        {
            for (int x = 0; x < 4; x++)
            {
                if (y != 2)
                    result[x][y] = 0;
                else
                    result[x][y] = 1;
            }
        }
        //Results axis-inverted inputs
        List<int[]> lists = new List<int[]>();/*
        lists.Add(new int[10] { 0, 0, 0, 0, 1, 0, 0, 0, 0, 0 });
        lists.Add(new int[10] { 0, 0, 0, 0, 1, 0, 0, 0, 0, 9 });
        lists.Add(new int[10] { 0, 0, 2, 3, 4, 3, 5, 0, 0, 1 });
        lists.Add(new int[10] { 0, 0, 1, 0, 0, 0, 1, 0, 0, 1 });
        lists.Add(new int[10] { 1, 1, 6, 0, 0, 0, 6, 1, 1, 6 });
        lists.Add(new int[10] { 0, 0, 1, 0, 0, 0, 1, 0, 0, 1 });
        lists.Add(new int[10] { 0, 0, 8, 3, 4, 3, 7, 0, 0, 1 });
        lists.Add(new int[10] { 0, 0, 0, 0, 1, 0, 0, 0, 0, 9 });
        lists.Add(new int[10] { 0, 0, 0, 0, 1, 0, 0, 0, 0, 0 });
        lists.Add(new int[10] { 0, 0, 0, 0, 1, 0, 0, 0, 0, 0 });
        
        lists.Add(new int[10] { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
        lists.Add(new int[10] { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
        lists.Add(new int[10] { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
        lists.Add(new int[10] { 1, 0, 0, 0, 4, 4, 0, 0, 0, 0 });
        lists.Add(new int[10] { 1, 0, 0, 0, 4, 4, 0, 0, 0, 0 });
        lists.Add(new int[10] { 1, 0, 0, 0, 1, 0, 0, 0, 0, 0 });
        lists.Add(new int[10] { 1, 0, 0, 0, 1, 0, 0, 0, 0, 0 });
        lists.Add(new int[10] { 1, 0, 0, 0, 1, 0, 0, 0, 0, 0 });
        lists.Add(new int[10] { 1, 0, 0, 0, 1, 0, 0, 0, 0, 0 });
        lists.Add(new int[10] { 3, 2, 2, 2, 3, 2, 2, 2, 2, 2 });

        result = lists.ToArray();
        return result;
    }*/
    #endregion
    #region Debug
    string ReadList(List<int> list)
    {
        string log = "";
        for (int i = 0; i < list.Count; i++)
            log += list[i] + " | ";
        return log;
    }
    string ReadArrayList(List<int>[][] arraylist)
    {
        string log = "";

        for (int y = 0; y < arraylist.Length; y++)
        {
            int s = arraylist[y].Length;
            for (int x = 0; x < s; x++)
            {
                if (arraylist[y][x].Count <= 0)
                    log += "▓";
                else if (arraylist[y][x].Count > 1)
                    log += "▒";
                else if (arraylist[y][x].Count == 1)
                    log += arraylist[y][x][0];
            }
            log += "\n";
        }
        return log;
    }
    string ReadArrayChar(char[][] arr)
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
                    log += arr[x][y];
                }
                log += "\n";

            }
            log += "\n";
        }

        log += "\n";
        return log;
    }
    string ReadArrayInt(int[][] arr)
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
                    log += arr[x][y];
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
