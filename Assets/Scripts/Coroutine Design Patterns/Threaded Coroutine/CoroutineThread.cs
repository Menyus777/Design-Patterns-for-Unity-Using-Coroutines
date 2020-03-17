using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class CoroutineThread : IEnumerator
{
    /// <summary>
    /// The unique identifier of the coroutine thread
    /// </summary>
    public Guid ID { get; } = Guid.NewGuid();

    /// <summary>
    /// Indicated whether the <see cref="CoroutineThread"/> finished executing or not.
    /// </summary>
    public bool IsFinished { get; } = false;

    bool _requestMainThread;

    public void Work()
    {
        Debug.Log("<color=yellow>Run method Thread ID:</color> " + Thread.CurrentThread.ManagedThreadId);
        Thread.Sleep(3000);
        _requestMainThread = true;
        Operations += WriteTestGameObjectPosition;
        Operations += WriteTestGameObjectRotation;
    }

    public Action Operations;

    public void WriteTestGameObjectPosition()
    {
        GameObject.Find("Test").transform.position = new Vector3(2.0f,5.0f,3.0f);
    }

    public void WriteTestGameObjectRotation()
    {
        GameObject.Find("Test").transform.eulerAngles = new Vector3(15.0f, 70.0f, 50.0f);
    }

    #region Yielder
    public object Current { get { return null; } }

    public bool MoveNext()
    {
        if (_requestMainThread)
        {
            _requestMainThread = false;
            return false;
        }
        else
        {
            return true;
        }
    }

    public void Reset() { }

    #endregion
}
