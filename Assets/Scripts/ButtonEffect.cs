using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonEffect : MonoBehaviour {

    public float duration = .2f;
    public float scaleOnHover = 1.1f;
    public UnityEvent onClick;

    void OnMouseEnter () {
        Hover(Vector3.one, Vector3.one * scaleOnHover);
    }

    void OnMouseExit () {
        Hover(Vector3.one * scaleOnHover, Vector3.one);
    }

    void OnMouseUpAsButton() {
        onClick.Invoke();
    }

    void Hover (Vector3 a, Vector3 b) {
        StartCoroutine(Misc.DurationFn(duration, (delta) => transform.localScale = Vector3.Lerp(a, b, delta)));
    }
}
