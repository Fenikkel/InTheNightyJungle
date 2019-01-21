using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : MonoBehaviour {

	private static SettingsManager instance;

	public static SettingsManager Instance
	{
		get {
			if(instance == null) instance = GameObject.FindObjectOfType<SettingsManager>();
			return instance;
		}
		
	}
	
	// Use this for initialization
	void Awake()
	{
		SettingsManager[] objs = GameObject.FindObjectsOfType<SettingsManager>();

		if (objs.Length > 1)
		{
			Destroy(this.gameObject);
		}

		DontDestroyOnLoad(this.gameObject);
	}

	private void Start()
	{

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Save(string key, string value)
	{
		PlayerPrefs.SetString(key, value);
	}

	public string Load(string key)
	{
		if(!PlayerPrefs.HasKey(key))
		{
			PlayerPrefs.SetString(key, "0.5");
			print("hola");
		}
			return PlayerPrefs.GetString(key);
	}
}
