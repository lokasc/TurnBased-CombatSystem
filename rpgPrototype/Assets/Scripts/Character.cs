using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Character
{
    public string name;
    public int maxhealthPoints;
    public int currentHp;
    public int manaPoints;
    public List<Ability> abilities;
    public Stats statistics;
    public List<baseEffect> statuses;

    // Constructor for easier editing 
    public Character(string name)
    {
        this.name = name;
        abilities = new List<Ability>();
        statistics = new Stats(0, 0, 0);
    }

    public Character(string name, int hp, int mana){
        this.name = name;
        this.maxhealthPoints = hp;
        this.manaPoints = mana;


        abilities = new List<Ability>();
        statistics = new Stats(0, 0, 0);
    } 

    // Constructor including stats
    public Character(string name, int hp, int mana, int strength, int speed, int def){
        this.name = name;
        this.maxhealthPoints = hp;
        this.manaPoints = mana;
        
        abilities = new List<Ability>();
        statistics = new Stats(0, 0, 0);
    }  

    

    // Called when an attack is applied. 
    public void TakeDamage(float damage, bool isDirectDamage = false)
    {
        currentHp -= (int)damage;
    }

    public void AddStatus(baseEffect status)
    {
        // We probably want to add checks such as defensive statuses that block things 
        // Architecture (Offensive will be on attacking character side and defensive would be recieving damage side)
        statuses.Add(status);
    }

    // First we go through the ability logic and how much damage it does or the abilities.
    // What if we want to take sp, or any other logic that wont bound us to damage and effects.

    // Lets just for now call this universal attack that takes in dmg float and status effect
    public void Attack(float damage, Character target, List<baseEffect> statuses = null)
    {
        target.TakeDamage(damage);
        if (statuses == null) {return;}
        foreach(baseEffect _status in statuses)
        {
            target.AddStatus(_status);
        }
    }
}


