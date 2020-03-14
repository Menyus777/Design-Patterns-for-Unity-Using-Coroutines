using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwaitersChached : MonoBehaviour
{
    WaitWhile waitWhile= new WaitWhile(() => false);

    void Start()
    {
        StartCoroutine(CWaitTillTrue());
    }

    #region Coroutines

    IEnumerator CWaitTillTrue()
    {
        while (true)
        {
            yield return waitWhile;
        }
    }

    #endregion
}
