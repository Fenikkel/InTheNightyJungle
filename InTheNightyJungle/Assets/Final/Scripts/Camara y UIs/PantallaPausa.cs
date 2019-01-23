using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PantallaPausa : MonoBehaviour {
    
    public static bool GameIsPaused = false;

    public Button botonJugar, botonOpciones, botonSalir, botonInventario, botonAtras, modifyBrightnessButton;
    public Slider volumen;
    public Image inventarioCindy, inventarioBrenda;

    public GeneralUIController UIController;

    public GameObject ContenidoInventarioCindy;
    public GameObject ContenidoInventarioBrenda;

    public GameObject modifyBrightnessScreen;

    [SerializeField]
    private AudioSource clickSound;

    void Awake()
    {

    }

    void Start()
    {
        volumen.value = AudioManager.Instance.GetVolume();

        InitialScreen();
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

    private void InitialScreen()
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

        GeneralUIController.Instance.GetComponent<InventoryUI>().UnshowInventory();
    }

    public void Resume()
    {
        UIController.ResumeGame();
        Time.timeScale = 1f;
        GameIsPaused = false;
        InitialScreen();

        clickSound.Play();
    }
    public void Pause()
    {
        UIController.PauseGame();
        Time.timeScale = 0f;
        GameIsPaused = true;

        //clickSound.Play();
    }
    public void LoadMenu()
    {
        clickSound.Play();

        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
    public void Inventario()
    {
        botonJugar.gameObject.SetActive(false);
        botonOpciones.gameObject.SetActive(false);
        botonSalir.gameObject.SetActive(false);
        botonInventario.gameObject.SetActive(false);
        botonAtras.gameObject.SetActive(true);
        modifyBrightnessButton.gameObject.SetActive(false);

        clickSound.Play();

        GeneralUIController.Instance.GetComponent<InventoryUI>().ShowInventory(GameManager.Instance.IsCindyPlaying());
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

        clickSound.Play();
    }
    public void Atras()
    {
        InitialScreen();
        
        SettingsManager.Instance.Save("volume", AudioManager.Instance.GetVolume().ToString());

        clickSound.Play();
    }

    public void ModifyBrightness()
    {
        modifyBrightnessScreen.SetActive(true);

        clickSound.Play();
    }

    public void TurnUpVolume()
    {
        AudioManager.Instance.ChangeGeneralVolume(volumen.value);
    }
}
