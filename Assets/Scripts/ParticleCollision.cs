
using UnityEngine;

namespace disusdev
{

public class ParticleCollision : MonoBehaviour
{
  public GameObject owner;
  private void OnParticleCollision(GameObject other)
  {
    if (other == owner) return;
    PlayerManager pm = other.GetComponent<PlayerManager>();
    if (pm == null) return;
    int dmg = 1;
    pm.Damage(dmg);
    HUDSystem.Instance.DrawIndicator((Vector2)pm.transform.position + (Vector2.one * 0.2f), dmg.ToString(), HUDSystem.IndicatorType.Damage);
  }
}

}
