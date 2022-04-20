using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseDirection : MonoBehaviour {
    private int mx, my;

    private Vector3 mousePrevious = Vector3.zero;
    private Vector3 mouseCurrent  = Vector3.zero;

    void FixedUpdate() {
        mousePrevious = mouseCurrent;
        mouseCurrent  = Input.mousePosition;
    }

    public Vector3 GetDirection () {
        var v = mouseCurrent - mousePrevious;
        return new Vector3(Mathf.Sign(v.x), Mathf.Sign(v.y), 0);
    }
}
