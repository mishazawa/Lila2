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

        var positions = HoudiniData.ParseVector(pos);

        for (int i = 0; i < positions.Length; i++) {
            var spotData = new PlayerSpot();
            spotData.position = VectorUtils.RotateVector(transform.parent.transform.eulerAngles, positions[i]);
            spotData.index = i;
            spotData.next  = i + 1;
            tiles[i] = spotData;
        }

        var paths = Constants.DEBUG_PATHS;
        for (int i = 0; i < paths.GetLength(0); i++) {
          int start = paths[i, 0];
          int end   = paths[i, 1];

          var spotData = tiles[start - 1];

          spotData.next = end - 1;
          spotData.isSnake = start > end;
          spotData.isLadder = start < end;
        }

        World.GetComponent<State>().InitTiles(tiles);
    }
}
