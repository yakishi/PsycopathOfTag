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

     //private List<Player> players = new List<Player>();

    public struct PlayerInfo
    {
        public NetworkInstanceId id;
        public int type; //0:Chase,1:escape

        public PlayerInfo(Player player)
        {
            id = player.ID;
            type = (int)player.Type;
        }

        public PlayerInfo(NetworkInstanceId pId,int typeInt)
        {
            id = pId;
            type = typeInt;
        }

        public void UpdateData(int pType)
        {
            type = pType;
        }
    }

    public class PlayerInfoList : SyncListStruct<PlayerInfo> { };

    PlayerInfoList playerList = new PlayerInfoList();

    [SyncVar]
    int red;
    [SyncVar]
    int blue;

    Player cp;
    int sendTypetoInt;

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

    public void AddPlayers(Player player)
    {

        playerList.Add(new PlayerInfo(player));

        foreach(PlayerInfo info in playerList) {
            Debug.Log(info.id + " : " + info.type);
        }
    }

    [Command]
    public void CmdUpdatePlayer(NetworkInstanceId pId,int typeInt)
    {
        Debug.Log(typeInt);

        for(int i = 0; i < playerList.Count; i++) {
            if (playerList[i].id == pId) {
                playerList.Add(new PlayerInfo(pId, typeInt));
                playerList.RemoveAt(i);
            }
        }

        foreach (PlayerInfo info in playerList) {
            Debug.Log(info.id + " : " + info.type);
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
            Player pObj = playerObjects[i].GetComponent<Player>();
            foreach(var p in playerList) {

                if (p.id != pObj.ID) continue;

                if(p.type != (int)pObj.Type) {
                    cp = pObj;
                    sendTypetoInt = (int)pObj.Type;

                    RpcSendPlayerInfo();
                }
            }

            //for (int j = 0; j < plyer.Count; j++) {
            //    Player pObj = playerObjects[i].GetComponent<Player>();

            //    if (players[j].ID != pObj.ID) continue;


            //    if (players[j].Type != pObj.Type) {
            //        RpcSendPlayerInfo(j);

            //    }

            //}
        }
    }

    [ClientRpc]
    public void RpcSendPlayerInfo()
    {
        switch (sendTypetoInt) {
            case 0:
                cp.gameObject.AddComponent<ChasePlayer>().StartChasePlayer();
                Destroy(cp.GetComponent<EscapePlayer>());
                break;
            case 1:
                cp.gameObject.AddComponent<EscapePlayer>().StartEscapePlayer();
                Destroy(cp.GetComponent<ChasePlayer>());
                break;
        }

        cp.Initialize();
        
        //cp.ChangeType(sendTypetoInt);
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
