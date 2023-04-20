
using UnityEngine;

namespace disusdev
{

public class PointerMover : MonoBehaviour
{
  public float Speed = 1.0f;

  float offsetDistance = 1.5f;
  float rotation = 0.0f;

  public Transform target;
  public Transform pointer;

  private void Awake()
  {
    pointer.localPosition = Vector3.up * offsetDistance;
  }

  public Quaternion GetRotation()
  {
    return target.localRotation;
  }

  public float GetYRotFromVec(Vector2 v)
  {
    float _r = -Mathf.Atan2(v.x, v.y);
    float _d = (_r / Mathf.PI) * 180.0f;
    return _d;
  }

  public void LateStep(Vector2 desired_dir, float dt)
  {
    desired_dir.Normalize();
    rotation = GetYRotFromVec(desired_dir);
    target.localRotation = Quaternion.Slerp(target.localRotation, Quaternion.Euler (0, 0, rotation), Speed * dt);
  }
}

}