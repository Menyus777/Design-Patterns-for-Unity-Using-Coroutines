using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Awaiters : MonoBehaviour
{

    void Start()
    {
        StartCoroutine(CWaitTillTrue());
    }

    #region Coroutines

    IEnumerator CWaitTillTrue()
    {
        while (true)
        {
            yield return new WaitWhile(() => false);
        }
    }

    #endregion
}
