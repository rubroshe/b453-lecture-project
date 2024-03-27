using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class BillionaireBase : MonoBehaviour // attach to billion bases (parent) 
{
    public float maxHealth = 300f;
    public float currentHealth;
    public HealthBarManager healthBarManager; // assign this from the inspector

    public ExperienceManager experienceManager;
    public int xpValue = 50; // xp value of this base when destroyed

    private void Start()
    {
        currentHealth = maxHealth;
        // UpdateHealthVisual();
    }
    public void TakeDamage(float damageToTake)
    {
        // Subtract the damage from the current health
        currentHealth -= damageToTake;

        // Update the health visual
        

        // Check if the base has been destroyed
        if (currentHealth <= 0)
        {
            ExperienceManager.Instance.AddExperience(xpValue);
            Die();
        }
        else
        {
            UpdateHealthVisual();
        }
    }

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
        
        if (experienceManager != null) experienceManager.gameObject.SetActive(false);
        
        // Destroy the base
        Destroy(gameObject);
    }
}
