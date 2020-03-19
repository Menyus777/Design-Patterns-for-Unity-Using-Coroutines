using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public abstract class ThreadedCoroutine : IEnumerator, IStartThreadedCoroutine
{
    /// <summary>
    /// Indicated whether the <see cref="ThreadedCoroutine"/> finished executing
    /// </summary>
    public bool IsFinished { get { return _isFinished && _task.IsCompleted; } }

    volatile bool _isFinished = false;

    /// <summary>
    /// Indicates if the <see cref="ThreadedCoroutine"/>s Thread is requesting a Unity main thread operation
    /// </summary>
    volatile bool _requestMainThread;

    /// <summary>
    /// Provides access to the task underlying the <see cref="ThreadedCoroutine"/>, that running off of Unitys Main Thread
    /// </summary>
    /// <remarks>
    /// The underlying task can be either run on the threadpool or on a completely separate thread depeding on the task
    /// </remarks>
    public Task UnderlyingCoroutineTask { get { return _task; } } 
    protected Task _task;

    /// <summary>
    /// Manages the the execution of the thread 
    /// </summary>
    private ManualResetEventSlim _taskManualResetEvent = new ManualResetEventSlim(false);

    IEnumerator IStartThreadedCoroutine.StartWithUnityThread(bool isLongRunning, CancellationToken cancellationToken)
    {
        yield return WorkOnUnityThread();
    }

    IEnumerator IStartThreadedCoroutine.StartWithCoroutineThread(bool isLongRunning, CancellationToken cancellationToken)
    {
        if (isLongRunning)
            _task = Task.Factory.StartNew(() => WorkOnCoroutineThread(cancellationToken), cancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        else
            _task = Task.Run(() => WorkOnCoroutineThread(cancellationToken), cancellationToken);

        yield return this;

        Debug.Log("<color=orange>After first yield</color> " + Thread.CurrentThread.ManagedThreadId);
        /// Continues execution on Unitys main thread when a <see cref="RequestMainThread()"/> method is called
        yield return WorkOnUnityThread();

        Debug.Log("<color=#00FF00>Finished execution of the coroutine</color>");
        Debug.Log("Task IsCompleted: " + _task.IsCompleted);
        Debug.Log("ThreadCoroutine" + _isFinished);
        CleanUp();
    }

    void CleanUp()
    {
        _taskManualResetEvent.Dispose();
    }

    protected void RequestMainThread(CancellationToken cancellationToken)
    {
        _requestMainThread = true;
        _taskManualResetEvent.Wait(cancellationToken);
    }

    protected IEnumerator RequestCoroutineThread()
    {
        _taskManualResetEvent.Set();
        yield return this;
    }

    protected void Finish()
    {
        _isFinished = true;
    }

    /// Define here what type of delegates you would like to call after requesting Unitys Thread
    protected abstract IEnumerator WorkOnUnityThread();

    /// Do a specific work here on a seperate thread then, configure some delegates and use <see cref="RequestMainThread()"/> method to execute them on Unitys main thread
    protected abstract void WorkOnCoroutineThread(CancellationToken cancellationToken);

    #region Yielder - Controls synchornization with the engine loop

    public bool MoveNext()
    {
        if (_requestMainThread)
        {
            _requestMainThread = false;
            return false;
        }
        else if (_isFinished)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    void IEnumerator.Reset() { throw new NotSupportedException("Threaded Coroutines does not support Reset operation!"); }
    object IEnumerator.Current { get { return null; } }

    #endregion
}
