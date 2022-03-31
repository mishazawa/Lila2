using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int ID;
    public float distanceThreshold = 0.25f;
    public float duration = .5f;
    public float amp = .1f;

    private State gameState;
    private const float SPOT_CENTER_OFFSET = .15f;
    private Vector3[] PLAYER_OFFSET = new Vector3[] {
      new Vector3(-SPOT_CENTER_OFFSET, 0, -SPOT_CENTER_OFFSET),
      new Vector3(-SPOT_CENTER_OFFSET, 0,  SPOT_CENTER_OFFSET),
      new Vector3( SPOT_CENTER_OFFSET, 0, -SPOT_CENTER_OFFSET),
      new Vector3( SPOT_CENTER_OFFSET, 0,  SPOT_CENTER_OFFSET),
    };

    private int spot = 0;
    private int targetSteps = 0;

    private Coroutine rotating;
    private Coroutine moving;

    private Animator anim;

    void Start() {
      anim = GetComponent<Animator>();
      anim.SetBool("Jump", false);
      StartCoroutine(ResetPosition());
    }

    public void LinkWorld (State gs) {
    	this.gameState = gs;
    }

    public void SetSteps (int steps) {
      targetSteps = steps;
      startMovement();
    }

    public void SetSpot(int s) {
      spot = s;
      var ms = gameState.MaxSpot();
      if (spot >= ms) {
        spot = ms;
      }
    }

    private void startMovement() {
      SetSpot(spot+1);

      // if (rotating != null) {
      //     StopCoroutine(rotating);
      // }
      if (moving != null) {
          StopCoroutine(moving);
      }


      // rotating = StartCoroutine(Rotating());
      moving = StartCoroutine(ResetPosition());
    }

    private IEnumerator Rotating() {
        var tile = gameState.GetTileByIndex(spot);

        float time = 0;
        var destination = (tile.position - transform.position);
        destination.Set(destination.x, transform.position.y, destination.z);
        var angle = Quaternion.LookRotation(destination, Vector3.up);
        while (time < duration) {
            transform.rotation = Quaternion.Lerp(transform.rotation, angle, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator Running() {
        var tile = gameState.GetTileByIndex(spot);

        var distance = Vector3.Distance(transform.position, tile.position);
        while (distance > distanceThreshold) {
            distance = Vector3.Distance(transform.position, tile.position);
            yield return null;
        }

        if (targetSteps > 0) {
          SetSteps(targetSteps-1);
        } else {
          checkSpotAndDoAction(spot);
        }
    }

    private IEnumerator ResetPosition() {
      var tile = gameState.GetTileByIndex(spot);

      var time = 0f;
      var startx = transform.position.x;
      var startz = transform.position.z;

      var distx = tile.position.x - startx;
      var distz = tile.position.z - startz;

      var denx = (-0.25f * distx * distx);
      var denz = (-0.25f * distz * distz);

      while (time < duration) {

          // base
          transform.position = Vector3.Lerp(transform.position, tile.position, time / duration);

          var tx = transform.position.x;
          var tz = transform.position.z;

          float arcx = (tx - startx) * (tx - tile.position.x) / denx;
          float arcz = (tz - startz) * (tz - tile.position.z) / denz;

          if (arcx == arcx) {
            transform.position += new Vector3(0, amp * arcx, 0);
          }
          if (arcz == arcz) {
            transform.position += new Vector3(0, amp * arcz, 0);
          }

          time += Time.deltaTime;
          yield return null;
      }

      if (targetSteps > 0) {
          SetSteps(targetSteps-1);
      } else {
          checkSpotAndDoAction(spot);
      }
    }

    private void checkSpotAndDoAction (int spot) {
      var tile = gameState.GetTileByIndex(spot);

      if (tile.isSnake || tile.isLadder) {
        SetSpot(tile.next - 1); // startMovement fn incrementing spot number
        startMovement();
        return;
      }

      if (spot != gameState.MaxSpot()) {
        gameState.SetState(GAME_STATE.WAIT_ROLL);
      } else {
        gameState.SetState(GAME_STATE.GAME_OVER);
      }
    }
}
