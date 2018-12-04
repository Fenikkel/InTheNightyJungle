using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrinkingTestGlassBehaviour : MonoBehaviour {

	private Transform rightHandBone;
	private Vector2 positionInBone;

	private Transform testPlace;
	private Transform placeOverBar;

	public Sprite[] sprites;
	private int currentSprite;

	// Use this for initialization
	void Start () {
		Restart();
	}

	public void Restart()
	{
		currentSprite = 0;
		GetComponent<SpriteRenderer>().sprite = sprites[currentSprite];
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void InitializeGlass(Transform param0, Vector2 param1, Transform param2)
	{
		placeOverBar = param0;
		positionInBone = param1;
		testPlace = param2;
	}

	public void SetRightHandBone(Transform param)
	{
		rightHandBone = param;
	}

	public void GlassInHand(string sortingLayer)
	{
		GetComponent<Transform>().parent = rightHandBone;
		GetComponent<Transform>().localPosition = positionInBone;
		GetComponent<Transform>().rotation = Quaternion.Euler(Vector2.zero);
		SetSortingLayer(sortingLayer);
	}

	public void GlassOverBar()
	{
		GetComponent<Transform>().parent = testPlace;
		GetComponent<Transform>().localPosition = placeOverBar.localPosition;
		GetComponent<Transform>().rotation = Quaternion.Euler(Vector2.zero);
	}

	public void SetSortingLayer(string sortingLayer)
	{
		GetComponent<SpriteRenderer>().sortingLayerName = sortingLayer;
	}

	public void NextSprite(float value)
	{
		if(value > (currentSprite + 1) / ((float)sprites.Length))
		{
			GetComponent<SpriteRenderer>().sprite = sprites[++currentSprite];
		}
	}
}
