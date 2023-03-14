using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatUI : MonoBehaviour
{
    public GameObject UI;
    public GameObject Button;


    void Start()
    {
        UI.SetActive(false);
        Button.SetActive(true);
    }

    public void OnClickExitBtn()
    {
        UI.SetActive(false);
        Button.SetActive(true);
    }

    public void OnClickOpenBtn()
    {
        UI.SetActive(true);
        Button.SetActive(false);
    }
}
