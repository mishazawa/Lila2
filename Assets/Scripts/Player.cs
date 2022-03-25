using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int ID;
    public float distanceThreshold = 0.25f;
    public float duration = 2f;

    private GameState gameState;
    private const float SPOT_CENTER_OFFSET = .15f;
    private Vector3[] PLAYER_OFFSET = new Vector3[] {
      new Vector3(-SPOT_CENTER_OFFSET, 0, -SPOT_CENTER_OFFSET),
      new Vector3(-SPOT_CENTER_OFFSET, 0,  SPOT_CENTER_OFFSET),
      new Vector3( SPOT_CENTER_OFFSET, 0, -SPOT_CENTER_OFFSET),
      new Vector3( SPOT_CENTER_OFFSET, 0,  SPOT_CENTER_OFFSET),
    };

    private int spot = 0;
    private int targetSteps = 0;


    private Animator anim;
    private Coroutine rotating;
    private Coroutine moving;


    void Start() {
      anim = GetComponent<Animator>();
    }

    public void LinkWorld (GameState gs) {
    	this.gameState = gs;
    }

    public void SetSteps (int steps) {
      targetSteps = steps;
      startMovement();
    }

    public void SetSpot(int s) {
      spot = s;
      if (spot >= 99) {
        spot = 99;
      }
    }

    private void startMovement() {
      SetSpot(spot+1);

      if (rotating != null) {
          StopCoroutine(rotating);
      }
      if (moving != null) {
          StopCoroutine(moving);
      }


      rotating = StartCoroutine(Rotating());
      moving = StartCoroutine(Running());
    }

    private IEnumerator Rotating() {
        var tile = gameState.grid.GetTileByIndex(spot);

        float time = 0;
        var angle = Quaternion.LookRotation(tile.transform.position - transform.position, Vector3.up);
        while (time < duration) {
            transform.rotation = Quaternion.Lerp(transform.rotation, angle, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator Running() {
        anim.SetFloat("Speed", 1);

        var tile = gameState.grid.GetTileByIndex(spot);

        var distance = Vector3.Distance(transform.position, tile.transform.position);
        while (distance > distanceThreshold) {
            distance = Vector3.Distance(transform.position, tile.transform.position);
            yield return null;
        }

        anim.SetFloat("Speed", 0);

        if (targetSteps > 0) {
          SetSteps(targetSteps-1);
        } else {
          checkSpotAndDoAction(spot);
        }
    }

    private void checkSpotAndDoAction (int spot) {
      var tile = gameState.grid.GetTileDataByIndex(spot);

      if (tile.isSnake || tile.isLadder) {
        SetSpot(tile.next - 1); // startMovement fn incrementing spot number
        startMovement();
        return;
      }

      if (spot != 99) {
        gameState.SetState(GAME_STATE.WAIT_ROLL);
      } else {
        gameState.SetState(GAME_STATE.GAME_OVER);
      }
    }
}
