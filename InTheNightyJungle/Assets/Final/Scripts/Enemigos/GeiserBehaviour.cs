using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeiserBehaviour : EnemyBehaviour {

	public float throwingTime;
	public float noThrowingTime;
	public float delayTime;

	private bool begin;

	public float pukeWallGrowingTime;

	private bool throwing;
	private float elapsedTime;

	public ParticleSystem puke;

	[SerializeField]
	private AudioSource vomitSound;

	// Use this for initialization
	void Start () 
	{
		TestDelay();
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

	private void TestDelay()
	{
		begin = delayTime == 0;
		if(!begin)
		{
			StartCoroutine(Delaying());
		}
	}

	private IEnumerator Delaying()
	{
		yield return new WaitForSeconds(delayTime);
		begin = true;
	}

	void OnEnable()
	{
		TestDelay();
		elapsedTime = 0;
		throwing = true;
		anim.SetBool("throwing", false);
		puke.Stop();
	}

	public void StartPuke()
	{
		puke.Play();
		vomitSound.Play();
	}

	public void StopPuke()
	{
		puke.Stop();
	}

	public override IEnumerator Steal(Transform rightHand)
	{
		yield return null;
		//Aquí no tiene que ir nada porque no se le puede robar a un géiser
	}
}
