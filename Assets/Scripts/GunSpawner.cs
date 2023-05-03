
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace disusdev
{

public class GunSpawner : Singleton<GunSpawner>
{
  public enum SpawnedGunType
  {
    Pistol,
    Auto,
    Shotgun,
    Rocket,
    Fire
  }

  [System.Serializable]
  public struct SpawnData
  {
    public GameObject gun;
    public float probability;
  }

  public Tilemap groundMap;
  public SpawnData[] SpawnsData;
  public int MaxGunsInGame = 5;

  private int spawnedGuns = 0;

  private List<int> occupiedSpawns = new List<int>();

  public void OnGunPicked(int spawn_index)
  {
    spawnedGuns--;
    occupiedSpawns.RemoveAll(si => si == spawn_index);
  }

  private void SpawnGun()
  {
    if (spawnedGuns >= MaxGunsInGame) return;

    float probability = Random.value;
    int spawn_index = Random.Range(0, SpawnPositions.Length);

    List<int> can_spawn = new List<int>();

    for (int i = 0; i < SpawnsData.Length; i++)
    {
      if (probability > (1.0f - SpawnsData[i].probability))
      {
        can_spawn.Add(i);
      }
    }

    int gun_index = 0;
    if (can_spawn.Count > 0)
    {
      gun_index = can_spawn[Random.Range(0, can_spawn.Count)];
    }

    Vector3 pos = SpawnPositions[spawn_index];
    pos.z = -1.0f;

    Gun gun = Instantiate(SpawnsData[gun_index].gun, pos, Quaternion.identity).GetComponent<Gun>();
    gun.spawn_point = spawn_index;
    gun.onPick += OnGunPicked;

    occupiedSpawns.Add(spawn_index);

    spawnedGuns++;

    InitialTimeToSpawn -= 0.1f;
  }

  private Vector2[] SpawnPositions;

  private void Start()
  {
    timer = InitialTimeToSpawn;

    BoundsInt bounds = groundMap.cellBounds;
    TileBase[] allTiles = groundMap.GetTilesBlock(bounds);

    Vector2Int[] directions =
    {
      new Vector2Int(1, 0)
    };

    List<Vector2> spawn_list = new List<Vector2>();

    for (int y = 0; y < bounds.size.y; y++)
    {
      for (int x = 0; x < bounds.size.x; x++)
      {
        TileBase tile = allTiles[x + y * bounds.size.x];

        if (tile != null)
        {
            foreach (var dir in directions)
            {
              int xx = x + dir.x;
              int yy = y + dir.y;
              TileBase t = allTiles[x + y * bounds.size.x];
              if (t == null) goto cant_place;
            }
        }
        else
        {
            goto cant_place;
        }

        Vector2 place_pos = new Vector2(bounds.xMin + x, bounds.yMin + y + 0.5f);
        spawn_list.Add(place_pos); x++;

        cant_place: continue;
      }
    }

    SpawnPositions = spawn_list.ToArray();
  }

  public float InitialTimeToSpawn = 1.0f;

  private float timer = 0.0f;

  public void EnableRocketSpawnMode()
  {
    for (int i = 0; i < SpawnsData.Length; i++)
    {
      SpawnsData[i].probability = 0.0f;
    }
    SpawnsData[(int)SpawnedGunType.Rocket].probability = 1.0f;
  }

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
