using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class SmokePlay : MonoBehaviour
{
    public VisualEffect vfx;
    private float asd = 1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.A)) {
            vfx.SetFloat("ptnum", asd++);
            vfx.SendEvent("OnPlay");
        }
    }
}
