using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundsmanager : MonoBehaviour {

    public static AudioClip alientoSound, coinSound, dañoSound, esquivarSound, seleccionarSound;
    static AudioSource audioSrc;
	// Use this for initialization
	void Start () {
        alientoSound = Resources.Load<AudioClip>("Aliento");
        esquivarSound = Resources.Load<AudioClip>("Esquivar");
        coinSound = Resources.Load<AudioClip>("Coin");
        dañoSound = Resources.Load<AudioClip>("Daño");
        seleccionarSound = Resources.Load<AudioClip>("Seleccionar");

        audioSrc = GetComponent<AudioSource>();

        DontDestroyOnLoad(this.gameObject);

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
        case "Esquivar":
                audioSrc.PlayOneShot(esquivarSound);
                break;
        case "Coin":
                audioSrc.PlayOneShot(coinSound);
                break;
        case "Daño":
                audioSrc.PlayOneShot(dañoSound);
                break;
        case "Seleccionar":
                audioSrc.PlayOneShot(seleccionarSound);
                break;
        }
    }
}
