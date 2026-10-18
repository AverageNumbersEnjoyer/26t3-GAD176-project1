using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class EnemySpawner : MonoBehaviour
{
    //[SerializeField] private GameObject enemyPrefab;
    //reference to EnemySelectionPattern class
    [SerializeField] private EnemySelectionPattern enemySelectionPattern;
    //[SerializeField] private Vector2 spawnAreaSize = new Vector2(8f, 4f);
    [SerializeField] private SpawnPattern spawnPattern;

    [SerializeField] private int currEnemyCount = 0;
    [SerializeField, Min(1)] private int targetEnemyCount = 3;
    [SerializeField] private float spawnInterval = 1.5f;
       

    // hashset over list to easily avoid duplicates
    private readonly HashSet<Enemy> activeEnemies = new HashSet<Enemy>();
    private bool spawningFinished;

    //observer
    public UnityEvent WaveCompleted = new UnityEvent();

    private void Start()
    {
        StartCoroutine(SpawnWave());

    }
    /*
    // weight table to determine which enemy is spawned
    private GameObject SelectEnemyPrefab()
    {
        float totalWeight = 0f;

        foreach (EnemySpawnEntry entry in enemyTypes)
        {
            if (entry != null && entry.enemyPrefab != null)
            {
                totalWeight += Mathf.Max(0f, entry.spawnWeight);
            }
        }

        // nothing spawns if nothing in table
        if (totalWeight <= 0f)
        {
            return null;
        }

        float roll = Random.Range(0f, totalWeight);

        foreach (EnemySpawnEntry entry in enemyTypes)
        {
            if (entry == null || entry.enemyPrefab == null)
            {
                continue;
            }

            float weight = Mathf.Max(0f, entry.spawnWeight);

            if (roll < weight)
            {
                return entry.enemyPrefab;
            }

            roll -= weight;
        }

        return null;
    }
    */


    private IEnumerator SpawnWave()
    {

        

        // null check and throw error
        if (enemySelectionPattern == null)
        {
            Debug.LogError("EnemySpawner: No selection pattern assigned");
            yield break;
        }
        if (spawnPattern == null)
        {
            Debug.LogError("EnemySpawner: No spawn pattern assigned");
            yield break;
        }

        // start from 0
        currEnemyCount = 0;
        spawningFinished = false;

        // loop spawn routine
        while (currEnemyCount < targetEnemyCount)
        {
            // 
            Vector3 spawnPosition = spawnPattern.GetSpawnPosition(currEnemyCount);
            GameObject selectedPrefab = enemySelectionPattern.SelectEnemyPrefab(currEnemyCount);

            GameObject instance = Instantiate(
                selectedPrefab,
                spawnPosition,
                Quaternion.identity
                );

            Enemy enemy = instance.GetComponent<Enemy>();

            if (enemy == null)
            {
                Debug.LogError("Enemy prefab must have an Enemy component", instance);
                Destroy(instance);
                yield break;
            }

            activeEnemies.Add(enemy);
            enemy.Removed += HandleEnemyRemoved;

            currEnemyCount++;

            Debug.Log($"Spawned enemy {currEnemyCount}/{targetEnemyCount}");

            if (currEnemyCount < targetEnemyCount)
            {
                // ensures the system isn't overloaded incase a large quantity of enemies need to be spawned
                yield return new WaitForSeconds(spawnInterval);
            }
        }

        spawningFinished = true;
        CheckWaveCompletion();



    }
    /*
    private Vector2 GetRandomSpawnPosition()
    {
        float x = Random.Range(
            -spawnAreaSize.x / 2f,
            spawnAreaSize.x / 2f
            );

        float y = Random.Range(
            -spawnAreaSize.y / 2f,
            spawnAreaSize.y / 2f
            );

        return transform.position = new Vector2(x, y);
    }
    */

    // remove enemies from the hashset
    private void HandleEnemyRemoved(Enemy enemy)
    {
        enemy.Removed -= HandleEnemyRemoved;
        activeEnemies.Remove(enemy);

        CheckWaveCompletion();
    }

    private void CheckWaveCompletion()
    {
        // check if we're still spawning or if there's still enemies alive
        if (!spawningFinished || activeEnemies.Count > 0)
        {
            return;
        }
        // if not signal the wave as complete
        Debug.Log("Wave Completed");
        WaveCompleted?.Invoke();
    }

}


