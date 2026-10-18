using UnityEngine;

[System.Serializable]
public class EnemySpawnEntry
{
    public GameObject enemyPrefab;

    [Min(0f)]
    public float spawnWeight = 1f;
}
