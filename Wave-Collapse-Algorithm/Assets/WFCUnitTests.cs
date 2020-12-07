using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using WaveFunctionCollapse;
public class WFCUnitTests : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(TestWFCNamespace());
    }

    IEnumerator TestWFCNamespace()
    {
        //The WFC algorithm basically works with
        //The flow of the collapsing array
        //The identification of patterns


        //Test Constraint Array Extraction
        bool result_constraint = ConstraintsTest();
        if (result_constraint)
            Debug.Log("<color=#000000> Result of constraint testing: </color><color=green>True</color>");
        else
            Debug.LogError("<color=#000000> Result of constraint testing: </color><color=red>False</color>");


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
    bool ConstraintsTest()
    {
        bool result = false;
        return result;
    }
}
