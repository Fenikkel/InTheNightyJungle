using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoriaDerrota : MonoBehaviour {

    //Sistemas de particulas ordenados de izquierda a derecha
    public ParticleSystem system1;
    public ParticleSystem system2;
    public ParticleSystem system3;
    public ParticleSystem system4;

    private float cameraRestoreSize;
    private float zoomTimePassed;
    private float finalZoom;
    private Vector3 finalPosition;
    private bool finPrueba=true;
    private Vector3 initialPosition;

    private Animator animCindy;
    private Animator animRetador;
    public Canvas UI;
    // Use this for initialization
    void Start () {
        cameraRestoreSize=Camera.main.GetComponent<CameraBehaviour>().initialSize;//Obtiene el tamaño al que la cámara debe volver
        initialPosition = Camera.main.transform.position;
        animCindy = GameObject.Find("Cindy").GetComponent<Animator>();
        animRetador = GameObject.Find("FrancotiradorHielo").GetComponent<Animator>();//En el caso general se buscaría al retador correcto
        Camera.main.GetComponent<CameraBehaviour>().enabled = false;
        GameObject.Find("Cindy").GetComponent<PlayerPlatformController>().enabled = false;
        Defeat(0);
    }
	
	// Update is called once per frame
	void Update () {
        if (finPrueba == true)//Cuando finalice la prueba hará un zoomOut para encuadrar toda la prueba
        {
            zoomTimePassed += Time.deltaTime;

            float t = zoomTimePassed / 2;

            Camera.main.orthographicSize = Mathf.SmoothStep(cameraRestoreSize, 3, t);
            Camera.main.transform.position = Vector3.Lerp(initialPosition, new Vector3(GetComponent<Transform>().position.x,
                                                                                        GetComponent<Transform>().position.y,
                                                                                        Camera.main.transform.position.z), t);//Mueve la camara al centro de la prueba
            finalZoom = Camera.main.orthographicSize;
            finalPosition = Camera.main.transform.position;
        }
        if (finPrueba == false)//Cuando la prueba ya haya acabado y el personaje deba salir la camara volverá a su medida original
        {
            zoomTimePassed += Time.deltaTime;

            float t = zoomTimePassed / 12;

            Camera.main.orthographicSize = Mathf.SmoothStep(finalZoom, cameraRestoreSize, t);
            Camera.main.transform.position = Vector3.Lerp(finalPosition, new Vector3(GameObject.Find("Cindy").GetComponent<Transform>().position.x,
                                                                                         GameObject.Find("Cindy").GetComponent<Transform>().position.y,
                                                                                         Camera.main.transform.position.z), t);//La camara se centra en Cindy
        }
    }

    void Victory(int orientacion)// Deberá recibir la orientación de la prueba para saber que particulas activar
    {     
        UI.transform.Find("DancingTestUI").gameObject.SetActive(false); //Esconder UI
        
        //Encuadre de la camara a toda la prueba y Animaciones
        finPrueba = true;
        StartCoroutine(WaitZoomOutVictory(orientacion)); 
    }

    IEnumerator WaitZoomOutVictory(int orientacion)
    {
        yield return new WaitForSeconds(2);//Aqui debera esperar tanto como cueste de hacer el zoomOut
        animCindy.SetTrigger("Victoria");//Animacion de victoria
        animRetador.Play("Derrota");//Animacion de derrota del retador
        StartCoroutine(WaitForConfetiVictory(0.5f, orientacion));
    }

    IEnumerator WaitZoomOutDefeat(int orientacion)
    {
        yield return new WaitForSeconds(2);//Aqui debera esperar tanto como cueste de hacer el zoomOut
        animCindy.SetTrigger("Derrota");//Animacion de derrota
        animRetador.Play("Victoria");//Animacion de victoria del retador
        StartCoroutine(WaitForConfetiDefeat(0.5f, orientacion));
    }

    IEnumerator WaitForConfetiVictory(float seconds, int orientacion)
    {

        yield return new WaitForSeconds(seconds);
        if (orientacion == 0)//Si Cindy esta a la izquierda
        {
            system1.Play();
            StartCoroutine(WaitForConfeti2Victory(Random.Range(0.5f,1.0f), orientacion));
        }
        else//Si Cindy esta a la derecha
        {
            system4.Play();
            StartCoroutine(WaitForConfeti2Victory(Random.Range(0.5f, 1.0f), orientacion));
        }
    }

    IEnumerator WaitForConfeti2Victory(float seconds, int orientacion)
    {
        yield return new WaitForSeconds(seconds);
        if (orientacion == 0) system2.Play();
        else system3.Play();
        StartCoroutine(Exit());
    }

    IEnumerator WaitForConfetiDefeat(float seconds, int orientacion)
    {

        yield return new WaitForSeconds(seconds);
        if (orientacion == 0)//Si Cindy esta a la izquierda
        {
            system3.Play();
            StartCoroutine(WaitForConfeti2Defeat(Random.Range(0.5f, 1.0f), orientacion));
        }
        else//Si Cindy esta a la derecha
        {
            system1.Play();
            StartCoroutine(WaitForConfeti2Defeat(Random.Range(0.5f, 1.0f), orientacion));
        }
    }

    IEnumerator WaitForConfeti2Defeat(float seconds, int orientacion)
    {
        yield return new WaitForSeconds(seconds);
        if (orientacion == 0) system4.Play();
        else system2.Play();
        StartCoroutine(Exit());
    }

    IEnumerator Exit()
    {
        yield return new WaitForSeconds(2);
        finPrueba = false;
        Camera.main.GetComponent<CameraBehaviour>().enabled = true;//Esto debería realizarse al salir completamente de la prueba
        GameObject.Find("Cindy").GetComponent<PlayerPlatformController>().enabled = true;//Esto debería realizarse al salir completamente de la prueba
    }

    void Defeat(int orientacion)
    {
        UI.transform.Find("DancingTestUI").gameObject.SetActive(false); //Esconder UI
        
        //Encuadre de la camara a toda la prueba y Animaciones
        finPrueba = true;
        StartCoroutine(WaitZoomOutDefeat(orientacion));
    }
}
