
using UnityEngine;

namespace disusdev
{

public class RocketLauncherGun : Gun
{
  public GameObject Projectile;

  public override void RegularUpdate(float dt)
  {
    timer += dt;

    if (timer > FireRate)
    {
      timer = FireRate;
    }
  }

  public override void SpawnProjectile(Vector2 position, Vector2 force)
  {
    // spawn projectile
    // Muzzle.forward

    Vector2 dir = fliped ? Muzzle.up : -Muzzle.up;

    float rotation = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

    GameObject instance = Instantiate(Projectile, position + force * 0.5f, Quaternion.Euler(0.0f, 0.0f, rotation), null);



    Projectile proj = instance.GetComponent<Projectile>();
    proj.Launch(force, Damage);

    Ammo--;

    if (Ammo <= 0) return;
    HUDSystem.Instance.DrawGluedIndicator(transform, transform.right * 0.3f, Ammo.ToString(), HUDSystem.IndicatorType.Ammo);
  }
}

}
