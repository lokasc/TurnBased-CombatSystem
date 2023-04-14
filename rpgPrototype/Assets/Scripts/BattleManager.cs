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


    public bool isBattleEnd = false; // This variable tracks if the battle has ended, used for preventing string from concatinating after the battle ends. 
    public bool waitingForAnimation = false; // This variable tracks whether an animation is currently playing
    public bool waitingForInput = false; // This variable tracks whether we're waiting for player input
    public int turnIndex; //Our pointer to who has the turn
    // Note: It doesn't display whos the current player but the next player (due to waiting for player input...we need to refactor)

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
        // Check if we're currently waiting for an animation or input
        if (waitingForAnimation || waitingForInput) {
            return;
        }

        if (isBattleEnd) { return; }

        if (turnOrder[turnIndex] is PlayerCharacter)
        {
            PlayerCharacter t = (PlayerCharacter)turnOrder[turnIndex];
            DisplayManager.instance.ShowTurn(t.name);
            
            t.AllowSelect(); // This allows the player to select an attack and see the UI.
            
            
            waitingForInput = true; // the turn is over when player selects.
        }
        else if (turnOrder[turnIndex] is EnemyCharacter)
        {
            Debug.Log("It is: " + turnOrder[turnIndex].name + "'s turn");



            // Set waitingForAnimation to true to wait for attack animation
            waitingForAnimation = true;
        }

        // Check win before next turn increments.
        DisplayManager.instance.ShowStatus(players, enemies, turnOrder, turnIndex);
        CheckWinOrLose();

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

    // Checks if the player has won or lost
    public void CheckWinOrLose()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            // If the enemies are still alive
            if (enemies[i].currentHp > 0)
            {
                // Check all players
                for (int j = 0; j < players.Count; j++)
                {
                    // if one player is still alive we return nothing.
                    if (players[j].currentHp > 0) { return; }
                }
                // Players are all dead, enemies win
                
                EndBattle(false);
                
            }
        }
        // Enemies are all dead, players win.
        EndBattle(true); 
    }


    // Alerts other related scripts, Ends the battle
    public void EndBattle(bool isWin)
    {
        isBattleEnd = true;
        DisplayManager.instance.OnBattleEnd(isWin);
        // Deletes this object
        Destroy(this);
    }
}
