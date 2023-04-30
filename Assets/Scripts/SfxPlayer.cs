
using UnityEngine;

namespace disusdev
{

public class SfxPlayer : Singleton<SfxPlayer>
{
  public enum SfxType
  {
    Damage,
    Pistol,
    Shotgun,
    Flame,
    RocketShot,
    RocketBlow,
    Death,
    Select,
    Click,

    COUNT
  }

  public FMODUnity.EventReference[] Sounds;
  public FMODUnity.EventReference Music;

  private FMOD.Studio.EventInstance instance;

  private void Start()
  {
    Debug.AssertFormat(Sounds.Length == (int)SfxType.COUNT, "Sounds should be same length as an enum SfxType!");

    instance = FMODUnity.RuntimeManager.CreateInstance(Music);

    instance.start();
  }

  public void PlaySfx(SfxType type)
  {
    FMODUnity.EventReference sfx = Sounds[(int)type];

    FMODUnity.RuntimeManager.PlayOneShot(sfx);
  }

  public void ChangeMusic(string label)
  {
    instance.setParameterByNameWithLabel("State", label);
  }
}

}
