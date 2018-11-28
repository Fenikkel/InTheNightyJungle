using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralUIController : MonoBehaviour {

	public GameObject stats;
	public GameObject conversation;
	public GameObject dancingTest;
	public GameObject drinkingTest;
	public GameObject blackScreen;
	public GameObject pauseMenu;

	private UILayer currentLayer;

	// Use this for initialization
	void Start () {
		ChangeMode(UILayer.Stats);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ChangeMode(UILayer code)
	{
		stats.SetActive(code == UILayer.Stats);
		conversation.SetActive(code == UILayer.Conversation);
		dancingTest.SetActive(code == UILayer.DancingTest);
		drinkingTest.SetActive(code == UILayer.DrinkingTest);

		if(code != UILayer.Empty) currentLayer = code;
	}

	public void PauseGame()
	{
		ChangeMode(UILayer.Empty);
		blackScreen.SetActive(false);
		pauseMenu.SetActive(true);
	}

	public void ResumeGame()
	{
		pauseMenu.SetActive(false);
		blackScreen.SetActive(true);
		ChangeMode(currentLayer);
	}
}

public enum UILayer
{
	Stats, Conversation, DancingTest, DrinkingTest, Empty
}
