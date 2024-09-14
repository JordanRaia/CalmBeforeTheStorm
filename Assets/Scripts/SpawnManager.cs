using UnityEngine;
using UnityEngine.Tilemaps;

public class SpawnManager : MonoBehaviour
{
    public Transform player; // Reference to the player
    public float minSpawnDistanceFromPlayer = 3f; // Minimum distance from player
    public Tilemap walkableTilemap; // Tilemap where enemies can spawn
    public Tilemap colliderTilemap; // Tilemap where enemies cannot spawn

    // Method to get a valid spawn position on the walkable tilemap
    public Vector2 GetValidSpawnPosition()
    {
        Vector2 spawnPosition;
        bool validPosition = false;

        do
        {
            // Get a random position from the walkable tilemap bounds
            spawnPosition = GetRandomPositionOnWalkableTilemap();

            // Check if the position is far enough from the player
            if (Vector2.Distance(spawnPosition, player.position) > minSpawnDistanceFromPlayer)
            {
                // Ensure the position is not blocked by a tile on the collider tilemap
                if (!IsPositionOnColliderTile(spawnPosition))
                {
                    validPosition = true;
                }
            }

        } while (!validPosition);

        return spawnPosition;
    }

    // Get a random position on the walkable tilemap
    Vector2 GetRandomPositionOnWalkableTilemap()
    {
        // Get the bounds of the walkable tilemap
        BoundsInt bounds = walkableTilemap.cellBounds;
        Vector3Int randomCellPosition;

        TileBase tile;
        do
        {
            // Pick a random cell within the bounds
            randomCellPosition = new Vector3Int(
                Random.Range(bounds.xMin, bounds.xMax),
                Random.Range(bounds.yMin, bounds.yMax),
                0
            );

            // Check if this cell has a valid walkable tile
            tile = walkableTilemap.GetTile(randomCellPosition);

        } while (tile == null); // Keep searching until a valid walkable tile is found

        // Convert the random cell position to world position and return
        return walkableTilemap.CellToWorld(randomCellPosition) + walkableTilemap.cellSize / 2;
    }

    // Check if a position is on the collider tilemap
    bool IsPositionOnColliderTile(Vector2 position)
    {
        Vector3Int gridPosition = colliderTilemap.WorldToCell(position);
        TileBase tile = colliderTilemap.GetTile(gridPosition);

        // If the tile exists on the collider tilemap, the position is invalid
        return tile != null;
    }
}
