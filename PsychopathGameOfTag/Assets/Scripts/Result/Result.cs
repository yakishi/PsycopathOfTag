using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UniRx;

public class Result : MonoBehaviour {

    [SerializeField]
    Button[] buttons = new Button[2];

    Game game;

    [SerializeField]
    Text redTeamPoint;

    [SerializeField]
    Text blueTeamPoint;

    [SerializeField]
    Sprite winnerImg;
    [SerializeField]
    Sprite loserImg;

    [SerializeField]
    Image redResult;
    [SerializeField]
    Image blueResult;

    // Use this for initialization
    void Start () {
        game = GameObject.Find("Game").GetComponent<Game>();

        buttons[0].OnClickAsObservable()
            .Subscribe(_ => {
                //ロビーへの遷移
                //SceneManager.LoadScene("");
            })
            .AddTo(this);

        buttons[1].OnClickAsObservable()
            .Subscribe(_ => {
                SceneManager.LoadScene("Psychopath Title");
            })
            .AddTo(this);

        redTeamPoint.text = game.getTeamPoint(Game.Team.red).ToString();
        blueTeamPoint.text = game.getTeamPoint(Game.Team.blue).ToString();

        if(game.getTeamPoint(Game.Team.red) > game.getTeamPoint(Game.Team.blue)) {
            redResult.sprite = winnerImg;
            blueResult.sprite = loserImg;
        }
        else if(game.getTeamPoint(Game.Team.red) < game.getTeamPoint(Game.Team.blue)) {
            redResult.sprite = loserImg;
            blueResult.sprite = winnerImg;
        }
        else {
            redResult.sprite = winnerImg;
            blueResult.sprite = winnerImg;
        }

        buttons[0].Select();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
