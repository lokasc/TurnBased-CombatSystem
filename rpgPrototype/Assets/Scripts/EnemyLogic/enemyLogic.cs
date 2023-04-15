using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class enemyLogic : ScriptableObject
{

    public abstract List<Character> DecideTarget(List<PlayerCharacter> players, List<EnemyCharacter> enemies = null);
}
