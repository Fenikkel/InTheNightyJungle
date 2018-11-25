using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Anima2D;

public class BarmanBehaviour : MonoBehaviour {

	public Transform leftPosition;
	public Transform rightPosition;

	public DrinkingTestGlassBehaviour leftGlass;
	public DrinkingTestGlassBehaviour rightGlass;

	private DrinkingTestGlassBehaviour currentGlass;

	public Transform rightHandBone;

	private string sortingLayer;

	// Use this for initialization
	void Start () {
		sortingLayer = GetComponentInChildren<SpriteMeshInstance>().sortingLayerName;		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void PlayChangeGlass(bool leftSide)
	{
		if(leftSide) currentGlass = leftGlass;
		else currentGlass = rightGlass;
	}

	private void AimGlass()
	{
		currentGlass.SetRightHandBone(rightHandBone);
		currentGlass.GlassInHand(sortingLayer);
	}

	private void RefillGlass()
	{
		currentGlass.Restart();
	}

	private void LeaveGlass()
	{
		currentGlass.GlassOverBar();
	}
}
