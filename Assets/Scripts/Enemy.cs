using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyAttackRange AttackRange;

    public float Speed;
    public float RotSpeed;

    Animator Anim;
    Player Player;

    int HP;


    void Start()
    {
        Anim = GetComponent<Animator>();
        Speed = 0.015f;
        RotSpeed = 5.0f;

        HP = 5;
    }

    void Update()
    {
        if (Anim.GetBool("IsDie"))
            return;

        Search();
        Move();
        Attack();
    }

    void Search()
    {
        if (Player != null)
            return;

        float min = 40.0f;
        Player[] players = FindObjectsOfType<Player>();
        foreach (Player p in players)
        {
            float dist = Vector3.Distance(transform.position, p.transform.position);
            if (dist < min)
            {
                min = dist;
                Player = p;
            }
        }

        Anim.SetBool("IsMove", true);
    }

    void Move()
    {
        if (Player == null || !Anim.GetBool("IsMove"))
            return;

        //transform.position = Vector3.MoveTowards(transform.position, Player.transform.position, Time.deltaTime * Speed);
        Vector3 dir = Vector3.RotateTowards(transform.forward, Player.transform.position - transform.position, Time.deltaTime * RotSpeed, 0.0f);
        transform.rotation = Quaternion.LookRotation(dir);
        transform.position += transform.forward * Speed;
    }

    void Attack()
    {
        if (Player == null || Anim.GetBool("IsAttack"))
            return;

        float dist = Vector3.Distance(transform.position, Player.transform.position);
        if (dist > 1.5f)
            return;

        Anim.SetBool("IsMove", false);
        Anim.SetBool("IsAttack", true);

        Invoke("StartMove", 1.5f);
    }

    public void StartAttack()
    {
        AttackRange.Col.enabled = true;
    }

    public void EndAttack()
    {
        AttackRange.Col.enabled = false;
    }

    public void StartMove()
    {
        Anim.SetBool("IsMove", true);
        Anim.SetBool("IsAttack", false);
    }

    public void Damage(int Damage)
    {
        HP -= Damage;

        if (HP <= 0)
            Die();
    }

    void Die()
    {
        Anim.SetBool("IsDie", true);
        Invoke("Disappear", 3.0f);
    }

    void Disappear()
    {
        Destroy(gameObject);
    }
}
