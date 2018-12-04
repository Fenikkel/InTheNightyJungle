using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralUIController : MonoBehaviour {

	public GameObject cindyStats;
	public GameObject brendaStats;
	public GameObject conversation;
	public GameObject dancingTest;
	public GameObject drinkingTest;
	public GameObject blackScreen;
	public GameObject pauseMenu;

	private bool playingWithCindy;

	private UILayer currentLayer;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Initialize(bool cindy)
	{
		playingWithCindy = cindy;
		if(cindy)
		{	
			currentLayer = UILayer.CindyStats;
			ChangeMode(UILayer.CindyStats);
		}
		else
		{
			currentLayer = UILayer.BrendaStats;
			ChangeMode(UILayer.BrendaStats);
		}
	}

	public void ChangeMode(UILayer code)
	{
		cindyStats.SetActive(code == UILayer.CindyStats);
		brendaStats.SetActive(code == UILayer.BrendaStats);
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

	public void BackToStats()
	{
		if(playingWithCindy) ChangeMode(UILayer.CindyStats);
		else ChangeMode(UILayer.BrendaStats);
	}
}

public enum UILayer
{
	BrendaStats, CindyStats, Conversation, DancingTest, DrinkingTest, Empty
}
