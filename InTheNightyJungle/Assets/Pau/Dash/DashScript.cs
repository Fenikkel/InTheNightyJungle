using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DashScript : MonoBehaviour {
    //CON "A" SE HACE EL DASH
    private Rigidbody2D rb;
    public float dashSpeed; //velocidad del dash
    private float dashTime; //de dashtime se va descontando hasta llegar a 0 que es el fin del dash
    public float startDashTime; //longitud del dash
    private int direction;
    private int facing = 2;
    private bool invulnerable; //Variable que decidira si lo de dentro de onTriggerStay del enemigo se hara o no (Supongo que tendremos que poner lo de que este calculando en cada momento que este tocando para cuando termine la invulnerabilidad)

    public GameObject dashEffect;

    private float actualGravity;


	void Start () {
        invulnerable = false;
        rb = GetComponent<Rigidbody2D>();
        dashTime = startDashTime;
        actualGravity = rb.gravityScale;
    }
	
	
	void Update () {

        if (Input.GetKeyDown(KeyCode.A))
        {
            direction = facing;
            CameraShakeScript.ShakeCamera();
            invulnerable = true;
            print(invulnerable);
            Instantiate(dashEffect, transform.position, Quaternion.identity);
            rb.gravityScale = 0;
            print(rb.gravityScale);
        }

        if(direction == 0 )
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                facing = 1;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                facing = 2;
            }//SI QUEREMOS DASH EN EJE Y
            /*else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                facing = 3;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                facing = 4;
            }*/
        }
        else
        {
            if(dashTime <= 0) //cuando termina el dash, se pone disponible este
            {
                direction = 0;
                dashTime = startDashTime;
                rb.velocity = Vector2.zero;
                invulnerable = false;
                print(invulnerable);
                rb.gravityScale = actualGravity;
                print(rb.gravityScale);
            }
            else
            {
                dashTime -= Time.deltaTime;

                if (direction == 1)
                {
                    rb.velocity = Vector2.left * dashSpeed;//ese vector dos parece que es el del personaje
                }
                else if (direction == 2 )
                {
                    rb.velocity = Vector2.right * dashSpeed;
                }
                //SI QUEREMOS DASH EN EJE Y
                /*else if (direction == 3)
                {
                    rb.velocity = Vector2.up * dashSpeed;
                }
                else if (direction == 4)
                {
                    rb.velocity = Vector2.down * dashSpeed;
                }*/
            }
        }

	}
}
