using System.Collections;
using UnityEngine;

/// <summary>
/// An unneficient way to use Yield Instructions
/// </summary>
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
            // In every frame that our coroutine is executed, a new instance of WaitWhile will be created causing us unwanted GC spikes
            yield return new WaitWhile(() => false);
        }
    }

    #endregion
}
