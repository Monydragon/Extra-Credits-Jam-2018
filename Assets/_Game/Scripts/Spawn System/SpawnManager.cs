using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Sirenix.Utilities;
using Unity.Linq;
using UnityEngine;
using ItemSystem;

public struct IntMinMax
{
    public int Min, Max;

    public int GetRandom() => Random.Range(Min, Max + 1);

}
public class SpawnManager : MonoBehaviour
{
    [Range(0, 30)] public int minSpawns = 6, maxSpawns = 12;
    public bool setPrefabsOnStart = true;
    public float spawnTimer = 5.0f;

    public int activeSpawns;

    [OdinSerialize] public List<SpawnPointSpawn> spawnPoints;
    [OdinSerialize] public List<GameObject> Prefabs;

    private void Awake()
    {
        if (spawnPoints.Any()) return;
        SetSpawnPoints(); if(setPrefabsOnStart){SetPrefabs();}
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

    public void SpawnAll()
    {
        int amountOfSpawns = Random.Range(minSpawns, maxSpawns + 1);
        var usedPoints = new List<int>(amountOfSpawns);
        for (int i = 0; i < amountOfSpawns && i < spawnPoints.Count; i++)
        {
            //Get a random spawn point to use and make sure we didn't already use it
            int index = Random.Range(0, spawnPoints.Count);
            while (usedPoints.Contains(index))
            {
                index = Random.Range(0, spawnPoints.Count);
            }

            usedPoints.Add(index);

            //Instantiate the prefab
            var s = spawnPoints[index];
            GameObject go = null;

            switch (s.Prefab.tag)
            {
                case "Item":
                    Item it = ItemSystemUtility.GetRandomItemCopy<Item>(ItemType.Item);
                    go = ItemInstance.CreateItemInstance((ItemItems)it.itemID, s.SpawnPoint.transform.position);
                    break;
                default:
                    go = Instantiate(s.Prefab, s.SpawnPoint.transform.position, Quaternion.identity);
                    break;
            }

            go.transform.SetParent(s.SpawnPoint.transform);
            go.tag = "SpawnedObject";

            //            activeSpawns++;
        }
    }
    public void Start()
    {
        SpawnAll();
    }

    private void Update()
    {
        activeSpawns = GameObject.FindGameObjectsWithTag("SpawnedObject").Length;
//        var spawnPoints = gameObject.Descendants(x => x.tag == "SpawnPoint").ToArray();
//        activeSpawns = spawnPoints.Count(x => x.Children().Any());
    }


}
