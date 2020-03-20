using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// A coroutine that has an underlying <see cref="Task"/> running on the ThreadPool or on a completely separate Thread.
/// Use this class when you want to make longer running operations in a coroutine like pattern while not blocking Unitys Main Thread.
/// </summary>
public abstract class ThreadedCoroutine : IEnumerator, IStartThreadedCoroutine
{
    /// <summary>
    /// Indicated whether the <see cref="ThreadedCoroutine"/> finished executing
    /// </summary>
    /// <remarks>Only returns true when both the task and the coroutine is completed</remarks>
    public bool IsFinished { get { return _isFinished && _task.IsCompleted; } }

    volatile bool _isFinished = false;

    /// <summary>
    /// Indicates if the <see cref="ThreadedCoroutine"/>s Thread is requesting a Unity main thread operation
    /// </summary>
    volatile bool _requestMainThread;

    /// <summary>
    /// Provides access to the <see cref="Task"/> underlying the <see cref="ThreadedCoroutine"/>
    /// </summary>
    /// <remarks>
    /// The underlying <see cref="Task"/> can be either run on the threadpool or on a completely separate thread depeding 
    /// wether you started the underlying <see cref="ThreadedCoroutine"/> with isLongRunning flag or not
    /// </remarks>
    public Task UnderlyingCoroutineTask { get { return _task; } } 
    protected Task _task;

    /// <summary>
    /// The <see cref="WaitHandle"/> for the <see cref="ThreadedCoroutine"/>
    /// </summary>
    private ManualResetEventSlim _taskManualResetEvent = new ManualResetEventSlim(false);

    IEnumerator IStartThreadedCoroutine.StartWithUnityThread(bool isLongRunning, CancellationToken cancellationToken)
    {
        yield return WorkOnUnityThread();
    }

    IEnumerator IStartThreadedCoroutine.StartWithCoroutineThread(bool isLongRunning, CancellationToken cancellationToken)
    {
        if (isLongRunning)
            _task = Task.Factory.StartNew(() => WorkOnCoroutineThread(cancellationToken),
                cancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Default);
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

    /// <summary>
    /// Cleanes up the unmanaged resources, like <see cref="ManualResetEventSlim"/>
    /// </summary>
    void CleanUp()
    {
        _taskManualResetEvent.Dispose();
    }

    /// <summary>
    /// Blocks the underlying <see cref="Task"/>s <see cref="WorkOnCoroutineThread(CancellationToken)"/> method 
    /// and waits for an engine loop to happen, then execution continues on the <see cref="WorkOnUnityThread"/> method
    /// </summary>
    /// <param name="cancellationToken">A lightweight token that can cancel the execution of the current task</param>
    protected void RequestUnitysMainThread(CancellationToken cancellationToken)
    {
        _requestMainThread = true;
        _taskManualResetEvent.Wait(cancellationToken);
    }

    /// <summary>
    /// Yields execution of the underlying coroutine back to Unitys Main Thread, then unblocks the underlying <see cref="Task"/>. 
    /// After that execution continues on the <see cref="WorkOnCoroutineThread(CancellationToken)"/> method
    /// </summary>
    /// <returns></returns>
    protected IEnumerator RequestThreadedCoroutineThread()
    {
        _taskManualResetEvent.Set();
        yield return this;
    }

    /// <summary>
    /// Indicates that the <see cref="ThreadedCoroutine"/> finished execution
    /// </summary>
    protected void Finish()
    {
        _isFinished = true;
    }

    /// <summary>
    /// The underlying coroutine running on Unitys Main Thread. Put here code that needs APIs from the Main thread.
    /// <para>For example: </para>
    /// <list type="table">
    /// <item><term>Search</term><description><see cref="GameObject.Find(string)"/></description></item>
    /// <item><term>Translation</term><description><see cref="Transform.Translate(Vector3)"/></description></item>
    /// <item><term>Instantiation</term><description><see cref="UnityEngine.Object.Instantiate{T}(T)"/></description></item>
    /// </list>
    /// </summary>
    /// <returns>A yield instruction about the further execution of the coroutine</returns>
    protected abstract IEnumerator WorkOnUnityThread();

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
