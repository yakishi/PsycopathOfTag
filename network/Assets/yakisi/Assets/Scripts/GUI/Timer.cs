using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer{

    /// <summary>
    /// 制限時間(s)
    /// </summary>
    private readonly float limitTime_;
    public float LimitTime { get { return limitTime_; } }

    /// <summary>
    /// 現在時間
    /// </summary>
    private float currentTime;

    /// <summary>
    /// 残り時間(s)
    /// </summary>
    public float RemainTime { get { return Mathf.Floor(limitTime_ - currentTime); } }

    public delegate bool Fire();
    public Fire fire;

    public bool stop;

	/// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="limitTime"> 制限時間 </param>
	public Timer (float limitTime)
    {
        limitTime_ = limitTime + 1;
        stop = false;
        Reset();
	}

    public bool Update ()
    {
        if (stop) {
            return true;
        }

        currentTime += Time.deltaTime;
        if (currentTime >= limitTime_) {
            currentTime = 0.0f;
            return true;
        }
        return false;

    }

    public void Start()
    {
        Reset();
        stop = false;
    }

    public void Reset()
    {
        currentTime = 0.0f;
    }

    public void Stop()
    {
        stop = true;
    }

    public bool isRunning()
    {
        return !stop;
    }
}
