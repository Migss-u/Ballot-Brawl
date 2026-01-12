using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemySpawner : MonoBehaviour
{
    [Header("Prefab to Spawn")]
    public GameObject prefab;

    [Header("Target Tilemap")]
    public Tilemap targetTilemap;

    [Header("Spawn Settings")]
    public int maxObjects = 3;
    public float spawnInterval = 10f;

    private List<Vector3> validPositions = new List<Vector3>();
    private Queue<GameObject> spawnedObjects = new Queue<GameObject>();

    void Start()
    {

        // Get all tile positions from the tilemap
        BoundsInt bounds = targetTilemap.cellBounds;
        TileBase[] allTiles = targetTilemap.GetTilesBlock(bounds);

        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int cellPosition = new Vector3Int(x, y, 0);

                if (targetTilemap.HasTile(cellPosition))
                {
                    // Convert cell position to world position
                    Vector3 worldPosition = targetTilemap.CellToWorld(cellPosition) + targetTilemap.tileAnchor;
                    validPositions.Add(worldPosition);
                }
            }
        }

        StartCoroutine(SpawnPrefabs());
    }

    IEnumerator SpawnPrefabs()
    {

        // Wait for 3 seconds before spawning the first object
        yield return new WaitForSeconds(3f);

        while (true)
        {
            SpawnPrefab();

            // Wait for the interval before spawning again
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnPrefab()
    {
        // Choose a random position from the list
        int randomIndex = Random.Range(0, validPositions.Count);
        Vector3 spawnPosition = validPositions[randomIndex];

        // Spawn the prefab at the chosen position
        GameObject newObject = Instantiate(prefab, spawnPosition, Quaternion.identity);

        // Add the new object to the queue
        spawnedObjects.Enqueue(newObject);

        // If we exceed the max number of objects, destroy the oldest one
        if (spawnedObjects.Count > maxObjects)
        {
            GameObject oldestObject = spawnedObjects.Dequeue();
            Destroy(oldestObject);
        }
    }
}
