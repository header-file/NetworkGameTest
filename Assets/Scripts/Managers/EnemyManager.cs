using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject Enemy_Skeleton;

    void Start()
    {
        Spawn();
    }

    void Update()
    {
        
    }

    void Spawn()
    {
        for (int i = 0; i < 50; i++)
        {
            GameObject obj = Instantiate(Enemy_Skeleton);

            float x = Mathf.Cos(Random.Range(-Mathf.PI, Mathf.PI)) * Random.Range(40, 60);
            float z = Mathf.Sin(Random.Range(-Mathf.PI, Mathf.PI)) * Random.Range(40, 60);

            obj.transform.position = new Vector3(x, 0.0f, z);
        }
    }
}
