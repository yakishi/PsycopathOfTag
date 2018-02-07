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

        return;
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

        CheckPlayer();

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

    void CheckPlayer()
    {
        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");

        for (int i = 0; i < playerObjects.Length; i++) {
            for (int j = 0; j < players.Count; j++) {
                Player pObj = playerObjects[i].GetComponent<Player>();

                if (players[j].ID != pObj.ID) continue;


                if (players[j].Type != pObj.Type) {
                    RpcSendPlayerInfo(j);

                }

            }
        }
    }

    [ClientRpc]
    public void RpcSendPlayerInfo(int i)
    {
        players[i].ChangeType();
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
