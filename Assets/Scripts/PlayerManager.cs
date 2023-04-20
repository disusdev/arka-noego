
using UnityEngine;

namespace disusdev
{

public class PlayerManager : MonoBehaviour
{
  static int ID;

  public struct InputState
  {
    public float h_move;
    public float v_move;
    public float h_look;
    public float v_look;
    public bool fire;
  }

  public bool GetInput(out InputState input_state)
  {
    // TODO: check for controller existence

    input_state.h_move = Input.GetAxis(hMoveStr);
    input_state.v_move = Input.GetAxis(vMoveStr);
    input_state.h_look = Input.GetAxis(hLookStr);
    input_state.v_look = Input.GetAxis(vLookStr);
    input_state.fire = Input.GetAxis(fireStr) > Mathf.Epsilon;

    return true;
  }

  public CharacterMover Mover;
  public PointerMover Pointer;
  public GunSystem GunSystem;

  public Animator Animator;
  public SpriteRenderer SpriteRenderer;

  // TODO: just for tests!
  public int hp = 100;
  public bool dead = false;
  public void Damage(int dmg)
  {
    if (dead) return;

    hp -= dmg;

    if (hp == 0)
    {
      // dead
      dead = true;
      //Destroy(gameObject, 1.0f);
      gameObject.SetActive(false);
      //GameStateManager.Instance.UpdatePlayers();
    }
    else
    if (hp < 0)
    {
      // ultra dead
      dead = true;
      //Destroy(gameObject, 1.0f);
      gameObject.SetActive(false);
      //GameStateManager.Instance.UpdatePlayers();
    }
  }

  public float MoveSpeed = 1.0f;

  private int playerId;
  private string hMoveStr;
  private string vMoveStr;
  private string hLookStr;
  private string vLookStr;
  private string fireStr;
  private void Awake()
  {
    playerId = ID++;

    hMoveStr = string.Format("h_move_{0}", playerId);
    vMoveStr = string.Format("v_move_{0}", playerId);
    hLookStr = string.Format("h_look_{0}", playerId);
    vLookStr = string.Format("v_look_{0}", playerId);
    fireStr = string.Format("fire_{0}", playerId);
  }

  // public ThompsonGun thompson;
  private void Start()
  {
    // GunSystem.GiveGun(thompson.gameObject);
  }

  private bool isFliped = false;

  private Vector2 move_dir;
  private Vector2 look_dir;
  private InputState input_state;
  public void Step(float dt)
  {
    GetInput(out input_state);

    {
      look_dir.x = input_state.h_look;
      look_dir.y = input_state.v_look;
    }

    { // flip sprite
      float x = Pointer.GetX();

      bool zero = x <= Mathf.Epsilon && x >= -Mathf.Epsilon;
      if (!zero)
      {
        isFliped = x < 0;
        SpriteRenderer.flipX = isFliped;
        GunSystem.Holder.Flip(isFliped);
      }
    }

    { // update gun
      if (input_state.fire)
      {
        GunSystem.Shoot();
      }

      GunSystem.Step(dt);  
    }
  }

  public void Simulate()
  {
    { // move player
      move_dir.x = input_state.h_move;
      move_dir.y = input_state.v_move * 0.75f;

      bool zero = move_dir.x <= Mathf.Epsilon && move_dir.x >= -Mathf.Epsilon;
      Animator.SetBool("Move", !zero);

      float vel_speed = Mover.Move(move_dir, MoveSpeed);
      Animator.SetFloat("Speed", vel_speed);
    }
  }

  public void LateStep(float dt)
  {
    bool zero = look_dir.x <= Mathf.Epsilon && look_dir.x >= -Mathf.Epsilon;
    if (!zero)
    {
      Pointer.LateStep(look_dir, dt);
    }
    GunSystem.Holder.Step();
  }
}

}