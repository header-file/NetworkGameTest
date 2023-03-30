using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Dice : MonoBehaviour, IPunObservable
{
    public int Number;
    public bool IsRolling;
    public bool IsLocked;

    Rigidbody Rig;
    Dictionary<Vector3, int> Dir;
    Vector3 OriPos;


    void Start()
    {
        Rig = GetComponent<Rigidbody>();
        Rig.useGravity = false;

        OriPos = transform.position;
        Number = 0;
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
        if (IsLocked)
            return;

        if (!IsRolling)
            Spin();
        else
            CheckValue();
    }

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

        Rig.useGravity = true;
        IsRolling = true;
    }

    public void ResetPos(Quaternion quat)
    {
        Rig.useGravity = false;
        transform.position = OriPos;
        transform.rotation = quat;
    }

    public void StayMode(Vector3 pos, Quaternion quat)
    {
        Rig.useGravity = true;
        transform.position = pos;
        transform.rotation = quat;
    }

    #region IPunObservable implementation

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(IsRolling);
            stream.SendNext(Rig.useGravity);
        }
        else
        {
            IsRolling = (bool)stream.ReceiveNext();
            Rig.useGravity = (bool)stream.ReceiveNext();
        }
    }

    #endregion
}
