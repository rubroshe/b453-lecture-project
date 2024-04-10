using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillionBaseBullet : BaseBullet
{ // attach to each big bullet prefab
     private static float baseDamage = 24f; // default for this bullet type
    // separate modifiers for each team color:
    private static Dictionary<TeamColor, float> damageModifiers = new Dictionary<TeamColor, float>
    {
        {TeamColor.Red, 0f},
        {TeamColor.Blue, 0f},
        {TeamColor.Green, 0f},
        {TeamColor.Yellow, 0f}
    };


    public static void UpdateDamageModifier(int rank, TeamColor teamColor)
    {
        if (damageModifiers.ContainsKey(teamColor))
        {
            damageModifiers[teamColor] = (rank / 1.5f) * 10f;
        }
    }

    private void Awake()
    {
        if (damageModifiers.ContainsKey(myTeamColor))
        {
            bulletDamage = baseDamage + damageModifiers[myTeamColor];
        }
    }

    // private static float originalBulletDamage = 24f; // default for this bullet type
    //   private static float upgradedDamage = originalBulletDamage; // Static to retain across instances

    /*  public Vector2 velocity;
      public float bulletDamage = 24f;
      public float maxTravelDistance;
      private Vector2 startPosition;
      public TeamColor myTeamColor; // set this in inspector for each color bullet prefab

      // Define a delegate and event for bullet hit
      public delegate void BulletHitHandler(Bullet bullet, Collider2D collider);
      public static event BulletHitHandler OnBulletHit;

      [SerializeField] public BillionaireBase shooterBase;  // assign in inspector

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
  */

    /*// Define a delegate and event for bullet hit
    public delegate void BulletHitHandler(BillionBaseBullet bullet, Collider2D collider);
    public static event BulletHitHandler OnBulletHit;
*/

    private void Start()
    {
      //  Events.rankChange.AddListener(UpdateBulletDamage);

        BillionaireBase[] allBillionBases = FindObjectsOfType<BillionaireBase>();
        foreach (BillionaireBase baseObj in allBillionBases)
        {
            if (baseObj.GetComponent<TeamIdentifier>().teamColor == myTeamColor)
            {
                shooterBase = baseObj;
                return;
            }
        }
    }

   /* private void UpdateBulletDamage(int rank, TeamColor team)
    {
        bulletDamage = 24f * (rank / 1.5f);
    }
*/
   // protected override void OnTriggerEnter2D(Collider2D other)
  //  {
        // add logic for going "through" billion bullets
        // method
  //  }
        // Fire the event when bullet hits something
        // OnBulletHit?.Invoke(this, other); // fire off event for checking if bullet got kill/xp
 //      base.OnTriggerEnter2D(other); // call base class method for bullet DAMAGE 

        /*// Get the TeamIdentifier of the object we've collided with
        TeamIdentifier targetIdentifier = other.GetComponent<TeamIdentifier>();

        // If it's an opponent billion, deal damage
        if (targetIdentifier != null && targetIdentifier.teamColor != myTeamColor)
        {
            Billions opponentBillions = other.GetComponent<Billions>();

            // If the Billions component exists on hit object, deal damage
            if (opponentBillions != null)
            {
                // Damage logic
                other.GetComponent<Billions>().TakeDamage(bulletDamage);
            }

            // Destroy the bullet regardless if damage was dealt
            Destroy(gameObject);

        }
        else if (other.CompareTag("Wall")) // You need to tag your walls with "Wall"
        {
            // Destroy the bullet if it hits a wall
            Destroy(gameObject);
        }
    }*/

  //  }
}
