﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gorira : MonoBehaviour {

    private void OnTriggerStay(Collider col)

    {
        if (col.tag == "Player")
        {
            col.transform.SendMessage("Change_Mode", "G"); ;
        }
    }
}
