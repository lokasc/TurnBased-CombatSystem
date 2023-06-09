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
    public int turnIndex; // Points to the current player.
    
    public int previousCount;
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
        previousCount = turnOrder.Count;
    }

    // Update is called once per frame
    void Update()
    {
        // Check if we're currently waiting for an animation or input
        if (waitingForAnimation || waitingForInput) {
            return;
        }
        CheckWinOrLose();
        if (isBattleEnd) { return; }

         // Resets the turns.
    

        if (turnIndex >= turnOrder.Count) {
            turnIndex = 0;
        }

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

            DisplayManager.instance.DisableInput();

            // Set waitingForAnimation to true to wait for attack animation
            waitingForAnimation = true;

            // Current we set the animation to false instantly because of no animation
            // When we do, we write code similar to how the waiting for input code works.  

            // We invoke this to delay the selection of attack
            Invoke("OnEnemyPrePauseComplete", 1.5f);

        }
        
        DisplayManager.instance.ShowStatus(players, enemies, turnOrder, turnIndex);
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

    // All player characters call this function when the turn ends. 
    // Temp function to call ignoring animations.
    public void TurnComplete()
    {
        waitingForInput = false;
        DisplayManager.instance.ShowStatus(players, enemies, turnOrder, turnIndex);
        Increment();
        // what if we incremented here. 
    }

    // Called by enemies, the animation part is for expanding into 3D later.
    public void EnemyComplete()
    {

        DisplayManager.instance.ShowStatus(players, enemies, turnOrder, turnIndex);

        // This line delays enemy turns in real time so players can read what just happened.
        Invoke("OnEnemyPostPauseComplete", 2f);
        
    }
    public void OnEnemyPostPauseComplete()
    {
        waitingForAnimation = false;
        Increment();
    }

    public void OnEnemyPrePauseComplete()
    {
        // invokes current enemy.
        EnemyCharacter e = (EnemyCharacter)turnOrder[turnIndex]; 
        e.OnEnemyTurn();
    }

    // Removes dead people from queue, checked before incrementation. 
    public bool CheckDeath()
    { 
        bool isKilled = false;
        previousCount = turnOrder.Count;
        for (int i = 0; i<turnOrder.Count; i++)
        {
            if (turnOrder[i].currentHp <= 0)
            {
                turnOrder.Remove(turnOrder[i]);
                isKilled = true;
            }
        }
        DisplayManager.instance.ShowStatus(players, enemies, turnOrder, turnIndex);
        return isKilled;
    }

    // Increments turn, checks if incrementing is suitable
    public void Increment()
    {
        bool sthHasDied = CheckDeath();

        // Edge Case Code Fix: If i kill an enemy and the my next turn index is equal to the new count.
        if(sthHasDied && (turnIndex+1 == turnOrder.Count))
        {
            return; 
        }
        else
        {  
            turnIndex++;
        }
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
