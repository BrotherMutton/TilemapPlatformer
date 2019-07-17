﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupRotator : MonoBehaviour
{

    public float speed;
    
    // Update is called once per frame
    void Update()
    {
        transform.Rotate (new Vector3 (0, 0, 45 * speed) * Time.deltaTime);
    }
}
