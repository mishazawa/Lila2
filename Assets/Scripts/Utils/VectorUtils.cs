using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorUtils {
    public static Vector3 RotateVector(Vector3 eulers, Vector3 vec) {
        return Quaternion.Euler(eulers.x, eulers.y, eulers.z) * vec;
    }
}
