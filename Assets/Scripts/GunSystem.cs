
using UnityEngine;

namespace disusdev
{

public class GunSystem : MonoBehaviour
{
  public GunHolder Holder;

  public void GiveGun(GameObject gun)
  {
    if (Holder.Weapon != null) return;
    gun.transform.SetParent(Holder.transform, false);
    gun.transform.localPosition = Vector3.zero;
    Holder.Weapon = gun.GetComponent<Gun>();
    Destroy(gun.GetComponent<CircleCollider2D>());
  }

  private void OnTriggerEnter2D(Collider2D collision)
  {
    Gun gun = collision.gameObject.GetComponent<Gun>();
    if (gun != null)
    {
      if (gun.Ammo <= 0)
      { // check if not our gun
        if (gun.Thrower != gameObject)
        {
          collision.gameObject.SetActive(false);
          // give damage
          PlayerManager pm = GetComponent<PlayerManager>();
          int dmg = gun.Damage / 2;
          pm.Damage(dmg);
          HUDSystem.Instance.DrawIndicator((Vector2)transform.position + Vector2.one * 0.2f, dmg.ToString(), Color.red);
        }
        return;
      }
      else
      {
        GiveGun(gun.gameObject);
      }
    }
  }

  public void Step(float dt)
  {
    Holder.Weapon?.RegularUpdate(dt);
  }

  public void Shoot()
  {
    if (Holder.Weapon && !Holder.Weapon.Shoot())
    {
      Holder.Weapon.Thrower = gameObject;
      Destroy(Holder.Weapon.gameObject, 1.0f);
      Holder.Weapon = null;
    }
  }
}

}
