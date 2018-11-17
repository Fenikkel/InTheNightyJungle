using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoriaDerrota : MonoBehaviour {

    //Sistemas de particulas ordenados de izquierda a derecha
    public ParticleSystem system1;
    public ParticleSystem system2;
    public ParticleSystem system3;
    public ParticleSystem system4;

    private float cameraSize;

    private Animator anim;
    public Camera camara;
    // Use this for initialization
    void Start () {
        Victory(0);
        //cameraSize=GameObject.Find("MainCamera").GetComponent<CameraBehaviour>().initialSize;//Obtiene el tamaño al que la cámara debe volver
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void Victory(int orientacion)// Deberá recibir la orientación de la prueba para saber que particulas activar
    {
        //Esconder UI


        //Reencuadre de la camara


        //Animaciones
        StartCoroutine(WaitForConfeti(0.5f, orientacion));

        
        //Fijacion de la camara al personaje


        //Salida del personaje de la prueba

    }

    IEnumerator WaitForConfeti(float seconds, int orientacion)
    {

        yield return new WaitForSeconds(seconds);
        if (orientacion == 0)//Si Cindy esta a la izquierda
        {
            system1.Play();
            StartCoroutine(WaitForConfeti2(Random.Range(0.5f,1.0f), orientacion));
        }
        else//Si Cindy esta a la derecha
        {
            system4.Play();
            StartCoroutine(WaitForConfeti2(Random.Range(0.5f, 1.0f), orientacion));
        }
    }

    IEnumerator WaitForConfeti2(float seconds, int orientacion)
    {
        yield return new WaitForSeconds(seconds);
        if (orientacion == 0) system2.Play();
        else system3.Play();
    }

    void Defeat()
    {

    }
}
