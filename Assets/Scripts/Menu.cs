using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour {
    public float duration = 1f;

    void OnMouseExit () {
        StartCoroutine(Misc.DurationFn(duration, (delta) => {
            transform.RotateAround(transform.position, transform.up, Mathf.Lerp(720, 0, delta) * Time.deltaTime);
        }));
    }
}
