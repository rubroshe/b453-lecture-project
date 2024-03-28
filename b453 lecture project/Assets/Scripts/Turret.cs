using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour // attach to turret child of each billion
{
    // Reference to this billion's TeamIdentifier component (moved into Start())
    private TeamIdentifier myTeamIdentifier;
    private TurretController myTurretController;

    // bullet refs 
    public float bulletSpeed = 0.5f;
    public float maxDistance = 5.0f;
    public GameObject bulletPrefab; // diff color bullets for each team
    private Transform nearestOpponent;
    public float firingCooldown = 0.0f; 
    public float firingInterval = 5.0f;


    void Start()
    {
        // Initialize myTeamIdentifier by getting the component from the parent billion
        myTeamIdentifier = GetComponentInParent<TeamIdentifier>();
        myTurretController = GetComponentInParent<TurretController>();
    }

    // Update is called once per frame
    void Update()
    {
        nearestOpponent = FindNearestOpponent();
        if (nearestOpponent != null )
        {
            myTurretController.RotateTurret();

            // Fire bullet if cooldown is finished 
            firingCooldown -= Time.deltaTime;
            if (firingCooldown <= 0)
            {
                // Use targetDirection from TurretController for firing direction
                FireBullet(myTurretController.targetDirection.normalized);
                firingCooldown = firingInterval;
            }
            
        }
    }

    private void FireBullet(Vector2 direction) 
    {
        // Calc angle from the direction 
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Create a rotation from teh angle (sub 90 bc sprite faces upward)
        Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));

        // spawn bullet at turret pos
        GameObject bullet = Instantiate(bulletPrefab, transform.position, rotation);
        BaseBullet bulletScript = bullet.GetComponent<BaseBullet>();

        if (bulletScript != null) // Check if the Bullet script is attached to the prefab
        {
            // Initialize bullet dir and speed
            bulletScript.Initialize(direction.normalized, bulletSpeed, maxDistance, bulletScript.myTeamColor); // Ensure these are set correctly
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
        BillionaireBase[] allBases = FindObjectsOfType<BillionaireBase>();

        Transform nearestTarget = null;
        float nearestDistance = Mathf.Infinity;

        // Loop through all the billions in the scene
        foreach (Billions billion in allBillions)
        {
            // Ignore the parent billion itself this turret is the child of
            if (billion != this.GetComponentInParent<Billions>() && IsOpponent(billion))
            {
                float distance = Vector2.Distance(transform.position, billion.transform.position);
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestTarget = billion.transform;
                }
            }
        }

        
        foreach (BillionaireBase bases in allBases) 
        {
            if (IsOpponent(bases))
            {
                float distance = Vector2.Distance(transform.position, base.transform.position);
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestTarget = base.transform;
                }
            }
        }
        return nearestTarget;
    }

    private bool IsOpponent(Component component) // from online
    {
        // Check if the targetIdentifier is not null and its team color is not the same as this billion's team color
        TeamIdentifier identifier = component.GetComponent<TeamIdentifier>();
        return identifier != null && identifier.teamColor != myTeamIdentifier.teamColor;
    }
}
