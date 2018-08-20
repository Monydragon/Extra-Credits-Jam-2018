using UnityEngine;

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