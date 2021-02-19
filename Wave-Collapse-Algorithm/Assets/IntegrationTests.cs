using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WaveFunctionCollapse;
public class IntegrationTests : UnitTestsBase
{
    //This script will test the wave function collapse functions working together before collapsing the output
    public float order = 0;
    private void Start()
    {
        StartCoroutine(Test());
    }

    IEnumerator Test()
    {
        yield break;
    }
}
 