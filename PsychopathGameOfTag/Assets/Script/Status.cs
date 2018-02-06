//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class Status : MonoBehaviour
//{
//    public float speed = 3.0f;
//    private int HP = 5;
//    private TrapList trapList;
//    private bool stan = false;

//    void Start()
//    {
//        trapList = Resources.Load("TrapList/Trap") as TrapList;
//    }

//    private void Update()
//    {
//        if (stan != true)
//        {
//            //移動などのコマンドするもの
//            if (Input.GetKey(KeyCode.W))
//            {
//                transform.position += transform.forward * speed * Time.deltaTime;
//            }
//            if (Input.GetKey(KeyCode.S))
//            {
//                transform.position -= transform.forward * speed * Time.deltaTime;
//            }
//            if (Input.GetKey(KeyCode.D))
//            {
//                transform.position += transform.right * speed * Time.deltaTime;
//            }
//            if (Input.GetKey(KeyCode.A))
//            {
//                transform.position -= transform.right * speed * Time.deltaTime;
//            }
//        }
//    }

//    public void Damage(int damage)
//    {
//        HP -= damage;
//        if (HP < 1)
//        {
//            Destroy(gameObject);
//        }
//    }

//    void Stan_Mode()
//    {
//        stan = true;
//        StartCoroutine("Stan_Time");
//    }

//    IEnumerator Stan_Time()
//    {
//        Debug.Log("ｯｱｧー！！");
//        yield return new WaitForSeconds(trapList.param[0].Time);
//        stan = false;
//    }

//}
