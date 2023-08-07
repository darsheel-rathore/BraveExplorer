using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private List<SpawnPoints> spawnPointList;
    private bool hasSpawned;

    private void Awake()
    {
        var spawnPointArray = transform.parent.GetComponentsInChildren<SpawnPoints>();
        spawnPointList = new List<SpawnPoints>(spawnPointArray);
    }

    public void SpawnCharacters()
    {

    }
}
