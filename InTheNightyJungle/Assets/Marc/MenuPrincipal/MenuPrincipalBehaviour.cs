using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuPrincipalBehaviour : MonoBehaviour {

    public Button botonJugar, botonOpciones, botonSalir;

    private float initialAlpha;
    private bool empezar = false;
    private float tiempo;
	// Use this for initialization
	void Start () {
        botonJugar.onClick.AddListener(EmpezarJuego);
        botonOpciones.onClick.AddListener(Opciones);
        //botonSalir.onClick.AddListener(Salir);
        initialAlpha = this.GetComponent<CanvasGroup>().alpha;
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

    void EmpezarJuego()
    {
        empezar = true;
        StartCoroutine(ChangeScene());
        
    }

    void Opciones()
    {
        this.transform.GetChild(2).gameObject.SetActive(false);
        this.transform.GetChild(3).gameObject.SetActive(false);
        this.transform.GetChild(1).gameObject.SetActive(true);
    }

    IEnumerator ChangeScene()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(2);//Aqui cambiara a la escena principal tras hacer el FadeOut
    }
}
