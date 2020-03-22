using System;
using System.Collections;

/// <summary>
/// A custom yield instruction using IEnumerator
/// </summary>
public class CustomWaitUntil : IEnumerator
{
    /// <summary>
    /// The predicate that will be evaluated every frame
    /// </summary>
    Func<bool> m_Predicate;

    // This is processed after Unity's coroutine scheduler executes the MoveNext() method
    public object Current { get { return null; } }

    public CustomWaitUntil(Func<bool> predicate) { m_Predicate = predicate; }

    // Comes from IEnumerator Interface, called by Unity in every frame after all Updates have been happened
    public bool MoveNext()
    {
        return !m_Predicate();
    }

    // Comes from IEnumerator Interface, this is not processed by Unity
    public void Reset() { throw new NotImplementedException(); }

}
