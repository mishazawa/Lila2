using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    GameState gameState;

    public int ID;
    public float speed = 1;
    int spot = 0;
    int targetSteps = 0;

    const float SPOT_CENTER_OFFSET = .15f;
    static Vector3[] PLAYER_OFFSET = new Vector3[] {
      new Vector3(-SPOT_CENTER_OFFSET, 0, -SPOT_CENTER_OFFSET),
      new Vector3(-SPOT_CENTER_OFFSET, 0,  SPOT_CENTER_OFFSET),
      new Vector3( SPOT_CENTER_OFFSET, 0, -SPOT_CENTER_OFFSET),
      new Vector3( SPOT_CENTER_OFFSET, 0,  SPOT_CENTER_OFFSET),
    };

    public void LinkWorld (GameState gs) {
    	this.gameState = gs;
    }

    public void SetSteps (int steps) {
      targetSteps = steps;
      StartCoroutine(move());
    }

    public void SetSpot(int s) {
      spot = s;
    }

    IEnumerator move() {
      bool moving = true;

      while (moving) {
          moving = targetSteps > 0;
          var positionToMoveTo = setNewPosition();
          yield return StartCoroutine(LerpPosition(positionToMoveTo, 1f/speed));
      }

      checkSpotAndDoAction(spot);
    }

    private Vector3 setNewPosition () {
      spot += 1;
      targetSteps -= 1;

      if (spot >= 99) {
        spot = 99;
      }

      var tile = gameState.grid.GetTileByIndex(spot);
      var me = GetComponent<Transform>();
      var offset = PLAYER_OFFSET[ID];
      return new Vector3(tile.transform.position.x + offset.x, me.position.y, tile.transform.position.z + offset.z);
    }

    private void checkSpotAndDoAction (int spot) {
      var tile = gameState.grid.GetTileDataByIndex(spot);

      if (tile.isSnake || tile.isLadder) {
        SetSpot(tile.next - 1); // move fn incrementing spot number
        StartCoroutine(move());
        return;
      }

      if (spot != 99) {
        gameState.SetState(GAME_STATE.WAIT_ROLL);
      } else {
        gameState.SetState(GAME_STATE.GAME_OVER);
      }
    }

    private IEnumerator LerpPosition(Vector3 targetPosition, float duration) {
      float time = 0;
      Vector3 startPosition = transform.position;
      while (time < duration)
      {
          transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
          time += Time.deltaTime;
          yield return null;
      }
      transform.position = targetPosition;
    }

}
