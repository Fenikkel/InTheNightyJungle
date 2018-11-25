using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Anima2D;

public class DrinkingChallengerBehaviour : MonoBehaviour {

	private string sortingLayer;

	private DrinkingTestGlassBehaviour glass;
	public Transform rightHandBone;

	// Use this for initialization
	void Start () {
		sortingLayer = GetComponentInChildren<SpriteMeshInstance>().sortingLayerName;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetGlass(DrinkingTestGlassBehaviour param)
	{
		glass = param;
		glass.SetRightHandBone(rightHandBone);
	}

	private void AimGlass()
	{
		glass.SetRightHandBone(rightHandBone);
		glass.GlassInHand(sortingLayer);
	}

	private void LeaveGlass()
	{
		glass.GlassOverBar();
	}

	public void PlayAimGlass()
	{

	}

	public void PlayDrinking(bool param)
	{

	}

	public void PlayLeaveGlass()
	{

	}
}
