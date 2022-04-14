using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverTip : MonoBehaviour {

    public Color snakeColor;
    public Color ladderColor;

    private GameObject level;
    private Vector3 dest;
    private bool isSnake;

    public void Init(GameObject l, Vector3 v, bool s) {
        level = l;
        dest = v;
        isSnake = s;
    }

    void OnMouseEnter() {
        level.GetComponent<Renderer>().material.SetVector("_impactPoint", dest);
        level.GetComponent<Renderer>().material.SetVector("_hintColor", isSnake ? snakeColor : ladderColor);
    }

    void OnMouseExit() {
        level.GetComponent<Renderer>().material.SetVector("_impactPoint", Vector3.zero);
    }
}
