using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TimeGUI : MonoBehaviour {

    /// <summary>
    /// タイマー
    /// </summary>
    private Timer timer;

    /// <summary>
    /// テキスト
    /// </summary>
    [SerializeField]
    private Text timeText;

	// Use this for initialization
	void Start () {
        timer = Game.Timer;
        timeText = GameObject.Find("Time").GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {

        timer = Game.Timer;

        timeText.text = Conversion(timer.RemainTime);

        if(timer.RemainTime <= 0) {
            timer.Stop();
            SceneManager.LoadScene("RESULT");
            DontDestroyOnLoad(GameObject.Find("Game"));
        }
    }
    
    /// <summary>
    /// [秒]を[分:秒]になおす
    /// </summary>
    /// <param name="time"> 残り時間 </param>
    /// <returns> [分：秒] </returns>
    string Conversion(float time)
    {
        float minutes = Mathf.Floor(time / 60);
        float seconds = Mathf.Floor(time % 60);

        return string.Format("{0:00} : {1:00}", minutes, seconds);
    }
}