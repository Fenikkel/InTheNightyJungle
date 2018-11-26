using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Anima2D;

public class DrinkingChallengerBehaviour : MonoBehaviour {

	private string sortingLayer;

	private DrinkingTestGlassBehaviour glass;
	public Transform rightHandBone;

	private Animator anim;

	// Use this for initialization
	void Start () {
		sortingLayer = GetComponentInChildren<SpriteMeshInstance>().sortingLayerName;
		anim = GetComponent<Animator>();
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

	public void PlayDrinking(bool param)
    {
        anim.SetBool("drinking", param);
        anim.ResetTrigger("aimGlass");
    }

    public void PlayAimGlass()
    {
        anim.SetTrigger("aimGlass");
        anim.ResetTrigger("leaveGlass");
    }

    public void PlayLeaveGlass()
    {
        anim.SetTrigger("leaveGlass");
    }
}
