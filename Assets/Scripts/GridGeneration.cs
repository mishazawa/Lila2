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
  public GameObject spotPrefab;

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

  GameObject[] tiles;

  public void Create () {
    tiles = new GameObject[rows * cols];

    int direction = 1;
    int x = 0;
    int y = (rows - 1) * TILE_SIZE;

    for (int i = 0; i < rows * cols; i++) {
      var spotObject = Instantiate(spotPrefab, gameObject.transform);
      var spotData = spotObject.GetComponent<Spot>();

      spotData.index = i;
      spotData.next  = i + 1;

      spotObject.transform.localPosition = tileOffset(x, y);

      x += (TILE_SIZE * direction);

      if (x >= GRID_SIZE || x <= -TILE_SIZE) {
        direction *= -1;
        x += TILE_SIZE * direction;
        y -= TILE_SIZE;
      }

      tiles[i] = spotObject;
    }

    foreach (int[] p in PATHS) {
      int spot = p[0];
      int dest = p[1];

      var spotObject = tiles[spot - 1];
      var spotData = spotObject.GetComponent<Spot>();


      spotData.next = dest - 1;
      spotData.isSnake = spot > dest;
      spotData.isLadder = spot < dest;
    }
  }

  public GameObject GetTileByIndex(int i) {
    return tiles[i];
  }
  public Spot GetTileDataByIndex(int i) {
    var spotData = tiles[i].GetComponent<Spot>();

    return spotData;
  }

  public void LinkWorld (GameState gs) {
    this.gameState = gs;
  }

  private Vector3 tileOffset(float x, float y) {
    return new Vector3(x, 0, y);
  }

}
