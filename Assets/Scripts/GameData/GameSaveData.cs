using System;
using UnityEngine;

[System.Serializable]
public class GameSaveData
{
    // Sound volume
    public float musicVolume = 1f;
    public float sfxVolume = 1f;

    // Wallet
    public int playerBalance = 0;

    // Potion recipes in book
    public bool[] unlockedRecipes;
    public bool IsValid()
    {
        return playerBalance >= 0 &&
               musicVolume >= 0f && musicVolume <= 1f &&
               sfxVolume >= 0f && sfxVolume <= 1f;
    }

    public void ResetToDefault()
    {
        musicVolume = 1f;
        sfxVolume = 1f;
        playerBalance = 0;
        unlockedRecipes = null;
    }
}