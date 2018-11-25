using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Anima2D;

public class DrinkingShadow : MonoBehaviour {

	private Animator anim;
	public float minTime;
	public float maxTime;
	private float timeToNextAnim;
	private float elapsedTime;

	public int idInLayer;
	public string sortingLayer;
	public SpriteMeshInstance[] bodyParts;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
		elapsedTime = 0;
		SetOrderInLayer();
	}
	
	// Update is called once per frame
	void Update () {
		
		if(timeToNextAnim == 0) timeToNextAnim = Random.Range(minTime, maxTime);
		elapsedTime += Time.deltaTime;

		if(elapsedTime >= timeToNextAnim)
		{
			elapsedTime = 0;
			timeToNextAnim = 0;
			anim.SetTrigger("drink");
		}
	}

	private void SetOrderInLayer()
	{
		for(int i = 0; i < bodyParts.Length; i++)
		{
			bodyParts[i].sortingLayerName = sortingLayer;
			bodyParts[i].sortingOrder = idInLayer * bodyParts.Length + i;
		}
	}
}
