using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BailadorBehaviour : EnemyBehaviour {

    public Transform target1;
    public Transform target2;
    private bool beginning;
    private float distance;

    public float initialMoveDirection;

    public float maxSpeed;
    private float movingTime;
    public float stopTime;

    public ParticleSystem whirlpool;

    private CharacterController2D controller;

    private Coroutine coroutine;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    
    private void Start()
    {
        distance = Mathf.Abs(target1.position.x - target2.position.x);

        movingTime = distance / maxSpeed;
    }

    private void OnEnable()
    {
        GetComponent<Transform>().localPosition = new Vector2(target1.localPosition.x, GetComponent<Transform>().localPosition.y);
        beginning = true;
        GetComponent<Transform>().localScale = new Vector3(initialMoveDirection * Mathf.Abs(GetComponent<Transform>().localScale.x), GetComponent<Transform>().localScale.y, GetComponent<Transform>().localScale.z);

        coroutine = StartCoroutine(Accelerate());
        anim.SetBool("accelerating", true);
        whirlpool.Play();
    }

    private void Update()
    {
        if(death)
        {
            StopCoroutine(coroutine);
        }
    }

    private IEnumerator Accelerate()
    {
        float elapsedTime = 0.0f;
        
        Vector2 initialPosition = GetComponent<Transform>().localPosition;
        Vector2 finalPosition = (beginning) ? target2.localPosition : target1.localPosition;

        while(elapsedTime < movingTime)
        {
            elapsedTime += Time.deltaTime;
            GetComponent<Transform>().localPosition = Vector2.Lerp(initialPosition, finalPosition, elapsedTime / movingTime);
            yield return null;
        }

        GetComponent<Transform>().localPosition = finalPosition;
        beginning = !beginning;

        coroutine = StartCoroutine(Stopping());
    }

    private IEnumerator Stopping()
    {
        anim.SetBool("accelerating", false);

        yield return new WaitForSeconds(stopTime / 2);
        anim.SetBool("accelerating", true);
        GetComponent<Transform>().localScale = new Vector3(-GetComponent<Transform>().localScale.x, GetComponent<Transform>().localScale.y, GetComponent<Transform>().localScale.z);

        yield return new WaitForSeconds(stopTime / 2);
        coroutine = StartCoroutine(Accelerate());
    }

    public override void CollideWithPlayer()
    {
        hitSound.Play();
    }
}
