using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Logic/Empty")]
public class EmptyLogic : baseAbilityLogic
{
    public override void Use(float modifiedStrength, Character target, Character self, Ability _ability)
    {
       // Empty, does nothing.
    }
}
