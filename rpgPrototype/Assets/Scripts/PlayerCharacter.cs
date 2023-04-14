using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class PlayerCharacter : Character
{
    
    /*
    public Weapon weapon
    public HeadArmour head
    public ChestArmor chest
    */

    // In C#, constructors must be created in the derived class. 
    // It also needs the keyword base
    public PlayerCharacter(string name):base(name)
    {

    }

    public PlayerCharacter(string name, int hp, int mana, int strength, int speed, int def):base(name, hp, mana, strength, speed, def)
    {

    }

    public void AllowSelect() // Its the player's move, allow it to do something.
    {
        // Display the UI for abilities the player has and allow user to enter stuff.
        // Show abilities must be before enable input.
        DisplayManager.instance.SetCurrentPlayer(this); 
        DisplayManager.instance.ShowAbilities();
        DisplayManager.instance.EnableInput();
    }

    public void OnCorrectSelection(int abilityNumber, List<Character> targets)
    {
        // This function is called when the player selects a thing
        
        // We want to add effects, calculate the damage and disable UI
        
        // We can do a try catch thing where we see if the character chosen is an enemy class or a player class
     
        foreach(Character target in targets)
        {
            Debug.Log("Using " + abilities[abilityNumber-1].abilityName + " on " + target.name);

            if (target is EnemyCharacter)
            {
                EnemyCharacter _target = (EnemyCharacter)target;
                Ability _ability = abilities[abilityNumber-1];

                // On use is a generic function that may have different kind of stuff per time.
                // The ability will take in the stats of the player and decide what to do with it.
                // The ability will also be the one doing the attacking. 

                _ability.OnUse(statistics.strength, target, this); // This deals with the shit regarding abilities 
            }
            else if (target is PlayerCharacter)
            {
                PlayerCharacter _target = (PlayerCharacter)target;
            }

            
        }
        

        BattleManager.instance.TurnComplete();
    }

    public void ExecuteAttack(List<Character> targets) // Once you've selected an ability and the targets, we start the attack.
    {
        // Here we might do animations when time comes.   
    } 
}
