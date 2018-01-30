using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UniRx;

public class Credit : MonoBehaviour {

    [SerializeField]
    Button button;

	// Use this for initialization
	void Start () {
        button.OnClickAsObservable()
            .Subscribe(_ => {
                SceneManager.LoadScene("Psychopath Title");
            })
            .AddTo(this);
        button.Select();
	}

}
