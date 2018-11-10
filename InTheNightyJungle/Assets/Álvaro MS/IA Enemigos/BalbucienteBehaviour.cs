using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalbucienteBehaviour : PhysicsObject {

    private BoxCollider2D influenceZone;
    public Vector2 sizeInfluenceZone;
    public Transform initialPosition;
    public Transform finalPosition;

    private Animator anim;
    private float lastMove;
    public float maxSpeed;

    private bool following;
    private int direction;
    private bool onInitialPosition;
    private bool onFinalPosition;

	// Use this for initialization
	void Start () {
        influenceZone = GetComponent<BoxCollider2D>();
        influenceZone.size = sizeInfluenceZone;

        anim = GetComponent<Animator>();
    }
	
	// Update is called once per frame

    private void OnTriggerStay2D(Collider2D collision)
    {
        print(collision.tag.Equals("Player") + " " + onInitialPosition + " " + onFinalPosition);
        following = collision.tag.Equals("Player") 
            && ((!onInitialPosition 
            && !onFinalPosition) 
            || (onInitialPosition && GetComponent<Transform>().position.x < collision.GetComponent<Transform>().position.x)
            || (onFinalPosition && GetComponent<Transform>().position.x > collision.GetComponent<Transform>().position.x));

        direction = (GetComponent<Transform>().position.x > collision.GetComponent<Transform>().position.x) ? 1 : -1;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        following = !collision.tag.Equals("Player");
    }

    protected override void ComputeVelocity()
    {
        Vector2 move = Vector2.zero;

        if (following)
        {
            move.x = direction;

            //anim.SetBool("movement", move.x != lastMove || move.x != 0);

            //lastMove = move.x;
        }

        if ((move.x > 0.01f && GetComponent<Transform>().localScale.x < 0) || (move.x < -0.01f && GetComponent<Transform>().localScale.x > 0))
        {
            GetComponent<Transform>().localScale = new Vector3(-GetComponent<Transform>().localScale.x, GetComponent<Transform>().localScale.y, GetComponent<Transform>().localScale.z);
        }

        targetVelocity = move * maxSpeed;

        onInitialPosition = GetComponent<Transform>().position == initialPosition.position;
        onFinalPosition = GetComponent<Transform>().position == finalPosition.position;
    }
}
