using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ChasePlayer : NetworkBehaviour
{
    Player player;

    public int magazine;
    private bool next_bullet = false;
    public float range;

    public ModeList modeList;
    [SerializeField]
    public MuzzleType muzzle_Type;
    public GameObject muzzle;
    public float muzzleRadius;
    private Vector3 muzzleHalf;
    private Player nearEnemy;
    [SerializeField]
    private AudioClip[] SE;
    AudioSource audioSource;

    private int mode = 0;
    public int GetMode
    {
        get
        {
            return mode;
        }
    }

    public override void OnStartLocalPlayer()
    {
        StartChasePlayer();
        base.OnStartLocalPlayer();
    }

    public void Start()
    {
        StartChasePlayer();
    }


    // Use this for initialization
    void OnEnable()
    {
        muzzle = gameObject;
        if (muzzle_Type != null) {
            muzzleRadius = muzzle_Type.muzzTypeList[mode].GetComponent<SphereCollider>().radius;
        }

        if (modeList != null) {
            range = modeList.param[0].Range;
            magazine = modeList.param[0].Bullet;
        }
        else {
            range = 0;
            magazine = 0;
        }
    }

    public void StartChasePlayer()
    { 
        muzzle = GameObject.Find("Head"); ;
        muzzleRadius = muzzle_Type.muzzTypeList[mode].GetComponent<SphereCollider>().radius;
        range = modeList.param[0].Range;
        magazine = modeList.param[0].Bullet;
        player = GetComponent<Player>();
    }

    // Update is called once per frame
    public void Update()
    {
        if (!isLocalPlayer) return;



        if (MyInput.OnTrigger()) {
            if (magazine == 0) {
                //Debug.Log(gameObject.name + " : 弾切れだ！！");
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

        if (player.isDead) {
            if (!player.pointFlag) {
                player.CmdAddPoint(player.EnemyTeam(), 3);
                player.pointFlag = true;
            }

            player.DeadTime();
        }
        if (player.isChange) {
            player.ChangeType();
            return;
        }
    }

    void Shoot()
    {
        Ray ray = new Ray(transform.position, player.cameraForward);
        RaycastHit[] hits;

        if (modeList.param[mode].ID == "G" || modeList.param[mode].ID == "K" || modeList.param[mode].ID == "N") {
            hits = Physics.SphereCastAll(ray, muzzleRadius, range, LayerMask.GetMask("Enemy", "Block"));
        }
        else {
            hits = Physics.BoxCastAll(muzzle.transform.position, muzzleHalf / 2, muzzle.transform.forward, muzzle.transform.rotation, range, LayerMask.GetMask("Enemy", "Block"));
        }

        float enemyDis = range;
        float blockDis = range;
        nearEnemy = null;

        foreach (RaycastHit hit in hits) {
            if (hit.collider.tag == "Block") {
                if (hit.distance < blockDis) {
                    blockDis = hit.distance;
                }
            }
        }

        foreach (RaycastHit hit in hits) {
            if (hit.collider.gameObject.GetComponent<Player>().team != player.team && hit.distance < blockDis && hit.distance < enemyDis) {
                enemyDis = hit.distance;
                nearEnemy = hit.collider.gameObject.GetComponent<Player>();
            }
        }

        if (nearEnemy != null) {
            nearEnemy.CmdHitBullet(modeList.param[mode].Power);

            if (nearEnemy.isDead) {
                player.killEnemy = true;
            }
            Debug.Log("hit");
        }

        Debug.DrawRay(ray.origin, ray.direction * range, Color.red, 1.0f, false);
    }

    public void Change_Mode(string ID)
    {
        Color newColor = Color.white;

        if (ID == "G") {
            mode = 1;
            newColor = new Color(252, 0, 252, 1);
            Debug.Log("ｺﾞﾘ");
        }
        else if (ID == "K") {
            mode = 2;
            newColor = Color.green;
            Debug.Log("ｾｲﾔ");
        }
        else if (ID == "Ks") {
            mode = 3;
            newColor = Color.gray;
            Debug.Log("ﾁｬｷ");
        }
        else if (ID == "S") {
            mode = 4;
            newColor = Color.cyan;
            Debug.Log("ﾊﾟﾙﾌﾟﾝﾃ");
        }
        else if (ID == "N") {
            mode = 0;
            Debug.Log("ｱｼｸﾋﾞｦｸｼﾞｷﾏｼﾀｰ");
        }

        if (ID == "G" || ID == "K" || ID == "N") {
            muzzleRadius = muzzle_Type.muzzTypeList[mode].GetComponent<SphereCollider>().radius;
        }
        else {
            muzzleHalf = muzzle_Type.muzzTypeList[mode].GetComponent<BoxCollider>().size;
        }

        range = modeList.param[mode].Range;
        magazine = modeList.param[mode].Bullet;
        gameObject.GetComponent<MeshRenderer>().material.color = newColor;
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
