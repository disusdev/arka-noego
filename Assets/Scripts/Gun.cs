
using UnityEngine;

namespace disusdev
{

public abstract class Gun : MonoBehaviour
{
  public float FireLength;
  public float FireRate;
  public SpriteRenderer GunRenderer;
  public Transform Muzzle;
  public int Ammo;
  public int Damage;
  protected bool fliped = false;
  protected float timer = 0.0f;
  public GameObject Thrower;

  public void Flip(bool is_fliped)
  {
    fliped = is_fliped;
    GunRenderer.flipX = is_fliped;
    Vector2 pos = Muzzle.localPosition;
    pos.x = fliped ? -Mathf.Abs(pos.x) : Mathf.Abs(pos.x);
    Muzzle.localPosition = pos;  
  }
  public abstract void SpawnProjectile(Vector2 position, Vector2 force);

  public bool Shoot()
  {
    if (timer >= FireRate)
    {
      if (Ammo <= 0)
      {
        // TODO: throw gun
        transform.parent = null;

        var cc = gameObject.AddComponent<CircleCollider2D>();
        cc.isTrigger = true;
        var rb = gameObject.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0.0f;
        rb.AddForce((fliped ? -transform.right : transform.right) * 15.0f, ForceMode2D.Impulse);
        rb.AddTorque(65.0f);
        // HUDSystem.Instance.DrawIndicator((Vector2)transform.position + Vector2.up * 0.4f + Vector2.right * 0.4f, "-", AmmoColor);
        return false;
      }

      timer = 0.0f;
      SpawnProjectile(Muzzle.position, (fliped ? -Muzzle.right : Muzzle.right) * FireLength);
    }

    return true;
  }

  public abstract void RegularUpdate(float dt);
}

}
