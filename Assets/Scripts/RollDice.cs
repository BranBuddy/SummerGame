using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Random = UnityEngine.Random;
public class RollDice : MonoBehaviour
{
    private int[] playerRoll = {1, 1, 1, 1};
    public TMP_Text[] playerNumber;
    private int[] sortRoll;
    private bool[] didEveryoneRoll = {false, false, false, false};
    public Button continueButton;

    private void Start()
    {
        continueButton.interactable = false;
    }

    private void Update()
    {
        //Checks if everyone has rolled, then you can continue
        if (didEveryoneRoll[0] == true && didEveryoneRoll[1] == true && didEveryoneRoll[2] == true && didEveryoneRoll[3] == true)
        {
            continueButton.interactable = true;
        }
    }

    //Roll each respective player's dice
    public void RollPlayer1()
    {
  
        playerRoll[0] = Random.Range(1, 10);
        playerNumber[0].text = playerRoll[0].ToString();
        didEveryoneRoll[0] = true;
  
    }

    public void RollPlayer2()
    {
        playerRoll[1] = Random.Range(1, 10);
        playerNumber[1].text = playerRoll[1].ToString();
        didEveryoneRoll[1] = true;
    }

    public void RollPlayer3()
    {
        playerRoll[2] = Random.Range(1, 10);
        playerNumber[2].text = playerRoll[2].ToString();
        didEveryoneRoll[2] = true;
    }

    public void RollPlayer4()
    {
        playerRoll[3] = Random.Range(1, 10);
        playerNumber[3].text = playerRoll[3].ToString();
        didEveryoneRoll[3] = true;
    }

    public void SortArray()
    {
        Array.Sort(playerRoll);
        Debug.Log(playerRoll[2]);
    }
}
