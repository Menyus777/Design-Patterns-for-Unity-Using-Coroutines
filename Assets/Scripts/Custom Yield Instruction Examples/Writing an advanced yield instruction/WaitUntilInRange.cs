using System.Collections;
using UnityEngine;

/// <summary>
/// Suspends the execution of the coroutine till the supplied transforms are further than 5 meters away
/// </summary>
public class WaitUntilInRange : IEnumerator
{
    // Transform is a class thus it is passed by reference so we can cache it
    Transform _observer;
    Transform _observed;

    /// <summary>
    /// Constructor of WaitUntilInRange
    /// </summary>
    /// <param name="observer">The observer that will monitor the observed</param>
    /// <param name="observed">The observed that are monitored by the observer</param>
    public WaitUntilInRange(Transform observer, Transform observed)
    {
        _observer = observer;
        _observed = observed;
    }


    /// <summary>
    /// Comes from IEnumerator Interface
    /// "Advances the enumerator to the next element of the collection."
    /// Here it will work as a: Should I Still Be Suspended?
    /// </summary>
    public bool MoveNext()
    {
        // Yes, the enemy is still out of range
        if (Vector3.Distance(_observer.position, _observed.position) > 5.0f)
            return true;
        // No, the enemy is in range
        else
            return false;
    }

    /// <summary>
    /// Comes from IEnumerator Interface "Sets the enumerator to its initial position,
    /// which is before the first element in the collection."
    /// </summary>
    public void Reset() { throw new System.NotSupportedException(); }

    /// <summary>
    /// This is processed after Unity's coroutine scheduler executes the MoveNext() method,
    /// this also comes from IEnumerator Interface 
    /// "Gets the element in the collection at the current position of the enumerator."
    /// </summary>
    public object Current { get { return null; } }
}
