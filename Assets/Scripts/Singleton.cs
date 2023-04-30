
namespace disusdev
{

public class Singleton<T> : UnityEngine.MonoBehaviour where T : Singleton<T>
{
  public static T Instance = null;

  protected virtual void Awake()
  {
    if (Instance != null)
    {
      Destroy(gameObject);
      return;
    }

    if (Instance == null)
    {
      Instance = FindObjectOfType<T>();
    }
    else if (Instance == this)
    {
      // Destroy(gameObject);

      // no hacks pls
      UnityEngine.Debug.Assert(false, "Double instancing of the singelton<" + typeof(T) + ">! On game object: " + gameObject.name);
    }

    // DontDestroyOnLoad(gameObject);
  }
}

}
