using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WeaponArrangeMent : NetworkBehaviour {

    [SerializeField]
    private int arrangeNumber;       //アイテム出現位置の最大数

    private GameObject[] arrangePoint;

    private Timer timer;
    [SerializeField]
    private float arrangeTime = 30.0f;

    bool arrangeFlag;

    int randomValue;

    // Use this for initialization
    public override void OnStartServer () {
        arrangePoint = new GameObject[arrangeNumber];
        for(int i = 1; i < arrangePoint.Length + 1; i++) {
            arrangePoint[i - 1] = GameObject.Find("ArrangePoint" + i);
        }
        
        timer = new Timer(arrangeTime);
        arrangeFlag = false;
    }

	// Update is called once per frame
	void Update () {

        if (!isServer) return;

        timer.Update();
        randomValue = Random.Range(0, arrangePoint.Length);

        if (isAllArrange()) return;

        if(timer.RemainTime <= 0 && !arrangeFlag) {
            var prefab = Resources.Load<GameObject>("Prefab/WeaponObject" + randomValue);
            CmdArrangeObject(arrangePoint[randomValue], prefab);            
            arrangeFlag = true;
        }
        
        if(timer.RemainTime > 0 && arrangeFlag) {
            arrangeFlag = false;
        }
	}

    [Command]
    void CmdArrangeObject(GameObject parent,GameObject prefab)
    {        
        if (parent.transform.childCount <= 0) {
            GameObject obj = Instantiate(prefab, parent.transform);
            NetworkServer.Spawn(obj);
            return;
        }
        else {
            Debug.Log("Already Arrange");
            CmdArrangeObject(arrangePoint[Random.Range(0,arrangePoint.Length)], prefab);
        }
    }

    bool isAllArrange()
    {
        int currentArrange = 0;
        foreach(var point in arrangePoint) {
            if(point.transform.childCount > 0) {
                currentArrange++;
            }
        }

        return arrangeNumber <= currentArrange;
    }
}
