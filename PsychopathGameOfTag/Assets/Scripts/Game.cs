using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour {

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

    [SerializeField]
    GameObject playerPrefabs;

    /// <summary>
    /// タイマー
    /// </summary>
    static private Timer timer;
    static public Timer Timer { get { return timer; } }

    [SerializeField]
    float limitTime;

    Dictionary<Team, int> team;

    Player[ , ] players;
    
    // Use this for initialization
    void Start () {
        GameStart();
    }

    void GameStart()
    {
        timer = new Timer(limitTime);
        team = new Dictionary<Team, int>();
        team.Add(Team.red, 0);
        team.Add(Team.blue, 0);
        players = new Player[2,4];

        /*float posX = 1.0f;
        float posZ = -3.0f;
        Vector3 pos;
        GameObject obj;
        for(int i = 0;i < 2; i++) {
            for (int j = 0; j < 4; j++) {
                pos = new Vector3(posX, 0.15f, posZ);
                obj = GameObject.Instantiate(playerPrefabs, pos, Quaternion.identity);
                if(j % 2 == 0) {
                    obj.AddComponent<TestChase>();
                }
                else {
                    obj.AddComponent<EscapePlayer>();
                }

                players[i ,j] = obj.GetComponent<Player>();

                if(i == 0) {
                    players[i, j].team = Team.red;
                }
                else {
                    players[i, j].team = Team.blue;
                }
            }

        }*/
    }
	
	// Update is called once per frame
	void Update () {
        if (timer.isRunning()) {
            timer.Update();
        }
	}

    public int getTeamPoint(Team side)
    {
        return team[side];
    }

    public void AddPoint(Team side,int point)
    {
        List<Team> list = new List<Team>(team.Keys);

        foreach(var t in list) {
            if(t == side) {
                team[t] += point;
            }
        }

        ;
    }
}
