using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float Speed;
    public GameObject Eff_Impact;

    Rigidbody Rig;
    int Power;


    void Awake()
    {
        Rig = GetComponent<Rigidbody>();
        Power = 1;
    }

    void Update()
    {
        var planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
        foreach(var p in planes)
        {
            if (p.GetDistanceToPoint(transform.position) < 0.0f)
                Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Player player = collision.gameObject.GetComponent<Player>();
            if (!player.PV.IsMine)
            {
                player.Damage(Power);

                HitEffect(collision.GetContact(0));
            }

            gameObject.SetActive(false);
        }
        else if(collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<Enemy>().Damage(Power);

            HitEffect(collision.GetContact(0));

            gameObject.SetActive(false);
        }
        else if(collision.gameObject.tag == "Wall")
            HitEffect(collision.GetContact(0));
    }

    public void Shoot()
    {
        Rig.AddForce(transform.forward * Speed);
    }

    void HitEffect(ContactPoint Point)
    {
        GameObject eff = Instantiate(Eff_Impact);
        eff.transform.position = Point.point;
        eff.transform.rotation = Quaternion.Euler(Point.normal);
    }
}
