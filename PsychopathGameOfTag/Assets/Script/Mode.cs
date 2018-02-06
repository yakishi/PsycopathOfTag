using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mode : MonoBehaviour {
    private int magazine;
    private bool next_bullet = false;
    private float range;
    public GameObject muzzle;
    [SerializeField]
    GameObject[] muzzle_Type;
    private float muzzleRadius;
    private Vector3 muzzleHalf;
    public ModeList modeList;
    public GameObject nearEnemy;
    public AudioClip[] SE;
    AudioSource audioSource;
    public int mode = 0;

    void Start()
    {
        muzzleRadius = muzzle_Type[mode].GetComponent<SphereCollider>().radius;

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
                audioSource.PlayOneShot(SE[mode]);
                magazine--;
                next_bullet = true;
                StartCoroutine("Cool_Time");
            }
        }
    }

    void Shoot()
    {
        Ray ray = new Ray(muzzle.transform.position, muzzle.transform.forward);
        RaycastHit[] hits;

        if(modeList.param[mode].ID == "G" || modeList.param[mode].ID == "K" || modeList.param[mode].ID == "N")
        {
            hits = Physics.SphereCastAll(ray, muzzleRadius, range, LayerMask.GetMask("Enemy", "Block"));
        }
        else
        {
            hits = Physics.BoxCastAll(muzzle.transform.position, muzzleHalf/2, muzzle.transform.forward, muzzle.transform.rotation, range, LayerMask.GetMask("Enemy", "Block"));
        }

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
            Debug.Log("ｺﾞﾘ");
        }
        else if (ID == "K")
        {
            mode = 2;
            Debug.Log("ｾｲﾔ");
        }
        else if (ID == "Ks")
        {
            mode = 3;
            Debug.Log("ﾁｬｷ");
        }
        else if (ID == "S")
        {
            mode = 4;
            Debug.Log("ﾊﾟﾙﾌﾟﾝﾃ");
        }
        else if (ID == "N")
        {
            mode = 0;
            Debug.Log("ｱｼｸﾋﾞｦｸｼﾞｷﾏｼﾀｰ");
        }

        if (ID == "G" || ID == "K" || ID == "N")
        {
            muzzleRadius = muzzle_Type[mode].GetComponent<SphereCollider>().radius;
        }
        else
        {
            muzzleHalf = muzzle_Type[mode].GetComponent<BoxCollider>().size;
        }

        range = modeList.param[mode].Range;
        magazine = modeList.param[mode].Bullet;
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
