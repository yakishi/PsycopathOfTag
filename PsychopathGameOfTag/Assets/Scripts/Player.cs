﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UniRx;

public class Player : MonoBehaviour {

    public enum PlayerMode
    {
        Chase,
        Escape
    }

    protected PlayerMode type;
    public PlayerMode Type
    {
        get
        {
            return type;
        }
    }
    protected GameObject player;

    [SerializeField]
    protected int hp;
    public int HP
    {
        get
        {
            return hp;
        }
    }

    public bool isDead
    {
        get
        {
            return hp <= 0;
        }
    }

    [SerializeField]
    protected Game game;

    protected Timer time;
    protected float respownTime;
    float nearPlayerDistance;

    [System.NonSerialized]
    public Vector3 cameraForward;
    [System.NonSerialized]
    public Vector3 moveForward;

    public Game.Team team;

    public ModeList modeList;

    protected bool pointFlag;

    public bool catchTrap;

    protected int mode = 0;
    public int getMode
    {
        get
        {
            return mode;
        }
    }


    private void Start()
    {
        Initialize();
    }

    protected void Initialize()
    {
        player = this.gameObject;
        respownTime = 5;
        hp = 50;
        time = new Timer(respownTime);
        pointFlag = false;
        catchTrap = false;
    }

    public virtual void ChangeType(Player p)
    {
        Initialize();
        game = p.game;
    }

    public virtual void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.T)) {
            SceneManager.LoadScene("RESULT");
            DontDestroyOnLoad(GameObject.Find("Game"));
        }

        if (Input.GetKeyDown(KeyCode.C)) {
            hp = 0;
        }
    }

    public void HitBullet(int damage)
    {
        hp -=damage;
        Debug.Log(gameObject.name + " HP : " + hp);
        if(hp <= 0) {
            hp = 0;
        }
    }

    public virtual void AddPoint()
    {

    }

    public void CatchTrap(TrapList.Param trap,GameObject obj)
    {
        if (trap == null) return;

        switch (trap.ID) {
            case "Sw":
                catchTrap = true;
                Observable.Timer(System.TimeSpan.FromSeconds(trap.Time))
                .Take(1)
                .Subscribe(_ => {
                    catchTrap = false;
                    Destroy(obj);
                })
                .AddTo(this);
                break;
            case "J":
                HitBullet((int)trap.Power);
                Observable.Timer(System.TimeSpan.FromSeconds(0.5))
                .Take(1)
                .Subscribe(_ => {
                    Destroy(obj);
                    catchTrap = false;
                })
                .AddTo(this);
                
                break;
            case "N":
                catchTrap = true;
                Observable.Timer(System.TimeSpan.FromSeconds(trap.Time))
                .Take(1)
                .Subscribe(_ => {
                    if(type == PlayerMode.Chase) {
                        ChasePlayer cahser = gameObject.GetComponent<ChasePlayer>();
                        cahser.Change_Mode("0");
                    }
                    Destroy(obj);
                    catchTrap = false;
                })
                .AddTo(this);
                break;

        }
    }

    protected void DeadTime()
    {
        time.Update();

        if (time.RemainTime <= 0) {
            switch (type) {
                case PlayerMode.Chase:
                    player.AddComponent<EscapePlayer>();
                    player.GetComponent<EscapePlayer>().ChangeType(this);
                    Destroy(GetComponent<ChasePlayer>());
                    type = PlayerMode.Escape;
                    break;
                case PlayerMode.Escape:
                    player.AddComponent<ChasePlayer>();
                    player.GetComponent<ChasePlayer>().ChangeType(this);
                    Destroy(GetComponent<EscapePlayer>());
                    type = PlayerMode.Chase;
                    break;
                default:
                    return;
            }

            foreach (Transform n in this.gameObject.transform) {
                if (n.gameObject.tag == "Weapon") {
                    Destroy(n.gameObject);
                }
            }

            hp = 50;
            pointFlag = false;
            time.Reset();
        }
    }

    /// <summary>
    /// 一番近いプレイヤータグを検索
    /// </summary>
    /// <param name="nowObj"></param>
    /// <param name="tagName"></param>
    /// <returns></returns>
    GameObject SerchTag(GameObject nowObj)
    {
        float tmpDis = 0;           //距離用一時変数
        float nearDis = 0;          //最も近いオブジェクトの距離
        string tagName = "Player";
        //string nearObjName = "";    //オブジェクト名称
        GameObject targetObj = null; //オブジェクト

        //タグ指定されたオブジェクトを配列で取得する
        foreach (GameObject obs in GameObject.FindGameObjectsWithTag(tagName)) {
            //自身を除外
            if (obs == gameObject) continue;
            //自身と取得したオブジェクトの距離を取得
            tmpDis = Vector3.Distance(obs.transform.position, nowObj.transform.position);

            //オブジェクトの距離が近いか、距離0であればオブジェクト名を取得
            //一時変数に距離を格納
            if (nearDis == 0 || nearDis > tmpDis) {
                nearDis = tmpDis;
                //nearObjName = obs.name;
                targetObj = obs;
            }

        }

        //最も近かったオブジェクトを返す
        //return GameObject.Find(nearObjName);
        return targetObj;
    }
}
