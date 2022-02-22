using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile {
	public bool snake, ladder;
	public int index, next, direction;
	public Vector3 coords;
}

public class GridGeneration : MonoBehaviour {
	public GameObject tile;

	GameState gameState;
	
	static int TILE_SIZE = 1;
	static int GRID_SIZE = 10;
	int rows = GRID_SIZE;
	int cols = GRID_SIZE;

    Tile[] tiles;

    public void Create () {
	    tiles = new Tile[rows * cols];
		int direction = 1;
	    int x = 0;
	    int y = (rows - 1) * TILE_SIZE;
    	
    	for (int i = 0; i < rows * cols; i++) {
    		var tile = new Tile();
    		tile.index = i;
    		tile.next = i+1;
    		tile.direction = 1;
    		tile.coords = new Vector3(x, 0, y);

			x += (TILE_SIZE * direction);

			if (x >= GRID_SIZE || x <= -TILE_SIZE) {
				direction *= -1;
				x += TILE_SIZE * direction;
				y -= TILE_SIZE;
			}

    		tiles[i] = tile;
    	}
    }

    public void Instantiate () {
    	foreach (Tile t in tiles) {
			var tobj = Instantiate(tile, t.coords, Quaternion.identity);
    		var tmesh = tobj.GetComponent<TextMesh>();
    		tmesh.text = "" + t.index;
    	}
    }

    public Tile GetTileByIndex(int i) {
    	return tiles[i];
    }

    public void LinkWorld (GameState gs) {
    	this.gameState = gs;
    }

}
