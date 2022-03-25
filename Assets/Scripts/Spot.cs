using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spot : MonoBehaviour
{
    public bool isSnake  = false;
    public bool isLadder = false;
    public int index, next;

    void Start () {

        var material = GetComponent<Renderer>().material;
        if (isSnake) {
            material.color = Color.red;
        }

        if (isLadder) {
            material.color = Color.green;
        }
    }

}
