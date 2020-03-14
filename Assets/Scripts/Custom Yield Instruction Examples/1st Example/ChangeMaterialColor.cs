using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class ChangeMaterialColor : MonoBehaviour
{
    public bool ToRedWithUnityYieldInstruction = false;
    public bool ToYellowCustomYieldInstruction = false;

    // For efficient caching you should always cache yield instructions
    private WaitUntil _waitUntil;
    private CustomWaitUntil _customWaitUntil;

    void Start()
    {
        _waitUntil = new WaitUntil(() => ToRedWithUnityYieldInstruction);
        _customWaitUntil = new CustomWaitUntil(() => ToYellowCustomYieldInstruction);

        StartCoroutine(CChangeColorWithUnitysYieldInstruction());
        StartCoroutine(CChangeColorWithCustomYieldInstruction());
    }

    #region Coroutines

    private IEnumerator CChangeColorWithUnitysYieldInstruction()
    {
        yield return _waitUntil;
        GetComponent<MeshRenderer>().material.color = Color.red;
    }

    private IEnumerator CChangeColorWithCustomYieldInstruction()
    {
        yield return _customWaitUntil;
        GetComponent<MeshRenderer>().material.color = Color.yellow;
    }

    #endregion
}
