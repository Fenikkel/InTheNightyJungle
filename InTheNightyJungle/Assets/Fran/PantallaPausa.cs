using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PantallaPausa : MonoBehaviour {

    public static bool GameIsPaused = false;
    public Button botonJugar, botonOpciones, botonSalir, botonInventario, botonAtras;
    public Slider volumen;

    public GameObject pauseMenuUI;

    void Start()
    {
        botonJugar.onClick.AddListener(Resume);
        botonOpciones.onClick.AddListener(Opciones);
        botonSalir.onClick.AddListener(LoadMenu);
        botonAtras.onClick.AddListener(Atras);
        botonAtras.gameObject.SetActive(false);
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
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }
    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }
    public void LoadMenu()
    {
        SceneManager.LoadScene("Menu Principal");
    }
    void Opciones()
    {
        botonJugar.gameObject.SetActive(false);
        botonOpciones.gameObject.SetActive(false);
        botonSalir.gameObject.SetActive(false);
        botonInventario.gameObject.SetActive(false);
        volumen.gameObject.SetActive(true);
        botonAtras.gameObject.SetActive(true);
    }
    void Atras()
    {
        SceneManager.LoadScene("PantallaPausa");
    }
}
