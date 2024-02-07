using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flags : MonoBehaviour
{
    [Header("Flag Materials")]
    public GameObject flagPrefab;  // Team 1 flag (green)
    public GameObject flag2Prefab; // Team 2 flag (yellow)
    public Material lineMaterial; // Material for line renderer

    [Header("References")]
    private float flagCounter;
    private float flagCounter2;
    private List<GameObject> team1Flags = new List<GameObject>(); // List to keep track of spawned flags
    private List<GameObject> team2Flags = new List<GameObject>(); // List to keep track of spawned flags for the second team

    private Vector3 originalFlagPosition; // store the original position when starting to drag
    private bool isDragging; // To track if currently dragging a flag




    private GameObject currentlyDraggingFlag; // Flag currently being dragged
    private LineRenderer lineRenderer; // LineRenderer to show dragging line
    private Vector3 draggingOffset;

    // Start is called before the first frame update
    void Start()
    {

        // Initialize the LineRenderer
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        lineRenderer.material = lineMaterial;
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.positionCount = 0; // Start with no line
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        clickPosition.z = 0; // "ground level" to make sure flag is visible to camera
        if (Input.GetMouseButtonDown(0)) // left click for green team spawn
        {
            StartDragging(ref team1Flags, clickPosition, flagPrefab, ref flagCounter);
        }
        else if (Input.GetMouseButtonDown(1)) // right click for yellow team spawn
        {
            StartDragging(ref team2Flags, clickPosition, flag2Prefab, ref flagCounter2);
        }
        else if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
        {
            StopDragging(clickPosition);
        }
        else if ((Input.GetMouseButton(0) || Input.GetMouseButton(1)) && currentlyDraggingFlag != null)
        {
            DragFlag(clickPosition);
        }

    }

    

    private void StartDragging(ref List<GameObject> flagList, Vector3 mousePosition, GameObject flagPrefab, ref float flagCount)
    {
        // Attempt to pick up a flag
        GameObject closestFlag = FindClosestFlag(flagList, mousePosition);
        if (closestFlag != null && Vector3.Distance(closestFlag.transform.position, mousePosition) < 0.5f)
        {
            currentlyDraggingFlag = closestFlag;
            draggingOffset = currentlyDraggingFlag.transform.position - mousePosition;
            // Initialize the line for dragging
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, currentlyDraggingFlag.transform.position);
            lineRenderer.SetPosition(1, currentlyDraggingFlag.transform.position);
        }
        else if (flagCount < 2)
        {
            // Place a new flag if there are fewer than two
            PlaceFlag(ref flagList, mousePosition, flagPrefab, ref flagCount);
        }
        else
        {
            // Move the closest flag if two are already placed
            MoveFlag(ref flagList, mousePosition);
        }
    }

    private void DragFlag(Vector3 mousePosition) // logic during dragging
    {
        // Update the line to the current mouse position
        lineRenderer.SetPosition(1, mousePosition + draggingOffset);
    }

    private void StopDragging(Vector3 mousePosition) // logic when dragging stops
    {
        // Drop the flag at the new position
        if (currentlyDraggingFlag != null)
        {
            currentlyDraggingFlag.transform.position = mousePosition + draggingOffset;
        }
        currentlyDraggingFlag = null;
        lineRenderer.positionCount = 0;
    }

    // (The PlaceFlag method is the same as the first part of old MoveFlag)
    private void PlaceFlag(ref List<GameObject> flagList, Vector3 position, GameObject flagPrefab, ref float flagCount)
    {
        GameObject newFlag = Instantiate(flagPrefab, position, Quaternion.identity);
        flagList.Add(newFlag);
        flagCount++;
    }

    // The MoveFlag method now just moves the nearest flag when 2 exist already
    private void MoveFlag(ref List<GameObject> flagList, Vector3 position)
    {
        GameObject closestFlag = FindClosestFlag(flagList, position);
        if (closestFlag != null)
        {
            closestFlag.transform.position = position;
        }
    }


    private GameObject FindClosestFlag(List<GameObject> flags, Vector3 position) // scan for flag closest to click/drag
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

/*private void MoveFlag(ref List<GameObject> flagList, GameObject flagPrefab, ref float flagCount)
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
    }*/


/* if (flagCounter2 < 2)
        {
            Vector3 spawnPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 offset = new Vector3(0, 0, 10); // make sure flag spawns in view of camera
            Instantiate(flag2, spawnPosition + offset, Quaternion.identity);
            flagCounter2++; // limit the amt of yellow flags to 2
            // search for closest flag, delete it, and place a new one wherever you clicked
           // insert code here
        }*/