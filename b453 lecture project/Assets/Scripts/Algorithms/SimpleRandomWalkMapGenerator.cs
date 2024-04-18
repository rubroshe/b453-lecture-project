using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class SimpleRandomWalkMapGenerator : AbstractDungeonGenerator
{
/*    [SerializeField]
    protected Vector2Int startPosition = Vector2Int.zero;*/

    [SerializeField]
    private SimpleRandomWalkSO randomWalkParameters;

    // Trying to add bases alongside generation of dungeon
    [SerializeField]
    private GameObject[] prefabBases;  // Array of the 4 different base prefabs
    [SerializeField]
    private int wallMargin = 5;
    [SerializeField]
    private int baseMargin = 10;

    /*  [SerializeField]
      private TileMapVisualizer tileMapVisualizer;*/

    protected override void RunProceduralGeneration()
    {
        // Clear existing prefabs before generating new dungeon/prefabs
        ClearExistingPrefabs();


        HashSet<Vector2Int> floorPositions = RunRandomWalk();
        tileMapVisualizer.Clear();
        tileMapVisualizer.PaintFloorTiles(floorPositions);
        WallGenerator.CreateWalls(floorPositions, tileMapVisualizer);


        List<Vector2Int> validPositions = FilterValidPositions(floorPositions, wallMargin);
        SpawnPrefabs(validPositions, baseMargin);

    }

    // Method to clear existing prefabs from the scene
    private void ClearExistingPrefabs()
    {
        GameObject[] existingPrefabs = GameObject.FindGameObjectsWithTag("Base");
        foreach (var prefab in existingPrefabs)
        {
            DestroyImmediate(prefab);
        }
    }

    private List<Vector2Int> FilterValidPositions(HashSet<Vector2Int> floorTiles, int margin)
    {
        List<Vector2Int> validPositions = new List<Vector2Int>();
        foreach (var pos in floorTiles)
        {
            bool isValid = true;
            for (int x = -margin; x <= margin; x++)
            {
                for (int y = -margin; y <= margin; y++)
                {
                    if (x == 0 && y == 0) continue;  // Skip the center tile where the base itself would go
                    Vector2Int checkPos = new Vector2Int(pos.x + x, pos.y + y);
                    if (!floorTiles.Contains(checkPos))
                    {
                        isValid = false;
                        break;
                    }
                }
                if (!isValid) break;
            }
            if (isValid)
                validPositions.Add(pos);
        }
        return validPositions;
    }

    private void SpawnPrefabs(List<Vector2Int> validPositions, int separationDistance)
    {
        List<Vector2Int> placedPositions = new List<Vector2Int>();
        System.Random rand = new System.Random();

        // Attempt to place each prefab
        foreach (var prefab in prefabBases)
        {
            bool positionFound = false;
            int retryCount = 45;  // Number of retries to find a valid position

            for (int retry = 0; retry < retryCount && !positionFound; retry++)
            {
                if (validPositions.Count == 0) break;

                int index = rand.Next(validPositions.Count);
                Vector2Int pos = validPositions[index];

                if (IsFarEnoughFromOtherBases(pos, placedPositions, separationDistance))
                {
                    Instantiate(prefab, new Vector3(pos.x, pos.y, 0), Quaternion.identity);
                    placedPositions.Add(pos);
                    validPositions.RemoveAt(index);  // Remove to prevent another base from spawning too close
                    positionFound = true;
                }
                else
                {
                    validPositions.RemoveAt(index);  // Remove and try another position
                }
            }

            // Reshuffle the remaining valid positions if not found in this retry
            if (!positionFound)
            {
                validPositions.Shuffle();  // This assumes you have a method to shuffle List<T>
            }
        }
    }

    private bool IsFarEnoughFromOtherBases(Vector2Int pos, List<Vector2Int> placedPositions, int minDistance)
    {
        foreach (var placedPos in placedPositions)
        {
            if (Vector2Int.Distance(pos, placedPos) < minDistance)
                return false;
        }
        return true;
    }
    protected HashSet<Vector2Int> RunRandomWalk()
    {
        var currentPosition = startPosition;
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();
        for (int i = 0; i < randomWalkParameters.iterations; i++)
        {
            var path = ProceduralGenerationAlgorithms.SimpleRandomWalk(currentPosition, randomWalkParameters.walkLength);
            floorPositions.UnionWith(path);
            if (randomWalkParameters.startRandomlyEachIteration)
                currentPosition = floorPositions.ElementAt(Random.Range(0, floorPositions.Count));
        }
        return floorPositions;
       
    }
}

