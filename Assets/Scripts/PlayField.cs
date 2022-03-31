using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoudiniEngineUnity;


public class PlayerSpot {
    public bool isSnake  = false;
    public bool isLadder = false;
    public int index, next;
    public Vector3 position;
}

[ExecuteInEditMode]
public class PlayField : MonoBehaviour
{
    private GameObject World = null;

    // houdini
    private bool metadata = false;
    private HEU_OutputAttributesStore attrStore;

    void Awake() {
        World = GameObject.Find("/World");
        if (World == null) return;

        attrStore = gameObject.GetComponent<HEU_OutputAttributesStore>();
        if (attrStore == null) return;

        var mdata = attrStore.GetAttribute("metadata");
        metadata = mdata._intValues[0] == 1;
        if (!metadata) return;
        HEU_Logger.LogFormat("Store loaded");

        BuildGrid();
    }

    public void BuildGrid () {
        HEU_Logger.LogFormat("Building Grid");

        var spot = attrStore.GetAttribute("spot");
        var pos = attrStore.GetAttribute("position");

        PlayerSpot[] tiles = new PlayerSpot[spot._count];

        var p = HoudiniData.ParseVector(pos);

        for (int i = 0; i < p.Length; i++) {
            var spotData = new PlayerSpot();
            spotData.position = VectorUtils.RotateVector(transform.parent.transform.eulerAngles, p[i]);
            spotData.index = i;
            spotData.next  = i + 1;
            tiles[i] = spotData;
        }

        World.GetComponent<State>().InitTiles(tiles);
    }
}
