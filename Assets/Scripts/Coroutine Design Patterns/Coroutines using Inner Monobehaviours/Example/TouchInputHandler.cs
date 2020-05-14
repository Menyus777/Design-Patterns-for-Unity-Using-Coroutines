using System.Collections;
using UnityEngine;

/// <summary>
/// A non monobehaviour class with access to Unitys engine loop, without any outer monobehaviour
/// </summary>
public class TouchInputHandler : Input
{
    #region Double Tap

    /// <summary>
    /// Tells whether a double tap was registered.
    /// </summary>
    /// <remarks>Change the <see cref="DoubleTapTimeFrame"/> to set custom time frame for a double tap</remarks>
    public static bool DoubleTap
    {
        get
        {
            if (CheckDoubleTap())
                return true;
            else
                return false;
        }
    }
    /// <summary>
    /// The time between two fast taps that counts as a double tap
    /// </summary>
    public static float DoubleTapTimeFrame { get; set; } = 0.35f;
    /// <summary>
    /// The screen position of the double tap
    /// </summary>
    public static Vector2 DoubleTapScreenPosition { get; private set; }

    #region Helper variables & methods

    static bool _tapped;
    static Coroutine startTimerForDoubleTap;

    static bool CheckDoubleTap()
    {
        if (_tapped && touchCount == 1 && GetTouch(0).phase == TouchPhase.Ended)
        {
            _innerMonoBehaviour.StopCoroutine(startTimerForDoubleTap);
            _innerMonoBehaviour.StartCoroutine(SetDoubleClickedMouseButtonToFalse());
            DoubleTapScreenPosition = GetTouch(0).position;
            return true;
        }
        else if (touchCount == 1 && GetTouch(0).phase == TouchPhase.Ended)
        {
            startTimerForDoubleTap = _innerMonoBehaviour.StartCoroutine(StartTimerForDoubleTapCoroutine());
            return false;
        }
        else
        {
            return false;
        }
    }

    static IEnumerator StartTimerForDoubleTapCoroutine()
    {
        _tapped = true;
        yield return new WaitForSeconds(DoubleTapTimeFrame);
        _tapped = false;
    }

    static IEnumerator SetDoubleClickedMouseButtonToFalse()
    {
        yield return new WaitForEndOfFrame();
        DoubleTapScreenPosition = Vector2.zero;
        _tapped = false;
    }

    #endregion

    #endregion

    #region InnerMonobehaviour Design Pattern

    static TouchInputHandler()
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

    #endregion
}
