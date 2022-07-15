using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TurnManager : MonoBehaviour
{
    private static TurnManager _instance;

    public static TurnManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject();
                go.AddComponent<TurnManager>();
            }

            return _instance;
        }
    }

    Player currentPlayer;


    List<Player> players = new List<Player>();
    [SerializeField] Button nextTurnButton;
    [SerializeField] TextMeshProUGUI turnIndex;
    [SerializeField] TextMeshProUGUI playerIndex;
    [SerializeField] int amountOfHumanPlayers;
    [SerializeField] int amountOfAIPlayers;

    int turnCount;
    int playerCount;

    private void Awake()
    {
        _instance = this;
    }


    void Start()
    {
        turnIndex.text = "Turn:" + 0;
        playerIndex.text = "Player:" + 0;

        if (amountOfHumanPlayers == 0)
            amountOfHumanPlayers++;

        for(int i = 0; i < amountOfHumanPlayers; i++)
        {
            players.Add(new Human());
        }
        for (int i = 0; i < amountOfAIPlayers; i++)
        {
            players.Add(new AI());
        }

        StartCoroutine(WaitForNextTurnButton());
    }

    //Thanks to whoever made this http://wiki.unity3d.com/index.php/UI/WaitForUIButtons
    IEnumerator WaitForNextTurnButton()
    {
        for (turnCount = 0; turnCount < Mathf.Infinity; turnCount++)
        {
            for (playerCount = 0; playerCount < players.Count; playerCount++)
            {
                currentPlayer = players[playerCount];

                if (currentPlayer is Human)
                {
                    //HUMAN STUFF
                    turnIndex.text = "Turn:" + turnCount;
                    playerIndex.text = "Player:" + playerCount;
                    yield return new WaitForUIButtons(nextTurnButton);
                }
                else
                {
                    //AI STUFF
                    playerIndex.text = "Player:" + playerCount;
                    yield return new WaitForSeconds(0.5f);     

                }
            }
        }
    }
}
