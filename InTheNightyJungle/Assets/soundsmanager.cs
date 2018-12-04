using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundsmanager : MonoBehaviour {

    public static AudioClip alientoSound, coinSound, dañoSound, esquivarSound, seleccionarSound;
    static AudioSource audioSrc;
	// Use this for initialization
	void Start () {
        alientoSound = Resources.Load<AudioClip>("Aliento");

        audioSrc = GetComponent<AudioSource>();

    }
	
	// Update is called once per frame
	void Update () {

	}

    public static void PlaySound(string clip)
    {
        switch (clip){
        case "Aliento":
                audioSrc.PlayOneShot(alientoSound);
                break;
        }
    }
}
