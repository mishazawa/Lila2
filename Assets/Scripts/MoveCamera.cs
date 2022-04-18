using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public float duration = .5f;
    public float amp = .0005f;
    public Vector3 offset = new Vector3(-10f, 9f, -10f);

    // Start is called before the first frame update
    void Start()
    {
        // StartCoroutine(IdleCamera());
    }


    void Update () {
        // idle anim

    }

    private IEnumerator IdleCamera () {
        while (true) {
            var v = Mathf.Sin(Time.time * Mathf.PI);
            transform.position += Vector3.up * v * amp;
            yield return null;
        }
        // yield return IdleCamera();
    }

    public IEnumerator SetCamera (Vector3 pos, bool useLerp = true) {
        var sp = transform.position;
        var ep = offset + new Vector3(pos.x, pos.y, pos.z);

        if (useLerp) {
            var time = 0f;
            while (time < duration) {
                var f = time / duration;
                transform.position = Vector3.Lerp(sp, ep, f);
                time += Time.deltaTime;
                yield return null;
            }
        }
        transform.position = ep;
    }
}
