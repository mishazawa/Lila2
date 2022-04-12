using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mice : MonoBehaviour {

    public float rangeStart   = 0f;
    public float rangeEnd     = 1f;
    public float pauseTimeMax = 1f;

    private Animator  anim     = null;
    private Coroutine playback = null;

    void Start() {
        if (rangeStart > rangeEnd) (rangeStart, rangeEnd) = (rangeEnd, rangeStart);
        
        anim = GetComponent<Animator>();
        playback = StartCoroutine(playAnimation());
    }

    public IEnumerator playToEnd () {
        var continueTime = anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
        anim.Play("mice", -1, 1f);

        // disable mouse loop
        StopCoroutine(playback);

        // speedup animation
        anim.enabled = true;
        anim.speed = 5f;
        anim.Play("mice", -1, continueTime);

        // wait until it ends
        yield return new WaitForSeconds(getDuration());

        // slowdown animation
        anim.speed = 1f;

        // rerun mouse loop
        playback = StartCoroutine(playAnimation());
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

        var duration = getDuration();

        yield return new WaitForSeconds(duration * randPosition);

        anim.enabled = false;

        yield return new WaitForSeconds(randPause);

        anim.enabled = true;

        yield return new WaitForSeconds(duration * 1f - randPosition);
        yield return new WaitForSeconds(randPause * 2);
    }

    private float getDuration() {
        // var ntime    = anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
        var clips    = anim.GetCurrentAnimatorClipInfo(0);
        return clips[0].clip.length;
    }
}
