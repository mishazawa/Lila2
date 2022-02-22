using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	GameState gameState;

	public int ID;
	int spot = 0;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LinkWorld (GameState gs) {
    	this.gameState = gs;
    }

    public bool Move (int steps) {
    	bool finished = false;
    	spot += steps;

    	if (spot >= 99) {
    		spot = 99;
    		finished = true;
    	}

    	var tile = gameState.grid.GetTileByIndex(spot);
    	var me = GetComponent<Transform>();
    	me.position = new Vector3(tile.coords.x, me.position.y, tile.coords.z);
    	return finished;
    }
}
