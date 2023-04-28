
using System.Collections.Generic;
using UnityEngine;

namespace disusdev
{

public class GunSpawner : MonoBehaviour
{
  public static GunSpawner Instance;

  [System.Serializable]
  public struct SpawnData
  {
    public GameObject gun;
    public float probability;
  }

  public Transform[] SpawnPoints;
  public SpawnData[] SpawnsData;
  public bool[] GunInUse;

  public List<int> NotUsedIndices = new List<int>();

  private void SpawnGun()
  {
    NotUsedIndices.Clear();

    for (int i = 0; i < GunInUse.Length; i++)
    {
      if (!GunInUse[i])
      {
        NotUsedIndices.Add(i);
      }
    }

    if (NotUsedIndices.Count == 0) return;
    
    int rnd = Random.Range(0, NotUsedIndices.Count);

    int srnd = Random.Range(0, SpawnPoints.Length);

    Instantiate(SpawnsData[NotUsedIndices[rnd]].gun, SpawnPoints[srnd].position, Quaternion.identity);

    GunInUse[NotUsedIndices[rnd]] = true;

    InitialTimeToSpawn -= 0.1f;
  }

  public void FreeOne()
  {
    for (int i = 0; i < GunInUse.Length; i++)
    {
      if (GunInUse[i])
      {
        GunInUse[i] = false;
        return;
      }
    }
  }

  private void Awake()
  {
    Instance = this;
  }

  private void Start()
  {
    timer = InitialTimeToSpawn;
  }

  public float InitialTimeToSpawn = 1.0f;

  private float timer = 0.0f;

  public void Step(float dt)
  {
    timer -= dt;

    if (timer < 0)
    {
      timer = 0.0f;

      SpawnGun();

      timer = InitialTimeToSpawn;
    }
  }
}

}
