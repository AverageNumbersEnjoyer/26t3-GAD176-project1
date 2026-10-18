using System.Collections.Generic;
using UnityEngine;

public class MixedEnemySelection : EnemySelectionPattern
{
    [SerializeField] private GameObject[] enemyTypes;

    private readonly List<GameObject> selectionOrder = new List<GameObject>();

    public override GameObject SelectEnemyPrefab(int spawnIndex)
    {
        // start a new shuffled sequence for each wave.
        if (spawnIndex == 0 || selectionOrder.Count == 0)
        {
            ShuffleEnemyTypes();
        }

        if (selectionOrder.Count == 0)
        {
            return null;
        }

        // cycle through the shuffled types if needed.
        return selectionOrder[spawnIndex % selectionOrder.Count];
    }

    private void ShuffleEnemyTypes()
    {
        // reset selection order
        selectionOrder.Clear();

        foreach (GameObject enemyType in enemyTypes)
        {
            if (enemyType != null)
            {
                selectionOrder.Add(enemyType);
            }
        }

        // fisher-yates shuffle
        // I did not want to use System.Linq for this tbh
        // start from the end (i), pick a random index (j), swap them, decrement until we reach the start
        for (int i = selectionOrder.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);

            GameObject temp = selectionOrder[i];
            selectionOrder[i] = selectionOrder[j];
            selectionOrder[j] = temp;
        }
    }
}