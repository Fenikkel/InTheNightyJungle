using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PantallaPausa : MonoBehaviour {

    public static bool GameIsPaused = false;
    //Cosas creadas por mi: Maria.
    public static PantallaPausa Instance;
    public Button botonJugar, botonOpciones, botonSalir, botonInventario, botonAtras;
    public Slider volumen;
    public Image inventario;
    //Cosas creadas por mi: Maria.
    public GameObject ContenidoInventario;

    public GeneralUIController UIController;

    private float currentVolume;

    private AudioSource music;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        volumen.gameObject.SetActive(false);
        botonAtras.gameObject.SetActive(false);

        music = GameObject.FindWithTag("music").GetComponent<AudioSource>();
        if(music) volumen.value = music.volume;
    }

    // Update is called once per frame
    public void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }else
            {
                Pause();
            }
        }
		
	}
    public void Resume()
    {
        UIController.ResumeGame();
        Time.timeScale = 1f;
        GameIsPaused = false;
    }
    public void Pause()
    {
        UIController.PauseGame();
        Time.timeScale = 0f;
        GameIsPaused = true;
    }
    public void LoadMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void Inventario()
    {
        botonJugar.gameObject.SetActive(false);
        botonOpciones.gameObject.SetActive(false);
        botonSalir.gameObject.SetActive(false);
        botonInventario.gameObject.SetActive(false);
        inventario.gameObject.SetActive(true);
        botonAtras.gameObject.SetActive(true);
    }
    public void Opciones()
    {
        botonJugar.gameObject.SetActive(false);
        botonOpciones.gameObject.SetActive(false);
        botonSalir.gameObject.SetActive(false);
        botonInventario.gameObject.SetActive(false);
        volumen.gameObject.SetActive(true);
        botonAtras.gameObject.SetActive(true);
    }
    public void Atras()
    {
        
        botonJugar.gameObject.SetActive(true);
        botonOpciones.gameObject.SetActive(true);
        botonSalir.gameObject.SetActive(true);
        botonInventario.gameObject.SetActive(true);
        botonAtras.gameObject.SetActive(false);
        volumen.gameObject.SetActive(false);
        inventario.gameObject.SetActive(false);

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
            if(music) music.volume = currentVolume;
        }
    }
}
