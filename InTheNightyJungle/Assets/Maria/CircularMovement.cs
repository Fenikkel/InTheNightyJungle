using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularMovement : MotionPlatform {

    [SerializeField]
    Transform rotationCenter;

    private float rotationRadius = 2f;

    public float initialAngle;
    float angle = 0f;
	
    protected override void initialization()
    {
        base.initialization();
        rotationRadius = (GetComponent<Transform>().position - rotationCenter.position).magnitude;
        angle = initialAngle;
    }

	// Update is called once per frame
	/*void FixedUpdate () {
        posX = rotationCenter.position.x + Mathf.Cos(angle * Mathf.Deg2Rad) * rotationRadius;
        posY = rotationCenter.position.y + Mathf.Sin(angle * Mathf.Deg2Rad) * rotationRadius;

        distance = new Vector2(posX, posY) - ((Vector2)transform.position); //Esto lo necesita al jugador para cuando está en contacto con una plataforma móvil

        transform.position = new Vector2(posX, posY);
        angle = angle + Time.deltaTime * speed;

        if(angle >= 360f)
        {
            angle = 0f;
        }

    }*/

    protected override void ComputeVelocity()
    {
        Vector2 move = Vector2.zero;
        angle += Time.deltaTime * speed;
        float nextPosX = (rotationCenter.position.x + Mathf.Cos(angle * Mathf.Deg2Rad) * rotationRadius) - GetComponent<Transform>().position.x;
        float nextPosY = (rotationCenter.position.y + Mathf.Sin(angle * Mathf.Deg2Rad) * rotationRadius) - GetComponent<Transform>().position.y;

        move = new Vector2(nextPosX, nextPosY);

        targetVelocity = move * speed;
    }

    protected override void OurFixedUpdate()
    {
        velocity = targetVelocity;

        grounded = false;

        Vector2 deltaPosition = velocity * Time.deltaTime;

        Vector2 moveAlongGround = new Vector2 (groundNormal.y, -groundNormal.x);

        move = Vector2.right * deltaPosition.x;

        move += Vector2.up * deltaPosition.y;

        Movement (move, false);
    }
/*
    protected override void ComputeVelocity()
    {
        Vector2 move = Vector2.zero;

        if (inputActivated)
        {
            if(Input.GetKeyDown(KeyCode.Z) && !dashCooldown)
            {
                Dash();
            }
            else if (Input.GetKeyDown(KeyCode.X) && !breathCooldown)
            {
                Breath();
            }
            move.x = Input.GetAxis("Horizontal");

            anim.SetBool("movement", move.x != lastMove || move.x != 0);
            anim.SetBool("grounded", grounded);
            anim.SetFloat("yVel", velocity.y);

            lastMove = move.x;

            if (Input.GetButtonDown("Jump") && grounded)
            {
                velocity.y = jumpTakeOffSpeed;
            }
            else if (Input.GetButtonUp("Jump"))
            {
                if (velocity.y > 0)
                {
                    velocity.y = velocity.y * 0.5f;
                }
            }
        }

        if ((move.x > 0.01f && GetComponent<Transform>().localScale.x < 0) || (move.x < -0.01f && GetComponent<Transform>().localScale.x > 0))
        {
            GetComponent<Transform>().localScale = new Vector3(-GetComponent<Transform>().localScale.x, GetComponent<Transform>().localScale.y, GetComponent<Transform>().localScale.z);
        }

        if (knockback)
        {
            targetVelocity = knockbackDirection * knockbackSpeed;
            velocity.y = targetVelocity.y;
        }
        else
        {
            if(!dash)
            {
                targetVelocity = move * maxSpeed;
            }
            else
            {
                targetVelocity = new Vector2(GetComponent<Transform>().localScale.x, 0).normalized * dashSpeed;
            }
        }
    }*/
}
