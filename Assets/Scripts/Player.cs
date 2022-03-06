using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    GameState gameState;

    public int ID;
    int spot = -1;
    int targetSteps = 0;

    bool moving = false;

    void Update() {
      if (moving) {
        Move();
      }
    }

    public void LinkWorld (GameState gs) {
    	this.gameState = gs;
    }

    public bool Move () {
    	bool finished = false;
    	spot += 1;

    	if (spot >= 99) {
    		spot = 99;
    		finished = true;
    	}

    	var tile = gameState.grid.GetTileByIndex(spot);
    	var me = GetComponent<Transform>();
    	me.position = new Vector3(tile.coords.x, me.position.y, tile.coords.z);

      moving = targetSteps > 0;
      targetSteps -= 1;

      return finished;
    }

    public void SetSpot (int steps) {
      targetSteps = steps;
      moving = true;
    }
}
