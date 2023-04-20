
using UnityEngine;

namespace disusdev
{

public abstract class Gun : MonoBehaviour
{
  public SpriteRenderer GunRenderer;
  public Transform Muzzle;
  public int Ammo;
  protected bool fliped = false;

  public abstract void Flip(bool is_fliped);
  public abstract void Shoot();
  public abstract void RegularUpdate(float dt);
}

}
