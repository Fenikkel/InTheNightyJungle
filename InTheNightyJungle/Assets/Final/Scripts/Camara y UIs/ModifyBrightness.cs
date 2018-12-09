using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModifyBrightness : MonoBehaviour {

	public GameObject modifyBrightnessScreen;

	public Slider brightnessBar;
	public Image overIcons;
	public SpriteRenderer[] shadowSprites;
	public int minValue;
	public int maxValue;

	// Use this for initialization
	void Start () {
		brightnessBar.value = (255 * overIcons.color.a - minValue)/(maxValue - minValue);
		for(int i = 0; i < shadowSprites.Length; i++)
		{
			shadowSprites[i].color = new Color(shadowSprites[i].color.r, shadowSprites[i].color.g, shadowSprites[i].color.b, overIcons.color.a);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ChangeBrightnessValue()
	{
		float finalValue = (minValue + (maxValue - minValue) * brightnessBar.value)/255;
		overIcons.color = new Color(overIcons.color.r, overIcons.color.g, overIcons.color.b, finalValue);
		for(int i = 0; i < shadowSprites.Length; i++)
		{
			shadowSprites[i].color = new Color(shadowSprites[i].color.r, shadowSprites[i].color.g, shadowSprites[i].color.b, finalValue);
		}
	}

	public void GetBackToOptions()
	{
		modifyBrightnessScreen.SetActive(false);
	}
}
