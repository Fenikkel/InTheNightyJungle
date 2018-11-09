using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShakeScript : MonoBehaviour {

    public static Animator anim;
	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        /*if (Input.GetKeyDown("t"))
        {
            anim.Play("Test");
        }*/
	}

    public static void ShakeCamera()
    {
        anim.Play("Test");
    }
}
