using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class Game : NetworkBehaviour
{

    public enum Team
    {
        red,
        blue
    }
    public enum Mode
    {
        Chase,
        Escape
    }

    static Game game;
    static public Game getGame
    {
        get
        {
            return game;
        }
    }

    [SerializeField]
    UIController ui;

    /// <summary>
    /// タイマー
    /// </summary>
    static private Timer timer;
    static public Timer Timer { get { return timer; } }

    [SerializeField]
    float limitTime;

    //static Dictionary<Team, int> team = new Dictionary<Team, int>();
    //static public Dictionary<Team, int> getTeam
    //{
    //    get
    //    {
    //        return team;
    //    }

    //    set
    //    {
    //        team = value;
    //    }
    //}

    [SyncVar]
    int red;
    [SyncVar]
    int blue;

    public void Start()
    {
        //if (isServer) {
        //    game = this;
        //    GameStart();

        //    GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        //    ui = GameObject.Find("Canvas").GetComponent<UIController>();
        //    ui.SetGame();

        //    foreach (var i in players) {
        //        i.GetComponent<Player>().SetGame(game);
        //    }
        //    return;
        //}
        //if (isClient) {
        //    GameStart();

        //    GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        //    ui = GameObject.Find("Canvas").GetComponent<UIController>();
        //    ui.SetGame();

        //    foreach (var i in players) {
        //        i.GetComponent<Player>().SetGame(game);
        //    }
        //}

         game = this;
        GameStart();

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        ui = GameObject.Find("Canvas").GetComponent<UIController>();
        ui.SetGame();

        foreach (var i in players) {
            i.GetComponent<Player>().SetGame(game);
        }
        return;

        base.OnStartServer();
    }


    void GameStart()
    {
        timer = new Timer(limitTime);
        red = 0;
        blue = 0;
    }

 
    // Update is called once per frame
    void Update()
    {
        //if (timer.isRunning()) {
        //    timer.Update();
        //}

        if (!isServer) return;


        if (ui != null) {
            RpcSendPointText();
        }

    }

    public int getTeamPoint(Team side)
    {
        if (side == Team.red) {
            return red;
            
        }

        return blue;
    }

    [ClientRpc]
    public void RpcSendPointText()
    {
        ui.Points.text = "<color=#ff0000>" + red + "</color> / " + "<color=#0000ff>" + blue + "</color>";
    }

    public void AddPoint(Team side, int point)
    {
        if (side == Team.red) {
            red += point;
            return;
        }

        blue += point;
        
    }
}
