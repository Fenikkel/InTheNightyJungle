using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalbucienteBehaviour : EnemyBehaviour {

    public Transform initialPosition;
    public Transform finalPosition;

    private CharacterController2D controller;

    private float move;
    public float maxSpeed;
    public float radius;

    private RaycastHit2D hitLeft;
    private RaycastHit2D hitRight;
    private float distanceToLeft;
    private float distanceToRight;

    private bool colliding;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController2D>();
        colliding = false;
    }

    private void Update()
    {
        if(!death)
        {
            Debug.DrawLine(GetComponent<Transform>().position, GetComponent<Transform>().position + new Vector3(radius, 0, 0));
            Debug.DrawLine(GetComponent<Transform>().position, GetComponent<Transform>().position + new Vector3(-radius, 0, 0));
            
            move = 0;
            
            anim.SetBool("movement", hitLeft.collider || hitRight.collider && !colliding);

            if (hitLeft.collider || hitRight.collider && !colliding)
            {
                move = (hitLeft.collider) ? -1 : 1;
            }

            if ((move > 0.01f && GetComponent<Transform>().localScale.x < 0) || (move < -0.01f && GetComponent<Transform>().localScale.x > 0))
            {
                GetComponent<Transform>().localScale = new Vector3(-GetComponent<Transform>().localScale.x, GetComponent<Transform>().localScale.y, GetComponent<Transform>().localScale.z);
            }

            move *= maxSpeed;

            distanceToLeft = ((GetComponent<Transform>().position.x - initialPosition.position.x) < radius) ? (GetComponent<Transform>().position.x - initialPosition.position.x) : radius;
            distanceToRight = ((finalPosition.position.x - GetComponent<Transform>().position.x) < radius) ? (finalPosition.position.x - GetComponent<Transform>().position.x) : radius;
        }
    }

    private void FixedUpdate()
    {
        if(!death)
        {
            hitLeft = Physics2D.Raycast(GetComponent<Transform>().position, Vector2.left, distanceToLeft, 1 << LayerMask.NameToLayer("Player"));
            hitRight = Physics2D.Raycast(GetComponent<Transform>().position, Vector2.right, distanceToRight, 1 << LayerMask.NameToLayer("Player"));

            controller.Move(move * Time.fixedDeltaTime, false, false);
        }
    }

    public override void CollideWithPlayer()
    {
        colliding = true;
        StartCoroutine(SportshipStop(0.5f));
    }

    IEnumerator SportshipStop(float time)
    {
        yield return new WaitForSeconds(time);
        colliding = false;
    }
}
