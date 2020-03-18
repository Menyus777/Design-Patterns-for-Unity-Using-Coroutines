using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public abstract class ThreadedCoroutine : IEnumerator
{
    /// <summary>
    /// Indicated whether the <see cref="ThreadedCoroutine"/> finished executing or not.
    /// </summary>
    //public bool IsFinished { get { return _isFinished; } protected set { _isFinished = value; } }
    volatile bool _isFinished = false;

    /// <summary>
    /// Indicates if the <see cref="ThreadedCoroutine"/>s Thread is requesting a Unity main thread operation
    /// </summary>
    volatile bool _requestMainThread;

    /// <summary>
    /// Provides access to the task, that running off of Unitys Main Thread
    /// </summary>
    protected Task _task;

    /// <summary>
    /// Manages the the execution of the thread 
    /// </summary>
    private ManualResetEventSlim _pauseManuealResetEvent = new ManualResetEventSlim(false);

    static readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

    public IEnumerator Start(bool isLongRunning)
    {
        if (isLongRunning)
            _task = Task.Factory.StartNew(WorkOnCoroutineThread, TaskCreationOptions.LongRunning);
        else
            _task = Task.Run(WorkOnCoroutineThread);

        while (!_isFinished)
        {
            Debug.Log("<color=yellow>ThreadedCoroutineManager method Thread ID:</color> " + Thread.CurrentThread.ManagedThreadId);
            yield return this;
            WorkOnUnityThread();
            _pauseManuealResetEvent.Set();
        }
    }

    protected void RequestMainThread()
    {
        _requestMainThread = true;
        _pauseManuealResetEvent.Wait();
    }

    protected void Finish()
    {
        _isFinished = true;
    }

    /// Define here what type of delegates you would like to call after requesting Unitys Thread
    protected abstract void WorkOnUnityThread();

    /// Do a specific work here on a seperate thread then, configure some delegates and use <see cref="RequestMainThread()"/> method to execute them on Unitys main thread
    protected abstract void WorkOnCoroutineThread();

    #region Yielder - Controls synchornization with the engine loop
    public object Current { get { return null; } }

    public bool MoveNext()
    {
        if (_requestMainThread)
        {
            _requestMainThread = false;
            return false;
        }
        else
        {
            return true;
        }
    }

    public void Reset() { throw new NotSupportedException(); }

    #endregion
}
