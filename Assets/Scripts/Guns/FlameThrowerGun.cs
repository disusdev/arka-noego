
using UnityEngine;

namespace disusdev
{

public class FlameThrowerGun : Gun
{
  public ParticleSystem FlameParticle;
  public ParticleCollision FlameCollision;

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
    FlameCollision.owner = owner;

    var sh = FlameParticle.shape;

    Vector3 pos = sh.position;
    pos.x = Mathf.Abs(pos.x) * (fliped ? -1.0f : 1.0f);
    sh.position = pos;

    sh.rotation = new Vector3(0.0f, 0.0f, (fliped ? 90.0f : -90.0f));

    FlameParticle.Emit(5);
    Ammo -= 5;
  }
}

}
