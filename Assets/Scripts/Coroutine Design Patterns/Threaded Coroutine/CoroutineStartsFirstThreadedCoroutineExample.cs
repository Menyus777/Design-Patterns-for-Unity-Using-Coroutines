using System.Collections;
using System.Threading;
using UnityEngine;

public class CoroutineStartsFirstThreadedCoroutineExample : ThreadedCoroutine
{
    GameObject Fred;

    protected override IEnumerator WorkOnUnityThread()
    {
        Debug.Log("Starting WorkOnUnityThread");

        Debug.Log("Getting Fred");
        Fred = GameObject.Find("Fred");
        Debug.Log("Changing localscale!");
        Fred.transform.localScale = Fred.transform.localScale * 1.5f;

        Debug.Log("Passing the execution to the task thread 1st time");
        yield return RequestThreadedCoroutineThread();


        Debug.Log("Setting Freds Position");
        // Multiplying Fred position value on the Unity Thread
        Fred.transform.position *= 2;

        Debug.Log("Passing the execution to the task thread 2nd time");
        // Yield work to the coroutine thread
        yield return RequestThreadedCoroutineThread();

        Debug.Log("Changing Freds color");
        // Now we change the gameobject color to blue indicating that we are finished with the process
        Fred.GetComponent<Renderer>().material.color = Color.green;

        Debug.Log("Finishing with the coroutine!");
    }

    protected override void WorkOnCoroutineThread(CancellationToken cancellationToken)
    {
        Debug.Log("Starting the WorkOnCoroutineThread");

        Debug.Log("Sleeping for 4000 miliseconds");
        // Emulating some hardwork on the Coroutines thread at start.
        Thread.Sleep(4000);

        Debug.Log("Passing the execution to the coroutine");
        // Use request main thread to pause execution of the current thread and yield control to Unitys Main Thread
        RequestUnitysMainThread(cancellationToken);

        Debug.Log("SLeeping for 4500 miliseconds");
        // After that another heavy workload takes place
        Thread.Sleep(4500);

        Debug.Log("Passing the execution to the coroutine 2nd time");
        // Use request main thread to pause execution of the current thread and yield control to Unitys Main Thread
        RequestUnitysMainThread(cancellationToken);

        Debug.Log("<color=green>Task Finished!</color>");
    }
}
