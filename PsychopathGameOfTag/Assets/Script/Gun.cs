using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    //public GameObject bullet;
    //public Transform b_spawn;
    public float speed = 100;
    public int magazine;
    private bool next_bullet = false;
    public float range;
    public GameObject muzzle;
    public float muzzleRadius;
    public WeaponList gunList;
    public GameObject nearEnemy;

    public int[] rack = new int[] { 0, 3, 4 };
    public int act = 0;
    public int weaponNumber = 0;
    private bool change_weapon = false;

    void Start()
    {
        muzzleRadius = muzzle.GetComponent<SphereCollider>().radius;

        gunList = Resources.Load("WeaponList/Gun") as WeaponList;

        range = gunList.param[rack[act]].Range;
        magazine = gunList.param[rack[act]].Bullet;

        Debug.Log("私の戦闘力は"+ magazine + "発ですよ！！");
    }

    void Update()
    {
        Debug.DrawLine(muzzle.transform.position, muzzle.transform.position + muzzle.transform.forward * range);

        if (Input.GetKey(KeyCode.C))
        {

        }

        if (Input.GetKey(KeyCode.Space))
        {
            if (magazine == 0)
            {
                Debug.Log("弾切れだ！！");
            }

            if (magazine > 0 && !next_bullet)
            {
                Shoot();
                magazine--;
                next_bullet = true;
                StartCoroutine("Cool_Time");
            }
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("リローリンｯ！！");
            StartCoroutine("Reload");
        }
    }

    void Shoot()
    {
        Vector3 cameraCenter = new Vector3(Screen.width/2, Screen.height/2, 0);
        Ray ray = new Ray(muzzle.transform.position, muzzle.transform.forward);
        RaycastHit[] hits = Physics.SphereCastAll(ray, muzzleRadius, range, LayerMask.GetMask("Enemy", "Block"));

        float enemyDis = range;
        float blockDis = range;
        nearEnemy = null;

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.tag == "Block")
            {
                if (hit.distance < blockDis)
                {
                    blockDis = hit.distance;
                    Debug.Log("ｶｷﾝｯ");
                }
            }
        }

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.tag == "Enemy" && hit.distance < blockDis && hit.distance < enemyDis)
            {
                enemyDis = hit.distance;
                nearEnemy = hit.collider.gameObject;
                Debug.Log("ﾋﾃﾞﾌﾞｯ");
            }
        }

        if (nearEnemy != null)
        {
            nearEnemy.transform.SendMessage("Damage", gunList.param[0].Power);
            Debug.Log("hit");
        }
        
        Debug.DrawRay(ray.origin, ray.direction * range, Color.red, 1.0f, false);
    }

    void Change_Act()
    {
        if(act == 2)
        {
            act = 0;
        }
        else
        {
            act += 1;
        }
    }

    IEnumerator Cool_Time()
    {
        float time = gunList.param[0].Speed;
        yield return new WaitForSeconds(1.0f / time);
        next_bullet = false;
    }

    IEnumerator Change_Time()
    {
        yield return new WaitForSeconds(1.0f);
        change_weapon = false;
    }

    IEnumerator Reload()
    {
        yield return new WaitForSeconds(gunList.param[0].Load);
        magazine = gunList.param[0].Bullet;
        Debug.Log("俺のリロードはレボ☆リューションッ！！");
    }
}