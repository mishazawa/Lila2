using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;


public class State : MonoBehaviour {

    public GameObject portalPrefab = null;
    public GameObject levelMesh = null;
    public GameObject hintPrefab = null;
    public GameObject cameraPivot = null;

    public float playerScale = .25f;
    public List<GameObject> avatars = null;
    public VisualEffect vfx;

    private List<Player> players = new List<Player>();
    private PlayerSpot[] tiles = null;
    private Constants.GAME_STATE state = Constants.GAME_STATE.WAIT_PLAYERS;
    private QueueRoll queue;
    private Coroutine cameraMovement;

    public void InitTiles (PlayerSpot[] t, GameObject lvl) {
        tiles = t;
    }

    public void Awake () {
        queue = GetComponent<QueueRoll>();
        queue.LinkWorld(this);
    }

    public void Start() {
      Screen.SetResolution(800, 600, false);

      initPortals();
      SetCameraOnPlayer(GetTileByIndex(tiles.Length).position);
    }

    public void Update() {
      if (state == Constants.GAME_STATE.NEW_TURN) {
          // first time
          var current = queue.Current();
          players[current].SetActive(true);
          SetState(Constants.GAME_STATE.WAIT_ROLL);
      }

      if (state == Constants.GAME_STATE.WAIT_ROLL) {

          if (Input.GetKeyUp(KeyCode.Q)) {
            SetState(Constants.GAME_STATE.MOVING);
            StartCoroutine(moving());
          }
      }

      if (state == Constants.GAME_STATE.GAME_OVER) {
          StartCoroutine(gameOver());
      }

      if (state == Constants.GAME_STATE.WAIT_PLAYERS) {
        if (Input.GetKeyUp(KeyCode.A)) {
          createPlayer();
          if (players.Count == Constants.MAX_PLAYERS) {
            SetState(Constants.GAME_STATE.NEW_TURN);
          }
        }

        if (Input.GetKeyUp(KeyCode.S)) {
          if (players.Count != 0) {
            SetState(Constants.GAME_STATE.NEW_TURN);
          } else {
            Debug.Log("Not enough players");
          }

        }
      }
    }

    private IEnumerator moving () {
      var roll = queue.Roll();
      // var roll = queue.DebugRoll(1);
      var current = queue.Current();

      yield return players[current].Move(roll);

      if (players[current].GetSpot() != MaxSpot()) {
        SetState(Constants.GAME_STATE.NEW_TURN);
        players[current].SetActive(false);
        var next = queue.NextPlayer();
        players[next].SetActive(true);
      } else {
        SetState(Constants.GAME_STATE.GAME_OVER);
      }
    }

    public PlayerSpot GetTileByIndex(int i) {
        if (i < 0) return tiles[0];
        if (i >= tiles.Length) return tiles[tiles.Length - 1];
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
      var go = Instantiate(avatars[players.Count], tiles[Constants.START_SPOT].position + Vector3.up, Quaternion.identity);

      // // player
      var player = go.GetComponent<Player>();
      player.ID = players.Count;
      player.LinkWorld(this);
      players.Add(player);

      go.name = "Player " + player.ID;
    }

    private void initPortals() {
      foreach (PlayerSpot ps in tiles) {
        if (ps.isLadder || ps.isSnake) {
          ps.go = Instantiate(portalPrefab, ps.position + portalPrefab.transform.position, Quaternion.identity);
          ps.go.name = "Mice " + ps.index;
          ps.go.GetComponent<MaterialScript>().setShaderPropertyFloat("_isLadder", ps.isLadder ? 1f : 0f);

          // create invisible spheres and highlight them on hover

          ps.tpFrom = Instantiate(hintPrefab, ps.position, Quaternion.identity);
          ps.tpFrom.GetComponent<HoverTip>().Init(levelMesh, tiles[ps.next].position, ps.isSnake);
        }
      }
    }


    private IEnumerator gameOver() {
      SetState(Constants.GAME_STATE.PAUSE);
      var current = queue.Current();
      var winner = players[current].ID;
      print("Game Over! Winner player: " + winner);

      vfx.SetFloat("ptnum", 50);
      vfx.SendEvent("OnPlay");
      yield return new WaitForSeconds(2f);


      var birds = GameObject.Find("/BirdsContainer");
      birds.SetActive(false);
      yield return new WaitForSeconds(2f);
      // remove birds
      // play out animation
      // show replay menu

      SceneManager.LoadScene(Constants.MAIN_SCENE);
      yield return null;
    }

    public void SetCameraOnPlayer(Vector3 pos, bool useLerp = true) {
      var mc = cameraPivot.GetComponent<MoveCamera>();
      if (cameraMovement != null) {
        StopCoroutine(cameraMovement);
      }

      cameraMovement = StartCoroutine(mc.SetCamera(pos, useLerp));
    }
}
