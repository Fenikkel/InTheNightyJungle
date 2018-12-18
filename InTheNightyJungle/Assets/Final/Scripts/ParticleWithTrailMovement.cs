using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleWithTrailMovement : MonoBehaviour {

	public int animationID;
	private Animator anim;

	// Use this for initialization
	void Start () {
		
		anim = GetComponent<Animator>();
		anim.Play("movement" + animationID.ToString());
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
