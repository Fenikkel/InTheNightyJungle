using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuPrincipalBehaviour : MonoBehaviour {

    public Button botonJugar, botonOpciones, botonSalir, botonMenu, ajustarBrillo;
    public Slider volumen;
    public GameObject pantallaBrillo;

    private float currentVolume;

    private AudioSource music;

    void Awake()
    {
        music = GameObject.FindWithTag("music").GetComponent<AudioSource>();
        volumen.value = music.volume;
    }

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

	}

    public void EmpezarJuego()
    {
        StartCoroutine(ChangeScene(1.5f));
        
    }

    public void Opciones()
    {
        botonJugar.gameObject.SetActive(false);
        botonOpciones.gameObject.SetActive(false);
        botonSalir.gameObject.SetActive(false);
        botonMenu.gameObject.SetActive(true);
        volumen.gameObject.SetActive(true);
        ajustarBrillo.gameObject.SetActive(true);
    }

    public void Salir()
    {
        Application.Quit();
    }

    public void Atras()
    {
        botonJugar.gameObject.SetActive(true);
        botonOpciones.gameObject.SetActive(true);
        botonSalir.gameObject.SetActive(true);
        botonMenu.gameObject.SetActive(false);
        volumen.gameObject.SetActive(false);
        ajustarBrillo.gameObject.SetActive(false);
    }

    public void AjustarBrillo()
    {
        pantallaBrillo.gameObject.SetActive(true);
    }

    IEnumerator ChangeScene(float time)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(1);
        asyncLoad.allowSceneActivation = false;

        float initialAlpha = this.GetComponent<CanvasGroup>().alpha;
        float finalAlpha = 0f;
        float elapsedTime = 0;

        while (!asyncLoad.isDone)
        {
            if(elapsedTime < time)
            {
                elapsedTime += Time.deltaTime;
                this.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(initialAlpha, finalAlpha, elapsedTime / time);
            }
            else 
            {
                asyncLoad.allowSceneActivation = true;
            }
            yield return null;
        }
    }

    public void TurnUpVolume()
    {
        music.volume = volumen.value;
    }
}
