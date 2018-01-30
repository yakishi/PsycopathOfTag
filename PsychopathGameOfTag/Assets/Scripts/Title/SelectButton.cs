using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectButton : MonoBehaviour {

    Vector3 defScale;

	// Use this for initialization
	void Start () {
		
	}

    public void Select()
    {
        defScale = gameObject.transform.localScale;

        Vector3 temp = new Vector3(gameObject.transform.localScale.x * 1.2f, gameObject.transform.localScale.y * 1.2f, gameObject.transform.localScale.z);

        gameObject.transform.localScale = temp;
    }

    public void DeSelect()
    {
        if(defScale != null)
            gameObject.transform.localScale = defScale;
    }
}
