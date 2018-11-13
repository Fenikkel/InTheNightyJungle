using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalbucienteBehaviour : EnemyBehaviour {

    public Transform initialPosition;
    public Transform finalPosition;

    private Animator anim;
    private float lastMove;
    public float maxSpeed;
    public float radius;

    private float distanceToLeft;
    private float distanceToRight;

    private bool colliding;

	// Use this for initialization
	void Awake () {
        anim = GetComponent<Animator>();

    }

    protected override void initialization()
    {
        base.initialization();
        colliding = false;
    }

    protected override void ComputeVelocity()
    {
        Debug.DrawLine(GetComponent<Transform>().position, GetComponent<Transform>().position + new Vector3(radius, 0, 0));
        Debug.DrawLine(GetComponent<Transform>().position, GetComponent<Transform>().position + new Vector3(-radius, 0, 0));
        
        Vector2 move = Vector2.zero;

        RaycastHit2D hitLeft = Physics2D.Raycast(GetComponent<Transform>().position, Vector2.left, distanceToLeft, 1 << LayerMask.NameToLayer("Player"));
        RaycastHit2D hitRight = Physics2D.Raycast(GetComponent<Transform>().position, Vector2.right, distanceToRight, 1 << LayerMask.NameToLayer("Player"));
        
        anim.SetBool("movement", hitLeft.collider || hitRight.collider && !colliding);

        if (hitLeft.collider || hitRight.collider && !colliding)
        {
            move.x = (hitLeft.collider) ? -1 : 1;
        }

        if ((move.x > 0.01f && GetComponent<Transform>().localScale.x < 0) || (move.x < -0.01f && GetComponent<Transform>().localScale.x > 0))
        {
            GetComponent<Transform>().localScale = new Vector3(-GetComponent<Transform>().localScale.x, GetComponent<Transform>().localScale.y, GetComponent<Transform>().localScale.z);
        }

        targetVelocity = move * maxSpeed;

        distanceToLeft = ((GetComponent<Transform>().position.x - initialPosition.position.x) < radius) ? (GetComponent<Transform>().position.x - initialPosition.position.x) : radius;
        distanceToRight = ((finalPosition.position.x - GetComponent<Transform>().position.x) < radius) ? (finalPosition.position.x - GetComponent<Transform>().position.x) : radius;
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
