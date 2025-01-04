using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class regenEffect : baseEffect
{
    public override void OnAdd()
    {
        base.OnAdd();
        duration = 2;
    }

    public override void Process(Character _char)
    {
        base.Process(_char);
        _char.currentHp += 1;
    }
}
