using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TurretController : MonoBehaviour
{ // attach this script to the parent (billion) of the turret
    
    public Transform turret; // Assign the child turret transform in the inspector

    void Update()
    {
        GameObject nearestOpponent = FindNearestOpponent();
        if (nearestOpponent != null)
        {
            Vector3 targetDirection = nearestOpponent.transform.position - transform.position;
            float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
          // ****simpler way**** both work turret.transform.up = targetDirection;
            turret.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward); // Assume the sprite is facing up
        }
        // If there's no opponent, you set a default direction or leave it as is
    }

    GameObject FindNearestOpponent()
    {
        GameObject[] billions = GameObject.FindGameObjectsWithTag("Billions"); // scan for all billions
        GameObject nearestOpponent = null;
        float nearestDistance = Mathf.Infinity;

        foreach (GameObject billion in billions)
        {
            TeamIdentifier teamIdentifier = billion.GetComponent<TeamIdentifier>();
            if (teamIdentifier != null && teamIdentifier.teamColor != GetComponent<TeamIdentifier>().teamColor)
            {
                float distance = Vector3.Distance(transform.position, billion.transform.position);
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestOpponent = billion;
                }
            }
        }
        return nearestOpponent;
    }
}
