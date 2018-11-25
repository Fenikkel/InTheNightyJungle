using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralUIController : MonoBehaviour {

	public GameObject stats;
	public GameObject conversation;
	public GameObject dancingTest;
	public GameObject drinkingTest;
	public GameObject blackScreen;

	// Use this for initialization
	void Start () {
		
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
	}
}

public enum UILayer
{
	Stats, Conversation, DancingTest, DrinkingTest, Empty
}
