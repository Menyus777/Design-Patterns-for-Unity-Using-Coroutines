# <p align="center">Proposing engine specific Design Patterns for Unity using Coroutines and the new Data-Oriented Technology Stack</p>

<br>
<br>

### <p align="center"><mark>Please Note that this is a work in progress repository!</mark></p>

<br>
<br>

### <div align="center"><span>Project Laboratory - Menyhárt Bence<span><br><span>Consultant - Dr. Magdics Milán<span></div>

<br>
<br>
<br>
<br>

![BME logo](imgs/BME_logo.jpg?raw=true "BME logo")


---

# <p align="center">Summary</p>

<dl>
    <details open>
        <summary><a href="#what-are-coroutines"><b>Coroutines</b></a></summary>
        <dd>
            <details open>
                <summary><a href="#about-coroutines"><b>About Coroutines</b></a></summary>
                &emsp; ⬥ <a href="#what-are-coroutines">What are Coroutines?</a><br>
                &emsp; ⬥ <a href="#implementation-of-coroutines-in-unity">Implementation of Coroutines in Unity</a>
            </details>
            <details open>
                <summary><a href="#understanding-yield-instructions"><b>Understanding Yield Instructions</b></a></summary>
                &emsp; ⬥ <a href="#what-are-coroutines">What are Coroutines?</a><br>
                &emsp; ⬥ <a href="#implementation-of-coroutines-in-unity">Implementation of Coroutines in Unity</a>
            </details>
        </dd>
</dl>

<br>

# <p align="center">Coroutines</p>

## <p align="center">About Coroutines</p>

#### What are Coroutines?<br>
In general, Coroutines are computer program components that generalize subroutines for non-preemptive  multitasking<sup>[1]</sup>, by allowing execution to be suspended and resumed.<br>

In Unity, Coroutines are a type of methods which can pause execution, save state, then yield controll back to Unitys game loop, so later in time (usually in the next frame) the coroutine can continue execution where it "left off".<br>

<small><sup>[1]</sup> Cooperative multitasking, also known as non-preemptive multitasking, is a style of computer multitasking in which the operating system never initiates a context switch from a running process to another process. Instead, processes voluntarily yield control periodically or when idle or logically blocked.</small>

#### Implementation of Coroutines in Unity<br>
A good way of implementing coroutines in .Net is by using iterators.<br>
Unity also used this concept when they implemented their own coroutines.<br>

A coroutine yields an `IEnumerator` interface (this is the iterator), which will tell to Unitys Coroutine Scheduler when the execution shall continue.
Let's see an example:
````c#
public class CoroutineExample : Monobehaviour
{
    public bool IsReady = false;

    void Start()
    {
        // Coroutines need to be started by Monobehaviour.StartCoroutine() method in order to behave like
        // coroutines otherwise they are just plain iterator methods
    /*1*/StartCoroutine(ExampleCoroutine());
    }

    void IEnumerator ExampleCoroutine()
    {
    /*2*/Debug.Log("Starting of ExampleCoroutine...");
        // WaitUntil is one of Unitys built in yield instruction
        // https://docs.unity3d.com/ScriptReference/WaitUntil.html
  /*3,4*/yield return new WaitUntil(() => _isReady);
    /*5*/Debug.Log("The coroutine is ready to continue");
/*6*/}
}
````
The above script does the following:<br>
&emsp;**1.** Starts the coroutine<br>
&emsp;**2.** Logs the <i>"Starting of ExampleCoroutine..."</i> string to Unitys console<br>
&emsp;**3.** Yields the execution of the coroutine till the supplied delegate to `WaitUntil`s constructor evaluates to true<br>
&emsp;**4.** After the delegate evaluates to true aka the `IsReady` variable becomes true the coroutines execution will continue<br>
&emsp;**5.** Logs the <i>"The coroutine is ready to continue"</i> string to Unitys console<br>
&emsp;**6.** The execution of the coroutine finishes.<br>

Now let's inspect the above code snippet a bit more in depth!<br>
&emsp;**1.** MonoBehaviours `StartCoroutine()` method registers the coroutine into Unitys coroutine scheduler. After that the scheduler will periodically call (basically in every frame) IEnumerators `public bool MoveNext()` method.<sup>[2]</sup><br>


<strong><i><sub><sup>[2]</sup> Note, that not all built-in Yield Instructions are implementing the `IEnumerator` interface (E.g.: `WaitForSeconds`, `WaitForEndOfFrame etc..`), some of them are pointing into Unitys Native code and we have no informations about their internal mechanism. Although they are still reacting correctly when you are calling `MoveNext()` on their yielded result.<br>
E.g.: Let's inspect `WaitForEndOfFrame`</sup></i></strong>


## <p align="center">Understanding Yield Instructions</p>

#### Writing Yield Instructions:
- Showing how to write custom yield instructions like WaitUntil, WaitWhile etc.<br>
- Comparison between IEnumerator interface and Unitys CustomYieldIntructions IEnumerator interface wrapper class.<br>
- Showing how to return a value from a coroutine with using callbacks (another solution would be class scoped variables)

#### Writing an advanced Yield Instruction:
- Writing a custom advanced yield instructions using cached transforms

#### The importance of caching Yield Instructions:
- The importance of caching yield instructions. Say not to GC Spikes like this!<br>

![GC Spike](imgs/GC_spikes_from_uncached_yield_instructions.JPG?raw=true "GC Spike")


#### Catching the return value of a Coroutine:
- Showing how to return a value from a coroutine with using callbacks (another solution would be class scoped variables)

<br>

## <p align="center">Proposing Design patterns</p>
<br>
<br>

#### Threaded Coroutine

**Description:** 



<br>
<br>
<br>

---

**Sources:<br>**
https://docs.unity3d.com/<br>
https://en.wikipedia.org/wiki/Coroutine<small>[1]</small><br>
https://en.wikipedia.org/wiki/Cooperative_multitasking<small>[1]</small><br>
