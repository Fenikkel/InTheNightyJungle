using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashScript : MonoBehaviour {

    private Rigidbody2D rb;
    public float dashSpeed; //velocidad del dash
    private float dashTime; //de dashtime se va descontando hasta llegar a 0 que es el fin del dash
    public float startDashTime; //longitud del dash
    private int direction;
    private int facing = 2;
    public Animator camAnim;

    public GameObject dashEffect;


	void Start () {
        rb = GetComponent<Rigidbody2D>();
        dashTime = startDashTime;
	}
	
	
	void Update () {

        //si haciendo el dash no queremos que le afecte la gravedad solo tenemos que poner a 0 la Gravity Scale en el rigid body

        if (Input.GetKeyDown(KeyCode.A))
        {
            direction = facing;
            Instantiate(dashEffect, transform.position, Quaternion.identity);
        }

        if(direction == 0 )//&& Input.GetKeyDown(KeyCode.A)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                facing = 1;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                facing = 2;
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                facing = 3;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                facing = 4;
            }
        }
        else
        {
            if(dashTime <= 0) //cuando termina el dash, se pone disponible este
            {
                direction = 0;
                dashTime = startDashTime;
                rb.velocity = Vector2.zero;
            }
            else
            {
                dashTime -= Time.deltaTime;
                camAnim.SetTrigger("ShakeCamera");
                
                if(direction == 1)
                {
                    rb.velocity = Vector2.left * dashSpeed;//ese vector dos parece que es el del personaje
                }
                else if (direction == 2 )
                {
                    rb.velocity = Vector2.right * dashSpeed;
                }
                else if (direction == 3)
                {
                    rb.velocity = Vector2.up * dashSpeed;
                }
                else if (direction == 4)
                {
                    rb.velocity = Vector2.down * dashSpeed;
                }
            }
        }

	}
}
