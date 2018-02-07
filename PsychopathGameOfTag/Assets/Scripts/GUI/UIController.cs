using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class UIController : MonoBehaviour {

    [SerializeField]
    Game game;

    [SerializeField]
    Sprite redImage;
    [SerializeField]
    Sprite blueImage;

    [SerializeField]
    Image teamIcon;

    [SerializeField]
    Player player;

    [SerializeField]
    Text points;
    public Text Points
    {
        get
        {
            return points;
        }
        set
        {
            points = value;
        }
    }

    [SerializeField]
    Image modeImg;

    [SerializeField]
    Sprite[] imgs;
    
	// Use this for initialization
	void Start () {
		
	}
	
    public void SetPlayer(Player p)
    {
        player = p;
    }

    public void SetGame()
    {
        game = Game.getGame;
    }

	// Update is called once per frame
	void Update () {
        if (player == null) return;

        TeamIconDisplayer();
        if(player.Type == Player.PlayerMode.Chase)  modeImg.sprite = WeaponImage(player.getMode);
    }

    Sprite WeaponImage(int mode)
    {
        switch (mode) {
            case 1:
                return imgs[0];
            case 2:
                return imgs[1];
            case 3:
                return imgs[2];
            case 4:
                return imgs[3];
            default:
                return imgs[4];
        }
    }

    void TeamIconDisplayer()
    {
        switch (player.team) {
            case Game.Team.red:
                teamIcon.sprite = redImage;
                break;
            case Game.Team.blue:
                teamIcon.sprite = blueImage;
                break;
            default:
                break;
        }
    }

}
