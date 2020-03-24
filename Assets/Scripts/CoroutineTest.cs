using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineTest : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Update - Started");
        StartCoroutine(TestCoroutine());
        Debug.Log("Update - Finishes execution");
    }

    IEnumerator<string> TestCoroutine()
    {
        Debug.Log("Coroutine - Started");
        yield return null;
        Debug.Log("Coroutine - After 5 seconds i'm finished!");
        yield return "Finished";
    }
}
