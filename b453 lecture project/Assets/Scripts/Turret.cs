using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    // Reference to this billion's TeamIdentifier component
    private TeamIdentifier myTeamIdentifier;

    void Start()
    {
        // Initialize myTeamIdentifier by getting the component from the parent billion
        myTeamIdentifier = GetComponentInParent<TeamIdentifier>();
    }

    // Update is called once per frame
    void Update()
    {
        FindNearestOpponent();
    }


    Billions FindNearestOpponent()
    {
        // Get all the billions in the scene
        Billions[] allBillions = FindObjectsOfType<Billions>();
        Billions nearestOpponent = null;
        float nearestDistance = Mathf.Infinity;

        foreach (Billions billion in allBillions)
        {
            // Ignore this billion
            if (billion.gameObject == this.gameObject) continue;

            TeamIdentifier billionTeamIdentifier = billion.GetComponent<TeamIdentifier>();

            // Ignore billions on the same team
            if (billionTeamIdentifier.teamColor == myTeamIdentifier.teamColor) continue;

            float distance = Vector2.Distance(transform.position, billion.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestOpponent = billion;
            }
        }

        return nearestOpponent;
    }
}
