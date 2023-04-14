using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]


// What you do with the information in the ability Scriptable Object.
public class baseAbilityLogic : ScriptableObject
{
    public virtual void Use(float modifiedStrength, Character target, Character self, Ability _ability)
    {
        
    }
}
