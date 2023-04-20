
using UnityEngine;

namespace disusdev
{

public class GunSystem : MonoBehaviour
{
  public GunHolder Holder;

  public void GiveGun(GameObject gun)
  {
    gun.transform.SetParent(Holder.transform, false);
    Holder.Weapon = gun.GetComponent<Gun>();
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
