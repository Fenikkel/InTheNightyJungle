using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangingColorMainMenuBG : MonoBehaviour {

	public float timeBetweenColors;

	// Use this for initialization
	void Start () {
		StartCoroutine(ChangeColor());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private IEnumerator ChangeColor()
	{
		Color finalColor = new Color(Random.value, Random.value, Random.value, GetComponent<SpriteRenderer>().color.a);
		Color initialColor = GetComponent<SpriteRenderer>().color;

		float elapsedTime = 0.0f;
		while(elapsedTime < timeBetweenColors)
		{
			elapsedTime += Time.deltaTime;
			GetComponent<SpriteRenderer>().color = Color.Lerp(initialColor, finalColor, elapsedTime / timeBetweenColors);
			yield return null;
		}
		GetComponent<SpriteRenderer>().color = finalColor;
		StartCoroutine(ChangeColor());
	}
}
