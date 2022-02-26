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

  public static int[][] PATHS = new int[][]
  {
    new int[]{8,   26},
    new int[]{21,  82},
    new int[]{43,  77},
    new int[]{50,  91},
    new int[]{54,  93},
    new int[]{62,  96},
    new int[]{66,  87},
    new int[]{80, 100},
    // snakes
    new int[]{98, 28},
    new int[]{95, 24},
    new int[]{92, 51},
    new int[]{83, 19},
    new int[]{73,  1},
    new int[]{69,  9},
    new int[]{64, 36},
    new int[]{59, 17},
    new int[]{55,  7},
    new int[]{52, 11},
    new int[]{48,  9},
    new int[]{46,  5},
    new int[]{44, 22},
  };

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

      foreach (int[] p in PATHS) {
        int spot = p[0];
        int dest = p[1];
        var t = tiles[spot - 1];

        t.next = dest - 1;
        t.snake = spot > dest;
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
