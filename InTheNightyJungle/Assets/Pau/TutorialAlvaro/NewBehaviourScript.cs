using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour {

    private Rigidbody2D rb;
    public float speed;

    private float xInput;
    private float yInput;


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
}
