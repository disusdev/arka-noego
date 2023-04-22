
using TMPro;
using UnityEngine;

namespace disusdev
{

public class HUDSystem : Singleton<HUDSystem>
{

  [System.Serializable]
  public struct TextIndicator
  {
    private Transform glued;
    private Vector2 offset;

    public TMP_Text text;
    public RectTransform trans;
    public float hide_speed;
    public bool avaliable;

    private Color color;
    public void Play(Vector2 position, string value, Color color)
    {
      glued = null;
      trans.position = Camera.main.WorldToScreenPoint(position);
      text.text = value;
      timer = 1.0f;

      this.color = color;

      this.color.a = 1.0f;
      text.color = this.color;

      avaliable = false;
    }

    public void PlayGlued(Transform parrent, Vector2 offset, string value, Color color)
    {
      glued = parrent;
      this.offset = offset;

      trans.position = Camera.main.WorldToScreenPoint((Vector2)glued.position + this.offset);
      text.text = value;
      timer = 1.0f;

      this.color = color;

      this.color.a = 1.0f;
      text.color = this.color;

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

      if (glued != null)
      {
        this.offset += Vector2.up * dt;
        trans.position = Camera.main.WorldToScreenPoint((Vector2)glued.position + this.offset);
      }
      else
      {
        trans.localPosition += Vector3.up * timer;
      }
    }
  }

  public TextIndicator[] Indicators;

  public void DrawIndicator(Vector2 position, string value, Color color)
  {
    for (int i = 0; i < Indicators.Length; i++)
    {
      if (Indicators[i].avaliable)
      {
        Indicators[i].Play(position, value, color);
        return;
      }
    }
  }

  public void DrawGluedIndicator(Transform parrent, Vector2 offset, string value, Color color)
  {
    for (int i = 0; i < Indicators.Length; i++)
    {
      if (Indicators[i].avaliable)
      {
        Indicators[i].PlayGlued(parrent, offset, value, color);
        return;
      }
    }
  }

  public void Step(float dt)
  {
    for (int i = 0; i < Indicators.Length; i++)
    {
      Indicators[i].Update(dt);
    }
  }
}

}
