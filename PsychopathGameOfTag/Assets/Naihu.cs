using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Naihu : MonoBehaviour {

    private void OnTriggerStay(Collider col)
    {
        if (Input.GetKeyDown(KeyCode.Space) && col.tag == "Enemy")
        {
            Destroy(col.gameObject);
        }
    }
}