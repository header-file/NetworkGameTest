using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Dice : MonoBehaviour, IPunObservable
{
    public PhotonView PV;
    public int Number;
    public bool IsRolling;
    public bool IsLocked;

    Rigidbody Rig;
    Dictionary<Vector3, int> Dir;
    Vector3 OriPos;
    Vector3 NetworkPosition;
    Quaternion NetworkRotation;


    void Awake()
    {
        Rig = GetComponent<Rigidbody>();
        Rig.useGravity = false;

        OriPos = transform.position;
        Number = Random.Range(1, 7);
        IsRolling = false;

        Dir = new Dictionary<Vector3, int>();
        Dir[Vector3.up] = 5;
        Dir[Vector3.right] = 4;
        Dir[Vector3.forward] = 1;
        Dir[-Vector3.up] = 2;
        Dir[-Vector3.right] = 3;
        Dir[-Vector3.forward] = 6;
    }

    void Update()
    {
        if (IsLocked || !GameManager.Inst().TurnManager.CheckIsTurn())
            return;

        if (!IsRolling)
            Spin();
        else
            CheckValue();
    }

    //void FixedUpdate()
    //{
    //    if(!PV.IsMine)
    //    {
    //        Rig.position = Vector3.MoveTowards(Rig.position, NetworkPosition, Time.fixedDeltaTime);
    //        Rig.rotation = Quaternion.RotateTowards(Rig.rotation, NetworkRotation, Time.fixedDeltaTime * 100.0f);
    //    }
    //}

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
            collision.transform.parent.gameObject.GetComponent<Box>().StartCheck();
    }

    void CheckValue()
    {
        float min = float.MaxValue;
        Vector3 TargetUp = transform.InverseTransformDirection(Vector3.up);

        foreach (Vector3 d in Dir.Keys)
        {
            float angle = Vector3.Angle(TargetUp, d);

            if(angle <= float.Epsilon && angle < min)
            {
                min = angle;
                Number = Dir[d];
            }
        }
    }

    void Spin()
    {
        int x = Random.Range(1, 10);
        int y = Random.Range(1, 10);
        int z = Random.Range(1, 10);
        Vector3 rot = new Vector3(x, y, z);

        transform.Rotate(rot);
    }

    public void StartRoll()
    {
        if (IsLocked)
            return;

        Rig.velocity = Vector3.zero;
        Rig.useGravity = true;
        IsRolling = true;
    }

    public void ResetPos(Quaternion quat)
    {
        Rig.velocity = Vector3.zero;
        Rig.useGravity = false;
        transform.position = OriPos;
        transform.rotation = quat;
    }

    public void StayMode(Vector3 pos, Quaternion quat)
    {
        Rig.velocity = Vector3.zero;
        Rig.useGravity = true;
        transform.position = pos;
        transform.rotation = quat;
    }

    public void SetOwner()
    {
        PV.TransferOwnership(GameManager.Inst().Player.PV.Owner);
    }

    public void Show(bool IsShow)
    {
        Rig.useGravity = false;
        Rig.velocity = Vector3.zero;
        IsRolling = false;

        if (IsShow)
        {
            IsRolling = false;
        }
        else
        {
            IsRolling = true;
        }
    }

    #region IPunObservable implementation

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(IsRolling);
            stream.SendNext(Rig.useGravity);

            //stream.SendNext(transform.position);
            //stream.SendNext(transform.rotation);
            //stream.SendNext(Rig.velocity);
        }
        else
        {
            IsRolling = (bool)stream.ReceiveNext();
            Rig.useGravity = (bool)stream.ReceiveNext();

            //NetworkPosition = (Vector3)stream.ReceiveNext();
            //NetworkRotation = (Quaternion)stream.ReceiveNext();
            //Rig.velocity = (Vector3)stream.ReceiveNext();

            //float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
            //NetworkPosition += Rig.velocity * lag;
        }
    }

    #endregion
}
