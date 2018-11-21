using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Anima2D;

public class LittleSilhouettoOfAMan : MonoBehaviour {

	private Animator anim;
	public float minTime;
	public float maxTime;
	private float timeToNextAnim;
	private float elapsedTime;

	public int totalNumAnim;
	private int currentAnim;

	public SpriteMeshInstance[] bodyParts;

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

	public IEnumerator FadeOut(float time)
	{
		float thisElapsedTime = 0.0f;

		Color initialColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
		Color finalColor = new Color(1.0f, 1.0f, 1.0f, 0.0f);

		for(int i = 0; i<bodyParts.Length; i++)
		{
			bodyParts[i].color = initialColor;
		}

		while(thisElapsedTime < time)
		{
			thisElapsedTime += Time.deltaTime;
			for(int i = 0; i<bodyParts.Length; i++)
			{
				bodyParts[i].color = Color.Lerp(initialColor, finalColor, thisElapsedTime / time);
			}
			yield return null;
		}

		for(int i = 0; i<bodyParts.Length; i++)
		{
			bodyParts[i].color = finalColor;
		}
	}

	public IEnumerator FadeIn(float time)
	{
		float thisElapsedTime = 0.0f;

		Color initialColor = new Color(1.0f, 1.0f, 1.0f, 0.0f);
		Color finalColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);

		for(int i = 0; i<bodyParts.Length; i++)
		{
			bodyParts[i].color = initialColor;
		}

		while(thisElapsedTime < time)
		{
			thisElapsedTime += Time.deltaTime;
			for(int i = 0; i<bodyParts.Length; i++)
			{
				bodyParts[i].color = Color.Lerp(initialColor, finalColor, thisElapsedTime / time);
			}
			yield return null;
		}

		for(int i = 0; i<bodyParts.Length; i++)
		{
			bodyParts[i].color = finalColor;
		}
	}
}
