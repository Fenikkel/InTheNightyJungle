using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeiserBehaviour : EnemyBehaviour {

	public float throwingTime;
	public float noThrowingTime;

	public float pukeWallGrowingTime;
	public Transform pukeWall;

	private bool throwing;
	private float elapsedTime;

	public ParticleSystem puke;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(throwing)
		{
			if(elapsedTime == 0)
			{
				anim.SetBool("throwing", true);
			}
			elapsedTime += Time.deltaTime;
			if(noThrowingTime != 0 && throwingTime <= elapsedTime)
			{
				elapsedTime = 0;
				throwing = false;
			}
		}
		else
		{
			if(elapsedTime == 0)
			{
				anim.SetBool("throwing", false);
			}
			elapsedTime += Time.deltaTime;
			if(throwingTime != 0 && noThrowingTime <= elapsedTime)
			{
				elapsedTime = 0;
				throwing = true;
			}
		}
	}

	void OnEnable()
	{
		elapsedTime = 0;
		throwing = true;
		anim.SetBool("throwing", false);
		puke.Stop();
		pukeWall.localScale = new Vector3(1,0,1);
	}

	public void StartPuke()
	{
		puke.Play();
		StartCoroutine(GrowWall());
	}

	public void StopPuke()
	{
		puke.Stop();
		UngrowWall();
	}

	private IEnumerator GrowWall()
	{
		float elapsedTime = 0;
		Vector3 initialScale = new Vector3(1,0,1);
		Vector3 finalScale = new Vector3(1,1,1);

		while(elapsedTime < pukeWallGrowingTime)
		{
			elapsedTime += Time.deltaTime;
			pukeWall.localScale = Vector3.Lerp(initialScale, finalScale, elapsedTime / pukeWallGrowingTime);
			yield return null;
		}
		pukeWall.localScale = finalScale;
	}

	private void UngrowWall()
	{
		pukeWall.localScale = new Vector3(1,0,1);
	}
}
