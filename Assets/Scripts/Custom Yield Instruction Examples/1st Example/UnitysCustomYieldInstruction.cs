// Decompiled with JetBrains decompiler
// Type: UnityEngine.CustomYieldInstruction
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null

using System.Collections;

public abstract class UnitysCustomYieldInstruction : IEnumerator
{
    /// <summary>
    /// Indicates if coroutine should be kept suspended.
    /// </summary>
    public abstract bool keepWaiting { get; }

    public object Current
    {
        get
        {
            return null;
        }
    }

    public bool MoveNext()
    {
        return keepWaiting;
    }

    public void Reset() { }
}
