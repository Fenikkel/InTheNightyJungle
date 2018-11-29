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

	private bool leftBusyBarman;
	private bool rightBusyBarman;

	private Animator anim;

	// Use this for initialization
	void Start () {
		sortingLayer = GetComponentInChildren<SpriteMeshInstance>().sortingLayerName;
		anim = GetComponent<Animator>();		
		leftBusyBarman = rightBusyBarman = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public IEnumerator PlayChangeGlass(bool leftSide, float time)
	{
		if(leftSide) 
		{
			currentGlass = leftGlass;

			GetComponent<Transform>().localScale = new Vector3( -1 * Mathf.Abs(GetComponent<Transform>().localScale.x), GetComponent<Transform>().localScale.y, GetComponent<Transform>().localScale.z);
			StartCoroutine(MoveTo(leftPosition.position, time));

			yield return new WaitForSeconds(time);

			anim.SetTrigger("changeGlass");
		}
		else
		{
			currentGlass = rightGlass;

			GetComponent<Transform>().localScale = new Vector3(Mathf.Abs(GetComponent<Transform>().localScale.x), GetComponent<Transform>().localScale.y, GetComponent<Transform>().localScale.z);
			StartCoroutine(MoveTo(rightPosition.position, time));

			yield return new WaitForSeconds(time);
			
			anim.SetTrigger("changeGlass");
		} 
	}

	private void ResetChangeGlass()
	{
		anim.ResetTrigger("changeGlass");
		leftBusyBarman = rightBusyBarman = false;
	}

	public bool GetLeftBusyBarman()
	{
		return leftBusyBarman;
	}

	public bool GetRightBusyBarman()
	{
		return rightBusyBarman;
	}

	public void SetLeftBusyBarman(bool param)
	{
		leftBusyBarman = param;
	}

	public void SetRightBusyBarman(bool param)
	{
		rightBusyBarman = param;
	}

	public string GetSortingLayer()
	{
		return sortingLayer;
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

	private IEnumerator MoveTo(Vector2 finalPosition, float time)
	{
		float elapsedTime = 0.0f;
		Vector2 initialPosition = GetComponent<Transform>().position;

		while(elapsedTime < time)
		{
			elapsedTime += Time.deltaTime;
			GetComponent<Transform>().position = Vector2.Lerp(initialPosition, finalPosition, elapsedTime / time);
			yield return null;
		}
		GetComponent<Transform>().position = finalPosition;
	}

}
