using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreadedCoroutineTest : MonoBehaviour
{
    ThreadedCoroutineManager _threadedCoroutineManager;

    void Awake()
    {
        _threadedCoroutineManager = GetComponent<ThreadedCoroutineManager>();
    }

    void Start()
    {
        var threadedCoroutine1 = new ExampleThreadedCoroutine();
        var threadedCoroutine2 = new ExampleThreadedCoroutine();
        var threadedCoroutine3 = new ExampleThreadedCoroutine();
        var threadedCoroutine4 = new ExampleThreadedCoroutine();
        var threadedCoroutine5 = new ExampleThreadedCoroutine();
        _threadedCoroutineManager.StartCoroutineThread(threadedCoroutine1);
    }

    void Update()
    {
        var threadedCoroutine = new ExampleThreadedCoroutine();
        _threadedCoroutineManager.StartCoroutineThread(threadedCoroutine);
    }
}
