using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class EnemyCharacter : Character
{
    public string patternDescription;



    // In C#, constructors must be created in the derived class. 
    // It also needs the keyword base
    public EnemyCharacter(string name):base(name)
    {

    }

    public EnemyCharacter(string name, int hp, int mana, int strength, int speed, int def):base(name, hp, mana, strength, speed, def)
    {

    }




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void AttackLogic()
    {
        
    }
}
