﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrinkingTestUIController : MonoBehaviour {
    
	private GeneralUIController UIController;
	public RectTransform canvas;
	public Image[] introductionTexts;
	public Image finalText;

	public Text leftNumDrinksText;
	private int leftNumDrinks;
	public Text rightNumDrinksText;
	private int rightNumDrinks;

	public Image leftDrinkingBar;
	public Image rightDrinkingBar;

	public float increaseDrinkingValue;
	
	// Use this for initialization
	void Start () {
		UIController = GetComponent<GeneralUIController>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public IEnumerator ShowIntroductionTexts(float totalTime)
	{
		StartCoroutine(MoveToCenter(introductionTexts[0], true, totalTime/6));
		yield return new WaitForSeconds(totalTime/6);

		for(int i = 0; i < introductionTexts.Length; i++)
		{
			StartCoroutine(ScaleAndFade(introductionTexts[i], 1.5f, totalTime/6));
			yield return new WaitForSeconds(totalTime/6);
		}
	}

    public IEnumerator ShowFinalText(float totalTime)
    {
        StartCoroutine(MoveToCenter(finalText, false, totalTime/3));
        yield return new WaitForSeconds(2 * totalTime/3);

        StartCoroutine(ScaleAndFade(finalText, 1.5f, totalTime/3));
        yield return new WaitForSeconds(totalTime/3);
    }

	private IEnumerator MoveToCenter(Image text, bool fromRight, float time)
	{
		int direction = (fromRight) ? 1 : -1;
		text.GetComponent<RectTransform>().localPosition = new Vector2(direction * (canvas.rect.width + text.GetComponent<RectTransform>().rect.width) / 2, 0);
		text.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
		Vector2 initialPosition = text.GetComponent<RectTransform>().localPosition;
		Vector2 finalPosition = new Vector2(0,0);

		float elapsedTime = 0.0f;
		while(elapsedTime < time)
		{
			elapsedTime += Time.deltaTime;
			text.GetComponent<RectTransform>().localPosition = Vector2.Lerp(initialPosition, finalPosition, elapsedTime/time);
			yield return null;
		}
		text.GetComponent<RectTransform>().localPosition = finalPosition;
	}

	private IEnumerator ScaleAndFade(Image text, float increaseScale, float time)
	{
		Color initialColor = text.color;
		Color finalColor = new Color(1.0f, 1.0f, 1.0f, 0.0f);

		Vector2 initialScale = text.GetComponent<RectTransform>().sizeDelta;
		Vector2 finalScale = text.GetComponent<RectTransform>().sizeDelta * increaseScale;

		float elapsedTime = 0.0f;

		if(text.color.a == 1.0f)
		{
			while(elapsedTime < time)
			{
				elapsedTime += Time.deltaTime;
				text.GetComponent<RectTransform>().sizeDelta = Vector2.Lerp(initialScale, finalScale, elapsedTime / time);
				text.color = Color.Lerp(initialColor, finalColor, elapsedTime / time);
				yield return null;
			}
			text.GetComponent<RectTransform>().sizeDelta = finalScale;
			text.color = finalColor;
            text.GetComponent<RectTransform>().sizeDelta = initialScale;
		}
		else
		{
			StartCoroutine(FadeInTwoParts(text, initialColor, new Color(1.0f, 1.0f, 1.0f, 1.0f), true, time/2));
			while(elapsedTime < time)
			{
				elapsedTime += Time.deltaTime;
				text.GetComponent<RectTransform>().sizeDelta = Vector2.Lerp(initialScale, finalScale, elapsedTime / time);
				yield return null;
			}
			text.GetComponent<RectTransform>().sizeDelta = finalScale;
            text.GetComponent<RectTransform>().sizeDelta = initialScale;
		}
	}

	private IEnumerator FadeInTwoParts(Image text, Color initialColor, Color finalColor, bool firstTime, float time)
	{
		float elapsedTime = 0.0f;

		while(elapsedTime < time)
		{
			elapsedTime += Time.deltaTime;
			text.color = Color.Lerp(initialColor, finalColor, elapsedTime / time);
			yield return null;
		}
		text.color = finalColor;

		if(firstTime) StartCoroutine(FadeInTwoParts(text, finalColor, initialColor, false, time));
	}

	public void InitializeUI()
	{
		UIController.ChangeMode(UILayer.DrinkingTest);
		leftNumDrinks = rightNumDrinks = 0;
		leftNumDrinksText.text = rightNumDrinksText.text = "0";
		leftDrinkingBar.fillAmount = rightDrinkingBar.fillAmount = 0.0f;

		for(int i = 0; i < introductionTexts.Length; i++)
        {
			introductionTexts[i].color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        }
        finalText.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
	}

	public bool IncreaseDrinkingBar(bool leftSide)
	{
		if(leftSide)
		{
			leftDrinkingBar.fillAmount += increaseDrinkingValue;
			if(leftDrinkingBar.fillAmount >= 1)
			{
				StartCoroutine(ChangeDrinkingBarValueInTime(leftSide, 0.0f, 1.0f));
				return true;
			}
			return false;
		}
		else
		{
			rightDrinkingBar.fillAmount += increaseDrinkingValue;
			if(rightDrinkingBar.fillAmount >= 1)
			{
				StartCoroutine(ChangeDrinkingBarValueInTime(leftSide, 0.0f, 1.0f));
				return true;
			}
			return false;
		}
	}

	public float GetDrinkingValue(bool leftSide)
	{
		if(leftSide)
			return leftDrinkingBar.fillAmount;
		else
			return rightDrinkingBar.fillAmount;
	}

	public void IncreaseNumDrinks(bool leftSide)
	{
		if(leftSide)
		{
			leftNumDrinks++;
			leftNumDrinksText.text = leftNumDrinks.ToString();
		}
		else
		{
			rightNumDrinks++;
			rightNumDrinksText.text = rightNumDrinks.ToString();
		}
	}

	private IEnumerator ChangeDrinkingBarValueInTime(bool leftSide, float finalValue, float time)
	{
		Image drinkingBar = leftSide ? leftDrinkingBar : rightDrinkingBar;
		float elapsedTime = 0.0f;
		float initialValue = drinkingBar.fillAmount;

		while(elapsedTime < time)
		{
			elapsedTime += Time.deltaTime;
			drinkingBar.fillAmount = Mathf.Lerp(initialValue, finalValue, elapsedTime / time);
			yield return null;
		}
		drinkingBar.fillAmount = finalValue;
		if(finalValue == 0) IncreaseNumDrinks(leftSide);
	}
}
