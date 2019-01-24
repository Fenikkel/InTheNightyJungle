using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

	[SerializeField]
	private float maxVolume;

	[SerializeField]
	public AudioClip menuMusic;
	[SerializeField]
	public AudioClip[] CindyMusics;
	[SerializeField]
	public AudioClip[] BrendaMusics;
	[SerializeField]
	public AudioClip TestMusic;

	private static AudioManager instance;

	public static AudioManager Instance
	{
		get {
			if(instance == null) instance = GameObject.FindObjectOfType<AudioManager>();
			return instance;
		}
		
	}

	public AudioSource backgroundMusicCindy;
	public AudioSource backgroundMusicBrenda;

	private bool CindyMusicPlaying;

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
		ChangeGeneralVolume(float.Parse(SettingsManager.Instance.Load("volume")));
		backgroundMusicCindy.volume = maxVolume;

		backgroundMusicCindy.clip = menuMusic;
		backgroundMusicCindy.Play();
	}

	public void TurnOnBackgroundMusic(bool cindy)
	{
		CindyMusicPlaying = cindy;

		backgroundMusicCindy.volume = 0.0f;
		backgroundMusicBrenda.volume = 0.0f;
		backgroundMusicCindy.clip = CindyMusics[0];
		backgroundMusicCindy.Play();
		backgroundMusicBrenda.Play();

		/* if(CindyMusicPlaying)
		{
			StartCoroutine(ChangeVolume(backgroundMusicCindy, 1.0f, time));
		}
		else
		{
			StartCoroutine(ChangeVolume(backgroundMusicBrenda, 1.0f, time));
		}*/
	}

	public void ChangeBackgroundMusic(float time)
	{
		if(CindyMusicPlaying)
		{
			StartCoroutine(ChangeVolume(backgroundMusicCindy, 0.0f, time));
			StartCoroutine(ChangeVolume(backgroundMusicBrenda, maxVolume, time));
			CindyMusicPlaying = false;
		}
		else
		{
			StartCoroutine(ChangeVolume(backgroundMusicCindy, maxVolume, time));
			StartCoroutine(ChangeVolume(backgroundMusicBrenda, 0.0f, time));
			CindyMusicPlaying = true;
		}
	}

	private IEnumerator ChangeVolume(AudioSource AS, float finalValue, float time)
	{
		float elapsedTime = 0.0f;
		float initialValue = AS.volume;
		while(elapsedTime < time)
		{
			elapsedTime += Time.deltaTime;
			AS.volume = Mathf.Lerp(initialValue, finalValue, elapsedTime/time);
			yield return null;
		}
		AS.volume = finalValue;
	}

	public void ChangeGeneralVolume(float value)
	{
		AudioListener.volume = value;
	}

	public float GetVolume()
	{
		return AudioListener.volume;
	}
}
