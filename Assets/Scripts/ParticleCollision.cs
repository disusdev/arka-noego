
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
    int dmg = 5;
    PlayerManager.Modifier modifier = new PlayerManager.Modifier(
      5, 1.0f,
      PlayerManager.ModifierType.Fire,
      delegate
      {
        int fire_dmg = dmg / 2;
        pm.Damage(fire_dmg);
        HUDSystem.Instance.DrawIndicator((Vector2)pm.transform.position + (Vector2.one * 0.2f), fire_dmg.ToString(), HUDSystem.IndicatorType.Damage);
      }
    );

    pm.Damage(dmg, modifier);
    HUDSystem.Instance.DrawIndicator((Vector2)pm.transform.position + (Vector2.one * 0.2f), dmg.ToString(), HUDSystem.IndicatorType.Damage);
  }
}

}
