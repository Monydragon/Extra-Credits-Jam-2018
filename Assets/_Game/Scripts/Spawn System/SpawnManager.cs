using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Sirenix.Utilities;
using Unity.Linq;
using UnityEngine;
using ItemSystem;

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
            Prefab = prefab;
        }
    }

    [OdinSerialize] public List<SpawnPointSpawn> spawnPoints;
    [OdinSerialize] public List<GameObject> Prefabs;

    [Range(0, 30)]
    public int minSpawns = 8, maxSpawns = 10;


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

    void Start()
    {
        int amountOfSpawns = Random.Range(minSpawns, maxSpawns + 1);
        var usedPoints = new List<int>(amountOfSpawns);
        for (int i = 0; i < amountOfSpawns && i < spawnPoints.Count; i++)
        {
            //Get a random spawn point to use and make sure we didn't already use it
            int index = Random.Range(0, spawnPoints.Count);
            while (usedPoints.Contains(index))
                index = Random.Range(0, spawnPoints.Count);

            usedPoints.Add(index);

            //Instantiate the prefab
            var s = spawnPoints[index];
            if (s.Prefab.name != "Item")
            {
                var go = Instantiate(s.Prefab, s.SpawnPoint.transform.position, Quaternion.identity);
            }

            else
            {
                Item it = ItemSystemUtility.GetRandomItemCopy<Item>(ItemType.Item);
                ItemInstance.CreateItemInstance((ItemItems)it.itemID, s.SpawnPoint.transform.position);
            }
        }
    }
}
