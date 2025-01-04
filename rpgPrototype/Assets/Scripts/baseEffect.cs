using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class baseEffect : MonoBehaviour
{
    public int effectId;
    public string statusName;

    public int duration;
    public int current_duration;

    public enum ProcessType  
    {
        TurnStart, // 0
        TurnEnd, // 1
        None // 2 (continuous not based on order)
    }

    public ProcessType statusProcessType;

    // When a status is added
    public virtual void OnAdd() {}
    // When a status is removed
    public virtual void OnRemove() {}
    // When a status is processed (based on process type)
    public virtual void Process(Character _char) {}
}
