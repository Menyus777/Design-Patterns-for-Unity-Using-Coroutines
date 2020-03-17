using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class Accessor : MonoBehaviour
{
    [SerializeField]
    private Dictionary<Guid, bool> CoroutineThreads = new Dictionary<Guid, bool>();

    public void StartCoroutineThread(CoroutineThread coroutineThread, bool isLongRunning = false) // Long running: Since tasks still run on the ThreadPool, they should not be used for long-running operations, since they can still fill up the thread pool and block new work. Instead, Task provides a LongRunning option, which will tell the TaskScheduler to spin up a new thread rather than running on the ThreadPool.
    {
        CoroutineThreads.Add(coroutineThread.ID, false);
        StartCoroutine(RunCoroutineThread(coroutineThread, isLongRunning));
    }

    private IEnumerator RunCoroutineThread(CoroutineThread coroutineThread, bool isLongRunning)
    {
        Task coroutineTask;
        if (isLongRunning)
            coroutineTask = Task.Factory.StartNew(() => coroutineThread.Work(), TaskCreationOptions.LongRunning);
        else
            coroutineTask = Task.Run(() => coroutineThread.Work());
        while (!coroutineThread.IsFinished)
        {
            Debug.Log("<color=yellow>Accessor method Thread ID:</color> " + Thread.CurrentThread.ManagedThreadId);
            yield return coroutineThread;

            Debug.Log("Need data from the Main Thread in frame " + Time.frameCount);
            CoroutineThreads[coroutineThread.ID] = true;

        }
    }
}
