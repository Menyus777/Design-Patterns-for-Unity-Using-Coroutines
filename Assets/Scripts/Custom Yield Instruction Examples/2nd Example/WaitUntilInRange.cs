using System.Collections;
using UnityEngine;

public class WaitUntilInRange : IEnumerator
{
    // Transform is a class thus it is passed by referency so we can cache it easily here for some performance boost
    Transform _observer;
    Transform _observed;

    public WaitUntilInRange(Transform observer, Transform observed)
    {
        _observer = observer;
        _observed = observed;
    }

    // Comes from IEnumerator Interface
    // "Advances the enumerator to the next element of the collection."
    public bool MoveNext()
    {
        if (Vector3.Distance(_observer.position, _observed.position) > 5.0f)
            return true;
        else
            return false;
    }

    // Comes from IEnumerator Interface "Sets the enumerator to its initial position,
    // which is before the first element in the collection."
    public void Reset() { }

    // This is processed after Unity's coroutine scheduler executes the MoveNext() method,
    // this also comes from IEnumerator Interface 
    // "Gets the element in the collection at the current position of the enumerator."
    public object Current { get { return null; } }
}
