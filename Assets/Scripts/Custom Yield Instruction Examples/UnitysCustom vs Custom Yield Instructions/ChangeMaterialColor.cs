using System.Collections;
using UnityEngine;

/// <summary>
/// Set the bools to true in the inspector to see the Yield Instructions in action
/// </summary>
public class ChangeMaterialColor : MonoBehaviour
{
    public bool ToRedUnityYieldInstruction = false;
    public bool ToYellowCustomYieldInstruction = false;

    // For performance reasons(to avoid GC spikes) you should always cache yield instructions whenever you can
    private WaitUntil _waitUntil;
    private CustomWaitUntil _customWaitUntil;

    void Start()
    {
        _waitUntil = new WaitUntil(() => ToRedUnityYieldInstruction);
        _customWaitUntil = new CustomWaitUntil(() => ToYellowCustomYieldInstruction);

        StartCoroutine(CChangeColorWithUnitysYieldInstruction());
        StartCoroutine(CChangeColorWithCustomYieldInstruction());
    }

    #region Coroutines
    /// <summary>
    /// Sets the GameObject color to red using Unitys built-in WaitUntil Yield Instruction
    /// </summary>
    private IEnumerator CChangeColorWithUnitysYieldInstruction()
    {
        yield return _waitUntil;
        GetComponent<MeshRenderer>().material.color = Color.red;
    }

    /// <summary>
    /// Sets the GameObject color to yellow using a Custom WaitUntil Yield Instruction
    /// </summary>
    private IEnumerator CChangeColorWithCustomYieldInstruction()
    {
        yield return _customWaitUntil;
        GetComponent<MeshRenderer>().material.color = Color.yellow;
    }

    #endregion
}
