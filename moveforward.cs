using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveforward : MonoBehaviour
{
    public float maxspeed=30f;

    void Update()
    {
        Vector3 pos = transform.position;
        Vector3 velocity=new Vector3(0,maxspeed*Time.deltaTime,0);
        pos+=transform.rotation*velocity;
        transform.position=pos;
    }
}
