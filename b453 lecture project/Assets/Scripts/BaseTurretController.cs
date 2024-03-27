using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTurretController : MonoBehaviour
{ // attach this script to the parent (billion base) of the base turret

    public Transform turret; // Assign the child turret transform in the inspector
    public Vector3 targetDirection; // store direction for Bullet to use
    public float rotationSpeed = 4f; // in degrees per second

    void Update()
    {
        GameObject nearestOpponent = FindNearestOpponent();
        if (nearestOpponent != null )
        {
            RotateTurretTowards(nearestOpponent.transform);
        }
    }

    public void RotateTurretTowards(Transform target) // adjusted for smoother rotation of base turret
    {
        GameObject nearestOpponent = FindNearestOpponent();
        if (nearestOpponent != null)
        {
            Vector2 directionToTarget = target.position - turret.position;
            float targetAngle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle); // Adjust 
            turret.rotation = Quaternion.RotateTowards(turret.rotation, targetRotation, rotationSpeed * Time.deltaTime * 4.5f);
        }

    }

    GameObject FindNearestOpponent() // same as billion turret controller 
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
