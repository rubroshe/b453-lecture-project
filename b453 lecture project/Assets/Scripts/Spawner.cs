using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject billionPrefab; // Assign your billion prefab in the inspector
    public float spawnInterval = 5f; // Time between each spawn
    private float timeSinceLastSpawn;

    [SerializeField] private int currentRank = 0;

    [SerializeField] private BillionaireBase thisSpawnersBase;

    private void Start()
    {
        timeSinceLastSpawn = spawnInterval; // Start the first spawn after the interval
        Events.rankChange.AddListener(HandleRankChange);
    }

    private void HandleRankChange(int newRank, TeamColor team)
    {
        // Check if this spawner's team matches the event's team
        TeamIdentifier myTeamIdentifier = GetComponent<TeamIdentifier>();
        if (myTeamIdentifier.teamColor == team)
        {
            currentRank = newRank;
            // get billion prefab TextMeshPro 
        // failed experiment:    billionPrefab.GetComponentInChildren<TextMeshPro>().text = newRank.ToString();
        }
    }

    private void Update()
    {
        int spawnedBillionRank = thisSpawnersBase.baseRank;
        timeSinceLastSpawn += Time.deltaTime;

        if (timeSinceLastSpawn >= spawnInterval)
        {
            SpawnBillion(currentRank);
            timeSinceLastSpawn = 0f;
        }
    }

    void SpawnBillion(int rank)
    {
        Vector3 spawnPosition = GetSpawnPosition();
        // change rank of the billion before spawning it in
        // ...billionPrefab .... = spawnedBillionRank;
        GameObject newBillion = Instantiate(billionPrefab, spawnPosition, Quaternion.identity);

        // Set rank according to the spawner's base
        Billions billionComponent = newBillion.GetComponent<Billions>();
        if (billionComponent != null)
        {
            billionComponent.SetRank(thisSpawnersBase.baseRank);
        }
        else
        {
            Debug.LogError("Failed to find Billion component on the new billion instance.");
        }
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
