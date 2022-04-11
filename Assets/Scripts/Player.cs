using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int ID;
    public float duration = .5f;
    public float amp = .1f;

    public Material teleportMaterial;
    public float teleportDuration = 2f;

    private State gameState;
    private int spot = 0;
    private int targetSteps = 0;
    private Vector3 slotOffset = Vector3.zero;
    private MaterialScript mat;

    void Start() {
      mat = GetComponent<MaterialScript>();
      mat.setShaderPropertyFloat("_color", ID);
      slotOffset = Constants.SPOT_OFFSETS[ID] * gameState.GetPlayerScale();
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
        gameState.SetState(Constants.GAME_STATE.WAIT_ROLL);
      } else {
        gameState.SetState(Constants.GAME_STATE.GAME_OVER);
      }

    }

    private IEnumerator teleport() {
      var time = 0f;
      while (time < teleportDuration) {
        time += Time.deltaTime;
        yield return null;
      }

      var tile = gameState.GetTileByIndex(spot);
      var nextTile = gameState.GetTileByIndex(spot+1);
      transform.position = tile.position + slotOffset;
      setRotation(nextTile.position + slotOffset);

      while (time >= 0) {
        time -= Time.deltaTime;
        yield return null;
      }
      yield return null;
    }

    private IEnumerator running() {
      var tile = gameState.GetTileByIndex(spot);

      var nextTile = tile;
      if (spot != gameState.MaxSpot()) {
        nextTile = gameState.GetTileByIndex(spot + 1);
      }

      var tp = tile.position + slotOffset;

      var startx = transform.position.x;
      var startz = transform.position.z;

      var distx = tp.x - startx;
      var distz = tp.z - startz;

      var denx = (-0.25f * distx * distx);
      var denz = (-0.25f * distz * distz);

      var time = 0f;
      while (time < duration) {

          // base
          transform.position = Vector3.Lerp(transform.position, tp, time / duration);

          var tx = transform.position.x;
          var tz = transform.position.z;

          float arcx = (tx - startx) * (tx - tp.x) / denx;
          float arcz = (tz - startz) * (tz - tp.z) / denz;

          if (Misc.IsValid(arcx)) {
            transform.position += new Vector3(0, amp * arcx, 0);
          }
          if (Misc.IsValid(arcz)) {
            transform.position += new Vector3(0, amp * arcz, 0);
          }

          // rotation
          setRotation(nextTile.position + slotOffset);

          time += Time.deltaTime;
          yield return null;
      }

    }

    public void LinkWorld (State gs) {
      this.gameState = gs;
    }

    private void setRotation (Vector3 dest) {
      transform.LookAt(new Vector3(dest.x, transform.position.y, dest.z));
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
