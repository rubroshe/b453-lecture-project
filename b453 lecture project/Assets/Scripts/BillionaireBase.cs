using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;
using UnityEngine.UI;
using TMPro;

public class BillionaireBase : MonoBehaviour // attach to billion bases (parent) 
{
    public float maxHealth = 400f;
    public float currentHealth;
    public Image xpBar;
    public float currentExperience = 0;
    private float experienceToNextLevel = 500; // Example value
    public int baseRank = 1; // base rank
    public TextMeshProUGUI rankText;

    public HealthBarManager healthBarManager; // assign this from the inspector

 //   public ExperienceManager experienceManager;
   // public int xpValue = 50; // xp value of this base when destroyed
    public float billionXPValue = 25; // xp value of each billion killed
    public float baseXPValue = 100; // xp value of each base destroyed

    private void Start()
    {
        Events.billionDeath.AddListener(AddExperience);

        currentHealth = maxHealth;
        // UpdateHealthVisual();
        xpBar.fillAmount = 0f;
        rankText.text = baseRank.ToString();
    }

    private void Update()
    {
        UpdateHealthVisual();
    }
    public bool TakeDamage(float damageToTake, TeamColor opposingHit)
    {
        // Subtract the damage from the current health
        currentHealth -= damageToTake;

        Debug.Log("Taken  hit! Current health: " + currentHealth);
        

        // Check if the base has been destroyed
        if (currentHealth <= 0)
        {
            Events.billionDeath.Invoke(opposingHit, false);
            Die();
            return true; // object dead
        }
        else
        {
            UpdateHealthVisual();
            return false; // object not dead
        }
    }

       
        /*// Check if this base was hit
        if (collider.gameObject == this.gameObject)
        {
         //   TakeDamage(bullet.bulletDamage);

            // Check if this base was destroyed
            if (currentHealth <= 0)
            {
                bullet.shooterBase?.AddExperience(xpValue);
            }
        }*/
   


    void UpdateHealthVisual()
    {
        if (healthBarManager != null)
        {
            healthBarManager.SetHealth(currentHealth, maxHealth);
        }
    }

    void Die()
    {
        if (healthBarManager != null) healthBarManager.gameObject.SetActive(false);
        
  //      if (experienceManager != null) experienceManager.gameObject.SetActive(false);
        
        // Destroy the base
        Destroy(gameObject, 0.2f);
    }

    public void AddExperience(TeamColor teamAwarded, bool isBillion)
    {
        if (this.GetComponent<TeamIdentifier>().teamColor == teamAwarded)
        {
            if (isBillion)
            {
                currentExperience += billionXPValue;
            }
            else
            {
                currentExperience += baseXPValue;
            }

        }

        UpdateXpBar();
        CheckForLevelUp();
    }

    // healthBar.fillAmount = health / maxHealth;
    private void UpdateXpBar()
    {
    //    Debug.Log("Updating XP bar: " + currentExperience + " / " + experienceToNextLevel);

        xpBar.fillAmount = currentExperience / experienceToNextLevel;
        // reset xp bar if level up
    }

    private void CheckForLevelUp()
    {
        // Check if the current experience is enough to level up
        while (currentExperience >= experienceToNextLevel)
        {
            // Subtract the experience needed for the level up and increase rank
            currentExperience -= experienceToNextLevel; 
            baseRank++;
            experienceToNextLevel += 500; // Adjust level-up threshold as needed

            // update rank text
            rankText.text = baseRank.ToString();

            // refresh health with new maximum 
            maxHealth += 125;
            currentHealth = maxHealth;
            UpdateHealthVisual();

            Events.rankChange.Invoke(baseRank, GetComponent<TeamIdentifier>().teamColor);
            Debug.Log("rankup event fired");
        }
        // Update the XP bar in case of multiple level-ups
        UpdateXpBar();
    }
}
