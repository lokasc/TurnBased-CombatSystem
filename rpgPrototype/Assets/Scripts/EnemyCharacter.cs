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
    
    // Called by the battleManager, this decides what the enemy does
    public void OnEnemyTurn()
    {
        List<Character> targets = new List<Character>();
        Ability _abilitySelected = SelectAttack();

        List<PlayerCharacter> alivePlayers = new List<PlayerCharacter>();

        // Removes dead people from the list of targetable players.
        foreach(PlayerCharacter _player in BattleManager.instance.players)
        {
            if (_player.currentHp > 0) { alivePlayers.Add(_player); }
        }
        
        Debug.Log(this.name + " "+ alivePlayers);

        // all players are added if the spell is AOE.
        if (_abilitySelected.isAOE == false)
        {
            targets.AddRange(decider.DecideTarget(alivePlayers));
        }
        else{
            targets.AddRange(alivePlayers);
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
