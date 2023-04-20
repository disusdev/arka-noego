
using TMPro;
using UnityEngine;

namespace disusdev
{

public class HUDSystem : Singleton<HUDSystem>
{
  [System.Serializable]
  public struct TextIndicator
  {
    public TMP_Text text;
    public RectTransform trans;
    public float hide_speed;
    public Color color;
    public bool avaliable;

    public void Play(Vector2 camera_position, string value)
    {
      trans.position = camera_position;
      text.text = value;
      timer = 1.0f;
      color.a = 1.0f;
      text.color = color;
      avaliable = false;
    }

    private float timer;
    public void Update(float dt)
    {
      timer -= dt * hide_speed;
      if (timer < 0.0f)
      {
          timer = 0.0f;
          avaliable = true;
      }

      color.a = timer;
      text.color = color;

      trans.localPosition += Vector3.up * timer;
    }
  }

  public TextIndicator[] DamageIndicators;

  public void DrawIndicator(Vector2 position, string value)
  {
    for (int i = 0; i < DamageIndicators.Length; i++)
    {
      if (DamageIndicators[i].avaliable)
      {
        DamageIndicators[i].Play(Camera.main.WorldToScreenPoint(position), value);
        return;
      }
    }
  }

  public void Step(float dt)
  {
    for (int i = 0; i < DamageIndicators.Length; i++)
    {
      DamageIndicators[i].Update(dt);
    }
  }
}

}
