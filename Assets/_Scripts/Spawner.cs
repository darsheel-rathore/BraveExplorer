using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Spawner : MonoBehaviour
{
    private List<SpawnPoints> spawnPointList;
    private List<Character> spawnedCharacterList;
    private bool hasSpawned;
    public Collider boxCollider;
    public UnityEvent OnAllSpawnedCharacterEliminated;

    private void Awake()
    {
        SpawnPoints[] spawnPointArray = transform.parent.GetComponentsInChildren<SpawnPoints>();
        spawnPointList = new List<SpawnPoints>(spawnPointArray);
        spawnedCharacterList = new List<Character>();  
    }

    private void Update()
    {
        if (!hasSpawned || spawnedCharacterList.Count == 0) return;

        bool allSpawnedEnemyAreDead = true;

        foreach (Character character in spawnedCharacterList)
        {
            if (character.currentState != Character.CharacterState.DEAD)
            {
                allSpawnedEnemyAreDead = false;
                break;
            }
        }

        if(allSpawnedEnemyAreDead)
        {
            if (OnAllSpawnedCharacterEliminated != null)
                OnAllSpawnedCharacterEliminated.Invoke();

            spawnedCharacterList.Clear();
        }
    }

    public void SpawnCharacters()
    {
        if (hasSpawned)
            return;

        hasSpawned = true;

        foreach (var point in spawnPointList)
        {
            if (point.enemyToSpawn != null)
            {
                GameObject spawnedGameObject = Instantiate(point.enemyToSpawn, point.transform.position, point.transform.rotation);

                // Add to the list
                spawnedCharacterList.Add(spawnedGameObject.GetComponent<Character>());
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            SpawnCharacters();
        }
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, boxCollider.bounds.size);
    }
}
