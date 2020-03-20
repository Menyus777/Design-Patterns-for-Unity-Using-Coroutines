using System.Collections;
using System.Threading;
using UnityEngine;

public class ThreadStartsFirstThreadedCoroutineExample : ThreadedCoroutine
{
    GameObject Fred;

    protected override IEnumerator WorkOnUnityThread()
    {
        Debug.Log("<color=yellow>WorkOnUnityThread method Thread ID:</color> " + Thread.CurrentThread.ManagedThreadId);

        // Find a gameObject called "Fred" and multiply its position value on the Unity Thread
        Fred = GameObject.Find("Fred");
        Fred.transform.position *= 2;

        Debug.Log("<color=yellow>Requesting the Tasks Thread...</color> " + Thread.CurrentThread.ManagedThreadId);
        // Yield work, and continue execution on the Tasks thread
        yield return RequestThreadedCoroutineThread();

        // Now we change the gameobject color to green indicating that we are finished with the process
        Fred.GetComponent<Renderer>().material.color = Color.green;

        Debug.Log("<color=yellow>Requesting the Tasks Thread...</color> " + Thread.CurrentThread.ManagedThreadId);
        // Yield work, and continue execution on the Tasks thread
        yield return RequestThreadedCoroutineThread();
    }

    protected override void WorkOnCoroutineThread(CancellationToken cancellationToken)
    {
        Debug.Log("<color=#00FF00>WorkOnCoroutineThread method Thread ID:</color> " + Thread.CurrentThread.ManagedThreadId);

        // Emulating some hardwork on the Coroutines thread at start
        Thread.Sleep(5000);

        Debug.Log("<color=#00FF00>Requesting Unitys Main thread...</color> " + Thread.CurrentThread.ManagedThreadId);
        // Use request main thread to pause execution of the current thread and yield control to Unitys Main Thread
        RequestUnitysMainThread(cancellationToken);

        // After that another heavy workload takes place
        Thread.Sleep(5000);

        Debug.Log("<color=#00FF00>Requesting Unitys Main thread...</color> " + Thread.CurrentThread.ManagedThreadId);
        // Use request main thread to pause execution of the current thread and yield control to Unitys Main Thread
        RequestUnitysMainThread(cancellationToken);

        Debug.Log("<color=#00FF00>Finished execution of thread</color>" + Thread.CurrentThread.ManagedThreadId);
        // Finishing the Task Thread
        Finish(); // Maybe put this into the Task.Run() method Action body, so users wont need to manually call it
    }
}
