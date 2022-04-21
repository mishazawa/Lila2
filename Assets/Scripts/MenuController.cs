using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour {

    public enum Types {
        START,
        REPLAY,
        MOVE,
    };

    public GameObject start  = null;
    public GameObject replay = null;
    public GameObject move   = null;

    public GameObject mute   = null;
    public GameObject unmute = null;


    public List<GameObject> numbers = null;
    public List<GameObject> players = null;
    public MaterialScript mat = null;

    public float duration = .5f;
    public float distance = 2f;
    public float pauseTime = .25f;

    public void Awake () {
        HideAll();
    }


    public void Enable (GameObject go) {
        go.SetActive(true);
    }

    public void Disable (GameObject go) {
        go.SetActive(false);
    }

    public void HideAll () {
        Disable(start);
        Disable(replay);
        Disable(move);

        var muted = FindObjectOfType<AudioManager>().muted;
        if (muted) Disable(mute);
        if (!muted) Disable(unmute);

    }

    public void ShowMenuType (MenuController.Types t) {
        HideAll();
        if (t == Types.START)  Enable(start);
        if (t == Types.REPLAY) Enable(replay);
        if (t == Types.MOVE)   Enable(move);
    }

    public void SetWinner (int ID) {
        mat.setShaderPropertyFloat("_color", ID);
    }

    public IEnumerator AnimateRoll (int roll) {
        var number = numbers[roll-1].transform;

        var startTransform = number.localPosition;

        yield return Misc.DurationFn(duration, (delta) => {
            number.Translate(distance * Vector3.up * Time.deltaTime);
        });

        yield return new WaitForSeconds(pauseTime);

        yield return Misc.DurationFn(duration, (delta) => {
            number.Translate(distance * -Vector3.right * Time.deltaTime);
        });

        number.localPosition = startTransform;
    }

    public void AnimatePlayerConnection (int pn) {
        var pl = players[pn-1].transform;
        pl.localPosition = new Vector3(pl.localPosition.x, 0, pl.localPosition.z);
    }

    public void Mute (bool val) {
        FindObjectOfType<AudioManager>().Mute(val);
    }
}
