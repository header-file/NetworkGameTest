using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager Instance;

    //public Player Player;
    public UIManager UiManager;
    public NetworkManager NetManager;
    public TurnManager TurnManager;
    public ScoreManager ScoManager;


    public static GameManager Inst() { return Instance; }

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        //Cursor.lockState = CursorLockMode.Confined;
    }
}
