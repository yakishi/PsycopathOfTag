using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class Trap : MonoBehaviour {
    private TrapList trapList;
    private GameObject trap;
    [System.NonSerialized]
    public TrapList.Param trapInfo;
    private Game.Team team;

    [SerializeField]
    ParticleSystem particle;

    [SerializeField]
    private string trapId;
    public string ID
    {
        get
        {
            return trapId;
        }
        set
        {
            trapId = value;
        }
    }

    // Use this for initialization
    void Start () {
        trapList = Resources.Load("TrapList/Trap") as TrapList;

        if (trapId == null) trapId = "";

        trapId.ObserveEveryValueChanged(_ => trapId)
            .Subscribe(_ => {
                SetTrapInfo();
            })
            .AddTo(this);
    }

    public void SetTrapInfo()
    {
        foreach (var t in trapList.param) {
            if (trapId == t.ID) {
                trapInfo = t;
            }
        }
    }

    public void SetPlayerTeamInfo(Game.Team playerTeam)
    {
        team = playerTeam;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Player") return;

        Player collisionPlayer = other.gameObject.GetComponent<Player>();

        if (collisionPlayer.team == team) return;

        particle.Play();

        collisionPlayer.CatchTrap(this.trapInfo, gameObject);
    }

}
