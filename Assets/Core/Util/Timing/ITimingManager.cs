using System;
using UnityEngine;

namespace Core.Util.Timing
{
    public interface ITimingManager
    {
        Coroutine SetInterval(float interval, Action onFinished);
        Coroutine SetInterval(float interval, int loops, Action onLoop,Action onFinished = null);
    }
}