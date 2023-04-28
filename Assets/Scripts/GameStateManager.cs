
using UnityEngine;
using UnityEngine.SceneManagement;

namespace disusdev
{

public class GameStateManager : Singleton<GameStateManager>
{
  private void OnSceneLoaded(Scene current, LoadSceneMode mode)
  {
    UpdatePlayers();
    HUDSystem.Instance.Pair.Activate();
  }

  private void OnSceneUnloaded(Scene current)
  {
  }

  public void RestartScene()
  {
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
  }

  public void LoadScene(int build_index)
  {
    PlayerManager.ID = 0;
    SceneManager.LoadScene(build_index);
  }

  public void TimeOut()
  {
    // show game end UI,
    // disable gameplay input
    // time out looser

    HUDSystem.Instance.ActiveWinnerPanel("None");

    inGame = false;

    SfxPlayer.Instance.PlaySfx(SfxPlayer.SfxType.Death);

    // Debug.LogError("Game End!");
  }

  public void OnlyOneLeft()
  {
    string player_name = "None";
    
    int id = GetFirstAliveID();

    if (id >= 0)
    {
      player_name = string.Format("player {0}", id);
    }

    HUDSystem.Instance.Timer.Stop();
    HUDSystem.Instance.ActiveWinnerPanel(player_name);

    inGame = false;

    SfxPlayer.Instance.PlaySfx(SfxPlayer.SfxType.Death);
  }

  public void StartGame()
  {
    HUDSystem.Instance.StartTimer(60.0f, delegate {
    TimeOut(); });
    inGame = true;
  }

  private PlayerManager[] players;
  private void Start()
  {
    SceneManager.sceneUnloaded += OnSceneUnloaded;
    SceneManager.sceneLoaded += OnSceneLoaded;

    players = FindObjectsOfType<PlayerManager>();
  }

  public int GetFirstAliveID()
  {
    foreach(var player in players)
    {
      if (!player.dead) return player.playerId;
    }
    return -1;
  }

  public void UpdatePlayers()
  {
    players = FindObjectsOfType<PlayerManager>(false);
  }

  bool inGame = false;

  private void Update()
  {
    if (!inGame) return;
    float dt = Time.deltaTime;

    GunSpawner.Instance.Step(dt);

    int players_alive = 0;
    foreach(var player in players)
    {
      if (player.dead) continue;
      player.Step(dt);
      players_alive++;
    }
    if (players_alive < 2)
    {
      OnlyOneLeft();
    }
  }

  private void FixedUpdate()
  {
    if (!inGame) return;
    foreach(var player in players)
    {
      if (player.dead) continue;
      player.Simulate();  
    }
  }

  private void LateUpdate()
  {
    float dt = Time.deltaTime;
    HUDSystem.Instance.Step(dt);

    if (!inGame) return;

    foreach(var player in players)
    {
      if (player.dead) continue;
      player.LateStep(dt);
    }
  }
}

}