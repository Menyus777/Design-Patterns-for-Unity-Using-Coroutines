using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreadedCoroutineTest: MonoBehaviour
{
    ThreadedCoroutineManager _threadedCoroutineManager;

    void Awake()
    {
        _threadedCoroutineManager = GetComponent<ThreadedCoroutineManager>();
    }

    void Start()
    {
        var threadedCoroutine1 = new ThreadStartsFirstThreadedCoroutineExample();
        var threadedCoroutine2 = new ThreadStartsFirstThreadedCoroutineExample();
        var threadedCoroutine3 = new ThreadStartsFirstThreadedCoroutineExample();
        var threadedCoroutine4 = new ThreadStartsFirstThreadedCoroutineExample();
        var threadedCoroutine5 = new ThreadStartsFirstThreadedCoroutineExample();
        _threadedCoroutineManager.StartCoroutineThread(threadedCoroutine1, threadStarts: true);
    }
}
