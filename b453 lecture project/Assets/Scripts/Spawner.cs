using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject billionPrefab; // Assign your billion prefab in the inspector
    public float spawnInterval = 5f; // Time between each spawn
    private float timeSinceLastSpawn;

    [SerializeField] private BillionaireBase thisSpawnersBase;

    private void Start()
    {
        timeSinceLastSpawn = spawnInterval; // Start the first spawn after the interval
    }

    private void Update()
    {
        timeSinceLastSpawn += Time.deltaTime;

        if (timeSinceLastSpawn >= spawnInterval)
        {
            SpawnBillion();
            timeSinceLastSpawn = 0f;
        }
    }

    void SpawnBillion()
    {
        Vector3 spawnPosition = GetSpawnPosition();
        GameObject newBillion = Instantiate(billionPrefab, spawnPosition, Quaternion.identity);

        // Set rank according to the spawner's base
        int currentRank = thisSpawnersBase.baseRank;

        // set the rank of the billion
        // visual script ref here
    }

    float GetRandomOffset(float min, float max) // similar to random.range
    {
        float value = Random.Range(min, max);

        while(value > 0.2275f && value < 0.455f)
        {
            value = Random.Range(min, max);
        }

        return value;
    }

    Vector3 GetSpawnPosition()
    {
        bool positionFound = false;
        Vector3 potentialPosition = Vector3.zero;
        int maxAttempts = 10;
        for (int i = 0; i < maxAttempts && !positionFound; i++)
        {
            Vector3 offset = new Vector3(GetRandomOffset(-1,1), GetRandomOffset(-1,1), 0);
            potentialPosition = transform.position + offset;

            // Check if the position is free
            Collider2D[] colliders = Physics2D.OverlapCircleAll(potentialPosition, 0.18f);
            if (colliders.Length == 0) // No colliders means free space
            {
                positionFound = true;
            }
        }

        if (!positionFound)
        {
            Debug.LogWarning("Could not find a free position for spawning!");
            return transform.position; // Fallback to the base position
        }

        return potentialPosition;
    }

}
