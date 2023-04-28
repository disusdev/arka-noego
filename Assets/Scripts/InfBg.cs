
using UnityEngine;

namespace disusdev
{

public class InfBg : MonoBehaviour
{
  public Renderer r;
  public Vector2 velocity;

  private void Start()
  {
    r = GetComponent<Renderer>();
  }

  private void LateUpdate()
  {
    r.material.mainTextureOffset += velocity * Time.deltaTime;
  }
}

}
