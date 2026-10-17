using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;

    [SerializeField] private int currEnemyCount = 0;
    [SerializeField, Min(1)] private int targetEnemyCount = 3;
    [SerializeField] private float spawnInterval = 1.5f;

    //temporary
    //[SerializeField] private Vector2 spawnAreaSize = new Vector2(8f, 4f);
    [SerializeField] private SpawnPattern spawnPattern;

    // hashset over list to easily avoid duplicates
    private readonly HashSet<Enemy> activeEnemies = new HashSet<Enemy>();
    private bool spawningFinished;

    //observer
    public UnityEvent WaveCompleted = new UnityEvent();

    private void Start()
    {
        StartCoroutine(SpawnWave());

    }

    private IEnumerator SpawnWave()
    {
        //null check and throw error
        if (enemyPrefab == null)
        {
            Debug.LogError("EnemySpawner: No enemy prefab assigned");
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

        // spawn routine
        while (currEnemyCount < targetEnemyCount)
        {
            Vector3 spawnPosition = spawnPattern.GetSpawnPosition(currEnemyCount);

            GameObject instance = Instantiate(
                enemyPrefab,
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

    private void HandleEnemyRemoved(Enemy enemy)
    {
        enemy.Removed -= HandleEnemyRemoved;
        activeEnemies.Remove(enemy);

        CheckWaveCompletion();
    }

    private void CheckWaveCompletion()
    {
        if (!spawningFinished || activeEnemies.Count > 0)
        {
            return;
        }

        Debug.Log("Wave Completed");
        WaveCompleted?.Invoke();
    }

}


