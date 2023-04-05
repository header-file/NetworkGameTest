using UnityEngine;
using System.Collections;
using Photon.Pun;

public class Player_ : MonoBehaviourPunCallbacks
{
    public PhotonView PV;
	public CharacterController controller;
    public GameObject Pointer;

    Animator Anim;

    //Move
    public float Speed;
    public float Gravity;

    //Attack
    //bool IsAttacking;
    //int AttackCount;

    //Status
    int MaxHP;
    int CurHP;


    void Awake()
    {
        Gravity = 20.0f;
        Speed = 5.0f;

        //IsAttacking = false;
        //AttackCount = 0;
    }

	void Start ()
    {
        Anim = GetComponent<Animator>();

        if (PV.IsMine)
        {
            Camera.main.GetComponent<FixedCamera>().Player = gameObject;
            Pointer.SetActive(true);
        }
        else
            Pointer.SetActive(false);

        //GameManager.Inst().UiManager.UpperUI.SetBulletTxt(CurBullets.ToString() + " / " + MaxBullets.ToString());
    }

    void Update()
    {
        if (!PV.IsMine && PhotonNetwork.IsConnected)
            return;

        Move();
        Rotate();
        Attack();

        PointerRotate();
    }

    void Move()
    {
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");

        Anim.SetFloat("Vertical", v);
        Anim.SetFloat("Horizontal", h);

        transform.Translate(Vector3.forward * v  * Speed * Time.deltaTime);
        transform.Translate(Vector3.right * h * Speed * Time.deltaTime);
    }

    void Rotate()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit))
        {
            Vector3 dir = new Vector3(hit.point.x, transform.position.y, hit.point.z) - transform.position;
            transform.forward = dir;
        }
    }

    void Attack()
    {
        if (Input.GetMouseButton(0))
        {
            Anim.SetBool("IsAttack", true);

            if (Anim.GetInteger("AttackCount") > 0)
            {
                Anim.SetInteger("AttackCount", 2);
                Debug.Log(Anim.GetInteger("AttackCount"));
            }

            Anim.SetInteger("AttackCount", 1);
        }        
    }

    public void EndAttack()
    {
        if (Anim.GetInteger("AttackCount") <= 1)
            ResetAnim();
    }

    public void ResetAnim()
    {
        Anim.SetBool("IsAttack", false);
        Anim.SetInteger("AttackCount", 0);
    }

    public void Move(Vector2 Normal, float Sqr)
    {
        if (!PV.IsMine && PhotonNetwork.IsConnected)
            return;

        Anim.SetInteger("Direction", 1);

        Vector3 vec = new Vector3(Normal.x * Speed * Time.deltaTime * Sqr, 0.0f, Normal.y * Speed * Time.deltaTime * Sqr);
        transform.eulerAngles = new Vector3(0.0f, Mathf.Atan2(Normal.x, Normal.y) * Mathf.Rad2Deg, 0.0f);

        if(controller.isGrounded)
           vec.y -= Gravity * Time.deltaTime;

        controller.Move(vec);
    }

    void PointerRotate()
    {
        if (!Pointer.activeSelf)
            return;

        Pointer.transform.Rotate(0.0f, 1.0f, 0.0f);
    }

    public void Stop()
    {
        Anim.SetInteger("Direction", 0);
    }

    public void Damage(int Dmg)
    {
        CurHP -= Dmg;

        //UI

        if (CurHP <= 0)
            Die();
    }

    void Die()
    {

    }
}
