using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mode : MonoBehaviour {
    public int magazine;
    private bool next_bullet = false;
    public float range;
    public GameObject muzzle;
    public float muzzleRadius;
    public ModeList modeList;
    public GameObject nearEnemy;
    public AudioClip[] SE;
    AudioSource audioSource;
    public int mode = 0;

    void Start()
    {
        muzzleRadius = muzzle.GetComponent<SphereCollider>().radius;

        modeList = Resources.Load("ModeList/mode") as ModeList;

        audioSource = GetComponent<AudioSource>();

        range = modeList.param[0].Range;
        magazine = modeList.param[0].Bullet;

        Debug.Log("私の戦闘力は" + magazine + "発ですよ！！");
    }

    void Update()
    {
        Debug.DrawLine(muzzle.transform.position, muzzle.transform.position + muzzle.transform.forward * range);
        
        if (Input.GetKey(KeyCode.Space))
        {
            if (magazine == 0)
            {
                Debug.Log("弾切れだ！！");
            }

            if (magazine > 0 && !next_bullet)
            {
                Shoot();
                audioSource.PlayOneShot(SE[mode], 1.0f);
                magazine--;
                next_bullet = true;
                StartCoroutine("Cool_Time");
            }
        }
    }

    void Shoot()
    {
        Vector3 cameraCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Ray ray = new Ray(muzzle.transform.position, cameraCenter +new Vector3(range*10.0f, 0, 0));
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
            nearEnemy.transform.SendMessage("Damage", modeList.param[mode].Power);
            Debug.Log("hit");
        }

        Debug.DrawRay(ray.origin, ray.direction * range, Color.red, 1.0f, false);
    }

    void Change_Mode(string ID)
    {
        if(ID=="G")
        {
            mode = 1;
            range = modeList.param[1].Range;
            magazine = modeList.param[1].Bullet;
            Debug.Log("ｺﾞﾘ");
        }
        else if (ID == "K")
        {
            mode = 2;
            range = modeList.param[2].Range;
            magazine = modeList.param[2].Bullet;
            Debug.Log("ｾｲﾔ");
        }
        else if (ID == "Ks")
        {
            mode = 3;
            range = modeList.param[3].Range;
            magazine = modeList.param[3].Bullet;
            Debug.Log("ﾁｬｷ");
        }
        else if (ID == "S")
        {
            mode = 4;
            range = modeList.param[4].Range;
            magazine = modeList.param[4].Bullet;
            Debug.Log("ﾊﾟﾙﾌﾟﾝﾃ");
        }
    }

    IEnumerator Cool_Time()
    {
        float time = modeList.param[mode].Speed;
        yield return new WaitForSeconds(time);
        next_bullet = false;
    }

    IEnumerator Reload()
    {
        yield return new WaitForSeconds(modeList.param[0].Speed);
        magazine = modeList.param[0].Bullet;
        Debug.Log("俺のリロードはレボ☆リューションッ！！");
    }
}
