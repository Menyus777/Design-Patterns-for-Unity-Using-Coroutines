using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomWaitUntil : IEnumerator
{
    Func<bool> m_Predicate;

    // This is processed after Unity's coroutine scheduler executes the MoveNext() method
    public object Current { get { return null; } }

    public CustomWaitUntil(Func<bool> predicate) { m_Predicate = predicate; }

    // Comes from IEnumerator Interface
    public bool MoveNext()
    {
        return !m_Predicate();
    }

    // Comes from IEnumerator Interface
    public void Reset() { }

}
