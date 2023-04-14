using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stats 
{
    // This class contains all of the revelant statistics that each player has.

    public int strength = 0;
    public int intellignece = 0;
    public int speed = 0;

    public Stats(int _str, int _int, int _spd)
    {
        this.strength = _str;
        this.intellignece = _int;
        this.speed = _spd;
        
    }

    
    
}
