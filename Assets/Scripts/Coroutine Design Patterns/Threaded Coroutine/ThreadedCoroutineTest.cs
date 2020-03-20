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
        var threadedCoroutine2 = new CoroutineStartsFirstThreadedCoroutineExample();
        _threadedCoroutineManager.StartCoroutineThread(threadedCoroutine2);
        _threadedCoroutineManager.StartCoroutineThread(threadedCoroutine1, false, true);
    }
}
