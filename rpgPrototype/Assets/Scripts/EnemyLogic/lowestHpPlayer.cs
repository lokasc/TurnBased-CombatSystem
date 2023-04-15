using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy Logic/Lowest Health Points")]
public class lowestHpPlayer : enemyLogic
{

    public override List<Character> DecideTarget(List<PlayerCharacter> players, List<EnemyCharacter> enemies = null)
    {
        List<Character> characters = new List<Character>();
        int _CurrentLowestHpPlayer = 0;
        for(int i = 0; i < players.Count; i++)
        {
            if (players[i].currentHp <= players[_CurrentLowestHpPlayer].currentHp)
            {
                _CurrentLowestHpPlayer = i; 
            }
        }
        // Add the selected character into the empty list and return it. 
        characters.Add(players[_CurrentLowestHpPlayer]);
        return characters;
    }
}
