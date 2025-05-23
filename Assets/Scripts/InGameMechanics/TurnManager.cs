using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
public class TurnManager : MonoBehaviour
{
    //Current turn counter
    private int turnNumber;

    public List<GameObject> players;
    public GameObject[] banners;

    //A different Player GameObject will be assigned to this variable depeneding on whose turn it is
    private GameObject playerSelected;
    
    //Player GO's
    public GameObject Player1;
    public GameObject Player2;
    public GameObject Player3;
    public GameObject Player4;
    public GameObject NPC;

    //How many players are in the game
    public int playerNumber;

    public TMP_Text whatTurnIsIt;

    //Which player is currently playing
    private int playerIndex;
    
    //How many turns the players will play for
    public int howManyTurns;

    void Start()
    {
        playerNumber = 1;
        turnNumber = 1;
        playerIndex = 1;
        playerSelected = players[0];
    }

    // Updates turn number and which player is currently playing
    void Update()
    {
        whatTurnIsIt.text = turnNumber.ToString() + "/" + howManyTurns;

        PlayerSelectedBanner();

        HowManyPlayersAreThere();
    }

    //When the button is clicked it allows the next player to play. 
    public void EndPlayerTurn()
    {
        //Makes it so specifc events can occur when that character is selected
        playerSelected = players[playerIndex];

        //NPC will always go last, so if the button is click and it isn't on NPC, the next player will queue
        if (playerSelected != NPC)
        {
            playerIndex++;
        }
        //And if it is the NPC it will reset the index to 0 aka Player1
        else
        {
            playerIndex = 0;
        }

        //When the playerIndex resets to Player1 the turn will increase
        if (playerSelected == Player1)
        {
            turnNumber++;
        }
    }

    //When the playerSelected equals a certain player, their respective banner will change to a more vibrant color
    private void PlayerSelectedBanner()
    {
        if(playerSelected == Player1) 
        {
            banners[0].GetComponent<Image>().color = new Color32(255, 54, 54, 255);
        } else
        {
            banners[0].GetComponent<Image>().color = new Color32(255, 143, 143, 255);
        }

        if (playerSelected == Player2)
        {
            banners[1].GetComponent<Image>().color = new Color32(44, 83, 253, 255);
        }
        else
        {
            banners[1].GetComponent<Image>().color = new Color32(143, 164, 255, 255);
        }

        if (playerSelected == Player3)
        {
            banners[2].GetComponent<Image>().color = new Color32(246, 242, 13, 255);
        }
        else
        {
            banners[2].GetComponent<Image>().color = new Color32(255, 253, 143, 255);
        }

        if (playerSelected == Player4)
        {
            banners[3].GetComponent<Image>().color = new Color32(80, 245, 50, 255);
        }
        else
        {
            banners[3].GetComponent<Image>().color = new Color32(160, 255, 143, 255);
        }

        if (playerSelected == NPC)
        {
            banners[4].GetComponent<Image>().color = new Color32(215, 88, 255, 255);
        }
        else
        {
            banners[4].GetComponent<Image>().color = new Color32(228, 143, 255, 255);
        }

    }

    //Adds the amount of people playing to the list of players
    public void HowManyPlayersAreThere()
    {
        if(playerNumber == 2)
        {
            //Clears the list so it doesn't get clogged
            players.Clear();

            //Adds Players to List
            players.Add(Player1);
            players.Add(NPC);

            //disables the other banners
            banners[1].gameObject.SetActive(false);
            banners[2].gameObject.SetActive(false);
            banners[3].gameObject.SetActive(false);
        }
        else if(playerNumber == 3)
        {
            players.Clear();

            players.Add(Player1);
            players.Add(Player2);
            players.Add(NPC);

            //In case the player decides to change # of players, this will reset the player to turn back on
            banners[1].gameObject.SetActive(true);

            banners[2].gameObject.SetActive(false);
            banners[3].gameObject.SetActive(false);
        }
        else if (playerNumber == 4)
        {
            players.Clear();

            players.Add(Player1);
            players.Add(Player2);
            players.Add(Player3);
            players.Add(NPC);

            banners[1].gameObject.SetActive(true);
            banners[2].gameObject.SetActive(true);

            banners[3].gameObject.SetActive(false);
        }
        else if (playerNumber == 5)
        {
            players.Clear();

            players.Add(Player1);
            players.Add(Player2);
            players.Add(Player3);
            players.Add(Player4);
            players.Add(NPC);

            banners[1].gameObject.SetActive(true);
            banners[2].gameObject.SetActive(true);
            banners[3].gameObject.SetActive(true);
        }
    }
}
