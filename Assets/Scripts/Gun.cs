
using UnityEngine;

namespace disusdev
{

public abstract class Gun : MonoBehaviour
{
  public SpriteRenderer GunRenderer;
  public Transform Muzzle;
  public int Ammo;
  public int Damage;
  protected bool fliped = false;

  public void Flip(bool is_fliped)
  {
    fliped = is_fliped;
    GunRenderer.flipX = is_fliped;
    Vector2 pos = Muzzle.localPosition;
    pos.x = fliped ? -Mathf.Abs(pos.x) : Mathf.Abs(pos.x);
    Muzzle.localPosition = pos;  
  }
  public abstract void Shoot();
  public abstract void RegularUpdate(float dt);
}

}
