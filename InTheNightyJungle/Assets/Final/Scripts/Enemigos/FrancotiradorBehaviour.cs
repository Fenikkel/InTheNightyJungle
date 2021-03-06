﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrancotiradorBehaviour : EnemyBehaviour {

	[SerializeField]
	private float noThrowingTime, launchSpeed, timeToReachPlayer, slowDownTime, slowDownFactor;

	private float elapsedTime;
	private GameObject target;

	[SerializeField]
	private GameObject icePrefab; //Prefab
	private GameObject iceObj;
	[SerializeField]
	private Transform rightHandBone;

	[SerializeField]
	private AudioSource iceSound, throwSound;

	// Use this for initialization
	void Awake()
	{
        anim = GetComponent<Animator>();
	}

	void Start () {
		elapsedTime = 0.0f;
		target = null;

		NewIce();
	}
	
	// Update is called once per frame
	void Update () {
		if(!death)
		{
			if(target != null)
			{
				elapsedTime += Time.deltaTime;
				if(elapsedTime >= noThrowingTime)
				{
					elapsedTime = 0.0f;
					ThrowIce();
				}

			}
		}
	}

	private void ThrowIce()
	{
		anim.SetTrigger("ThrowIce");
	}

	private void LeaveIce()
	{
		throwSound.Play();
		iceObj.GetComponent<Transform>().SetParent(null);
		iceObj.GetComponent<CircleCollider2D>().enabled = true;
		if(target != null)
			iceObj.GetComponent<IceBehaviour>().Launch(target.GetComponent<Transform>().position, launchSpeed, timeToReachPlayer, damage, slowDownTime, slowDownFactor, iceSound);
		else
			Destroy(iceObj.gameObject);
	}

	private void NewIce()
	{
		anim.ResetTrigger("ThrowIce");
		iceObj = Instantiate(icePrefab, rightHandBone);
		iceObj.GetComponent<CircleCollider2D>().enabled = false;
	}

	public void SetTarget(GameObject param)
	{
		target = param;
	}

	public override void Death(int normalDirection)
    {
        GameObject parent = GetComponent<Transform>().parent.gameObject;
        GetComponent<Transform>().SetParent(null);
        Destroy(parent);
        base.Death(normalDirection);
    }
}
