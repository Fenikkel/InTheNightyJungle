using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrinkingTestGlassBehaviour : MonoBehaviour {

	private Transform rightHandBone;
	private Vector2 positionInBone;

	private Transform testPlace;
	private Transform placeOverBar;

	public SpriteRenderer[] sprites;
	private int currentSprite;

	// Use this for initialization
	void Start () {
		Restart();
	}

	public void Restart()
	{
		currentSprite = 0;
		//GetComponent<SpriteRenderer>().sprite = sprites[currentSprite].sprite;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void InitializeGlass(Transform param0, Vector2 param1)
	{
		placeOverBar = param0;
		positionInBone = param1;
	}

	public void SetRightHandBone(Transform param)
	{
		rightHandBone = param;
	}

	public void GlassInHand(string sortingLayer)
	{
		GetComponent<Transform>().parent = rightHandBone;
		GetComponent<Transform>().position = positionInBone;
		GetComponent<SpriteRenderer>().sortingLayerName = sortingLayer;
	}

	public void GlassOverBar()
	{
		GetComponent<Transform>().parent = testPlace;
		GetComponent<Transform>().position = placeOverBar.position;
	}

	public void NextSprite()
	{
		GetComponent<SpriteRenderer>().sprite = sprites[++currentSprite].sprite;
	}
}
