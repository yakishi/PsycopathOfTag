using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class Game : MonoBehaviour
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

    /// <summary>
    /// タイマー
    /// </summary>
    static private Timer timer;
    static public Timer Timer { get { return timer; } }

    [SerializeField]
    float limitTime;

    Dictionary<Team, int> team;

    private void Start()
    {
        game = this;
        GameStart();

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach (var i in players) {
            i.GetComponent<Player>().SetGame(game);
        }
    }

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
    }

    public int getTeamPoint(Team side)
    {
        return team[side];
    }

    public void AddPoint(Team side, int point)
    {
        List<Team> list = new List<Team>(team.Keys);

        foreach (var t in list) {
            if (t == side) {
                team[t] += point;
            }
        }

        //ui.RpcscoreSync
    }


}
