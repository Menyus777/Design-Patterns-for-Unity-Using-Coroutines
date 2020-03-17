using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private void Start()
    {
        Accessor accessor = GetComponent<Accessor>();
        accessor.StartCoroutineThread(new CoroutineThread());
    }
}
