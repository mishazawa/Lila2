using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MaterialScript : MonoBehaviour
{
    public Material defaultMaterial;
    public bool useDefault = true;

    void Awake() {
      if (!useDefault) return;
      modifyShader(renderer => {
          if (renderer.name == "cursor") return; // xex sorry)
          renderer.material = defaultMaterial;
      });
    }

    public void setShaderProgress (float val) {
      modifyShader(renderer => renderer.material.SetFloat("_progress", val));
    }

    public void setShaderPropertyFloat (string prop, float val) {
      modifyShader(renderer => renderer.material.SetFloat(prop, val));
    }

    /* todo: use adequate model */
    private void modifyShader (Action<Renderer> fn) {
      var renderers = GetComponentsInChildren<Renderer>();
      foreach(var r in renderers) {
        fn(r);
      }
    }
}
