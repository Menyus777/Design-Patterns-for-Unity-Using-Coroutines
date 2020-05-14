using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrintOnDoubleTap : MonoBehaviour
{  
    void Update()
    {
        if (TouchInputHandler.DoubleTap)
        {
            Debug.Log("A double tap was registered");
        }
    }
}
