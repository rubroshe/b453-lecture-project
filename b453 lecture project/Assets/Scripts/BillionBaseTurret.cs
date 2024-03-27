using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillionBaseTurret : MonoBehaviour // attach to large turret on each base
{
    // Reference to this billion BASE'S TeamIdentifier component (moved into Start())
    private TeamIdentifier myTeamIdentifier;
 //   private TurretController myTurretController;

    // bullet refs 
    public float bulletSpeed = 0.8f; // slower but more dmg than billion bullet
    public float maxDistance = 2.5f; // dissappear at shorter distance than billion bullet
    public GameObject bulletPrefab; // diff colr bullets for each team
//    private Billions nearestOpponent;
    public float firingCooldown = 0.0f;
    public float firingInterval = 8.0f;
    public float turretRange = 15.0f; // when turret decides to start shooting
    private Transform nearestOpponent;
    // reference to the tip of turret
    public Transform bulletSpawnPoint; // assign in inspector

    void Start()
    {
        // Initialize myTeamIdentifier by getting the component from the parent billion
        myTeamIdentifier = GetComponentInParent<TeamIdentifier>();
        StartCoroutine(TurretOperation());
    }


    IEnumerator TurretOperation()
    {
        while (true)
        {
            Transform targetOpponent = FindNearestOpponent();
            if (targetOpponent != null)
            {
                // Rotate turret towards target
                Vector2 directionToTarget = (targetOpponent.position - transform.position).normalized;
                
                // Check if the utrret is facing the target (estimate) BEFORE firing
                float angleDifference = Vector2.Angle(transform.right, directionToTarget);

                // Only fire if the turret is approximately facing the target
                if (angleDifference < 10.0f) // && Vector2.Distance(transform.position, nearestOpponent.position) <= turretRange)
                {
                    if (firingCooldown <= 0)
                    {
                        FireBullet(); // Now called without a direction parameter compared to billions
                        firingCooldown = firingInterval;
                    }
                }
            }
            else
            {
                // No one is in range, turret should NOT be firing 
                firingCooldown = firingInterval;
            }

            firingCooldown -= Time.deltaTime;
            yield return null;
        }
    }


    private void FireBullet()
    {
        Vector2 firingDirection = transform.right.normalized; // sets the right side (red axis) as forward firingdirection

        Quaternion rotation = Quaternion.Euler(0, 0, transform.eulerAngles.z - 90); // - 90);

        // spawn bullet at turret pos
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, rotation);
        BillionBaseBullet bulletScript = bullet.GetComponent<BillionBaseBullet>();

        if (bulletScript != null) // Check if the Bullet script is attached to the prefab
        {
            
            // Initialize bullet dir and speed
            bulletScript.Initialize(firingDirection, bulletSpeed, maxDistance, myTeamIdentifier.teamColor); // might need to adjust to bulletScript.myTeamColor
        }
        else
        {
            Debug.LogError("Bullet script not found on bulletPrefab!");
        }

        // Initialize bullet dir and speed
        // bulletScript.Initialize(direction, bulletSpeed, maxDistance);
    }


    private Transform FindNearestOpponent() // returns nearest billion opponent
    {
        // Get all the billions in the scene
        Billions[] allBillions = FindObjectsOfType<Billions>();
        Transform nearestOpponent = null;
        float nearestDistance = Mathf.Infinity;

        foreach (Billions billion in allBillions)
        {
            TeamIdentifier billionTeamIdentifier = billion.GetComponent<TeamIdentifier>();

            // Only consider opponents that are within turretRange
            float distance = Vector2.Distance(transform.position, billion.transform.position);

            if (billionTeamIdentifier != null && 
                billionTeamIdentifier.teamColor != myTeamIdentifier.teamColor &&
                distance <= turretRange)
            {
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestOpponent = billion.transform;
                }
            }
        }

        return nearestOpponent;
    }

}
