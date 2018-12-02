using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreathLightController : MonoBehaviour {

	private Vector2 originalSize;
	public float lifeTime;

	// Use this for initialization
	void Start () {

	}

	void OnEnable()
	{
		if(originalSize == Vector2.zero) originalSize = GetComponent<Transform>().localScale;
		GetComponent<Transform>().localScale = originalSize;
		StartCoroutine(Scale(lifeTime));
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private IEnumerator Scale(float time)
	{
		float elapsedTime = 0.0f;

		Vector2 initialSize = GetComponent<Transform>().localScale;
		Vector2 finalSize = GetComponent<Transform>().localScale * 8;

		while(elapsedTime < time)
		{
			elapsedTime += Time.deltaTime;
			GetComponent<Transform>().localScale = Vector2.Lerp(initialSize, finalSize, elapsedTime / time);
			yield return null;
		}
		GetComponent<Transform>().localScale = finalSize;
	}
}
