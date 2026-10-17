using UnityEngine;

// Inherits SpawnPattern and overrides GetSpawnPosition()
public class RandomSpawnPattern : SpawnPattern
{
    [SerializeField] private Vector2 areaSize = new Vector2(8f, 4f);

    public override Vector3 GetSpawnPosition(int spawnIndex)
    {
        float x = Random.Range(-areaSize.x / 2f, areaSize.x / 2f);
        float y = Random.Range(-areaSize.y / 2f, areaSize.y / 2f);

        return transform.position + new Vector3(x, y, 0f);
    }
}
