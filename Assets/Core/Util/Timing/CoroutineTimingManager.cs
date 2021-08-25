using System;
using System.Collections;
using UnityEngine;

namespace Core.Util.Timing
{
    public class CoroutineTimingManager : MonoBehaviour, ITimingManager
    {
        public Coroutine SetInterval(float interval,int loops,Action onLoop,Action onFinished = null)
        {
            var intervalCoroutine = StartCoroutine(LoopIntervalCoroutine(interval,loops,onLoop,onFinished));
            return intervalCoroutine;
        }
        public Coroutine SetInterval(float interval,Action onFinished)
        {
            var intervalCoroutine = StartCoroutine(IntervalCoroutine(interval, onFinished));
            return intervalCoroutine;
        }

        private IEnumerator IntervalCoroutine(float interval, Action onFinished)
        {
            yield return new WaitForSeconds(interval);
            onFinished?.Invoke();
        }

        private IEnumerator LoopIntervalCoroutine(float interval,int loopCount, Action onLoop, Action onFinished)
        {
            var isLoopingAlways = loopCount == -1;
            var counter = 0;
            
            while (isLoopingAlways || counter < loopCount)
            {
                yield return IntervalCoroutine(interval, onLoop);
                counter++;
            }
            
            onFinished?.Invoke();
        }
    }
}