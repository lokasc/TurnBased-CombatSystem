using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Logic/Heal")]
public class HealLogic : baseAbilityLogic
{
    public override void Use(float modifiedStrength, Character target, Character self, Ability _ability)
    {
        target.TakeDamage(-_ability.damage); //Heals all alies health.
        //target.AddStatus((baseEffect)statusEffects[0]);
    }
}
