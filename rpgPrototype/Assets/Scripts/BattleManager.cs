using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance; 

    public string Status; 
    public List<PlayerCharacter> players;
    public List<EnemyCharacter> enemies;
    public List<Character> turnOrder;

    public bool waitingForAnimation = false; // This variable tracks whether an animation is currently playing
    public bool waitingForInput = false; // This variable tracks whether we're waiting for player input
    public int turnIndex; //Our pointer to who has the turn

    void Awake()
    {
        // Singleton pattern
        if (instance == null){
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }
    }


    void Start()
    {
        // Decide turn order with different speed but for now players start first.
        turnOrder = new List<Character>();
        turnOrder.AddRange(players);
        turnOrder.AddRange(enemies);
        
        turnIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //print(players.Count);
        //print(turnOrder);
        // Check if we're currently waiting for an animation or input
        if (waitingForAnimation || waitingForInput) {
            return;
        }

        if (turnOrder[turnIndex] is PlayerCharacter)
        {
            //Debug.Log("It is: " + turnOrder[turnIndex].name + "'s turn");
            
            PlayerCharacter t = (PlayerCharacter)turnOrder[turnIndex];
            DisplayManager.instance.ShowStatus(players, enemies);
            DisplayManager.instance.ShowTurn(t.name);
            
            t.AllowSelect(); // This allows the player to select an attack and see the UI.
            
            
            waitingForInput = true; // the turn is over when 
        }
        else if (turnOrder[turnIndex] is EnemyCharacter)
        {
            Debug.Log("It is: " + turnOrder[turnIndex].name + "'s turn");



            // Set waitingForAnimation to true to wait for attack animation
            waitingForAnimation = true;
        }

        turnIndex++; // Increment turns
        
        // Resets the turns. 
        if (turnIndex >= turnOrder.Count) {
            turnIndex = 0;
        }
    }

    public void SetCharacters(List<PlayerCharacter> goodies, List<EnemyCharacter> baddies)
    {
        players = goodies;
        enemies = baddies;
    }

     // This function is called by the attacking character when their attack animation is complete
    public void AttackAnimationComplete()
    {
        // Set waitingForAnimation to false since the animation is complete
        waitingForAnimation = false;
    }

    // This function is called by the player when they have selected their target and attack
    public void TakeTurn(PlayerCharacter player, List<Character> targets)
    {
        // Call the Attack function on the player and pass in the target
        player.ExecuteAttack(targets);

        // Set waitingForAnimation to true to wait for attack animation
        waitingForAnimation = true;

        // Set waitingForInput to false since the player has taken their turn
        waitingForInput = false;
    }

    // Completes the turn, no animations;
    public void TurnComplete()
    {
        waitingForInput = false;
    }
}
