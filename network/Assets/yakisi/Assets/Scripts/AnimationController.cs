using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour {

    [SerializeField]
    Animator anim;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PlayAnimation(string animation)
    {
        anim.SetBool(animation, true);
    }

    public void StopAnimation(string animation)
    {
        anim.SetBool(animation, false);
    }
}
