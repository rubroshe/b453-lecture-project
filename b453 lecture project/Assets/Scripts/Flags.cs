using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flags : MonoBehaviour
{
    public GameObject flagPrefab;  // Team 1 flag (green)
    public GameObject flag2Prefab; // Team 2 flag (yellow)
    public Material lineMaterial; // Material for line renderer

    private float flagCounter;
    private float flagCounter2;
    private List<GameObject> team1Flags = new List<GameObject>(); // List to keep track of spawned flags
    private List<GameObject> team2Flags = new List<GameObject>(); // List to keep track of spawned flags for the second team

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // left click for green spawn
        {
            MoveFlag(ref team1Flags, flagPrefab, ref flagCounter);

            
        }

        if (Input.GetMouseButtonDown(1)) // right click for yellow spawn
        {
            MoveFlag(ref team2Flags, flag2Prefab, ref flagCounter2);
        }
    }

    private void MoveFlag(ref List<GameObject> flagList, GameObject flagPrefab, ref float flagCount)
    {
        Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        clickPosition.z = 0; // "ground level" to make sure flag is visible to camera

        if (flagCount < 2) // limit the amt of flags in scene to 2 
        {
            GameObject newFlag = Instantiate(flagPrefab, clickPosition, Quaternion.identity);
            flagList.Add(newFlag);
            flagCount++; 
        }
        else
        {
            // If there are already 2 flags, move the closest flag to the click position
            GameObject closestFlag = FindClosestFlag(flagList, clickPosition);
            if (closestFlag != null)
            {
                closestFlag.transform.position = clickPosition;
            }
        }
    }

    private GameObject FindClosestFlag(List<GameObject> flags, Vector3 position)
    {
        GameObject closestFlag = null;
        float closestDistance = Mathf.Infinity;
        
        foreach (GameObject flag in flags)
        {
            float distance = (flag.transform.position - position).sqrMagnitude;
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestFlag = flag;
            }
        }
        return closestFlag;
    }
}

// old way of instantiating in case all goes wrong
/* if (flagCounter2 < 2)
        {
            Vector3 spawnPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 offset = new Vector3(0, 0, 10); // make sure flag spawns in view of camera
            Instantiate(flag2, spawnPosition + offset, Quaternion.identity);
            flagCounter2++; // limit the amt of yellow flags to 2
            // search for closest flag, delete it, and place a new one wherever you clicked
           // insert code here
        }*/