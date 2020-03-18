using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class ThreadedCoroutineManager : MonoBehaviour
{
    /// <summary>
    /// Starts a Coroutine on the threadpool or on a new thread
    /// </summary>
    /// <param name="coroutineThread">The <see cref="ThreadedCoroutine"/> to start</param>
    /// <param name="isLongRunning">Set this to true if your task is a long running one in order to avoid filling up the threadpool </param>
    /// <remarks>
    /// Tasks run on the ThreadPool, they should not be used for long-running operations, since they can fill up the thread pool and block new work.
    /// Instead, Task provides a LongRunning option, which will tell the TaskScheduler to spin up a new thread rather than running on the ThreadPool.
    /// </remarks>
    public void StartCoroutineThread(ThreadedCoroutine coroutineThread, bool isLongRunning = false)
    {
        StartCoroutine(coroutineThread.Start(isLongRunning));
    }
}
