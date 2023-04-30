
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace disusdev
{

public class PlayerManager : MonoBehaviour
{
  public static int ID;

  public enum ModifierType
  {
    Fire  
  }

  public class Modifier
  {
    public UnityAction StepAction;
    public int Count;
    public float Rate;
    public ModifierType Type;

    public Modifier(int count,
                    float rate,
                    ModifierType type,
                    UnityAction stepAction)
    {
      StepAction = stepAction;
      Count = count;
      Rate = rate;
      Type = type;
      timer = Rate;
    }

    private float timer = 0.0f;

    public void Step(float dt)
    {
      timer -= dt;
      if (timer < 0)
      {
        timer = Rate;
        StepAction();
        Count--;
      }
    }
  }

  private const int MaxActiveModifiers = 1;
  private Modifier[] modifiers = new Modifier[MaxActiveModifiers];

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
    Vector2 move = pad.leftStick.ReadValue();
    input_state.h_move = move.x;
    input_state.v_move = move.y;
    Vector2 look = pad.rightStick.ReadValue();
    input_state.h_look = look.x;
    input_state.v_look = look.y;
    input_state.fire = pad.rightTrigger.ReadValue() > Mathf.Epsilon;

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
  public void Damage(int dmg, Modifier modifier = null)
  {
    if (dead) return;

    if (modifier != null)
    {
      bool can_add = true;
      for (int i = 0; i < modifiers.Length; i++)
      {
        if (modifiers[i] != null && modifiers[i].Type != modifier.Type)
        {
          can_add = false;
          break;
        }
      }
      if (can_add)
      {
        for (int i = 0; i < modifiers.Length; i++)
        {
          if (modifiers[i] == null)
          {
            modifiers[i] = modifier;
          }
        }
      }
    }

    hp -= dmg;

    if (hp > 0.0f)
    {
      RumbleSystem.Instance.Play(playerId, dmg / 100.0f, 0.2f);
      HUDSystem.Instance.DrawGluedBarIndicator(playerId, transform, transform.up * 1.0f, hp);
    }

    CameraShaker.Instance.AddDuration(dmg * 0.02f);

    if (hp == 0)
    { // dead
      dead = true;

      SfxPlayer.Instance.PlaySfx(SfxPlayer.SfxType.Death);

      RumbleSystem.Instance.Play(playerId, 0.5f, 1.0f);
      gameObject.SetActive(false);
    }
    else
    if (hp < 0)
    { // ultra dead
      dead = true;

      SfxPlayer.Instance.PlaySfx(SfxPlayer.SfxType.Death);

      RumbleSystem.Instance.Play(playerId, 0.7f, 1.0f);
      gameObject.SetActive(false);
    }
  }

  public float MoveSpeed = 1.0f;

  public int playerId;
  private Gamepad pad;
  public void Init(bool is_active)
  {
    if (is_active)
    {
      playerId = ID++;
      pad = Gamepad.all[playerId];

      gameObject.SetActive(true);
    }
    else
    {
      dead = true;
    }
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

    { // update gun
      if (input_state.fire)
      {
        GunSystem.Shoot();
      }

      GunSystem.Step(dt);  
    }

    for (int i = 0; i < modifiers.Length; i++)
    {
      if (modifiers[i] == null) continue;
      modifiers[i].Step(dt);
      if (modifiers[i].Count == 0) modifiers[i] = null;
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

    { // flip sprite
      float x = Pointer.GetX();

      bool nzero = x <= Mathf.Epsilon && x >= -Mathf.Epsilon;
      if (!nzero)
      {
        isFliped = x < 0;
        SpriteRenderer.flipX = isFliped;
        GunSystem.Holder.Flip(isFliped);
      }
    }
  }
}

}