using System.Collections;
using UnityEngine;

/// <summary>
/// An efficient way to use Yield Instructions, by reusing the Yield Instruction whenever we can
/// </summary>
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
            // Only one instance of WaitWhile will be created and we will use that over the lifetime of our application
            yield return waitWhile;
        }
    }

    #endregion
}
