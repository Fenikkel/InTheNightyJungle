﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject : MonoBehaviour {

    public float minGroundNormalY = .65f;
    protected float gravityModifier;
    public float initialGravityModifier = 1f;

    protected Vector2 targetVelocity;
    protected bool grounded;
    protected MotionPlatform onMotionPlatform;
    protected Vector2 groundNormal;
    protected Rigidbody2D rb2d;
    protected Vector2 velocity;
    protected ContactFilter2D contactFilter;
    protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16];
    protected List<RaycastHit2D> hitBufferList = new List<RaycastHit2D> (16);
    protected float distance;
    protected Vector2 move;

    protected const float minMoveDistance = 0.001f;
    protected const float shellRadius = 0.05f;

    void OnEnable()
    {
        rb2d = GetComponent<Rigidbody2D> ();
    }

    void Start () 
    {
        initialization();
    }

    protected virtual void initialization()
    {
        ContactFilterInitialization();
        gravityModifier = initialGravityModifier;
        onMotionPlatform = null;
    }

    protected virtual void ContactFilterInitialization()
    {
        contactFilter.useTriggers = false; //No tomará los colliders triggereados
        contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer)); //Con esto le estamos diciendo que el contactFilter guarde como máscara de layers aquellas layers contra las que puede colisionar el objeto que estamos tratando, que pueden ser definidas cambiando los settings de las físicas 2D de la escena. Una maravilla, la verdad :)
        contactFilter.useLayerMask = true;
    }
    
    void Update () 
    {
        targetVelocity = Vector2.zero;
        ComputeVelocity (); 
    }

    protected virtual void ComputeVelocity()
    {
    
    }

    void FixedUpdate()
    {
        OurFixedUpdate();
    }

    protected virtual void OurFixedUpdate()
    {
        velocity += gravityModifier * Physics2D.gravity * Time.deltaTime;
        velocity.x = targetVelocity.x;

        grounded = false;

        Vector2 deltaPosition = velocity * Time.deltaTime;

        Vector2 moveAlongGround = new Vector2 (groundNormal.y, -groundNormal.x);

        move = moveAlongGround * deltaPosition.x;

        Movement (move, false);

        move = Vector2.up * deltaPosition.y;

        Movement (move, true);
    }

    protected void Movement(Vector2 move, bool yMovement)
    {
        distance = move.magnitude;

        if (distance > minMoveDistance) 
        {
            int count = rb2d.Cast (move, contactFilter, hitBuffer, distance + shellRadius);
            hitBufferList.Clear ();
            for (int i = 0; i < count; i++) {
                hitBufferList.Add (hitBuffer [i]);
            }

            for (int i = 0; i < hitBufferList.Count; i++) 
            {
                if(!(this is MotionPlatform))
                {
                    Vector2 currentNormal = hitBufferList [i].normal;
                    if (currentNormal.y > minGroundNormalY) 
                    {
                        grounded = true;
                        if (yMovement) 
                        {
                            groundNormal = currentNormal;
                            currentNormal.x = 0;
                        }
                        if(onMotionPlatform)
                        {
                            if(yMovement) 
                                distance = onMotionPlatform.GetDistance().y;
                            else 
                                distance = onMotionPlatform.GetDistance().x;
                        }
                    }
                    else
                    {
                        if(onMotionPlatform)
                        {
                            if(!yMovement)
                            {
                                if(Mathf.Sign(currentNormal.x) != Mathf.Sign(move.x))
                                {
                                    distance = 0;
                                    velocity = Vector2.zero;
                                }
                                else
                                {
                                    distance += onMotionPlatform.GetDistance().x + shellRadius;
                                    velocity = Vector2.zero;
                                }
                            }
                        }
                    }
                    
                    float projection = Vector2.Dot (velocity, currentNormal);
                    if (projection < 0) 
                    {
                        velocity = velocity - projection * currentNormal;
                    }

                    float modifiedDistance = hitBufferList [i].distance - shellRadius;

                    /*print(LayerMask.LayerToName(hitBufferList[i].collider.gameObject.layer) + " " + yMovement);
                    if(LayerMask.LayerToName(hitBufferList[i].collider.gameObject.layer).Equals("PlatformCollider"))
                    {
                        if(!yMovement)  modifiedDistance -= hitBufferList[i].collider.gameObject.GetComponent<MotionPlatform>().GetDistance().y;
                        else modifiedDistance += hitBufferList[i].collider.gameObject.GetComponent<MotionPlatform>().GetDistance().x;
                    }*/

                    distance = modifiedDistance < distance ? modifiedDistance : distance;
                }
            }
        }
        else
        {
            int count = rb2d.Cast (move, contactFilter, hitBuffer, distance + shellRadius);
            hitBufferList.Clear ();
            for (int i = 0; i < count; i++) {
                hitBufferList.Add (hitBuffer [i]);
            }

            for (int i = 0; i < hitBufferList.Count; i++) 
            {
                Vector2 currentNormal = hitBufferList [i].normal;
                if (currentNormal.y > minGroundNormalY) 
                {
                    grounded = true;
                    if(onMotionPlatform)
                    {
                        if(yMovement) 
                            distance = onMotionPlatform.GetDistance().y;
                        else 
                            distance = onMotionPlatform.GetDistance().x;
                    }
                }
                else
                {
                    if(onMotionPlatform)
                    {
                        if(!yMovement)
                        {
                            move = currentNormal;
                            distance = onMotionPlatform.GetDistance().x + shellRadius;
                            velocity -= currentNormal;
                        }
                    }
                }
            }
        }
                    
        rb2d.position = rb2d.position + move.normalized * distance;
    }

}
