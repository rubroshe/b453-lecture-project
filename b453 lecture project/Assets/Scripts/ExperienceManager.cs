using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExperienceManager : MonoBehaviour
{
    public static ExperienceManager Instance;

    public Image xpBar; // Assign this from the inspector
    private int currentExperience = 0;
    private int experienceToNextLevel = 1000; // Example value

    // Singleton pattern to ensure only one ExperienceManager exists
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void AddExperience(int amount)
    {
        currentExperience += amount;
        UpdateXpBar();
        CheckForLevelUp();
    }

    private void UpdateXpBar()
    {
       // Check if xpBar is not null and its GameObject is active before attempting to update it
        if (xpBar != null && xpBar.gameObject.activeInHierarchy)
        {
            xpBar.fillAmount = (float)currentExperience / experienceToNextLevel;
        }
    }

    private void CheckForLevelUp()
    {
        if (currentExperience >= experienceToNextLevel)
        {
            // Handle leveling up
            LevelUp();
        }
    }

    private void LevelUp()
    {
        currentExperience -= experienceToNextLevel;
        experienceToNextLevel += 500; // Increase the XP needed for next level, example increment
        UpdateXpBar();

        // Additional logic for leveling up, such as increasing stats, etc.
    }
}
