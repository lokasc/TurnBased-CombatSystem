using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text.RegularExpressions; // this is RE
using System.Linq;


public class DisplayManager : MonoBehaviour
{
    public static DisplayManager instance;
    public TextMeshProUGUI displayObject;
    public TMP_InputField inputObject;
    public string displayAbilityText;
    public TextMeshProUGUI statusObject;
    public string displayStatusText;
    public BattleManager battleStatus;
    

    private PlayerCharacter _currentPlayer;

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
        inputObject.interactable = false;
    }

    // Update is called once per frame
    void Update()
    {
        displayObject.text = displayAbilityText;
        statusObject.text = displayStatusText;
        
        if (Input.GetKeyDown(KeyCode.Return) && inputObject.interactable && inputObject.text != "")
        {
            CheckInput(inputObject.text);
        }
    }

    public void ShowTurn(string name)
    {
        // Language
        if (name.EndsWith("s")){
             displayAbilityText += "Its " + name + "' turn\n";
        }
        else{
            displayAbilityText += "Its " + name + "'s turn\n";
        }
    }

    // This displays the health, name, turn order and everything.
    // Takes the party and the enemies as input
    public void ShowStatus(List<PlayerCharacter> goodies, List<EnemyCharacter> baddies)
    {
        /* Example of display:
        
        Turn Order: A | B | C | D | E | F  (Bold if its the current turn)

        Name   :   HP       SP      Status:
        XXXXXXX: 100/100    20/20   Regen(1), AttUp(2)
        YYYYYA : 100/100    20/20   Fuck(1), CRY(2)
        ZZZZZ  : 100/100    20/20   

        Enemies:   HP       Status:
        XXXXXXX: 100/100    Regen(1), AttUp(2)
        YYYYYA : 100/100    Weak(2), Poisoned(2)
        ZZZZZ  : 100/100    

        */
        string tempDisplay = "Players\n\n"; 
        tempDisplay += string.Format("{0,-30} {1,-20} {2,-10}\n",  
                                        "Name", "Hp", "Status");  

        // Loop through every player character and format them.
        for (int i = 0; i < goodies.Count; i++)
        {
            string _healthString = goodies[i].currentHp + "/" + goodies[i].maxhealthPoints;
            
            // Note: formating only works if the font is monospaced
            tempDisplay += string.Format("{0,-30} {1,-20}", goodies[i].name, _healthString) + "\n";
        }

        tempDisplay += "\nEnemies\n\n";
        tempDisplay += string.Format("{0,-30} {1,-20} {2,-10}\n",  
                                        "Name", "Hp", "Status"); 
        // Likewise, loop through every enemy
        for (int i = 0; i < baddies.Count; i++)
        {
            string _healthString = baddies[i].currentHp + "/" + baddies[i].maxhealthPoints;
            tempDisplay += string.Format("{0,-30} {1,-20}", baddies[i].name, _healthString) + "\n";
        }

        displayStatusText = tempDisplay;
    }

    public void EnableInput()
    {
        // Allows the input field to be seen and can be entered
        inputObject.interactable = true;

    }
    
    public void DisableInput()
    {
        // Disables input field
        inputObject.interactable = false;
    }
    
    // Shows the current abilites of the character
    public void ShowAbilities()
    {
        string tempDisplay = "";
        List<Ability> _abilities = _currentPlayer.abilities;
        for (int i = 0; i < _currentPlayer.abilities.Count; i++)
        {
            tempDisplay += i+1 +". "+_abilities[i].abilityName + "\n";
        }

        displayAbilityText += tempDisplay + "\n";
    }

    public void SetCurrentPlayer(PlayerCharacter player)
    {
        _currentPlayer = player;
    }

    // This function takes the string input from Input Field,
    // Outputs different things depending on input. 
    public void CheckInput(string input)
    {
        // Skill usage: skill number | (target or All)
        // we want to split the string into an array.

        string[] inputArray = input.Split(' ');


        switch (inputArray[0])
        {
            case "help":
                displayAbilityText += "Input a number representing the ability \n";
                break;
            case "skip":
                 displayAbilityText += "Please remove all empty spaces in the front";
                break;
            case " ":
                displayAbilityText += "Please remove all empty spaces in the front";
                break;   
            default:
                // We check if the first string includes a number and check if its in the count
                int? number = GetNumber(input[0].ToString(), _currentPlayer.abilities.Count);
                // int? to int u need to add .Value cuz its not the same type.
                if (number.HasValue)
                { 
                    if (_currentPlayer.abilities[number.Value-1].isAOE == true && _currentPlayer.abilities[number.Value-1].isFriendly == false)
                    {
                        // Here we will down cast the derived class to its base class in order for the function to work.
                        List<Character> enemyCharacters = BattleManager.instance.enemies.Cast<Character>().ToList();
                        _currentPlayer.OnCorrectSelection(number.Value, enemyCharacters);
                    }
                    else if (_currentPlayer.abilities[number.Value-1].isAOE == true && _currentPlayer.abilities[number.Value-1].isFriendly == true)
                    {
                        _currentPlayer.OnCorrectSelection(number.Value, BattleManager.instance.players.Cast<Character>().ToList());
                    }
                    else if (_currentPlayer.abilities[number.Value-1].isAOE == false)
                    {
                        // Checks if the number is within or not the bounds of 1 and Count.    
                        // theres no check if no enemies is selected.
                        if (inputArray.Count() < 2) { 
                            displayAbilityText += "Incorrect input or syntax\n"; 
                            break;
                        }

                        int? enemyIndex = GetNumber(inputArray[1].ToString(), BattleManager.instance.enemies.Count);
                        
                        if (enemyIndex.HasValue)
                        {
                             _currentPlayer.OnCorrectSelection(number.Value, ConvertList((Character)BattleManager.instance.enemies[enemyIndex.Value-1]));
                        }
                        else {displayAbilityText += "Incorrect input or syntax\n"; }        
                    }
                }
                else { displayAbilityText += "Incorrect input or syntax\n"; }
                break;
        }

        

        inputObject.text = "";
    }


    // The ? indicates that a Null can be returned.
    // One to the count number
    public static int? GetNumber(string input, int count)
    {
        string pattern = $@"\b([1-{count}])\b";
        Match match = Regex.Match(input, pattern); // If a substring matches the pattern.
        if (match.Success)
        {
            return int.Parse(match.Groups[1].Value);
        }
        else
        {
            return null;
        }
    }

    // Converts a character object to a List<Character>
    public List<Character> ConvertList(Character x)
    {
        List<Character> y = new List<Character> { x };
        return y;
    }
}
