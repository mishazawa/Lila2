using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GAME_STATE {
  WAIT_ROLL,
  MOVING,
  GAME_OVER,
}

public class GameState : MonoBehaviour {
  private GAME_STATE state = GAME_STATE.WAIT_ROLL;

  public Player[] players;
  public QueueRoll queue;
  public GridGeneration grid;

  void Awake () {
    grid.LinkWorld(this);
    grid.Create();
    grid.Instantiate();

    queue.LinkWorld(this);

    foreach (Player p in players) {
      p.LinkWorld(this);
    }
  }

    void Update()
    {

      if (state == GAME_STATE.WAIT_ROLL) {
          if (Input.GetKeyUp(KeyCode.Q)) {
            SetState(GAME_STATE.MOVING);

            var roll = queue.Roll();
            var current = queue.Current();
            players[current].SetSpot(roll);

          }
      }

      if (state == GAME_STATE.MOVING) {
        // if (finished) {
        //  SetState(GAME_STATE.GAME_OVER);
        // } else {
        //  SetState(GAME_STATE.WAIT_ROLL);
        //  queue.NextPlayer();
        // }
      }

      if (state == GAME_STATE.GAME_OVER) {
          var current = queue.Current();
          var winner = players[current].ID;
        print("Game Over! Winner player: " + winner);
      }
    }

    public GAME_STATE GetState() {
      return state;
    }

    public void SetState(GAME_STATE s) {
      state = s;
    }
}
