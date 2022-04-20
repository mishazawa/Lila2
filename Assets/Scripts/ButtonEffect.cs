using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonEffect : MonoBehaviour {

    public float duration     = .2f;
    public float scaleOnHover = 1.1f;
    public UnityEvent onClick = null;
    public MouseDirection mdh   = null;

    private float amp         = .25f;
    private Coroutine unblock = null;

    void OnMouseEnter () {
        Hover(Vector3.one, Vector3.one * scaleOnHover);
        if (unblock != null) return;
        unblock = StartCoroutine(wobble());
    }

    void OnMouseExit () {
        Hover(Vector3.one * scaleOnHover, Vector3.one);
    }

    void OnMouseUpAsButton() {
        Hover(Vector3.one * scaleOnHover, Vector3.one);
        onClick.Invoke();
        Hover(Vector3.one, Vector3.one * scaleOnHover);
    }

    void Hover (Vector3 a, Vector3 b) {
        if (gameObject.activeInHierarchy) {
            StartCoroutine(Misc.DurationFn(duration, (delta) => transform.localScale = Vector3.Lerp(a, b, delta)));
            return;
        }

        transform.localScale = b;
    }

    IEnumerator wobble () {
        var initRotation = transform.localRotation;
        var dir = mdh.GetDirection();

        // turn
        yield return Misc.DurationFn(duration, (delta) => {
            var vx = Mathf.Lerp(0, dir.x, delta);
            var vy = Mathf.Lerp(0, dir.y, delta);
            transform.RotateAround(transform.position, transform.up, amp * Mathf.Sin(vx));
            transform.RotateAround(transform.position, transform.right, amp * Mathf.Sin(vy));
        });

        // unturn
        yield return Misc.DurationFn(duration, (delta) => {
            var vx = Mathf.Lerp(0, -dir.x, delta);
            var vy = Mathf.Lerp(0, -dir.y, delta);
            transform.RotateAround(transform.position, transform.up, amp * Mathf.Sin(vx));
            transform.RotateAround(transform.position, transform.right, amp * Mathf.Sin(vy));
        });

        transform.localRotation = initRotation;
        unblock = null;
    }
}
