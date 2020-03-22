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

# <p align="center">Summary</p>

<dl>
    <h2><a href="#coroutines-1">Coroutines</a></h2>
    <dd>
        <details open>
            <summary><a href="#about-coroutines"><b>About Coroutines</b></a></summary>
            &emsp; ⬥ <a href="#what-are-coroutines">What are Coroutines?</a><br>
            &emsp; ⬥ <a href="#implementation-of-coroutines-in-unity">Implementation of Coroutines in Unity</a><br>
        </details>
        <details open>
            <summary><a href="#understanding-yield-instructions"><b>Understanding Yield Instructions</b></a></summary>
            &emsp; ⬥ <a href="#execution-pipeline-of-yield-instructions">Execution pipeline of Yield Instructions</a><br>
            &emsp; ⬥ <a href="#implementation-of-coroutines-in-unity">Implementation of Coroutines in Unity</a><br>
        </details>
        <details open>
            <summary><a href="#design-patterns-using-coroutines"><b>Design patterns using Coroutines</b></a></summary>
            &emsp; ⬥ <a href="#threaded-coroutine">Threaded Coroutine</a><br>
        </details>
    </dd>
</dl>

<br>

# <p align="center">Coroutines</p>

## <p align="center">About Coroutines</p>

#### What are Coroutines?<br>
In general, Coroutines are computer program components that generalize subroutines for non-preemptive  multitasking<sup>[1]</sup>, by allowing execution to be suspended and resumed.<br>

In Unity, Coroutines are a type of methods which can pause execution, save state, then yield controll back to Unitys game loop, so later in time (usually in the next frame) the coroutine can continue execution where it "left off".<br>

<sub><sup>[1]</sup> Cooperative multitasking, also known as non-preemptive multitasking, is a style of computer multitasking in which the operating system never initiates a context switch from a running process to another process. Instead, processes voluntarily yield control periodically or when idle or logically blocked.</sub>

#### Implementation of Coroutines in Unity<br>
A good way of implementing coroutines in .Net is by using iterators.<br>
Unity also used this concept when they implemented their own coroutines.<br>

A coroutine yields an `IEnumerator` interface (this is the iterator), which will tell to Unitys Coroutine Scheduler when the execution shall continue.
Let's see an example:
```c#
public class CoroutineExample : Monobehaviour
{
    public bool IsReady = false;

    void Start()
    {
        // Coroutines need to be started by Monobehaviour.StartCoroutine() method in order to behave like
        // coroutines otherwise they are just plain iterator methods
        StartCoroutine(ExampleCoroutine());/*1*/
    }

    void IEnumerator ExampleCoroutine()
    {
        Debug.Log("Starting of ExampleCoroutine...");/*2*/
        // WaitUntil is one of Unitys built in yield instruction
        // https://docs.unity3d.com/ScriptReference/WaitUntil.html
        yield return new WaitUntil(() => _isReady);/*3,4*/
        Debug.Log("The coroutine is ready to continue");/*5*/
    }/*6*/
}
```
The above script does the following:<br>
&emsp;**1.** Starts the coroutine<br>
&emsp;**2.** Logs the <i>"Starting of ExampleCoroutine..."</i> string to Unitys console<br>
&emsp;**3.** Yields the execution of the coroutine till the supplied delegate to `WaitUntil`s constructor evaluates to true<br>
&emsp;**4.** After the delegate evaluates to true aka the `IsReady` variable becomes true the coroutines execution will continue<br>
&emsp;**5.** Logs the <i>"The coroutine is ready to continue"</i> string to Unitys console<br>
&emsp;**6.** The execution of the coroutine finishes.<br>

Now let's inspect the above code snippet a bit more in depth!

&emsp;**1.** MonoBehaviours `StartCoroutine()` method registers the coroutine into Unitys coroutine scheduler. After that the scheduler will periodically ask (basically in every frame)
the yield instruction if it is done with waiting or not.<br>
<sub><i>(Assuming you yielded on a YieldInstruction, not just returned a value or let the coroutine finish in that frame.</sub><br>
<sup>Also a simple `yield return <object>;` means you yielded the execution for one frame.)</i>.</sup><br>

There are two types of Yield Instructions in Unity:

&emsp; <i>**i**</i>, &nbsp;Yield Instructions that implements the interface `IEnumerator`<sup>[2]</sup><br>
&emsp; <i>**ii**</i>, Yield Instructions that inherits from the class named `YieldInstruction`<sup>[3]</sup>

We will only speak about the first type of Yield Instructions since we don't have insight into the second type. You can learn how to write your own Yield Instruction in the [Writing Yield Instructions](#writing-yield-instructions) section.

When you start your iterator method aka your coroutine with `StartCoroutine(myCoroutine())` Unity will start executing it immediately and continues execution till the first `yield` statement, unlike when you just simply call an iterator method like this
```c#
IEnumerator myEnumerator = myCoroutine();
```
in the above case the execution of the method won't start with the `()` operator, you need to call `myEnumerator.MoveNext()` to start the execution of the iterator method till the first `yield` statement.

So a C# equivalent of `StartCoroutine()` would be something like this
```c#
IEnumerator myEnumerator = myCoroutine();
myEnumerator.MoveNext();
```
 Its easy to see now that what the Coroutine Scheduler does is just simply calling the `bool IEnumerator.Movenext()` method. So a Yield Instructions `MoveNext()` method can be translated to `Should_I_Still_Be_Suspended()` where true means yes, you **shall not** proceed to the next `yield` statement please yield control back to Unity and false means no please proceed and yield me the control back at the next `yield` statement or at the end of the method.

&emsp;**3.** Yielding back happens here, with a Yield Instruction called `WaitUntil`. It's important to note here that yielding back the execution is not a blocking operation, and also the execution of the coroutines happens on the Main thread.<br>
Let's inspect `WaitUntil` implementation<sup>[4]</sup>:
```C#
// Unity C# reference source
// Copyright (c) Unity Technologies. For terms of use, see
// https://unity3d.com/legal/licenses/Unity_Reference_Only_License

using System;

namespace UnityEngine
{
    public sealed class WaitUntil : CustomYieldInstruction
    {
        Func<bool> m_Predicate;

        public override bool keepWaiting { get { return !m_Predicate(); } }

        public WaitUntil(Func<bool> predicate) { m_Predicate = predicate; }
    }
}
```

Looks like there is no sign of `MoveNext()` method, but in fact `WaitUntil`s parent class `CustomYieldInstruction` implements the `IEnumerator` interface and this is how its `MoveNext()` method looks like:<br>
```C#
public bool MoveNext() { return keepWaiting; }
```
It looks like it just passes the keepWaiting property. So in our case, the coroutine will be suspended till the `IsReady` Property evaluates to true.

Note, that `keepWaiting` is returning `!m_Predicate()` rather then just `!m_Predicate()` because as we talked about this earlier `IEnumerator.MoveNext()` in this context means basically `Should_I_Still_Be_Suspended()` (or according to Unity `keepWaiting`) so just returning our `IsReady` boolean default false value would let the coroutine proceed with it's execution in the next frame rather then waiting for it to become true.

<details open>
    <summary><i><sub><sup>[2]</sup> There is an abstract class named `CustomYieldInstruction` which basically just implements the IEnumerator interface so you can derive from it and make a custom yield instruction. Because of this class also introduces a sometimes useless bool, and prevents inheriting another class we will rather just simply implement the `IEnumerator` interface. (Click on the arrow to view the class implementation [4])</sub></i></summary>

```c#
// Unity C# reference source
// Copyright (c) Unity Technologies. For terms of use, see
// https://unity3d.com/legal/licenses/Unity_Reference_Only_License

using System.Collections;

namespace UnityEngine
{
    public abstract class CustomYieldInstruction : IEnumerator
    {
        public abstract bool keepWaiting
        {
            get;
        }

        public object Current
        {
            get
            {
                return null;
            }
        }
        public bool MoveNext() { return keepWaiting; }
        public virtual void Reset() {}
    }
}
}
```
</details>
<br>
<i><sub><sup>[3]</sup> As of 2020 there are only three that inherits from `YieldInstruction`, namely `WaitForSeconds`, `WaitForEndOfFrame` and `WaitForFixedUpdate`. All of these are pointing into Unitys Native code and we have no informations about their internal mechanism.</sub></i>
<br>
<br>
<i><sub><sup>[4]</sup> Unitys C# code is Open Source since 2019 you can inspect it here: <a href="https://github.com/Unity-Technologies/UnityCsReference"> https://github.com/Unity-Technologies/UnityCsReference</a></sub></i>


## <p align="center">Understanding Yield Instructions</p>

#### Execution pipeline of Yield Instructions

Before diving into deeper to Yield Instructions let's see when the evaluation of the different yield instructions happens. The following picture from the Unity Manual will help us understand it<sup>[5]</sup>:<br>

![GC Spike](imgs/Yield-execution-pipeline.jpg?raw=true "GC Spike")
As you can see coroutines are executed after all Fixed and normal Updates haven taken place, unless you used a `WaitForFixedUpdate` Yield instruction. There is a built-in Yield Instruction that is cropped from the image namely the `WaitForEndOfFrame` which happens after Unity has renderd every Camera and GUI and just before the displaying of the current frame.

As you can see it is guaranteed that our coroutines will resume execution after all Updates have been finished, but it's important to note that we have no guarantee that the order of execution of our coroutines will stay the same on the cycle of our application. There are improtant to note things before diving into writing custom yield instructions or designing architectures.

<sub><sup>[5]</sup> You can watch the uncropped picture here: <a href="https://docs.unity3d.com/Manual/ExecutionOrder.html"> https://docs.unity3d.com/Manual/ExecutionOrder.html</a></sub>

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

## <p align="center">Design patterns using Coroutines</p>

## <p align="center">Threaded Coroutine</p>

**Description:** 



<br>
<br>
<br>

---

**Sources:<br>**
https://docs.unity3d.com/<sup></sup><br>
https://en.wikipedia.org/wiki/Coroutine<sup>[1]</sup><br>
https://en.wikipedia.org/wiki/Cooperative_multitasking<sup>[1]</sup><br>
