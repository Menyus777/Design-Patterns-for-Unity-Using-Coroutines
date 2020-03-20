using System;
using System.Collections;
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
    public bool IsFinished { get { return _task.IsCompleted && _task.IsCompleted; } }

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
    /// The <see cref="WaitHandle"/> for the <see cref="ThreadedCoroutine"/> instance
    /// </summary>
    private ManualResetEventSlim _taskManualResetEvent = new ManualResetEventSlim(false);

    /// <summary>
    /// Starts the <see cref="ThreadedCoroutine"/> and begins working on the underlying coroutine. 
    /// Work starts in the <see cref="WorkOnUnityThread"/> method
    /// </summary>
    /// <param name="isLongRunning">A flag indicating whether the thread should start on a separate Thread or on the ThreadPool</param>
    /// <param name="cancellationToken">A lightweight token that can cancel the execution of the underlying <see cref="Task"/></param>
    IEnumerator IStartThreadedCoroutine.StartWithUnityThread(bool isLongRunning, CancellationToken cancellationToken)
    {
        if (isLongRunning)
        {
            _task = Task.Factory.StartNew(() =>
            {
                _taskManualResetEvent.Wait(cancellationToken);
                _taskManualResetEvent.Reset();
                WorkOnCoroutineThread(cancellationToken);
                CleanUp();
            }, cancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }
        else
        {
            _task = Task.Run(() =>
            {
                _taskManualResetEvent.Wait(cancellationToken);
                _taskManualResetEvent.Reset();
                WorkOnCoroutineThread(cancellationToken);
                CleanUp();
            }, cancellationToken);
        } 

        yield return WorkOnUnityThread();

        // Unblocking the Tasks thread if the coroutine does not ends with RequestThreadedCoroutineThread
        _taskManualResetEvent.Set();
    }

    /// <summary>
    /// Starts the <see cref="ThreadedCoroutine"/> and begins working on the underlying <see cref="Task"/>. 
    /// Work starts in the <see cref="WorkOnCoroutineThread(CancellationToken)"/> method
    /// </summary>
    /// <param name="isLongRunning">A flag indicating whether the thread should start on a separate Thread or on the ThreadPool</param>
    /// <param name="cancellationToken">A lightweight token that can cancel the execution of the underlying <see cref="Task"/></param>
    IEnumerator IStartThreadedCoroutine.StartWithCoroutineThread(bool isLongRunning, CancellationToken cancellationToken)
    {
        if (isLongRunning)
            _task = Task.Factory.StartNew(() => { WorkOnCoroutineThread(cancellationToken); CleanUp(); },
                cancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        else
            _task = Task.Run(() => { WorkOnCoroutineThread(cancellationToken); CleanUp(); }, cancellationToken);

        yield return this;
        
        yield return WorkOnUnityThread();

        // Unblocking the Tasks thread if the coroutine does not ends with RequestThreadedCoroutineThread
        _taskManualResetEvent.Set();
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
        _taskManualResetEvent.Reset();
    }

    /// <summary>
    /// Yields execution of the underlying coroutine back to Unitys Main Thread, then unblocks the underlying <see cref="Task"/>. 
    /// After that execution continues on the <see cref="WorkOnCoroutineThread(CancellationToken)"/> method
    /// </summary>
    /// <returns>
    /// A Yield isntruction that halts the execution of the underlying coroutine till a 
    /// <see cref="RequestUnitysMainThread(CancellationToken)"/> call happens on underlying Tasks thread
    /// </returns>
    protected IEnumerator RequestThreadedCoroutineThread()
    {
        _taskManualResetEvent.Set();
        yield return this;
    }

    /// <summary>
    /// The underlying coroutine running on Unitys Main Thread. Put here code that needs APIs from the Main thread.
    /// <para>For example: </para><list type="table">
    /// <item><term>Search</term> <description><see cref="GameObject.Find(string)"/></description></item>
    /// <item><term>Translation</term> <description><see cref="Transform.Translate(Vector3)"/></description></item>
    /// <item><term>Instantiation</term> <description><see cref="UnityEngine.Object.Instantiate{T}(T)"/> etc..</description></item>
    /// </list></summary>
    /// <returns>A yield instruction about the further execution of the coroutine</returns>
    protected abstract IEnumerator WorkOnUnityThread();

    /// <summary>
    /// The underlying Task running on a ThreadPool or on a separate Thread. Put here code that should not run on Unitys Main Thread.
    /// <para>For example: </para><list type="bullet">
    /// <item>Time consuming calculations</item>
    /// <item>Calculations that results do not need in every frame</item>
    /// <item>Operations that communicate with other services/applications, 
    /// and you need a synchronization mechansim with them and Unity</item>
    /// </list></summary>
    /// <param name="cancellationToken"></param>
    protected abstract void WorkOnCoroutineThread(CancellationToken cancellationToken);

    #region Yielder - Controls synchornization with the engine loop

    /// <summary>
    /// Tells whether the coroutine should continue execution or not
    /// </summary>
    bool IEnumerator.MoveNext()
    {
        if (_requestMainThread)
        {
            _requestMainThread = false;
            return false;
        }
        else if (_task.IsCompleted)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    void IEnumerator.Reset() { throw new NotSupportedException("Threaded Coroutines does not support Reset!"); }
    object IEnumerator.Current { get { return null; } }

    #endregion
}
