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

    Dictionary<Team, int> team;
    public Dictionary<Team, int> getTeam
    {
        get
        {
            return team;
        }

        set
        {
            team = value;
        }
    }

    public override void OnStartServer()
    {
        game = this;
        GameStart();

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        ui = GameObject.Find("Canvas").GetComponent<UIController>();
        ui.SetGame();

        foreach (var i in players) {
            i.GetComponent<Player>().SetGame(game);
        }
        base.OnStartServer();
    }

    //private void Start()
    //{
    //    game = this;
    //    GameStart();

    //    GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
    //    ui = GameObject.Find("Canvas").GetComponent<UIController>();
    //    ui.SetGame();

    //    foreach (var i in players) {
    //        i.GetComponent<Player>().SetGame(game);
    //    }
    //}

    // Use this for initialization
    //public override void OnStartLocalPlayer()
    //{
    //    GameStart();

    //    GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

    //    foreach (var i in players) {
    //        i.GetComponent<Player>().SetGame(game);
    //    }
    //    base.OnStartLocalPlayer();
    //}

    void GameStart()
    {
        timer = new Timer(limitTime);
        team = new Dictionary<Team, int>();
        team.Add(Team.red, 0);
        team.Add(Team.blue, 0);

    }

    // Update is called once per frame
    void Update()
    {
        if (timer.isRunning()) {
            timer.Update();
        }

        if (!isServer) return;


        if (ui != null) {
            RpcSendPointText();
        }

    }

    public int getTeamPoint(Team side)
    {
        return team[side];
    }

    [ClientRpc]
    public void RpcSendPointText()
    {
        ui.Points.text = "<color=#ff0000>" + team[Game.Team.red] + "</color> / " + "<color=#0000ff>" + team[Game.Team.blue] + "</color>";
    }



    //public void AddPoint(Team side, int point)
    //{
    //    List<Team> list = new List<Team>(team.Keys);
    //
    //    foreach (var t in list) {
    //        if (t == side) {
    //            team[t] += point;
    //        }
    //    }
    //}


}
