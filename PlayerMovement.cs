using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float maxspeed=20f;
    public float rotspeed=180;
    float shipboundaryradius=10f;
    // Start is called before the first frame update
    void Start()
    { 
        
    }

    // Update is called once per frame
    void Update()
    {
        Quaternion rot=transform.rotation;
        float z = rot.eulerAngles.z;
        z-=Input.GetAxis("Horizontal")*rotspeed*Time.deltaTime;
        rot=Quaternion.Euler(0,0,z);
        transform.rotation=rot;

        Vector3 pos = transform.position;
        Vector3 velocity=new Vector3(0,Input.GetAxis("Vertical")*maxspeed*Time.deltaTime,0);
        pos+=rot*velocity;
        if(pos.y+shipboundaryradius>Camera.main.orthographicSize)
        {
            pos.y=Camera.main.orthographicSize-shipboundaryradius;
        }
        if(pos.y-shipboundaryradius<-Camera.main.orthographicSize)
        {
            pos.y=-Camera.main.orthographicSize+shipboundaryradius;
        }

        float ScreenRatio= (float)Screen.width/(float)Screen.height;
        float widthOrtho=Camera.main.orthographicSize*ScreenRatio;
         if(pos.x+shipboundaryradius>widthOrtho)
        {
            pos.x=widthOrtho-shipboundaryradius;
        }
        if(pos.x-shipboundaryradius<-widthOrtho)
        {
            pos.x=-widthOrtho+shipboundaryradius;
        }
    
        transform.position=pos;
    }
}
