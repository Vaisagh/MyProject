﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class selfdestruct : MonoBehaviour
{
    public float timer=8f;
 
    void Update()
    {
        timer-=Time.deltaTime;
        if(timer<=0)
        {
            Destroy(gameObject);
        }
    }
}
