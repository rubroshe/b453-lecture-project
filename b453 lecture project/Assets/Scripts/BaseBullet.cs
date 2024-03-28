using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBullet : MonoBehaviour
{
    public Vector2 velocity;
    public float bulletDamage; // = 10f; for bullet, 24f for base bullet
    public float maxTravelDistance;
    private Vector2 startPosition;
    public TeamColor myTeamColor; // set this in inspector for each color bullet prefab

    // Define a delegate and event for bullet hit
    public delegate void BulletHitHandler(BaseBullet bullet, Collider2D collider);
    public static event BulletHitHandler OnBulletHit;

    public BillionaireBase shooterBase;  // assign in inspector

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


    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        // Get the TeamIdentifier of the object we've collided with
          TeamIdentifier targetIdentifier = other.GetComponent<TeamIdentifier>();

          // If it's an opponent billion, deal damage
          if (targetIdentifier != null && targetIdentifier.teamColor != myTeamColor)
          {
          //    bool shouldDestroy = false; // assume we don't destroy immediately
              Billions opponentBillions = other.GetComponent<Billions>();
              BillionaireBase opponentBase = other.GetComponent<BillionaireBase>();
              

            // If the Billions component exists on hit object, deal damage
            if (opponentBillions != null)
              {
                  // Damage logic
                  other.GetComponent<Billions>().TakeDamage(bulletDamage, myTeamColor);
              }

              // If the BillionaireBase component exists on hit object, deal damage
              if (opponentBase != null)
              {
                  // Damage logic
                  other.GetComponent<BillionaireBase>().TakeDamage(bulletDamage, myTeamColor);
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
