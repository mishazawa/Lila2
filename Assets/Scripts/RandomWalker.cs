using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomWalker : MonoBehaviour
{
    Vector3 min = Vector3.zero;
    Vector3 max = Vector3.zero;
    Vector3 scale = Vector3.zero;

    void Start()
    {
        var z     = GameObject.Find("WalkZone");
        Mesh mesh = z.GetComponent<MeshFilter>().mesh;
        scale     = z.transform.localScale;
        min       = mesh.bounds.min;
        max       = mesh.bounds.max;

        StartCoroutine(rotate());
    }

    IEnumerator rotate () {
        var dest = genRandPoint();
        // Vector3.RotateTowards
        var time = 0f;
        while (time < 1f) {
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, dest, time, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDirection);
            time += Time.deltaTime;
            yield return null;
        }
        // Vector3.MoveTowards
        time = 0f;
        while (time < 1f) {
            transform.position = Vector3.MoveTowards(transform.position, dest, time);
            time += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(1);
        yield return rotate();
    }

    Vector3 genRandPoint() {
        var x = Misc.Fit(Random.value, 0, 1, min.x, max.x) * scale.x;
        var y = Misc.Fit(Random.value, 0, 1, min.y, max.y) * scale.y;
        var z = Misc.Fit(Random.value, 0, 1, min.z, max.z) * scale.z;
        return transform.position + new Vector3(x, y, z);
    }
}
