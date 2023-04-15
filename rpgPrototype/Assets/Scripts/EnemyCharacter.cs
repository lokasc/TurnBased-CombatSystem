using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public class EnemyCharacter : Character
{
    public string patternDescription;
    public enemyLogic decider; 



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
    
    //Called by the battleManager, this decides what the enemy does
    public void OnEnemyTurn()
    {
        List<Character> targets = new List<Character>();
        Ability _abilitySelected = SelectAttack();
        if (_abilitySelected.isAOE == false)
        {
            targets.AddRange(decider.DecideTarget(BattleManager.instance.players));
        }
        else{
            targets.AddRange(BattleManager.instance.players);
        }
        
        for (int i = 0; i<targets.Count; i++)
        {
            _abilitySelected.OnUse(0, targets[i], this);
            DisplayManager.instance.WriteAttackString(targets[i], this, _abilitySelected);
        }

        BattleManager.instance.EnemyComplete();
    }

    //The random selector that decides which attack to use
    public Ability SelectAttack()
    {
        int x = Random.Range(0, abilities.Count);
        return abilities[x];
    }

    

}
