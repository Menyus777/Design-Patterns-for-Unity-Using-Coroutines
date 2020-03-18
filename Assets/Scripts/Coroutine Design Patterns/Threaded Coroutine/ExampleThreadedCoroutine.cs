using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class ExampleThreadedCoroutine : ThreadedCoroutine
{
    Action Operations = null;
    Func<string, GameObject> OperationWithReturnValue = null;
    Action<Transform> OperationsTParam = null;

    GameObject Fred;

    protected override void WorkOnUnityThread()
    {
        Debug.Log("<color=yellow>WorkOnUnityThread method Thread ID:</color> " + Thread.CurrentThread.ManagedThreadId);
        Operations?.Invoke();
        Fred = OperationWithReturnValue?.Invoke("Fred");
        OperationsTParam?.Invoke(Fred.transform);
    }

    protected override void WorkOnCoroutineThread()
    {
        Debug.Log("<color=yellow>WorkOnCoroutineThread method Thread ID:</color> " + Thread.CurrentThread.ManagedThreadId);
        // Emulating some hardwork on the Coroutines thread at start.
        Thread.Sleep(3500);
        // We then request a gameObject called "Fred" (its a cube) and multiply its position value on the Unity Thread
        OperationWithReturnValue += (string gameObjectName) => GameObject.Find(gameObjectName);
        OperationsTParam += (Transform transform) => transform.position *= 2;
        RequestMainThread(); // Use request main thread to pause execution of the current thread and yield control to Unitys Thread

        // After that another heavy workload takes place
        Thread.Sleep(2000);

        // Now we change the gameobject color to green indicating that we are finished with the process
        // Removing the previous method calls
        OperationWithReturnValue = null;
        OperationsTParam = null;
        // Adding the new change color method
        Operations += () => Fred.GetComponent<Renderer>().material.color = Color.green;
        RequestMainThread(); // Use request main thread to pause execution of the current thread and yield control to Unitys Thread
        Debug.Log("<color=#00FF00>Finished execution of thread</color>" + Thread.CurrentThread.ManagedThreadId);
    }

}
