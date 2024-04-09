using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillionRankVisual : MonoBehaviour
{
    public GameObject spikePrefab; // Assign a prefab for the spikes in the inspector
    public int rank; // The rank of this billion

    private void Start()
    {
        UpdateVisualRank();
    }

    public void SetRank(int newRank)
    {
        rank = newRank;
        UpdateVisualRank();
    }

    void UpdateVisualRank()
    {
        // Remove old spikes if they exist
        foreach (Transform child in transform)
        {
            if (child.CompareTag("Spike"))
            {
                Destroy(child.gameObject);
            }
        }

        // Add new spikes based on rank
        for (int i = 0; i < rank; i++)
        {
            // Instantiate spikes and position them around the billion
            // The specific implementation will depend on how you want to arrange the spikes
        }
    }
}
