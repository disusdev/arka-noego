
using UnityEngine;

namespace disusdev
{

public class CharacterMover : MonoBehaviour
{
  public Transform body;

  float moveTimer = 0.0f;

  public float Height = 1.0f;
  public float Speed = 1.0f;

  public void Move(Vector2 direction, float speed)
  {
    Rigidbody2D rb = GetComponent<Rigidbody2D>();

    rb.velocity = direction * speed;

    float length = rb.velocity.magnitude;
    if (length > Mathf.Epsilon)
    {
      moveTimer += speed * length * Speed;
      Vector3 pos = body.localPosition;
      pos.y = 0.35f + Mathf.Abs(Mathf.Sin(moveTimer * Mathf.Deg2Rad)) * Height;
      body.localPosition = pos;
    }
    else
    {
      Vector3 pos = body.localPosition;
      pos.y = 0.35f;
      body.localPosition = pos;
    }
  }
}

}
