using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackRange : MonoBehaviour
{
    public CapsuleCollider Col;

    //int Power;


    void Awake()
    {
        Col = GetComponent<CapsuleCollider>();

        //Power = 2;
    }

    void Start()
    {
        Col.enabled = false;
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //collision.gameObject.GetComponent<Player>().Damage(Power);
            Debug.Log("You Damaged!");
        }
    }
}
