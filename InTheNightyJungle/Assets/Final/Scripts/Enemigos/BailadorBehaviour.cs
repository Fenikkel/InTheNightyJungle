﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BailadorBehaviour : EnemyBehaviour {

    public Transform target;
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
        distance = Mathf.Abs(GetComponent<Transform>().position.x - target.position.x);

        movingTime = distance / maxSpeed;

        GetComponent<Transform>().localScale = new Vector3(initialMoveDirection * GetComponent<Transform>().localScale.x, GetComponent<Transform>().localScale.y, GetComponent<Transform>().localScale.z);

        whirlpool.Play();
    }

    private void OnEnable()
    {
        coroutine = StartCoroutine(Accelerate());
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
        
        Vector2 initialPosition = GetComponent<Transform>().position;
        Vector2 finalPosition = target.position;

        while(elapsedTime < movingTime)
        {
            elapsedTime += Time.deltaTime;
            GetComponent<Transform>().position = Vector2.Lerp(initialPosition, finalPosition, elapsedTime / movingTime);
            yield return null;
        }

        GetComponent<Transform>().position = finalPosition;

        target.position = initialPosition;

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
}
