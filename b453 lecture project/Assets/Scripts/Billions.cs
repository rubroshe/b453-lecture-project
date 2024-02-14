using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billions : MonoBehaviour
{
    public Transform targetFlag; // Flag this billion should go toward
    public float maxSpeed = 5f; // The maximum speed the billion can move at.
    public float acceleration = 0.2f; // The rate of acceleration towards the flag.
    public float decelerationDistance = 3f; // The distance from the flag at which the billion begins to decelerate.

    private Rigidbody2D rb;
    public float updateTargetInterval = 1f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(UpdateTargetFlag());
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
            Debug.LogWarning("No flags found for " + gameObject.name);
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
}
