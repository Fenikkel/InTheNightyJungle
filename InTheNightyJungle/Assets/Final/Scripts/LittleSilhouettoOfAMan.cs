using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LittleSilhouettoOfAMan : MonoBehaviour {

	private Animator anim;
	public float minTime;
	public float maxTime;
	private float timeToNextAnim;
	private float elapsedTime;

	public int totalNumAnim;
	private int currentAnim;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
		timeToNextAnim = Random.Range(minTime, maxTime);
		elapsedTime = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
		elapsedTime += Time.deltaTime;
		if(elapsedTime >= timeToNextAnim)
		{
			int newAnim;
			do
			{
				newAnim = Random.Range(0, totalNumAnim);
			}
			while(newAnim == currentAnim);
			currentAnim = newAnim;
			anim.SetTrigger("dancing" + currentAnim);

			elapsedTime = 0.0f;
			timeToNextAnim = Random.Range(minTime, maxTime);
		}
	}
}
