using UnityEngine;

public class GridSpawnPattern : SpawnPattern
{
    [SerializeField, Min(1)] private int columns = 3;
    [SerializeField, Min(1)] private int rows = 2;

    [SerializeField] private Vector2 spacing = new Vector2(2f, 2f);

    public override Vector3 GetSpawnPosition(int spawnIndex)
    {
        //converts spawnIndex into a grid coordinate
        int column = spawnIndex % columns;
        int row = spawnIndex / columns;

        //centre the grid around the pattern's position
        float x = (column - (columns - 1) / 2f) * spacing.x;
        float y = ((rows - 1) / 2f - row) * spacing.y;

        return transform.position + new Vector3(x, y, 0f);
    }
}
