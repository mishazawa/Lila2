using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int ID;
    public float duration = .5f;
    public float amp = .1f;

    private State gameState;

    private int spot = 0;
    private int targetSteps = 0;

    private Animator anim;

    void Start() {
      anim = GetComponent<Animator>();
      StartCoroutine(running());
    }

    public IEnumerator Move(int steps) {
      targetSteps = steps;

      while (targetSteps > 0) {
        incrementSpot();
        decrementTarget();
        yield return StartCoroutine(running());
      }

      var tile = gameState.GetTileByIndex(spot);

      if (tile.isSnake || tile.isLadder) {
        setSpot(tile.next);
        yield return StartCoroutine(teleport());
      }

      if (spot != gameState.MaxSpot()) {
        gameState.SetState(GAME_STATE.WAIT_ROLL);
      } else {
        gameState.SetState(GAME_STATE.GAME_OVER);
      }

    }

    private IEnumerator teleport() {
      var tile = gameState.GetTileByIndex(spot);
      transform.position = tile.position;
      yield return null;
    }

    private IEnumerator running() {
      var tile = gameState.GetTileByIndex(spot);

      var startx = transform.position.x;
      var startz = transform.position.z;

      var distx = tile.position.x - startx;
      var distz = tile.position.z - startz;

      var denx = (-0.25f * distx * distx);
      var denz = (-0.25f * distz * distz);

      var time = 0f;
      while (time < duration) {

          // base
          transform.position = Vector3.Lerp(transform.position, tile.position, time / duration);

          var tx = transform.position.x;
          var tz = transform.position.z;

          float arcx = (tx - startx) * (tx - tile.position.x) / denx;
          float arcz = (tz - startz) * (tz - tile.position.z) / denz;

          if (Misc.IsValid(arcx)) {
            transform.position += new Vector3(0, amp * arcx, 0);
          }
          if (Misc.IsValid(arcz)) {
            transform.position += new Vector3(0, amp * arcz, 0);
          }

          time += Time.deltaTime;
          yield return null;
      }
    }

    public void LinkWorld (State gs) {
      this.gameState = gs;
    }

    private void setSpot(int s) {
      spot = s;
      var ms = gameState.MaxSpot();
      if (spot >= ms) {
        spot = ms;
      }
    }
    private void incrementSpot() {
      setSpot(spot+1);
    }

    private void decrementTarget() {
      targetSteps--;
    }
}
