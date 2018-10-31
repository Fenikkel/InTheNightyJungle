using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour {

    private Rigidbody2D rb;
    public float speed;

    private float xInput;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {

        xInput = Input.GetAxis("Horizontal");

        rb.velocity = speed * new Vector2(xInput, rb.velocity.y);
        
        /*if (Input.GetKeyDown(KeyCode.A))
        {
            rb.velocity = speed * Vector2.left;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            rb.velocity = speed * Vector2.right;
        }*/
        

    }
}
