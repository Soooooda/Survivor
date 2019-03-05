using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerActController : MonoBehaviour {
    public Animator anim;
	// Use this for initialization
	void Start () {
        anim.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.S))
            anim.SetBool("forward",true);
        else// if (Input.GetKey(KeyCode.S))
            anim.SetBool("forward", false);
        if (Input.GetKey(KeyCode.D))
            anim.SetBool("right", true);
        else// if (Input.GetKey(KeyCode.S))
            anim.SetBool("right", false);

    }
}
