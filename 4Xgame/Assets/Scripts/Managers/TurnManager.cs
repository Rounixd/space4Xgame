using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TurnManager : MonoBehaviour
{
    private static TurnManager _instance;

    public delegate Player OnNewPlayer(Player p);
    public delegate void OnNewTurn();

    public static OnNewPlayer onNewPlayer;
    public static OnNewTurn onNewTurn;

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

    public static Player currentPlayer { get; private set; }


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

        //A safety check so the game wont crash
        if (amountOfHumanPlayers == 0)
            amountOfHumanPlayers++;

        //Spawn player instances
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
            turnIndex.text = "Turn:" + (turnCount + 1);

            for (playerCount = 0; playerCount < players.Count; playerCount++)
            {
                currentPlayer = players[playerCount];

                if (currentPlayer is Human)
                {
                    //HUMAN STUFF             
                    playerIndex.text = "Player:" + (playerCount + 1);
                    yield return new WaitForUIButtons(nextTurnButton);
                }
                else
                {
                    //AI STUFF
                    playerIndex.text = "Player:" + (playerCount + 1);
                    yield return new WaitForSeconds(0.5f);     

                }

                onNewPlayer?.Invoke(players[playerCount]);
            }

            onNewTurn?.Invoke();

        }
    }
}
