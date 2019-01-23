using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuPrincipalBehaviour : MonoBehaviour {

    public Button botonJugar, botonOpciones, botonSalir, botonMenu, ajustarBrillo;
    public Slider volumen;
    public GameObject pantallaBrillo;

    [SerializeField]
    private AudioSource clickSound;

	// Use this for initialization
	void Start () {
        volumen.value = AudioManager.Instance.GetComponent<AudioSource>().volume;
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void EmpezarJuego()
    {
        StartCoroutine(ChangeScene(1.5f));
        clickSound.Play();
    }

    public void Opciones()
    {
        botonJugar.gameObject.SetActive(false);
        botonOpciones.gameObject.SetActive(false);
        botonSalir.gameObject.SetActive(false);
        botonMenu.gameObject.SetActive(true);
        volumen.gameObject.SetActive(true);
        ajustarBrillo.gameObject.SetActive(true);

        clickSound.Play();
    }

    public void Salir()
    {
        clickSound.Play();

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

        SettingsManager.Instance.Save("volume", AudioManager.Instance.GetComponent<AudioSource>().volume.ToString());

        clickSound.Play();
    }

    public void AjustarBrillo()
    {
        pantallaBrillo.SetActive(true);
        
        clickSound.Play();
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
        AudioManager.Instance.GetComponent<AudioSource>().volume = volumen.value;
    }
}
