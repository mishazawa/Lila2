using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class MoveCamera : MonoBehaviour
{
    public float duration = .5f;
    public float amp = .0005f;
    public Vector3 offset = new Vector3(-10f, 9f, -10f);

    public Volume cameraEffectVolume;
    public float minDOF = 20f;
    public float maxDOF = 4f;
    public float blurDuration = .5f;

    public float minZoom = 4f;
    public float maxZoom = 9f;

    private DepthOfField dof;

    void Start() {
      cameraEffectVolume.sharedProfile.TryGet<DepthOfField>(out dof);
      dof.focusDistance.value = minDOF;
    }

    public IEnumerator SetCamera (Vector3 pos, bool useLerp = true) {
        var sp = transform.position;
        var ep = offset + new Vector3(pos.x, pos.y, pos.z);

        if (useLerp) {
            yield return Misc.DurationFn(duration, (delta) => {
                transform.position = Vector3.Lerp(sp, ep, delta);
            });
        }
        transform.position = ep;
    }

    public IEnumerator BlurCamera (float targetDof) {
        var val = dof.focusDistance.value;
        return Misc.DurationFn(blurDuration, (delta) => {
            dof.focusDistance.value = Mathf.Lerp(val, targetDof, delta);
        });
    }

    public IEnumerator Zoom (float targetZoom) {
        yield return null;
        Camera.main.orthographicSize = targetZoom;
    }
}
