using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedCamera : MonoBehaviour
{
    public Vector3 Pos;
    public GameObject Player;

    Quaternion Rot;


    void Start()
    {
        //Pos = transform.localPosition;
        Rot = transform.rotation;
    }

    void Update()
    {
        if (Player == null)
            return;

        transform.position = Player.transform.position + Pos;
        transform.rotation = Rot;
    }

    public void Shake(float Time = 0.1f, float Scale = 0.25f)
    {
        transform.position -= new Vector3(0.0f, Scale, 0.0f);
    }
}
