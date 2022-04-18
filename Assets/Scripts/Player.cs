using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int ID;
    public float duration = .5f;
    public float amp = .1f;
    public int numberOfJumps = 4;
    public GameObject cursor;


    private State gameState;
    private int spot = Constants.START_SPOT;
    private int targetSteps = 0;
    private Vector3 slotOffset = Vector3.zero;


    void Start() {
      slotOffset = Constants.SPOT_OFFSETS[ID] * gameState.GetPlayerScale();
      GetComponent<MaterialScript>().setShaderPropertyFloat("_color", ID);
      StartCoroutine(running(spot));
      SetActive(false);
    }

    public void SetActive(bool val) {
      cursor.SetActive(val);
      if (!val) return;
      gameState.SetCameraOnPlayer(transform.position);
    }

    public IEnumerator Move(int steps) {
      targetSteps = validateSteps(steps);

      while (targetSteps > 0) {
        incrementSpot();
        decrementTarget();
        yield return running(spot);
      }

      var tile = gameState.GetTileByIndex(spot);

      if (tile.isSnake || tile.isLadder) {
        yield return teleport(tile.next);
      }
    }

    private IEnumerator teleport(int dest) {
      var ratio = 1f / numberOfJumps;
      for (int i = 0; i < numberOfJumps-1; i++) {
        yield return running((int)Misc.Fit((i+1)*ratio, 0, 1, spot, dest), spot > dest ? -1 : 1);
      }

      yield return running(dest);
      setSpot(dest);
    }

    private IEnumerator running(int spot, int direction = 1) {
      var tile = gameState.GetTileByIndex(spot);

      if (tile.go != null) {
        tile.go.GetComponent<Mice>().playToEnd();
      }

      var nextTile = tile;
      if (spot != gameState.MaxSpot()) {
        nextTile = gameState.GetTileByIndex(spot + 1 * direction);
      }

      var tp = tile.position + slotOffset;

      var startx = transform.position.x;
      var startz = transform.position.z;

      var distx = tp.x - startx;
      var distz = tp.z - startz;

      var denx = (-0.25f * distx * distx);
      var denz = (-0.25f * distz * distz);

      var time = 0f;

      gameState.SetCameraOnPlayer(tile.position);

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

    public int GetSpot() {
      return spot;
    }

    private int validateSteps(int steps) {
      var t = steps + spot;
      var ms = gameState.MaxSpot();

      if (t >= ms) {
        return t + (ms - t) - spot; // return delta from max
      }
      return steps;
    }
}
