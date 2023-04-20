
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

  public SpriteRenderer SpriteRenderer;

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

  public ThompsonGun thompson;
  private void Start()
  {
    GunSystem.GiveGun(thompson.gameObject);
  }

  private bool isFliped = false;

  private Vector2 move_dir;
  private Vector2 look_dir;
  private InputState input_state;
  public void Step(float dt)
  {
    GetInput(out input_state);

    { // flip sprite
      look_dir.x = input_state.h_look;
      look_dir.y = input_state.v_look;

      bool zero = look_dir.x <= Mathf.Epsilon && look_dir.x >= -Mathf.Epsilon;
      if (!zero)
      { // TODO: set by pointer, to prevent early flip
        isFliped = look_dir.x < 0;
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
      move_dir.y = input_state.v_move;

      Mover.Move(move_dir, MoveSpeed);
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