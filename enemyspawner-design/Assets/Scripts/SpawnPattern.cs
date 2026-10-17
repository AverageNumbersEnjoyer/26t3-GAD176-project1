using UnityEngine;

// Abstract means this defines common contract
public abstract class SpawnPattern : MonoBehaviour
{
    // index tells the pattern which enemy is being spawned
    public abstract Vector3 GetSpawnPosition(int spawnIndex);
}
