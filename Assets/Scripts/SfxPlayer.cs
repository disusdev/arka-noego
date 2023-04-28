
using UnityEngine;

namespace disusdev
{

public class SfxPlayer : Singleton<SfxPlayer>
{
  public enum SfxType
  {
    Gun_1,  
    Gun_2,
    Death,
    Pinguin_1,
    Pinguin_2,
    Point,
    Click
  }

  public FMODUnity.EventReference[] Sounds;

  public void PlaySfx(SfxType type)
  {
    FMODUnity.EventReference sfx = Sounds[(int)type];

    FMODUnity.RuntimeManager.PlayOneShot(sfx);
  }
}

}
