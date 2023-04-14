using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "absData_", menuName = "Ability")]
// This is a scriptable Object that has a custo script attached as its abilityEffect.
public class Ability : ScriptableObject
{
    [Header("General Stats")]
    public int abilityID = 0; // Unique ID for each ability 
    public string abilityName = "..."; // Name
    public string Description = "...";

    [Header("Combat Settings")]
    public bool isFriendly;
    public bool isAOE;

    [Header("Ability Statistics")]
    public int cost;
    public int damage;

    

    [SerializeField]
    public baseAbilityLogic logic;
    

    // On use probs takes in all the user's stats and the current buffs and such. 

    // We want this function to do alot of different things. 
    public void OnUse(float modifiedStrength, Character target, Character self)
    {
        logic.Use(modifiedStrength, target, self, this); 
    }
}
