using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoudiniEngineUnity;

public class TestPlayer : MonoBehaviour
{
    public GameObject hda_level;
    public GameObject spotPrefab;
    public float distanceThreshold = 0.25f;
    public float duration = 2f;

    int counter = 0;

    Animator anim;
    Coroutine rotating;
    Coroutine moving;


    GameObject[] tiles;

    void Start() {
    }

    void Update() {
        if (Input.GetKeyUp(KeyCode.Q)) {
            counter++;
            StartCoroutine(Move());
            // if (rotating != null) {
            //     StopCoroutine(rotating);
            // }
            // if (moving != null) {
            //     StopCoroutine(moving);
            // }
            // rotating = StartCoroutine(Rotating());
            // moving = StartCoroutine(Running());
        }
    }
    private IEnumerator Move() {
        var spot = tiles[counter % tiles.Length];

        gameObject.transform.position = spot.transform.position;
        yield return null;
    }
    // private IEnumerator Rotating() {
    //     var destination = destinations[counter];
    //     float time = 0;
    //     var angle = Quaternion.LookRotation(destination.transform.position - transform.position, Vector3.up);
    //     while (time < duration) {
    //         transform.rotation = Quaternion.Lerp(transform.rotation, angle, time / duration);
    //         time += Time.deltaTime;
    //         yield return null;
    //     }
    // }

    // private IEnumerator Running() {
    //     var destination = destinations[counter];
    //     anim.SetFloat("Speed", 1);
    //     var distance = Vector3.Distance(transform.position, destination.transform.position);
    //     while (distance > distanceThreshold) {
    //         distance = Vector3.Distance(transform.position, destination.transform.position);
    //         yield return null;
    //     }
    //     anim.SetFloat("Speed", 0);
    //     counter += 1;
    // }
}
