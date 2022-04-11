using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class State : MonoBehaviour {

    public GameObject portalPrefab = null;
    public List<GameObject> avatars = null;
    public float playerScale = .25f;
    public Material teleportMaterial;

    private List<Player> players = new List<Player>();
    private PlayerSpot[] tiles = null;
    private Constants.GAME_STATE state = Constants.GAME_STATE.WAIT_PLAYERS;
    private QueueRoll queue;
    private Coroutine currentMouseAnimation;


    public void InitTiles (PlayerSpot[] t) {
        Debug.Log("Init tiles: " + t.Length);
        tiles = t;
    }

    public void Awake () {
        queue = GetComponent<QueueRoll>();
        queue.LinkWorld(this);
    }

    void Start() {
      initPortals();
    }

    void Update() {
      if (state == Constants.GAME_STATE.WAIT_ROLL) {
          if (Input.GetKeyUp(KeyCode.Q)) {
            SetState(Constants.GAME_STATE.MOVING);

            var roll = queue.DebugRoll(1);
            // var roll = queue.Roll();
            var current = queue.Current();
            Debug.Log("Player: " + players[current].ID + " move -> +" + roll);
            StartCoroutine(players[current].Move(roll));

            queue.NextPlayer();
          }
      }

      if (state == Constants.GAME_STATE.GAME_OVER) {
          var current = queue.Current();
          var winner = players[current].ID;
          print("Game Over! Winner player: " + winner);
          SceneManager.LoadScene("Test");
      }

      if (state == Constants.GAME_STATE.WAIT_PLAYERS) {
        if (Input.GetKeyUp(KeyCode.A)) {
          createPlayer();
          if (players.Count == Constants.MAX_PLAYERS) {
            SetState(Constants.GAME_STATE.WAIT_ROLL);
          }
        }

        if (Input.GetKeyUp(KeyCode.S)) {
          if (players.Count != 0) {
            SetState(Constants.GAME_STATE.WAIT_ROLL);
          } else {
            Debug.Log("Not enough players");
          }

        }
      }
    }

    public PlayerSpot GetTileByIndex(int i) {
        return tiles[i];
    }

    public Constants.GAME_STATE GetState() {
      return state;
    }

    public void SetState(Constants.GAME_STATE s) {
      // Debug.Log(s);
      state = s;
    }

    public int MaxSpot() {
        return tiles.Length - 1;
    }

    public List<Player> GetPlayers() {
      return players;
    }

    public float GetPlayerScale () {
      return playerScale;
    }

    private void createPlayer() {
      if (players.Count >= Constants.MAX_PLAYERS) return;
      var go = Instantiate(avatars[players.Count], Vector3.zero, Quaternion.identity);

      // // player
      var player = go.AddComponent<Player>();
      player.ID = players.Count;
      player.teleportMaterial = teleportMaterial;
      player.LinkWorld(this);
      players.Add(player);

      // // game object

      go.name = "Player " + player.ID;
    }

    private void initPortals() {
      foreach (PlayerSpot ps in tiles) {
        if (ps.isLadder || ps.isSnake) {
          ps.go = Instantiate(portalPrefab, ps.position + portalPrefab.transform.position, Quaternion.identity);
          ps.go.GetComponent<MaterialScript>().setShaderPropertyFloat("_isLadder", ps.isLadder?1f:0f);
        }
      }
    }
}
