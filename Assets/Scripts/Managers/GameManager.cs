using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Pun;

public class GameManager : MonoBehaviour
{
    private static GameManager Instance;

    //public Player Player;
    public UIManager UiManager;
    public NetworkManager NetManager;
    public TurnManager TurnManager;
    public ScoreManager ScoManager;

    public Hashtable GamePlayerProperties;

    string[] ScoreName;


    public static GameManager Inst() { return Instance; }

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        SetCustomProperties();
    }

    void Start()
    {
        //Cursor.lockState = CursorLockMode.Confined;
    }

    void SetCustomProperties()
    {
        GamePlayerProperties = new Hashtable();
        GamePlayerProperties.Add("Ones", 0);
        GamePlayerProperties.Add("Twos", 0);
        GamePlayerProperties.Add("Threes", 0);
        GamePlayerProperties.Add("Fours", 0);
        GamePlayerProperties.Add("Fives", 0);
        GamePlayerProperties.Add("Sixes", 0);
        GamePlayerProperties.Add("FourOfAKind", 0);
        GamePlayerProperties.Add("FullHouse", 0);
        GamePlayerProperties.Add("LittleStraight", 0);
        GamePlayerProperties.Add("BigStraight", 0);
        GamePlayerProperties.Add("Yacht", 0);
        GamePlayerProperties.Add("Choice", 0);

        ScoreName = new string[12];
        ScoreName[0] = "Ones";
        ScoreName[0] = "Twos";
        ScoreName[0] = "Threes";
        ScoreName[0] = "Ones";
        ScoreName[0] = "Fours";
        ScoreName[0] = "Fives";
        ScoreName[0] = "Sixes";
        ScoreName[0] = "FourOfAKind";
        ScoreName[0] = "FullHouse";
        ScoreName[0] = "LittleStraight";
        ScoreName[0] = "BigStraight";
        ScoreName[0] = "Choice";
    }

    public void ResetScore(int index, int score)
    {
        GamePlayerProperties.Remove(ScoreName[index]);
        GamePlayerProperties.Add(ScoreName[index], score);
        PhotonNetwork.LocalPlayer.SetCustomProperties(GamePlayerProperties);
    }
}
