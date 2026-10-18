using UnityEngine;


public abstract class EnemySelectionPattern : MonoBehaviour
{
    public abstract GameObject SelectEnemyPrefab(int spawnIndex);
}
