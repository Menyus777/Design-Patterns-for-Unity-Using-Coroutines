using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class ThreadStartsFirstThreadedCoroutineExample : ThreadedCoroutine
{
    GameObject Fred;

    protected override IEnumerator WorkOnUnityThread()
    {
        Debug.Log("Starting WorkOnUnityThread");

        // Find a gameObject called "Fred" and multiply its position value on the Unity Thread
        Debug.Log("Getting Fred");
        Fred = GameObject.Find("Fred");
        Debug.Log("Setting Freds Position");
        Fred.transform.position *= 2;

        Debug.Log("Passing the execution to the task thread");
        // Yield work, and continue execution on the Tasks thread
        yield return RequestThreadedCoroutineThread();

        Debug.Log("Changing Freds color");
        // Now we change the gameobject color to green indicating that we are finished with the process
        Fred.GetComponent<Renderer>().material.color = Color.green;

        //Debug.Log("Passing the execution to the task thread 2nd time");
        // Yield work, and continue execution on the Tasks thread
        //yield return RequestThreadedCoroutineThread();
    }

    protected override void WorkOnCoroutineThread(CancellationToken cancellationToken)
    {
        Debug.Log("Starting the WorkOnCoroutineThread");

        Debug.Log("Sleeping for 1000 miliseconds");
        // Emulating some hardwork on the Coroutines thread at start
        Thread.Sleep(1000);

        Debug.Log("Passing the execution to the coroutine");
        // Use request main thread to pause execution of the current thread and yield control to Unitys Main Thread
        RequestUnitysMainThread(cancellationToken);

        Debug.Log("SLeeping for 1500 miliseconds");
        // After that another heavy workload takes place
        Thread.Sleep(1500);

        Debug.Log("Passing the execution to the coroutine 2nd time");
        // Use request main thread to pause execution of the current thread and yield control to Unitys Main Thread
        RequestUnitysMainThread(cancellationToken);

        // Finishing the Task Thread
        Debug.Log("<color=green>Task Finished!</color>");
    }
}
