using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdAnimationRandom : MonoBehaviour
{
    public float minPause = 1f;
    public float maxPause = 3f;
    public float transitionTime = 1f;

    Animator anim;
    bool val = false;

    void Start() {
        anim = GetComponent<Animator>();
        StartCoroutine(animate());
    }

    IEnumerator animate() {
        val = !val;
        anim.SetFloat("Walking", val ? 1 : 0);
        yield return new WaitForSeconds(Misc.Fit(Random.value, 0, 1, minPause, maxPause));
        yield return animate();
    }
}
