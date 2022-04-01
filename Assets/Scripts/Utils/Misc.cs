using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Misc {
    public static bool IsValid(float a) {
        return !Single.IsNaN(a) && !Single.IsInfinity(a);
    }
}
