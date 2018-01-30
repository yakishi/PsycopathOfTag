using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour {

    [SerializeField]
    Button[] buttons = new Button[3];

	// Use this for initialization
	void Start () {
        buttons[0].OnClickAsObservable()
            .Subscribe(_ => {
                SceneManager.LoadScene("Test");
            })
            .AddTo(this);

        buttons[1].OnClickAsObservable()
            .Subscribe(_ => {
                SceneManager.LoadScene("CREDIT");
            })
            .AddTo(this);

        buttons[2].OnClickAsObservable()
            .Subscribe(_ => {
                Application.Quit();
            })
            .AddTo(this);

        for(int i = 0;i < buttons.Length; i++) {
            buttons[i].enabled = true;
        }

        buttons[0].Select();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
