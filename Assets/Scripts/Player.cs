using UnityEngine;
using System.Collections;
using Photon.Pun;

public class Player : MonoBehaviourPunCallbacks
{
    public PhotonView PV;
	public CharacterController controller;
    public GameObject BulletPref;
    public GameObject Flash;
    public Transform BulletPos;
    public GameObject Pointer;
    public NameTag NameTag;

    Animator Anim;

    //Move
    public float Speed;
    public float Gravity;

    //Shoot
    int CurBullets;
    int MaxBullets;
    bool IsReload;

    //Status
    int MaxHP;
    int CurHP;


    void Awake()
    {
        Gravity = 20.0f;

        CurBullets = 80;
        MaxBullets = 320;
        IsReload = false;
    }

	void Start ()
    {
        //GameManager.Inst().Pad.Player = GetComponent<Player>();
        //GameObject.Find("Pad").GetComponent<Pad>().Player = GetComponent<Player>();
        Anim = GetComponent<Animator>();

        if (PV.IsMine)
        {
            Camera.main.GetComponent<FixedCamera>().Player = gameObject;
            //Pointer.SetActive(true);
            NameTag.gameObject.SetActive(false);
        }
        else
        {
            //Pointer.SetActive(false);
            NameTag.gameObject.SetActive(true);
        }

        GameManager.Inst().UiManager.UpperUI.SetBulletTxt(CurBullets.ToString() + " / " + MaxBullets.ToString());
    }

    void Update()
    {
        if (!PV.IsMine && PhotonNetwork.IsConnected)
            return;

        Move();
        Rotate();
        Shoot();
        Reload();

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

    void Shoot()
    {
        if (IsReload || CurBullets <= 0)
        {
            Anim.SetBool("IsShoot", false);
            return;
        }

        if (Input.GetMouseButton(0))
            Anim.SetBool("IsShoot", true);
        else
            Anim.SetBool("IsShoot", false);
    }

    public void MuzzleFlash()
    {
        if (CurBullets <= 0)
        {
            Anim.SetBool("IsShoot", false);
            return;
        }

        Flash.SetActive(true);
        Camera.main.GetComponent<FixedCamera>().Shake();

        Bullet();
    }

    void Bullet()
    {
        Bullet bullet = Instantiate(BulletPref).GetComponent<Bullet>();
        bullet.transform.position = BulletPos.position;
        bullet.transform.rotation = transform.rotation;
        bullet.Shoot();

        CurBullets -= 1;
        GameManager.Inst().UiManager.UpperUI.SetBulletTxt(CurBullets.ToString() + " / " + MaxBullets.ToString());
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

    void Reload()
    {
        if (CurBullets >= 80)
            return;

        if (Input.GetKey(KeyCode.R) ||
            (Input.GetMouseButton(0) && CurBullets <= 0))
        {
            IsReload = true;

            Anim.SetTrigger("Reload");
        }
    }

    public void CalcReload()
    {
        if (MaxBullets + CurBullets >= 80)
        {
            MaxBullets -= (80 - CurBullets);
            CurBullets = 80;
        }
        else
        {
            CurBullets += MaxBullets;
            MaxBullets = 0;            
        }
        GameManager.Inst().UiManager.UpperUI.SetBulletTxt(CurBullets.ToString() + " / " + MaxBullets.ToString());

        IsReload = false;
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
