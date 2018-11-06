using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

using UnityEngine.UI;
using System; //para Int32
using System.Text;//para el parse de string a enteros


public class NewBehaviourScript : MonoBehaviour {

    private Rigidbody2D rb;
    public float speed;

    private float xInput;
    private float yInput;

    Vector2 futuraPosicion;
    private int indicePuerta;




    public Image black; // poner la imagen del canvas que haga de negro(o lo que sea)
    public Animator anim; //poner la animacion de esa imagen (lo mismo, la imagen esa)

    public CameraBehaviour cameraScript;


    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D>();

        
	}
	
	// Update is called once per frame
	void LateUpdate () {

        xInput = Input.GetAxis("Horizontal");
        yInput = Input.GetAxis("Vertical");


        rb.velocity = speed * new Vector2(xInput, yInput);//rb.velocity.y);
        
        /*if (Input.GetKeyDown(KeyCode.A))
        {
            rb.velocity = speed * Vector2.left;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            rb.velocity = speed * Vector2.right;
        }*/
        

    }/*
    private void OnTriggerEnter2D(Collider2D collision)
    {
        string nombrePuerta = collision.name;
        print(nombrePuerta);
        nombrePuerta = nombrePuerta.Substring(nombrePuerta.Length-1);
        

        if (Int32.TryParse(nombrePuerta, out indicePuerta))
        {
            print("IndicePuerta = " + indicePuerta);

        }
        else
        {
            Console.WriteLine("Las puertas tienen que tener un numero al final que indique a que indice del vector se tiene que ir para trasladar al jugador alli");
        }
       
        if (collision.CompareTag("Door"))
        {
            futuraPosicion = posicionesNiveles[indicePuerta];
            //esto solo entra una vez, se necesita salir y volver a entrar para que lo haga
            StartCoroutine(Fading());
        }
        else if (collision.CompareTag("LateralDoor"))
        {

            futuraPosicion = posicionesLateralesNiveles[indicePuerta];
            StartCoroutine(LateralChamberTransition());

            print("LateralColisionado");
        }
    }*/

    IEnumerator Fading()
    {
        anim.SetBool("Fade", true);
        yield return new WaitUntil(() => black.color.a == 1); //esperar a que sea totalmente opaco
        anim.SetBool("Fade", false);
        //rb.transform.Translate(futuraPosicion.x, futuraPosicion.y, 0); //esto suma no indica donde ponerlo
        rb.transform.SetPositionAndRotation(new Vector3(futuraPosicion.x,futuraPosicion.y, 0), new Quaternion());
        cameraScript.rightBounds = GameObject.Find("RightBoundary"+indicePuerta).transform;
        cameraScript.leftBounds = GameObject.Find("LeftBoundary" + indicePuerta).transform;
        cameraScript.upperBounds = GameObject.Find("UpperBoundary" + indicePuerta).transform;
        cameraScript.downerBounds = GameObject.Find("DownerBoundary" + indicePuerta).transform;



        //cameraScript.ReStard();

        anim.SetBool("ChamberLoaded", true);
        yield return new WaitUntil(() => black.color.a == 0); //esperar a que sea totalmente transparente
        anim.SetBool("ChamberLoaded", false);


        //aqui el cambio de estancia
    }

    IEnumerator LateralChamberTransition()
    {
        
        rb.transform.SetPositionAndRotation(new Vector3(futuraPosicion.x, futuraPosicion.y, 0), new Quaternion());
        cameraScript.rightBounds = GameObject.Find("RightBoundary" + indicePuerta).transform;
        cameraScript.leftBounds = GameObject.Find("LeftBoundary" + indicePuerta).transform;
        cameraScript.upperBounds = GameObject.Find("UpperBoundary" + indicePuerta).transform;
        cameraScript.downerBounds = GameObject.Find("DownerBoundary" + indicePuerta).transform;



        //cameraScript.ReStard();

        
        yield return new WaitForSeconds(1); 
        
    }
}
