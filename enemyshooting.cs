using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyshooting : MonoBehaviour
{
   public GameObject bulletPrefab;
   public Vector3 bulletoffset = new Vector3(0,0.5f,0);
   public float fireDelay=5f;
   float cooldownTimer=0;
    
    void Update()
    {   
        cooldownTimer-=Time.deltaTime;
        if (cooldownTimer<=0)
        {
            
            cooldownTimer=fireDelay;
            Vector3 offset =transform.rotation* new Vector3(0,0.5f,0);
           GameObject bulletGO=(GameObject)Instantiate(bulletPrefab,transform.position+offset,transform.rotation);
           bulletGO.layer=gameObject.layer;
        }
    }
}
