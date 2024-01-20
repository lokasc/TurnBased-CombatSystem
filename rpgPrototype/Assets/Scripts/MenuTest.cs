using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MenuTest : MonoBehaviour
{
    // Lets think about it.

    // When its your turn, pop up a prefab.

    // Before this prefab is instantiated process the data and dynamically modify the menu.

    // Assign button on click button.

    // When clicked execute thing.

    public GameObject pointerObject;
    public GameObject[] enemyList;

    public bool isLeft = true;
    public void PrintMessage(string yippie)
    {
        Debug.LogWarning(yippie);
    }



    // Called when the button is clicked, moves pointer to the first enemy.
    public void SelectAttack()
    {
        if (isLeft) { 
        pointerObject.transform.position = enemyList[0].transform.position;
        }
        else
        {
           pointerObject.transform.position = enemyList[1].transform.position;
        }
        isLeft = !isLeft;
    }
}
