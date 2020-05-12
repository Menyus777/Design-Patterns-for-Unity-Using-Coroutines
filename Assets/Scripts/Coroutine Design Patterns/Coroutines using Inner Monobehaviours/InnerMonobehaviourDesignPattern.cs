using UnityEngine;

/// <summary>
/// A non monobehaviour class with access to Unitys engine loop, without any outer monobehaviour
/// </summary>
public class InnerMonobehaviourDesignPattern : Input
{
    static InnerMonobehaviourDesignPattern()
    {
        // Creating a gameobject that will hold our "secret" component
        var gameObject = new GameObject();
        // Properly hiding it from other colleagues that shall not modify it
        gameObject.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector;
        // Adding the component
        _innerMonoBehaviour = gameObject.AddComponent<InnerMonoBehaviour>();
    }

    /// <summary>
    /// A static reference to our inner monobehaviour
    /// </summary>
    static InnerMonoBehaviour _innerMonoBehaviour;
    /// <summary>
    /// The hidden inner monobehaviour
    /// </summary>
    class InnerMonoBehaviour : MonoBehaviour 
    {
        void Awake()
        {
            hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector;
        }
    }
}
