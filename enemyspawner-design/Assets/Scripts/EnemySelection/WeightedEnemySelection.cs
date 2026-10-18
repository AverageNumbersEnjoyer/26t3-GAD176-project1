using UnityEngine;

public class WeightedEnemySelection : EnemySelectionPattern
{
    [SerializeField] private EnemySpawnEntry[] enemyTypes;


    // essentially a random drop table for enemies
    // sum total weights, pick a random number between zero and total
    // iterate through the list of items until the number falls within an items weight range
    public override GameObject SelectEnemyPrefab(int spawnIndex)
    {
        // weight table to determine which enemy is spawned
        float totalWeight = 0f;

        // add the weight of all enemies
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

        // iterate through enimies
        foreach (EnemySpawnEntry entry in enemyTypes)
        {
            if (entry == null || entry.enemyPrefab == null)
                continue;

            float weight = Mathf.Max(0f, entry.spawnWeight);

            if (roll < weight)
                return entry.enemyPrefab;

            // shift the roll down
            roll -= weight;
        }

        return null;
    }
}