using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mice : MonoBehaviour {

    public float rangeStart   = 0f;
    public float rangeEnd     = 1f;
    public float pauseTimeMax = 1f;

    private Animator anim     = null;

    void Start() {
        if (rangeStart > rangeEnd) (rangeStart, rangeEnd) = (rangeEnd, rangeStart);
        
        anim = GetComponent<Animator>();
        StartCoroutine(playAnimation());
    }

    private IEnumerator playAnimation () {
        anim.Play("mice", -1, 1f); // disable first playback

        yield return new WaitForSeconds(Misc.Fit(Random.value, 0, 1, 1, 5));
        yield return mouseLoop();
        yield return playAnimation();
    }

    private IEnumerator mouseLoop () {
        var randPosition = Misc.Fit(Random.value, 0, 1, rangeStart, rangeEnd);
        var randPause    = Misc.Fit(Random.value, 0, 1, 0, pauseTimeMax);

        anim.Play("mice", -1, 0f);

        var ntime = anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
        var clips = anim.GetCurrentAnimatorClipInfo(0);
        var pause = clips[0].clip.length;

        yield return new WaitForSeconds(pause * randPosition);

        anim.enabled = false;

        yield return new WaitForSeconds(randPause);

        anim.enabled = true;

        yield return new WaitForSeconds(pause * 1f - randPosition);
        yield return new WaitForSeconds(randPause * 2);
    }
}
