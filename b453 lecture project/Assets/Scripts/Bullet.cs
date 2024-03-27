using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour // attach to each team's bullet prefab
{
    public Vector2 velocity;
    public float bulletDamage = 10f;
    public float maxTravelDistance; 
    private Vector2 startPosition;
    public TeamColor myTeamColor; // set this in inspector for each color bullet prefab
    

    public void Initialize(Vector2 direction, float speed, float maxDistance, TeamColor teamColor)
    {
        velocity = direction * speed;
        maxTravelDistance = maxDistance;
        startPosition = transform.position;
        // myTeamColor already set 
    }

    // Update is called once per frame
    void Update()
    {
        // move the bullet
        transform.position += (Vector3)velocity * Time.deltaTime;

        // check for max distance
        if (Vector2.Distance(startPosition, transform.position) >= maxTravelDistance)
        {
            Destroy(gameObject);
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        // Get the TeamIdentifier of the object we've collided with
        TeamIdentifier targetIdentifier = other.GetComponent<TeamIdentifier>();

        // If it's an opponent billion, deal damage
        if (targetIdentifier != null && targetIdentifier.teamColor != myTeamColor)
        {
            Billions opponentBillions = other.GetComponent<Billions>();
            BillionaireBase opponentBase = other.GetComponent<BillionaireBase>();

            // If the Billions component exists on hit object, deal damage
            if (opponentBillions != null)
            {
                // Damage logic
                other.GetComponent<Billions>().TakeDamage(bulletDamage);
            }

            // If the BillionaireBase component exists on hit object, deal damage
            if (opponentBase != null)
            {
                // Damage logic
                other.GetComponent<BillionaireBase>().TakeDamage(bulletDamage);
            }

            // Destroy the bullet regardless if damage was dealt
            Destroy(gameObject);
            
        }
        else if (other.CompareTag("Wall")) // You need to tag your walls with "Wall"
        {
            // Destroy the bullet if it hits a wall
            Destroy(gameObject);
        }
    }
}
