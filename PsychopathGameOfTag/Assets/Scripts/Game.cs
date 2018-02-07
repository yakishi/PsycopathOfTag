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

    static private List<Player> players = new List<Player>();

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

        RpcSendPlayerInfo();

        if (ui != null) {
            RpcSendPointText();
        }

    }

    public void UpdatePlayers(Player player)
    {
        bool newPlayer = true;
        for (int i = 0; i < players.Count; i++) {
            if (players[i].ID == player.ID) {
                players[i] = player;
                newPlayer = false;
                break;
            }
        }

        if (newPlayer) {
            players.Add(player);
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
    public void RpcSendPlayerInfo()
    {
        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");

        Debug.Log(players.Count);

        for(int i = 0;i < playerObjects.Length; i++) {
            for(int j= 0; j < players.Count; j++) {
                Player pObj = playerObjects[i].GetComponent<Player>();

                if (players[j].ID != pObj.ID) continue;
                
                if(players[j].Type != pObj.Type) {
                    switch (players[j].Type) {
                        case Player.PlayerMode.Chase:
                            playerObjects[i].AddComponent<ChasePlayer>().ChangeType(players[j]);
                            Destroy(playerObjects[i].GetComponent<EscapePlayer>());
                            break;
                        case Player.PlayerMode.Escape:
                            playerObjects[i].AddComponent<EscapePlayer>().ChangeType(players[j]);
                            Destroy(playerObjects[i].GetComponent<ChasePlayer>());
                            break;
                        default:
                            break;
                    }
                }
                
            }
        }
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
