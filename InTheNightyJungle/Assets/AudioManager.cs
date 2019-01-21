using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

	private static AudioManager instance;

	public static AudioManager Instance
	{
		get {
			if(instance == null) instance = GameObject.FindObjectOfType<AudioManager>();
			return instance;
		}
		
	}

	// Use this for initialization
	void Awake()
	{
		
	}

	// Update is called once per frame
	void Update () 
	{
		
	}

	private void Start()
	{
		GetComponent<AudioSource>().volume = float.Parse(SettingsManager.Instance.Load("volume"));
	}
}
