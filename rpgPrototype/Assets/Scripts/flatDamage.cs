using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Logic/Flat Damage")]
public class flatDamage : baseAbilityLogic
{
    public override void Use(float modifiedStrength, Character target, Character self, Ability _ability)
    {
        target.TakeDamage(_ability.damage); // Direct Damage with no modifier.
    }
}
