using UnityEngine;

public class GameSettings : MonoBehaviour
{
    private TurnManager turnManager;
    void Start()
    {
        turnManager = GameObject.Find("TurnManager").GetComponent<TurnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Changes player number
    public void SinglePlayer()
    {
        turnManager.playerNumber = 2;
    }

    public void TwoPlayers()
    {
        turnManager.playerNumber = 3;
    }

    public void ThreePlayers()
    {
        turnManager.playerNumber = 4;
    }

    public void FourPlayers()
    {
        turnManager.playerNumber = 5;
    }
}
