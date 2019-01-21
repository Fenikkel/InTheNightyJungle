﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PantallaPausa : MonoBehaviour {
    
    public static bool GameIsPaused = false;

    public static PantallaPausa Instance;

    public Button botonJugar, botonOpciones, botonSalir, botonInventario, botonAtras, modifyBrightnessButton;
    public Slider volumen;
    public Image inventarioCindy, inventarioBrenda;

    public GeneralUIController UIController;

    public GameObject ContenidoInventarioCindy;
    public GameObject ContenidoInventarioBrenda;

    public GameObject modifyBrightnessScreen;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        volumen.value = AudioManager.Instance.GetComponent<AudioSource>().volume;

        Atras();
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
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
    public void Inventario()
    {
        botonJugar.gameObject.SetActive(false);
        botonOpciones.gameObject.SetActive(false);
        botonSalir.gameObject.SetActive(false);
        botonInventario.gameObject.SetActive(false);
        inventarioCindy.gameObject.SetActive(GameManager.Instance.IsCindyPlaying());
        inventarioBrenda.gameObject.SetActive(!GameManager.Instance.IsCindyPlaying());
        botonAtras.gameObject.SetActive(true);
        modifyBrightnessButton.gameObject.SetActive(false);
    }
    public void Opciones()
    {
        botonJugar.gameObject.SetActive(false);
        botonOpciones.gameObject.SetActive(false);
        botonSalir.gameObject.SetActive(false);
        botonInventario.gameObject.SetActive(false);
        volumen.gameObject.SetActive(true);
        botonAtras.gameObject.SetActive(true);
        modifyBrightnessButton.gameObject.SetActive(true);
    }
    public void Atras()
    {
        
        botonJugar.gameObject.SetActive(true);
        botonOpciones.gameObject.SetActive(true);
        botonSalir.gameObject.SetActive(true);
        botonInventario.gameObject.SetActive(true);
        botonAtras.gameObject.SetActive(false);
        volumen.gameObject.SetActive(false);
        inventarioCindy.gameObject.SetActive(false);
        inventarioBrenda.gameObject.SetActive(false);
        modifyBrightnessButton.gameObject.SetActive(false);
        
        SettingsManager.Instance.Save("volume", AudioManager.Instance.GetComponent<AudioSource>().volume.ToString());
    }

    public void ModifyBrightness()
    {
        modifyBrightnessScreen.SetActive(true);
    }

    public void TurnUpVolume()
    {
        AudioManager.Instance.GetComponent<AudioSource>().volume = volumen.value;
    }
}
