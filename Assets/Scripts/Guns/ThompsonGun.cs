
using UnityEngine;

namespace disusdev
{

public class ThompsonGun : Gun
{
  public float FireRate;
  public float FireLength;
  float timer = 0.0f;

  public float recoil = 0.1f;
  float trauma = 0.0f;

  public LineRenderer Line;
  public LayerMask TargetMask;

  private void Awake()
  {
    timer = FireRate;
  }

  private void SpawnProjectile(Vector2 position, Vector2 force)
  {
    float rnd = Mathf.PerlinNoise(position.x * Time.time * 2.0f, -position.y * Time.time * 2.0f) * 2.0f - 1.0f;
    rnd *= trauma * trauma * trauma;

    rnd = Mathf.Clamp(rnd, -5.0f, 5.0f);
    force = Quaternion.Euler(0.0f, 0.0f, rnd) * force;

    RaycastHit2D hit = Physics2D.Linecast(position, position + force, TargetMask);

    float length = FireLength;
    if (hit.collider != null)
    {
      length = hit.distance;
    }

    Color col = Color.white;
    Line.endColor = col;
    col.a = 0.25f;
    Line.startColor = col;


    Line.SetPosition(0, position);
    Line.SetPosition(1, position + force.normalized * length);
  }

  public override void Flip(bool is_fliped)
  {
    fliped = is_fliped;
    GunRenderer.flipX = is_fliped;
    Vector2 pos = Muzzle.localPosition;
    pos.x = fliped ? -Mathf.Abs(pos.x) : Mathf.Abs(pos.x);
    Muzzle.localPosition = pos;
  }

  public override void Shoot()
  {
    if (timer >= FireRate)
    {
      timer = 0.0f;
      trauma += recoil;
      SpawnProjectile(Muzzle.position, (fliped ? -Muzzle.right : Muzzle.right) * FireLength);
    }
  }

  public override void RegularUpdate(float dt)
  {
    timer += dt;
    trauma -= dt * 2.0f;

    if (timer > FireRate)
    {
      timer = FireRate;
    }

    if (trauma < 0.0f)
    {
      trauma = 0.0f;
    }

    Color col = Color.white;
    col.a = 1.0f - Mathf.InverseLerp(0.0f, FireRate, timer);
    Line.endColor = col;

    col.a -= 0.75f;
    Line.startColor = col;
  }
}

}
