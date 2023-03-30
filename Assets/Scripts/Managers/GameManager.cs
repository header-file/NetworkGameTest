using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using NetPlayer = Photon.Realtime.Player;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class GameManager : MonoBehaviour
{
    private static GameManager Instance;

    public Player Player;
    public List<Player> OtherPlayers;

    public UIManager UiManager;
    public NetworkManager NetManager;
    public TurnManager TurnManager;
    public ScoreManager ScoManager;

    string[] ScoreName;


    public static GameManager Inst() { return Instance; }

    public string GetScoreName(int index) { return ScoreName[index]; }

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        SetCustomProperties();

        OtherPlayers = new List<Player>();
    }

    void Start()
    {
        //Cursor.lockState = CursorLockMode.Confined;
    }

    void SetCustomProperties()
    {
        ScoreName = new string[12];
        ScoreName[0] = "Ones";
        ScoreName[1] = "Twos";
        ScoreName[2] = "Threes";
        ScoreName[3] = "Fours";
        ScoreName[4] = "Fives";
        ScoreName[5] = "Sixes";
        ScoreName[6] = "FourOfAKind";
        ScoreName[7] = "FullHouse";
        ScoreName[8] = "LittleStraight";
        ScoreName[9] = "BigStraight";
        ScoreName[10] = "Yacht";
        ScoreName[11] = "Choice";
    }

    public void SpanwPlayer()
    {
        PhotonNetwork.Instantiate("Prefabs/Player", Vector3.zero, Quaternion.identity);
    }
}
