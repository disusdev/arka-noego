
using UnityEngine;

namespace disusdev
{

public class VinchesterGun : Gun
{
  public LayerMask TargetMask;
  public float FireLength;
  public float FireRate;
  public LineRenderer[] Lines;

  private void SpawnProjectile(Vector2 position, Vector2 force)
  {
    int i = 0;
    foreach (var Line in Lines)
    {
      // float rnd = Mathf.PerlinNoise(position.x * Time.time * 2.0f + i, -position.y * Time.time * 2.0f - i) * 2.0f - 1.0f;
      float rnd = Random.Range(-6.0f, 6.0f);

      force = Quaternion.Euler(0.0f, 0.0f, rnd) * force;
      
      RaycastHit2D hit = Physics2D.Linecast(position, position + force, TargetMask);
      
      float length = FireLength;
      if (hit.collider != null)
      {
        length = hit.distance;
      
        // TODO: just tests
        PlayerManager pm = hit.collider.GetComponent<PlayerManager>();
        if (pm != null && pm.gameObject.activeSelf)
        {
          pm.Damage(Damage);
          HUDSystem.Instance.DrawIndicator(hit.point + Vector2.one * 0.2f, Damage.ToString());
        }
      }
      
      Color col = Color.white;
      col.a = 1.0f - i * 0.015f;
      Line.endColor = col;
      col.a = 0.25f - i * 0.025f;
      Line.startColor = col;
      
      Line.SetPosition(0, position);
      Line.SetPosition(1, position + force.normalized * length);
    }
  }

  private float timer = 0.0f;
  public override void RegularUpdate(float dt)
  {
    timer += dt;

    if (timer > FireRate)
    {
      timer = FireRate;
    }

    Color col = Color.white;
    col.a = 1.0f - Mathf.InverseLerp(0.0f, FireRate, timer);

    foreach (var Line in Lines)
    {
      Color c = col;
      c.a -= Random.Range(0.0f, 0.2f);
      Line.endColor = c;
    }

    col.a -= 0.75f;

    foreach (var Line in Lines)
    {
      Color c = col;
      c.a -= Random.Range(0.0f, 0.2f);
      Line.startColor = c;
    }
  }

  public override void Shoot()
  {
    if (timer >= FireRate)
    {
      timer = 0.0f;
      SpawnProjectile(Muzzle.position, (fliped ? -Muzzle.right : Muzzle.right) * FireLength);
    }
  }
}

}
