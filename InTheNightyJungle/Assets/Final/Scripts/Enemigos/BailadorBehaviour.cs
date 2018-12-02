using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BailadorBehaviour : EnemyBehaviour {

    /*public Transform accZoneInitialPos;
    public Transform accZoneFinalPos;
    public Transform spinZoneInitialPos;
    public Transform spinZoneFinalPos;*/

    public Transform target;
    private float distance;

    public float initialMoveDirection;
    private float moveDirection;
    
    /*private bool spinning;
    private bool accelerating;

    private float move;*/
    public float maxSpeed;
    private float movingTime;
    public float stopTime;

    /*private float accelerationLeft;
    private float accelerationRight;
    private float timeToAccelerateLeft;
    private float timeToAccelerateRight;*/

    public ParticleSystem whirlpool;

    private Animator anim;
    private CharacterController2D controller;

    private void Start()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController2D>();

        distance = Mathf.Abs(GetComponent<Transform>().position.x - target.position.x);

        movingTime = distance / maxSpeed;

        moveDirection = initialMoveDirection;

        whirlpool.Play();

        StartCoroutine(Accelerate());
    }

    private void Update()
    {
        //move = moveDirection * maxSpeed;
    }
    /*
    private void FixedUpdate()
    {
        controller.Move(move * Time.fixedDeltaTime, false, false);
    }*/

    private IEnumerator Accelerate()
    {
        float elapsedTime = 0.0f;
        
        Vector2 initialPosition = GetComponent<Transform>().position;
        //Vector2 interPosition1 = new Vector2(initialPosition.x + moveDirection * distance/5, initialPosition.y);
        Vector2 finalPosition = target.position;
        //Vector2 interPosition2 = new Vector2(finalPosition.x - moveDirection * distance/5, finalPosition.y);

        //float thirdTime = movingTime/3;

        /* while(elapsedTime < thirdTime)
        {
            elapsedTime += Time.deltaTime;
            GetComponent<Transform>().position = Vector2.Lerp(initialPosition, interPosition1, elapsedTime / thirdTime);
            yield return null;
        }
        GetComponent<Transform>().position = interPosition1;

        while(elapsedTime < 2 * thirdTime && elapsedTime > thirdTime)
        {
            elapsedTime += Time.deltaTime;
            GetComponent<Transform>().position = Vector2.Lerp(interPosition1, interPosition2, elapsedTime / (2 * thirdTime));
            yield return null;
        }
        GetComponent<Transform>().position = interPosition2;

        while(elapsedTime < 3 * thirdTime && elapsedTime > 2 * thirdTime)
        {
            elapsedTime += Time.deltaTime;
            GetComponent<Transform>().position = Vector2.Lerp(interPosition1, interPosition2, elapsedTime / (3 * thirdTime));
            yield return null;
        }*/

        while(elapsedTime < movingTime)
        {
            elapsedTime += Time.deltaTime;
            GetComponent<Transform>().position = Vector2.Lerp(initialPosition, finalPosition, elapsedTime / movingTime);
            yield return null;
        }

        GetComponent<Transform>().position = finalPosition;

        target.position = initialPosition;

        StartCoroutine(Stopping());
    }

    private IEnumerator Stopping()
    {
        anim.SetBool("accelerating", false);
        //whirlpool.Stop();

        yield return new WaitForSeconds(stopTime / 2);
        anim.SetBool("accelerating", true);
        //whirlpool.Play();
        moveDirection *= -1;
        GetComponent<Transform>().localScale = new Vector3(-GetComponent<Transform>().localScale.x, GetComponent<Transform>().localScale.y, GetComponent<Transform>().localScale.z);

        yield return new WaitForSeconds(stopTime / 2);
        StartCoroutine(Accelerate());
    }
}
