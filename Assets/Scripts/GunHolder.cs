
using UnityEngine;

namespace disusdev
{

public class GunHolder : MonoBehaviour
{
  public PointerMover Pointer;
  public Gun Weapon = null;

  private float add_rotation = 85.0f;

  // rotate gun & hold in place
  public void Step()
  {
    transform.localRotation = Pointer.GetRotation() * Quaternion.Euler(0.0f, 0.0f, add_rotation);
  }

  public void Flip(bool is_fliped)
  {
    Vector3 pos = transform.localPosition;
    pos.z = is_fliped ? 0.1f : -0.1f;
    transform.localPosition = pos;
    add_rotation = (is_fliped ? 180.0f + 95.0f : 85.0f);
    Weapon.Flip(is_fliped);
  }
}

}
