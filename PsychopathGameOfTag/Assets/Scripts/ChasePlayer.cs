using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasePlayer : Player {
    public int magazine;
    private bool next_bullet = false;
    public float range;
    public GameObject muzzle;
    public float muzzleRadius;
    private Player nearEnemy;
    [SerializeField]
    private AudioClip[] SE;
    AudioSource audioSource;

    void Start()
    {
        Initialize();
        type = PlayerMode.Chase;
    }


    // Use this for initialization
    void OnEnable () {
        muzzle = GameObject.Find("Head");
        muzzleRadius = muzzle.GetComponent<SphereCollider>().radius;

        if (modeList != null) {
            range = modeList.param[0].Range;
            magazine = modeList.param[0].Bullet;
        }
        else {
            range = 0;
            magazine = 0;
        }
    }

    public override void ChangeType(Player p)
    {
        modeList = p.modeList;

        base.ChangeType(p);
    }

    // Update is called once per frame
    public override void FixedUpdate () {
        type = PlayerMode.Chase;

        if (MyInput.OnTrigger()) {
            if (magazine == 0) {
                Debug.Log(gameObject.name + " : 弾切れだ！！");
            }

            if (magazine > 0 && !next_bullet) {
                Shoot();
                magazine--;
                next_bullet = true;
                StartCoroutine("Cool_Time");
            }
        }

        if (Input.GetKeyDown(KeyCode.F1)) {
            Change_Mode("G");
        }
        if (Input.GetKeyDown(KeyCode.F2)) {
            Change_Mode("K");
        }
        if (Input.GetKeyDown(KeyCode.F3)) {
            Change_Mode("Ks");
        }
        if (Input.GetKeyDown(KeyCode.F4)) {
            Change_Mode("S");
        }

        base.FixedUpdate();
    }


    void Shoot()
    {
        //Vector3 cameraCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        
        Ray ray = new Ray(player.transform.position,cameraForward) ;
        RaycastHit[] hits = Physics.SphereCastAll(ray, muzzleRadius, range, LayerMask.GetMask("Enemy", "Block"));

        float enemyDis = range;
        float blockDis = range;
        nearEnemy = null;

        foreach (RaycastHit hit in hits) {
            if (hit.collider.tag == "Block") {
                if (hit.distance < blockDis) {
                    blockDis = hit.distance;
                    Debug.Log("ｶｷﾝｯ");
                }
            }
        }

        foreach (RaycastHit hit in hits) {
            if (hit.collider.gameObject.GetComponent<Player>().team != this.team && hit.distance < blockDis && hit.distance < enemyDis) {
                enemyDis = hit.distance;
                nearEnemy = hit.collider.gameObject.GetComponent<Player>();
                Debug.Log("ﾋﾃﾞﾌﾞｯ");
            }
        }

        if (nearEnemy != null) {
            nearEnemy.HitBullet(modeList.param[mode].Power);
            Debug.Log("hit");
        }

        Debug.DrawRay(ray.origin, ray.direction * range, Color.red, 1.0f, false);
    }

    public void Change_Mode(string ID)
    {
        if (ID == "G") {
            mode = 1;
            range = modeList.param[1].Range;
            magazine = modeList.param[1].Bullet;
            gameObject.GetComponent<MeshRenderer>().material.color = new Color(252,0,252,0);
            Debug.Log("ｺﾞﾘ");
        }
        else if (ID == "K") {
            mode = 2;
            range = modeList.param[2].Range;
            magazine = modeList.param[2].Bullet;
            gameObject.GetComponent<MeshRenderer>().material.color = Color.green;
            Debug.Log("ｾｲﾔ");
        }
        else if (ID == "Ks") {
            mode = 3;
            range = modeList.param[3].Range;
            magazine = modeList.param[3].Bullet;
            gameObject.GetComponent<MeshRenderer>().material.color = Color.gray;
            Debug.Log("ﾁｬｷ");
        }
        else if (ID == "S") {
            mode = 4;
            range = modeList.param[4].Range;
            magazine = modeList.param[4].Bullet;
            gameObject.GetComponent<MeshRenderer>().material.color = Color.cyan;
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
