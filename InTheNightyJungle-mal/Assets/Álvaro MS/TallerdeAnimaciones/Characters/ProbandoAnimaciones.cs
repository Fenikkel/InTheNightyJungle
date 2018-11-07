using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProbandoAnimaciones : MonoBehaviour {

    public Animator anim;
    public string animacion;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space))
        {
            anim.SetTrigger("anim");
        }
	}
    
}
