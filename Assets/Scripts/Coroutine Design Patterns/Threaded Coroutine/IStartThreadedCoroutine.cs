using System.Collections;

/// <summary>
/// An explicitly implemented interface for <see cref="ThreadedCoroutine"/> to prevent programmers accidentally
/// calling <see cref="StartWithCoroutineThread(bool, System.Threading.CancellationToken)"/> and <see cref="StartWithUnityThread(bool, System.Threading.CancellationToken)"/>
/// rather than via <see cref="ThreadedCoroutineManager"/>
/// </summary>
public interface IStartThreadedCoroutine
{
    /// <summary>
    ///  This method shall be called by <see cref="ThreadedCoroutineManager"/> when starting a <see cref="ThreadedCoroutine"/> with the Unity thread first, 
    ///  so the <see cref="ThreadedCoroutine"/> starts with traditional a coroutine part first
    /// </summary>
    IEnumerator StartWithUnityThread(bool isLongRunning, System.Threading.CancellationToken cancellationToken);

    /// <summary>
    ///  This method shall be called by <see cref="ThreadedCoroutineManager"/> when starting a <see cref="ThreadedCoroutine"/> with the underlying task thread first, 
    ///  so the <see cref="ThreadedCoroutine"/> starts with a task runniong either on a Threadpool or on a standalone thread
    /// </summary>
    IEnumerator StartWithCoroutineThread(bool isLongRunning, System.Threading.CancellationToken cancellationToken);
}
