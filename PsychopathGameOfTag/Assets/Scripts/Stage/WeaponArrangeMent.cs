using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponArrangeMent : MonoBehaviour {

    [SerializeField]
    private int arrangeNumber;       //アイテム出現位置の最大数

    private GameObject[] arrangePoint;

    private Timer timer;
    [SerializeField]
    private float arrangeTime = 30.0f;

    bool arrangeFlag;

    int randomValue;

    // Use this for initialization
    void Start () {
        arrangePoint = new GameObject[arrangeNumber];
        for(int i = 1; i < arrangePoint.Length + 1; i++) {
            arrangePoint[i - 1] = GameObject.Find("ArrangePoint" + i);
        }
        
        timer = new Timer(arrangeTime);
        arrangeFlag = false;
    }

	// Update is called once per frame
	void Update () {
        timer.Update();
        randomValue = Random.Range(0, arrangePoint.Length);

        if (isAllArrange()) return;

        if(timer.RemainTime <= 0 && !arrangeFlag) {
            var prefab = Resources.Load<GameObject>("Prefab/WeaponObject" + randomValue);
            ArrangeObject(arrangePoint[randomValue], prefab);            
            arrangeFlag = true;
        }
        
        if(timer.RemainTime > 0 && arrangeFlag) {
            arrangeFlag = false;
        }
	}

    void ArrangeObject(GameObject parent,GameObject prefab)
    {        
        if (parent.transform.childCount <= 0) {
            Instantiate(prefab, parent.transform);
            return;
        }
        else {
            Debug.Log("Already Arrange");
            ArrangeObject(arrangePoint[Random.Range(0,arrangePoint.Length)], prefab);
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
