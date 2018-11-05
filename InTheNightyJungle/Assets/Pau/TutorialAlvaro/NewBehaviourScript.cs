using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour {

    private Rigidbody2D rb;
    public float speed;

    private float xInput;
    private float yInput;

    public Image black; // poner la imagen del canvas que haga de negro(o lo que sea)
    public Animator anim; //poner la animacion de esa imagen (lo mismo, la imagen esa)




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
        

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Door"))
        {
            print("Colisionado");
            StartCoroutine(Fading());
        }
    }

    IEnumerator Fading()
    {
        anim.SetBool("Fade", true);
        yield return new WaitUntil(() => black.color.a == 1); //esperar a que sea totalmente opaco
        anim.SetBool("Fade", false);

        anim.SetBool("ChamberLoaded", true);
        yield return new WaitUntil(() => black.color.a == 0);
        anim.SetBool("ChamberLoaded", false);


        //aqui el cambio de estancia
    }
}
