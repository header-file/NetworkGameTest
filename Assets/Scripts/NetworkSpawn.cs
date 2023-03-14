using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkSpawn : MonoBehaviour
{
    int count = 0;


    void Update()
    {
        count++;

        if (count >= 900)
            gameObject.SetActive(false);
    }

    void OnDisable()
    {
        count = 0;
    }
}
