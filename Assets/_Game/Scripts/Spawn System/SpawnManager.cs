using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Sirenix.Utilities;
using Unity.Linq;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [System.Serializable]
    public class SpawnPointSpawn
    {
        public GameObject SpawnPoint;
        public GameObject Prefab;

        public SpawnPointSpawn()
        {
            
        }

        public SpawnPointSpawn(GameObject spawnPoint = null, GameObject prefab = null)
        {
            SpawnPoint = spawnPoint;
            prefab = prefab;
        }
    }

    [OdinSerialize] public List<SpawnPointSpawn> spawnPoints;
    [OdinSerialize] public List<GameObject> Prefabs;

    [Range(0, 30)]
    public int amountOfSpawns = 5;


    [ExecuteInEditMode]
    private void Awake()
    {
        if (!spawnPoints.Any()) { SetSpawnPoints(); }
    }

    [Button("Set SpawnPoints", ButtonSizes.Medium, ButtonStyle.Box, ButtonHeight = 15, Expanded = false)]
    private void SetSpawnPoints()
    {
        spawnPoints.Clear();
        var spawns = gameObject.Child("SpawnPoints").Children().ToList();
        foreach (GameObject spawn in spawns)
        {
            spawnPoints.Add(new SpawnPointSpawn(spawn, null));
        }
    }

    [Button("Set Prefabs", ButtonSizes.Medium, ButtonStyle.Box, ButtonHeight = 15, Expanded = false)]
    private void SetPrefabs()
    {
        if (spawnPoints.Any() && Prefabs.Any())
        {
            foreach (var spawn in spawnPoints)
            {
                var randomPrefabIndex = Random.Range(0, Prefabs.Count);

                spawn.Prefab = Prefabs[randomPrefabIndex];
            }
        }
        else
        {
            SetSpawnPoints();
            SetPrefabs();
        }
        
    }
    private void Start()
    {

    }


}
