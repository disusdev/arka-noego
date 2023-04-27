
using UnityEngine;
using UnityEngine.SceneManagement;

namespace disusdev
{

public class GameStateManager : Singleton<GameStateManager>
{
  private void OnSceneLoaded(Scene current, LoadSceneMode mode)
  {
    //string[] names = Input.GetJoystickNames();
    //foreach(var j_name in names)
    //{
    //  Debug.Log("Joystick: " + j_name);
    //}
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
    SceneManager.LoadScene(build_index);
  }

  public void TimeOut()
  {
    // show game end UI,
    // disable gameplay input
    // time out looser
    Debug.LogError("Game End!");
  }

  public void StartGame()
  {
    HUDSystem.Instance.StartTimer(60.0f, delegate {
    TimeOut(); });
  }

  private PlayerManager[] players;
  private void Start()
  {
    SceneManager.sceneUnloaded += OnSceneUnloaded;
    SceneManager.sceneLoaded += OnSceneLoaded;

    players = FindObjectsOfType<PlayerManager>();
  }

  public void UpdatePlayers()
  {
    players = FindObjectsOfType<PlayerManager>(false);
  }

  private void Update()
  {
    float dt = Time.deltaTime;
    foreach(var player in players)
    {
      if (player.dead) continue;
      player.Step(dt);  
    }
  }

  private void FixedUpdate()
  {
    foreach(var player in players)
    {
      if (player.dead) continue;
      player.Simulate();  
    }
  }

  private void LateUpdate()
  {
    float dt = Time.deltaTime;
    foreach(var player in players)
    {
      if (player.dead) continue;
      player.LateStep(dt);
    }

    HUDSystem.Instance.Step(dt);
  }
}

}