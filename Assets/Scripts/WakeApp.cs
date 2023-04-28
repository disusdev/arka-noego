
using UnityEngine;

namespace disusdev
{

public class WakeApp
{

  [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
  private static void Wake()
  {
    Cursor.visible = false;

    var wapp = Object.Instantiate(Resources.Load("WakeApp")) as GameObject;
    if (wapp == null)
    {
      Debug.LogError("No WakeApp prefab in Resources!");
    }

    Object.DontDestroyOnLoad(wapp);
  }

}

}
