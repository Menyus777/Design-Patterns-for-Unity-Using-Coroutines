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


    volatile bool RequestMainThread = false;

    public void Work()
    {
        Debug.Log("<color=yellow>Run method Thread ID:</color> " + Thread.CurrentThread.ManagedThreadId);
        Thread.Sleep(5000);
        RequestMainThread = true;
    }

    #region Yielder
    public object Current { get { return null; } }

    public bool MoveNext()
    {
        if (!RequestMainThread)
        {
            return true;
        }
        else
        {
            RequestMainThread = false;
            return false;
        }
    }

    public void Reset() { }

    #endregion
}
