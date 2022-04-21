using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;


public class State : MonoBehaviour {

    public GameObject portalPrefab  = null;
    public GameObject levelMesh     = null;
    public GameObject hintPrefab    = null;
    public GameObject cameraPivot   = null;
    public GameObject birds         = null;
    public VisualEffect vfx         = null;
    public MenuController menu      = null;
    public List<GameObject> avatars = null;
    public float playerScale        = .25f;


    // private PostProcessProfile ppProfile;
    private List<Player> players       = new List<Player>();
    private PlayerSpot[] tiles         = null;
    private QueueRoll queue            = null;
    private Coroutine cameraMovement   = null;
    private Constants.GAME_STATE state = Constants.GAME_STATE.WAIT_PLAYERS;


    public void Awake () {
      SetupGame();
    }

    public void Start() {
      SetupPortals();
      SetupFinishSpot();
      SetupMenu();
      SetupAmbientSound();

      if (Constants.DEMO_AUTOPLAY) {
        StartCoroutine(Misc.Delay(.5f, () => AddPlayer()));
        StartCoroutine(Misc.Delay(.5f, () => StartGame()));
      }
    }

    public void Update() {
      switch(state) {
        case Constants.GAME_STATE.NEW_TURN:
          onNewTurn();
          break;
        case Constants.GAME_STATE.WAIT_ROLL:
          onWaitRoll();
          break;
        case Constants.GAME_STATE.WAIT_PLAYERS:
          onWaitPlayers();
          break;
        case Constants.GAME_STATE.GAME_OVER:
          onGameOver();
          break;
        case Constants.GAME_STATE.PAUSE:
          onPause();
          break;
      }
      onCameraZoom();
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
      state = s;
    }

    public void InitTiles (PlayerSpot[] t, GameObject lvl) {
        tiles = t;
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

    public void SetCameraOnPlayer(Vector3 pos, bool useLerp = true) {
      var mc = cameraPivot.GetComponent<MoveCamera>();
      if (cameraMovement != null) StopCoroutine(cameraMovement);
      cameraMovement = StartCoroutine(mc.SetCamera(pos, useLerp));
    }

    private void createPlayer() {
      if (players.Count >= Constants.MAX_PLAYERS) return;
      var go = Instantiate(avatars[players.Count], tiles[Constants.START_SPOT].position + Vector3.up, Quaternion.identity);

      var player = go.GetComponent<Player>();
      player.ID = players.Count;
      player.LinkWorld(this);
      players.Add(player);

      go.name = "Player " + player.ID;
    }

    private void SetupPortals() {
      foreach (PlayerSpot ps in tiles) {
        if (ps.isLadder || ps.isSnake) {
          ps.go = Instantiate(portalPrefab, ps.position + portalPrefab.transform.position, Quaternion.identity);
          ps.go.name = "Mice " + ps.index;
          ps.go.GetComponent<MaterialScript>().setShaderPropertyFloat("_isLadder", ps.isLadder ? 1f : 0f);


          // create invisible spheres and highlight them on hover
          ps.hint = Instantiate(hintPrefab, ps.position, Quaternion.identity);
          ps.hint.GetComponent<HoverTip>().Init(levelMesh, tiles[ps.next].position, ps.isSnake);
        }
      }
    }


    private IEnumerator gameOver() {
      SetState(Constants.GAME_STATE.PAUSE);

      FindObjectOfType<AudioManager>().Play(Constants.SOUND_WIN);

      // show particles
      vfx.SetFloat("Points", Constants.PARTICLES_COUNT);
      vfx.SendEvent("OnPlay");

      // rm birds
      yield return Misc.Delay(Constants.BIRDS_LIFETIME, () => birds.SetActive(false));

      // show winner
      var current = queue.Current();
      menu.SetWinner(players[current].ID);
      menu.ShowMenuType(MenuController.Types.REPLAY);
      // blur cam
      var mc = cameraPivot.GetComponent<MoveCamera>();
      yield return mc.BlurCamera(mc.minDOF);
    }

    private IEnumerator moving (int roll) {
      yield return menu.AnimateRoll(roll);

      var current = queue.Current();

      yield return players[current].Move(roll);

      if (players[current].GetSpot() != MaxSpot()) {
        // switch players
        SetState(Constants.GAME_STATE.NEW_TURN);
        players[current].SetActive(false);
        var next = queue.NextPlayer();
        players[next].SetActive(true);
      } else {
        SetState(Constants.GAME_STATE.GAME_OVER);
      }
    }

    private void SetupGame() {
      Screen.SetResolution(800, 600, false);
      queue = GetComponent<QueueRoll>();
      queue.LinkWorld(this);
    }

    private void SetupMenu() {
      menu.ShowMenuType(MenuController.Types.START);
    }
    private void SetupAmbientSound() {
      StartCoroutine(asd());
    }

    IEnumerator asd () {
        yield return new WaitForSeconds(Misc.Fit(Random.value, 0, 1, 50, 100));
        FindObjectOfType<AudioManager>().Play(Constants.SOUND_CAT);
        yield return asd();
    }

    private void SetupFinishSpot() {
      var lastSpotPos = GetTileByIndex(tiles.Length).position;
      birds.transform.position = lastSpotPos;
      vfx.gameObject.transform.position = lastSpotPos + Vector3.up * .25f;
      SetCameraOnPlayer(lastSpotPos, false);
    }

    private void onNewTurn () {
        var current = queue.Current();
        players[current].SetActive(true);
        SetState(Constants.GAME_STATE.WAIT_ROLL);
    }

    private void onWaitRoll () {
        if (Input.GetKeyUp(KeyCode.Q)) {
          RollDie();
        }
    }

    private void onWaitPlayers () {
        if (Input.GetKeyUp(KeyCode.A)) {
          AddPlayer();
        }
        if (Input.GetKeyUp(KeyCode.S)) {
          StartGame();
        }
    }

    private void onGameOver () {
        StartCoroutine(gameOver());
    }

    private void onCameraZoom () {
        if (Input.GetKeyDown(KeyCode.BackQuote)) {
          Debug.Log("~asd+");
          var mc = cameraPivot.GetComponent<MoveCamera>();
          StartCoroutine(mc.Zoom(mc.maxZoom));
        }
        if (Input.GetKeyUp(KeyCode.BackQuote)) {
          Debug.Log("~asd-");
          var mc = cameraPivot.GetComponent<MoveCamera>();
          StartCoroutine(mc.Zoom(mc.minZoom));
        }
    }

    private void onPause() {
      if (Input.GetKeyUp(KeyCode.R)) {
        Replay();
      }
    }

    public void StartGame() {
      if (players.Count == 0) return;

      menu.HideAll();
      menu.ShowMenuType(MenuController.Types.MOVE);

      var mc = cameraPivot.GetComponent<MoveCamera>();
      StartCoroutine(mc.BlurCamera(mc.maxDOF));
      SetState(Constants.GAME_STATE.NEW_TURN);
      // paly Anim;
    }

    public void AddPlayer() {
      if (players.Count == Constants.MAX_PLAYERS) return;
      createPlayer();
      menu.AnimatePlayerConnection(players.Count);
    }

    public void Replay () {
      SceneManager.LoadScene(Constants.MAIN_SCENE);
    }

    public void RollDie () {
      if (state != Constants.GAME_STATE.WAIT_ROLL) return;

      // var roll = queue.DebugRoll(1);
      var roll    = queue.Roll();
      SetState(Constants.GAME_STATE.MOVING);
      StartCoroutine(moving(roll));
    }

    public Vector3 GetMouseDirection () {
      return GetComponent<MouseDirection>().GetDirection();
    }
}
