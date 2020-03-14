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

![Alt text](imgs/BME_logo.jpg?raw=true "Bme logo")


<br>
<br>

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
- The importance of caching yield instructions no more yield return new WaitForSeconds(1.0f); and GC Spikes!
