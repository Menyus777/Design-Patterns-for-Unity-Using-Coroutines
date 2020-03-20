using System.Collections;
using System.Threading;
using UnityEngine;

public class CoroutineStartsFirstThreadedCoroutineExample : ThreadedCoroutine
{
    GameObject Fred;

    protected override IEnumerator WorkOnUnityThread()
    {

        Fred = GameObject.Find("Fred");
        Fred.transform.localScale = Fred.transform.localScale * 1.5f;

        yield return RequestThreadedCoroutineThread();

        // Doing work on Unitys Main Thread
        Debug.Log("<color=yellow>WorkOnUnityThread method Thread ID:</color> " + Thread.CurrentThread.ManagedThreadId);

        // Multiplying Fred position value on the Unity Thread
        Fred.transform.position *= 2;

        // Yield work to the coroutine thread
        yield return RequestThreadedCoroutineThread();

        // Now we change the gameobject color to blue indicating that we are finished with the process
        Fred.GetComponent<Renderer>().material.color = Color.green;

        yield return RequestThreadedCoroutineThread();
    }

    protected override void WorkOnCoroutineThread(CancellationToken cancellationToken)
    {
        Debug.Log("<color=#00FF00>WorkOnCoroutineThread method Thread ID:</color> " + Thread.CurrentThread.ManagedThreadId);
        // Emulating some hardwork on the Coroutines thread at start.
        Thread.Sleep(3500);

        Debug.Log("<color=#00FF00>Requesting main thread...</color> " + Thread.CurrentThread.ManagedThreadId);
        // Use request main thread to pause execution of the current thread and yield control to Unitys Main Thread
        RequestUnitysMainThread(cancellationToken);

        // After that another heavy workload takes place
        Thread.Sleep(2000);

        // Use request main thread to pause execution of the current thread and yield control to Unitys Main Thread
        RequestUnitysMainThread(cancellationToken);

        Debug.Log("<color=#00FF00>Finished execution of thread</color>" + Thread.CurrentThread.ManagedThreadId);
        Finish();
    }
}
