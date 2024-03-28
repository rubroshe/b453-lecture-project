using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : BaseBullet // attach to each team's bullet prefab
{
    /*public Vector2 velocity;
    public float bulletDamage = 10f;
    public float maxTravelDistance; 
    private Vector2 startPosition;
    public TeamColor myTeamColor; // set this in inspector for each color bullet prefab*/

    
    /*// Define a delegate and event for bullet hit
    public delegate void BulletHitHandler(Bullet bullet, Collider2D collider);
    public static event BulletHitHandler OnBulletHit;*/

    //  [SerializeField] public BillionaireBase shooterBase;  // assign in inspector

    /* public void Initialize(Vector2 direction, float speed, float maxDistance, TeamColor teamColor)
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
 */
    private void Start()
    {
        BillionaireBase[] allBillionBases = FindObjectsOfType<BillionaireBase>();
        foreach (BillionaireBase baseObj in allBillionBases)
        {
            if (baseObj.GetComponent<TeamIdentifier>().teamColor == myTeamColor)
            {
                shooterBase = baseObj;
                return;
            }
        }

        bulletDamage = 10f;
    }

 //   protected override void OnTriggerEnter2D(Collider2D other)
  //  {
        // Fire the event when bullet hits something
    //    OnBulletHit?.Invoke(this, other); // fire off event for checking if bullet got kill/xp
     //   base.OnTriggerEnter2D(other); // call base class method for bullet DAMAGE 

      /*  // Get the TeamIdentifier of the object we've collided with
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
        }*/
   // }
}
