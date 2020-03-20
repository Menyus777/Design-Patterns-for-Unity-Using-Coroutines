# <p align="center">Proposing engine specific Design Patterns for Unity using Coroutines and the new Data-Oriented Technology Stack</p>

<br>
<br>
<br>
<br>

### <div align="center"><span>Project Laboratory - Menyhárt Bence<span><br><span>Consultant - Dr. Magdics Milán<span></div>

<br>
<br>
<br>
<br>

![BME logo](imgs/BME_logo.jpg?raw=true "BME logo")


---

<br>
<br>

## <p align="center">Coroutines - Custom Yield Instruction Examples</p>

### Coroutines:

**What are Coroutines?**<br>
In general, Coroutines are computer program components that generalize subroutines for non-preemptive(cooperative) multitasking, by allowing execution to be suspended and resumed.<br>

In Unity, Coroutines are a type of functions which can pause execution, save state, then yield controll back to Unitys game loop, so later in time (usually in the next frame) the coroutine can continue execution where it "left off".<br>

**How they are implemented in Unity?**<br>
A good way of implementing coroutines in .Net is by using iterators. Unity also used this concept when they implemented they coroutines.<br>

A coroutine `yield return` an `IEnumerator` interface, which will tell to Unitys Coroutine Scheduler when should the execution continue.
Let's see an example:
````cs
void IEnumerator()
{
    Debug.Log("Starting of coroutine...");
    yield return 
    Debug.Log("Not yet finished example...");
}
````


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


## <p align="center">Coroutines - Design patterns</p>
<br>
<br>

#### Threaded Coroutine

**Description:** 



<br>
<br>
<br>
---
**Sources:<br>**
https://en.wikipedia.org/wiki/Coroutine