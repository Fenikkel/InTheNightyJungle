using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModifyBrightness : MonoBehaviour {

	public GameObject modifyBrightnessScreen;

	public Slider brightnessBar;
	public Image overIcons;
	public Image[] shadowSprites;
	public int minValue;
	public int maxValue;

	private float brightnessValue;

	[SerializeField]
    private AudioSource clickSound;

	// Use this for initialization
	void Start () {
		brightnessValue = float.Parse(SettingsManager.Instance.Load("brightness"));

		overIcons.color = new Color(overIcons.color.r, overIcons.color.g, overIcons.color.b, brightnessValue);
		brightnessBar.value = (255 * brightnessValue - minValue)/(maxValue - minValue);
		for(int i = 0; i < shadowSprites.Length; i++)
		{
			shadowSprites[i].color = new Color(shadowSprites[i].color.r, shadowSprites[i].color.g, shadowSprites[i].color.b, brightnessValue);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ChangeBrightnessValue()
	{
		brightnessValue = (minValue + (maxValue - minValue) * brightnessBar.value)/255;
		overIcons.color = new Color(overIcons.color.r, overIcons.color.g, overIcons.color.b, brightnessValue);
		for(int i = 0; i < shadowSprites.Length; i++)
		{
			shadowSprites[i].color = new Color(shadowSprites[i].color.r, shadowSprites[i].color.g, shadowSprites[i].color.b, brightnessValue);
		}
	}

	public void GetBackToOptions()
	{
		SettingsManager.Instance.Save("brightness", brightnessValue.ToString());
		modifyBrightnessScreen.SetActive(false);

		clickSound.Play();
	}
}
