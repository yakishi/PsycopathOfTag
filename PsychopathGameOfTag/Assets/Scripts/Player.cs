using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UniRx;

public class Player : NetworkBehaviour {

    public enum PlayerMode
    {
        Chase,
        Escape
    }

    [SyncVar]
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

    [SyncVar]
    [System.NonSerialized]
    public Vector3 cameraForward;
    [System.NonSerialized]
    public Vector3 moveForward;

    [SyncVar]
    public Game.Team team;

    public ModeList modeList;
    [SerializeField]
    public MuzzleType muzzle_Type;

    protected bool pointFlag;

    public bool catchTrap;

    private MainCamera mainCamera;
    private UIController uIController;

    protected int mode = 0;
    public int getMode
    {
        get
        {
            return mode;
        }
    }

    public override void OnStartLocalPlayer()
    {
        Initialize();
    }

    public override void OnStartServer()
    {
        Initialize();
        base.OnStartServer();
    }

    protected void Initialize()
    {
        game = GameObject.Find("Game").GetComponent<Game>();

        mainCamera = GameObject.Find("Main Camera").GetComponent<MainCamera>();
        uIController = GameObject.Find("Canvas").GetComponent<UIController>();

        mainCamera.SetPlayer(gameObject);
        uIController.SetPlayer(this);

        player = this.gameObject;
        respownTime = 5;
        hp = 10;
        time = new Timer(respownTime);
        pointFlag = false;
        catchTrap = false;
    }

    public void SetGame(Game g)
    {
        game = g;
    }

    public virtual void ChangeType(Player p)
    {
        Initialize();
        game = p.game;
    }

    public virtual void Update()
    {
        if (!isLocalPlayer) return;

        if (Input.GetKeyDown(KeyCode.T)) {
            SceneManager.LoadScene("RESULT");
            DontDestroyOnLoad(GameObject.Find("Game"));
        }

        if (Input.GetKeyDown(KeyCode.C)) {
            hp = 0;
        }
    }

    [Command]
    public void CmdHitBullet(int damage)
    {
        hp -=damage;
        Debug.Log(gameObject.name + " HP : " + hp);
        if(hp <= 0) {
            hp = 0;
        }
    }

    [Command]
    public void CmdAddPoint(Game.Team enemyTeam,int point)
    {
        game.AddPoint(enemyTeam, point);
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
                CmdHitBullet((int)trap.Power);
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

    public Game.Team EnemyTeam()
    {
        Game.Team temp = team;
        switch (team) {
            case Game.Team.red:
                temp = Game.Team.blue;
                break;
            case Game.Team.blue:
                temp = Game.Team.red;
                break;
        }

        return temp;
    }

    protected void DeadTime()
    {
        time.Update();

        if (time.RemainTime <= 0) {
            switch (type) {
                case PlayerMode.Chase:
                    Destroy(GetComponent<ChasePlayer>());
                    player.AddComponent<EscapePlayer>();
                    player.GetComponent<EscapePlayer>().ChangeType(this);
                    type = PlayerMode.Escape;
                    break;
                case PlayerMode.Escape:
                    Destroy(GetComponent<EscapePlayer>());
                    player.AddComponent<ChasePlayer>();
                    player.GetComponent<ChasePlayer>().ChangeType(this);
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

            hp = 20;
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
