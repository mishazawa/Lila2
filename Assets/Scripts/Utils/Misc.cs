using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Misc {
    public static bool IsValid(float a) {
        return !Single.IsNaN(a) && !Single.IsInfinity(a);
    }

    public static float Fit (float val, float min, float max, float a, float b) {
        return (b - a) * (val - min) / (max - min) + a;
    }

    public static IEnumerator DurationFn(float duration, Action<float> fn) {
        var time = 0f;
        while (time < duration) {
            fn(time/duration);
            time += Time.deltaTime;
            yield return null;
        }
    }

    public static IEnumerator Delay(float duration, Action fn) {
        yield return new WaitForSeconds(duration);
        fn();
    }
}
