using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Billions : MonoBehaviour
{
    public Transform targetFlag; // Flag this billion should go toward
    public float maxSpeed = 5f; // The maximum speed the billion can move at.
    public float acceleration = 0.2f; // The rate of acceleration towards the flag.
    public float decelerationDistance = 3f; // The distance from the flag at which the billion begins to decelerate.

    private Rigidbody2D rb;
    public float updateTargetInterval = 1f;
    public int baseRank = 0;
    public TextMeshProUGUI rankText;


    [Header("Health Refs")]
    public float maxHealth = 100f;
    public float minHealthCircleSize = 0.4f; // minimum scale of the health circle 
    public float currentHealth;
    public SpriteRenderer healthCircle; // this should match billion team color/sprite
    

    //  public static float upgradedHealth;

    // xp reference 
    //   public ExperienceManager experienceManager;
    // public int xpValue = 10; // xp value of this billion when killed


    // separate HEALTH modifiers for each team color:
    private static Dictionary<TeamColor, float> billionHealthModifiers = new Dictionary<TeamColor, float>
    {
        {TeamColor.Red, 0f},
        {TeamColor.Blue, 0f},
        {TeamColor.Green, 0f},
        {TeamColor.Yellow, 0f}
    };

    public static void UpdateHealthModifier(int rank, TeamColor teamColor)
    {
        if (billionHealthModifiers.ContainsKey(teamColor))
        {
            billionHealthModifiers[teamColor] = rank * 20f;
     //       baseRank++;
       //     Debug.Log("Rank changed for team " + teamColor + " to " + baseRank);
        }
    }

    private void Awake()
    {
        if (billionHealthModifiers.ContainsKey(GetComponent<TeamIdentifier>().teamColor))
        {
            maxHealth = maxHealth + billionHealthModifiers[GetComponent<TeamIdentifier>().teamColor];
        }
    }

    public void SetRank (int newRank) // for keeping up with billion rank text
    {
        baseRank = newRank;
        rankText.text = baseRank.ToString();
    }
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(UpdateTargetFlag());

        currentHealth = maxHealth;
        UpdateHealthVisual();

        Events.rankChange.AddListener(HandleRankChange); // REMEMBER TO REMOVE LISTENER WHEN DEAD!!
 //  moved to  SetRank method     rankText.text = baseRank.ToString();
        
    }

    // The listener method
    private void HandleRankChange(int rank, TeamColor team)
    {
        // Check if the team matches this object's team before applying changes
        if (GetComponent<TeamIdentifier>().teamColor == team)
        {
           // baseRank++;
         //   Debug.Log("Rank changed for team " + team + " to " + baseRank);
        }
    }

    private void Update()
    {
        // Check for middle mouse button click
        if (Input.GetMouseButtonDown(2))
        {
            CheckForClickDamage();
        }
    }

    IEnumerator UpdateTargetFlag()
    {
        while (true)
        {
            FindNearestFlag();
            yield return new WaitForSeconds(updateTargetInterval); // Wait for the specified interval before searching again.
        }
    }

    private void FindNearestFlag()
    {
        TeamIdentifier myTeamIdentifier = GetComponent<TeamIdentifier>();
        TeamColor myTeamColor = myTeamIdentifier.teamColor;

        // Find the flags by team color enum
        TeamIdentifier[] allFlags = FindObjectsOfType<TeamIdentifier>();
        float closestDistance = Mathf.Infinity;
        Transform closestFlag = null;

        foreach (TeamIdentifier flagIdentifier in allFlags)
        {
            // Check for flag type so billion doesn't target itself for movement
            if (flagIdentifier.isFlag && flagIdentifier.teamColor == myTeamColor)
            {
                float distance = Vector2.Distance(transform.position, flagIdentifier.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestFlag = flagIdentifier.transform;
                }
            }
        }


        // If found a flag, set it as the target
        if (closestFlag != null)
        {
            targetFlag = closestFlag;
        }
        else
        {
           // Debug.LogWarning("No flags found for " + gameObject.name);
        }
    }

    private void FixedUpdate()
    {
        // check if the flag exists
        if (targetFlag != null)
        {
            MoveTowardFlag();
        }
    }

    private void MoveTowardFlag()
    {   // The normalized direction vector towards the flag.   
        Vector2 direction = (targetFlag.position - transform.position).normalized;
        // The distance from the billion to the target flag.
        float distance = Vector2.Distance(transform.position, targetFlag.position);

        // Desired speed calculated based on the distance to the flag.
        // If within the deceleration distance, start slowing down, otherwise accelerate to max speed (GPT)
        float targetSpeed = (distance < decelerationDistance) ? Mathf.Lerp(0, maxSpeed, distance / decelerationDistance) : maxSpeed;

        // The desired velocity is the direction times the target speed.
        Vector2 targetVelocity = direction * targetSpeed;

        // Smoothly update the velocity towards the target velocity.
        rb.velocity = Vector2.Lerp(rb.velocity, targetVelocity, acceleration * Time.fixedDeltaTime);

        /*// tried using ChatGPT to adjust the rotation of the billion to face the direction of movement.
        if (rb.velocity != Vector2.zero)
        {
            float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }*/
    }
    
    public bool TakeDamage(float damageToTake, TeamColor opposingHit) // handles hp/death
    {
        currentHealth -= damageToTake;
        
       // Debug.Log("Ouch!" + " \n " + "-" + gameObject);

        if (currentHealth <= 0)
        {
            Events.billionDeath.Invoke(opposingHit, true);
            Die();
            return true; // object dead
        }
        else
        {
            UpdateHealthVisual();
            return false; // object not dead
        }
    }



    void UpdateHealthVisual()
    {
        if (healthCircle != null)
        {
            // calc scale of health circle 
            float healthRatio = Mathf.Clamp((currentHealth / maxHealth), minHealthCircleSize, 1);
            healthCircle.transform.localScale = Vector3.one * Mathf.Max(healthRatio, minHealthCircleSize);
           // healthCircle.transform.localScale = Vector3.one * healthRatio;
        }
    }

    void CheckForClickDamage()
    {
        // Raycast to the mouse position to see if hit this billion
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);

        if (hit.collider != null && hit.collider.gameObject == this.gameObject) // check if hit object with this script (billion)
        {
            
        //    TakeDamage(10f);
        }
    }

    void Die()
    {
       // if (experienceManager != null) experienceManager.gameObject.SetActive(false);
        Destroy(gameObject);
        Events.rankChange.RemoveListener(HandleRankChange); // remove listener when dead
    }

        /* private void OnMouseDown()
         {
             if (Input.GetMouseButtonDown(2)) // check for middle mouse click 
             {
                 TakeDamage(10f); 
             }
         }*/

    }
