using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoudiniEngineUnity;


public class PlayerSpot {
    public bool isSnake  = false;
    public bool isLadder = false;
    public int index, next;
    public Vector3 position;
    public GameObject go, hint;
}

public class PlayField : MonoBehaviour {
    private HEU_OutputAttributesStore attrStore () {
        return gameObject.GetComponent<HEU_OutputAttributesStore>();
    }

    public PlayerSpot[] BuildGrid () {
        var store = attrStore();
        var spot = store.GetAttribute("spot");
        var pos = store.GetAttribute("position");

        var tiles = new PlayerSpot[spot._count];
        Debug.Log(spot._count);
        var positions = HoudiniData.ParseVector(pos);

        for (int i = 0; i < positions.Length; i++) {
            var spotData = new PlayerSpot();
            spotData.position = VectorUtils.RotateVector(transform.parent.transform.eulerAngles, positions[i]);
            spotData.index = i;
            spotData.next  = i + 1;
            tiles[i] = spotData;
        }

        var paths = Constants./*DEBUG_*/PATHS;
        for (int i = 0; i < paths.GetLength(0); i++) {
          int start = paths[i, 0];
          int end   = paths[i, 1];

          var spotData = tiles[start - 1];

          spotData.next = end - 1;
          spotData.isSnake = start > end;
          spotData.isLadder = start < end;
        }
        return tiles;
    }

}
