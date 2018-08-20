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

    public int maxEnemies = 3;

    public int activeSpawns;
    public int activeBarrels;
    public int activeEnemies;

    [OdinSerialize] public List<SpawnPointSpawn> spawnPoints;
    [OdinSerialize] public List<GameObject> Prefabs;

    public Transform playerTransform;
    private void Awake()
    {
        if (spawnPoints.Any()) return;
        SetSpawnPoints(); if (setPrefabsOnStart) { SetPrefabs(); }
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
            if (playerTransform != null)
            {
                if (Vector2.Distance(playerTransform.position, s.SpawnPoint.transform.position) < 1.5f)
                {
                    Debug.Log("Skipping because player is blocking the spawn point");
                    continue;
                }
            }

            GameObject go = null;

            var tags = s.Prefab.MultiTags();
            var spawned = false;
            foreach (var tag in tags)
            {
                switch (tag)
                {
                    case "Item":
                        Item it = ItemSystemUtility.GetRandomItemCopy<Item>(ItemType.Item);
                        go = ItemInstance.CreateItemInstance((ItemItems)it.itemID, s.SpawnPoint.transform.position);
                        spawned = true;
                        break;
                    case "Enemy":
                        if (activeEnemies < maxEnemies)
                        {
                            go = Instantiate(s.Prefab, s.SpawnPoint.transform.position, Quaternion.identity);
                            spawned = true;
                            activeEnemies++;
                        }
                        else
                        {
                            Debug.Log("Skipped because max enemies!");
                        }
                        break;
                    default:
                        go = Instantiate(s.Prefab, s.SpawnPoint.transform.position, Quaternion.identity);
                        spawned = true;
                        break;
                }
                if (spawned) { break; }
            }

            if (go == null) continue;

            go.transform.SetParent(s.SpawnPoint.transform);
            go.AddTag("SpawnedObject");

        }
    }

    public void Start()
    {
        if (playerTransform == null) { playerTransform = MultiTags.FindGameObjectWithMultiTag("Player").transform; }

        SpawnAll();
        StartCoroutine(SpawnMore());
    }

    IEnumerator SpawnMore()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnTimer);
            Spawn(1);
            SetPrefabs();
        }
    }

    private void Spawn(int amount)
    {
        if (activeSpawns + amount > maxSpawns) return;

        var spawnLimit = amount;
        var usedPoints = new List<int>(amount);

        for (int i = 0; i < spawnLimit; i++)
        {
            //Get a random spawn point to use and make sure we didn't already use it
            int index = Random.Range(0, spawnPoints.Count);
            while (usedPoints.Contains(index) || transform.GetChild(0).GetChild(index).childCount > 0)
            {
                index = Random.Range(0, spawnPoints.Count);
            }

            usedPoints.Add(index);

            //Instantiate the prefab
            var s = spawnPoints[index];
            var playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
            if (Vector2.Distance(playerTransform.position, s.SpawnPoint.transform.position) < 1.5f)
            {
                Debug.Log("Skipping because player is blocking the spawn point");
                continue;
            }

            GameObject go = null;
            var tags = s.Prefab.MultiTags();
            var spawned = false;
            foreach (var tag in tags)
            {
                switch (tag)
                {
                    case "Item":
                        Item it = ItemSystemUtility.GetRandomItemCopy<Item>(ItemType.Item);
                        go = ItemInstance.CreateItemInstance((ItemItems)it.itemID, s.SpawnPoint.transform.position);
                        spawned = true;
                        break;
                    case "Enemy":
                        if (activeEnemies < maxEnemies)
                        {
                            go = Instantiate(s.Prefab, s.SpawnPoint.transform.position, Quaternion.identity);
                            spawned = true;
                        }
                        else
                        {
                            Debug.Log("Max Enemies Reached");
                        }
                        break;
                    default:
                        go = Instantiate(s.Prefab, s.SpawnPoint.transform.position, Quaternion.identity);
                        spawned = true;
                        break;
                }
                if (spawned) { break; }
            }

            if (go == null) continue;
            go.transform.SetParent(s.SpawnPoint.transform);
            go.AddTag("SpawnedObject");
        }

    }

    private void Update()
    {
        activeSpawns = MultiTags.FindGameObjectsWithMultiTagCount("SpawnedObject");
        activeBarrels = MultiTags.FindGameObjectsWithMultiTagCount("Barrel");
        activeEnemies = MultiTags.FindGameObjectsWithMultiTagCount("Enemy");
    }
}