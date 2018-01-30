using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{

    public int HP = 500;
    public WeaponList gunList;

    void Start()
    {
        gunList = Resources.Load("WeaponList/Gun") as WeaponList;
    }

    public void Damage(int damage)
    {
        HP -= damage;
        if (HP <= 0)
        {
            Destroy(gameObject);
        }
    }

}
