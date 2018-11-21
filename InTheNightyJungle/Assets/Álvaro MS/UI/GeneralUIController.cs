using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralUIController : MonoBehaviour {

	public GameObject stats;
	public GameObject conversation;
	public GameObject dancingTest;
	public GameObject blackScreen;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ChangeMode(string code)
	{
		stats.SetActive(code[2] == '1');
		conversation.SetActive(code[1] == '1');
		dancingTest.SetActive(code[0] == '1');
	}
}
