using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            players[current].SetSteps(roll);
            queue.NextPlayer();
          }
      }

      if (state == GAME_STATE.GAME_OVER) {
          var current = queue.Current();
          var winner = players[current].ID;
          print("Game Over! Winner player: " + winner);
          SceneManager.LoadScene("Main");
      }
    }

    public GAME_STATE GetState() {
      return state;
    }

    public void SetState(GAME_STATE s) {
      // Debug.Log(s);
      state = s;
    }
}
