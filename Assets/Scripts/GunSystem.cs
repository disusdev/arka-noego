
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
      GiveGun(gun.gameObject);
    }
  }

  public void Step(float dt)
  {
    Holder.Weapon?.RegularUpdate(dt);
  }

  public void Shoot()
  {
    Holder.Weapon?.Shoot();
  }
}

}
