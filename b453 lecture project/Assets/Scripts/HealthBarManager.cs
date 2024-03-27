using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarManager : MonoBehaviour
{
    public Image healthBar; // Assign this from the inspector

    // Call this method to update the health bar
    public void SetHealth(float health, float maxHealth)
    {
        healthBar.fillAmount = health / maxHealth;
    }
}
