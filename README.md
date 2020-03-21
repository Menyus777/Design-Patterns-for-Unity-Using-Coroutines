# <p align="center">Proposing engine specific Design Patterns for Unity using Coroutines and the new Data-Oriented Technology Stack</p>

<br>
<br>

### <p align="center">Please Note that this is a work in progress repository!</p>

<br>
<br>

### <div align="center"><span>Project Laboratory - Menyhárt Bence<span><br><span>Consultant - Dr. Magdics Milán<span></div>

<br>
<br>
<br>
<br>

![BME logo](imgs/BME_logo.jpg?raw=true "BME logo")


---

## <p align="center">Summary</p>

<details open>
<summary>[Coroutines](#coroutines)</summary>
    <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>[Coroutines](#coroutines)<br>
    <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>test 2<br>
</details>



<br>
<br>

## <p align="center">Coroutines</p>

#### What are Coroutines?<br>
In general, Coroutines are computer program components that generalize subroutines for non-preemptive(cooperative) multitasking, by allowing execution to be suspended and resumed.<br>

In Unity, Coroutines are a type of functions which can pause execution, save state, then yield controll back to Unitys game loop, so later in time (usually in the next frame) the coroutine can continue execution where it "left off".<br>

#### How they are implemented in Unity?<br>
A good way of implementing coroutines in .Net is by using iterators.<br>
Unity also used this concept when they implemented their own coroutines.<br>

A coroutine yields an `IEnumerator` interface, which will tell to Unitys Coroutine Scheduler when the execution shall continue.
Let's see an example:
````cs
public class CoroutineExample : Monobehaviour
{
    public bool IsReady = false;

    void Start()
    {
        // Coroutines need to be started by Monobehaviour.StartCoroutine() method in order to behave like
        // coroutines otherwise they are just plain methods
        StartCoroutine(ExampleCoroutine());
    }

    void IEnumerator ExampleCoroutine()
    {
        Debug.Log("Starting of ExampleCoroutine...");
        // WaitUntil is one of Unitys built in yield instruction
        // https://docs.unity3d.com/ScriptReference/WaitUntil.html
        yield return new WaitUntil(() => _isReady);
        Debug.Log("The coroutine is ready to continue");
    }
}
````
Let's inspect the above code snippet!<br>


## <p align="center">Coroutines - Custom Yield Instruction Examples</p>

#### 1st Example:
- Showing how to write custom yield instructions like WaitUntil, WaitWhile etc.<br>
- Comparison between IEnumerator interface and Unitys CustomYieldIntructions IEnumerator interface wrapper class.<br>
- Showing how to return a value from a coroutine with using callbacks (another solution would be class scoped variables)
- The importance of caching yield instructions no more GC Spikes!

#### 2nd Example:
- Writing a custom advanced yield instructions using cached transforms

#### 3rd Example:
- Showing how to return a value from a coroutine with using callbacks (another solution would be class scoped variables)

#### 4th Example:
- The importance of caching yield instructions. By caching the yield instructions you can avoid GC Spikes like this
![GC Spike](imgs/GC_spikes_from_uncached_yield_instructions.JPG?raw=true "GC Spike")


## <p align="center">Coroutines - Proposing Design patterns</p>
<br>
<br>

#### Threaded Coroutine

**Description:** 



<br>
<br>
<br>

---

**Sources:<br>**
https://en.wikipedia.org/wiki/Coroutine<br>
https://docs.unity3d.com/
