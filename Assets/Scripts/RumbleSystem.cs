using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace disusdev
{

public class RumbleSystem : Singleton<RumbleSystem>
{
  private int[] activeRumbles = new int[4];

  private IEnumerator PlayRumble(int id, float speed, float time)
  {
    Gamepad.all[id].SetMotorSpeeds(speed, speed);
    activeRumbles[id]++;
    yield return new WaitForSeconds(time);
    if (activeRumbles[id] < 2)
    {
      Gamepad.all[id].SetMotorSpeeds(0.0f, 0.0f);
    }
    activeRumbles[id]--;
  }

  public void Play(int id, float speed, float time)
  {
    StartCoroutine(PlayRumble(id, speed, time));
  }
}

}
