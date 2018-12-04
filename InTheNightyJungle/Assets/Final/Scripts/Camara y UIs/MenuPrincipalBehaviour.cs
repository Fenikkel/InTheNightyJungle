using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuPrincipalBehaviour : MonoBehaviour {

    public Button botonJugar, botonOpciones, botonSalir, botonMenu;
    public Slider volumen;

    private float currentVolume;

    private AudioSource music;

    private float initialAlpha;
    private bool empezar = false;
    private float tiempo;
	// Use this for initialization
	void Start () {
        botonJugar.onClick.AddListener(EmpezarJuego);
        botonOpciones.onClick.AddListener(Opciones);
        botonSalir.onClick.AddListener(Salir);
        botonMenu.onClick.AddListener(Atras);
        initialAlpha = this.GetComponent<CanvasGroup>().alpha;
        
        music = GameObject.FindWithTag("music").GetComponent<AudioSource>();
        music.volume = volumen.value;
	}
	
	// Update is called once per frame
	void Update () {
        if (empezar == true)
        {
            tiempo += Time.deltaTime;

            float t = tiempo / 1.5f;

            this.GetComponent<CanvasGroup>().alpha=Mathf.Lerp(initialAlpha, 0.0f, t);
        }
	}

    public void EmpezarJuego()
    {
        soundsmanager.PlaySound("Seleccionar");
        empezar = true;
        StartCoroutine(ChangeScene());
        
    }

    void Opciones()
    {
        soundsmanager.PlaySound("Seleccionar");
        botonJugar.gameObject.SetActive(false);
        botonOpciones.gameObject.SetActive(false);
        botonSalir.gameObject.SetActive(false);
        botonMenu.gameObject.SetActive(true);
        volumen.gameObject.SetActive(true);
    }

    void Salir()
    {
        soundsmanager.PlaySound("Seleccionar");
        Application.Quit();
    }

    void Atras()
    {
        soundsmanager.PlaySound("Seleccionar");
        botonJugar.gameObject.SetActive(true);
        botonOpciones.gameObject.SetActive(true);
        botonSalir.gameObject.SetActive(true);
        botonMenu.gameObject.SetActive(false);
        volumen.gameObject.SetActive(false);
    }

    IEnumerator ChangeScene()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(1);//Aqui cambiara a la escena principal tras hacer el FadeOut
    }

    public void TurnUpVolume()
    {
        if(currentVolume > volumen.value)
        {
            volumen.value = currentVolume;
        }
        else
        {
            currentVolume = volumen.value;
            music.volume = currentVolume;
        }
    }
}
