
using UnityEngine;

namespace disusdev
{

public class Projectile : MonoBehaviour
{
  public float Speed;
  public float Radius;
  public float Aimbot;

  public Rigidbody2D rb;
  public ParticleSystem ps;

  public LayerMask TargetMask;
  private Collider2D[] colliders = new Collider2D[4];

  private int damage;

  public void Launch(Vector2 force, int damage)
  {
    this.damage = damage;

    rb.gravityScale = 0.0f;
    rb.AddForce(force * Speed, ForceMode2D.Impulse);
  }

  private void OnCollisionEnter2D(Collision2D collision)
  {
    SfxPlayer.Instance.PlaySfx(SfxPlayer.SfxType.RocketBlow);

    int collision_count = Physics2D.OverlapCircleNonAlloc(collision.contacts[0].point, Radius, colliders, TargetMask);
    for (int i = 0; i < collision_count; i++)
    {
      PlayerManager pm = colliders[i].GetComponent<PlayerManager>();
      if (pm == null) continue;
      pm.Damage(damage);
      HUDSystem.Instance.DrawIndicator((Vector2)pm.transform.position + (Vector2.one * 0.2f), damage.ToString(), HUDSystem.IndicatorType.Damage);
    }

    ps.Play();
    Destroy(gameObject, 1.0f);
    GetComponent<SpriteRenderer>().enabled = false;
    GetComponent<Collider2D>().enabled = false;
    this.enabled = false;
  }
}

}