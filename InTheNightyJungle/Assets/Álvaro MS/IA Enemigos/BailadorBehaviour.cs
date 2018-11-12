using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BailadorBehaviour : EnemyBehaviour {

    public Transform accZoneInitialPos;
    public Transform accZoneFinalPos;
    public Transform spinZoneInitialPos;
    public Transform spinZoneFinalPos;

    public float initialMoveDirection;
    private float moveDirection;
    private bool spinning;
    private bool accelerating;

    public float maxSpeed;
    public float stopTime;

    private float accelerationLeft;
    private float accelerationRight;
    private float timeToAccelerateLeft;
    private float timeToAccelerateRight;

    protected override void initialization()
    {
        base.initialization();
        moveDirection = initialMoveDirection;
        timeToAccelerateLeft = TimeInMRUA(accZoneInitialPos.position, spinZoneInitialPos.position, 0.0f, maxSpeed, out accelerationLeft);
        timeToAccelerateRight = TimeInMRUA(spinZoneFinalPos.position, accZoneFinalPos.position, 0.0f, maxSpeed, out accelerationRight);
    }

    private float TimeInMRUA(Vector2 initialPos, Vector2 finalPos, float initialSpeed, float finalSpeed, out float acceleration)
    {
        acceleration = (Mathf.Pow(finalSpeed, 2) - Mathf.Pow(initialSpeed, 2)) / (2 * (finalPos.x - initialPos.x)); // a = (v^2 - v0^2)/(2 * (x - x0))
        return (finalSpeed - initialSpeed) / acceleration;
    }

    protected override void ComputeVelocity()
    {
        Vector2 move = Vector2.zero;

        move.x = moveDirection;

        //anim.SetBool("movement", move.x != lastMove || move.x != 0);

        //lastMove = move.x;

        if ((move.x > 0.01f && GetComponent<Transform>().localScale.x < 0) || (move.x < -0.01f && GetComponent<Transform>().localScale.x > 0))
        {
            GetComponent<Transform>().localScale = new Vector3(-GetComponent<Transform>().localScale.x, GetComponent<Transform>().localScale.y, GetComponent<Transform>().localScale.z);
        }

        targetVelocity = move * maxSpeed;

        spinning = !(GetComponent<Transform>().position.x < spinZoneInitialPos.position.x || GetComponent<Transform>().position.x > spinZoneFinalPos.position.x);
        if(!spinning && !accelerating)
        {
            accelerating = true;
            bool toLeft = GetComponent<Transform>().position.x < spinZoneInitialPos.position.x;
            if (toLeft) StartCoroutine(Accelerate(timeToAccelerateLeft, -1, 0, true));
            else StartCoroutine(Accelerate(timeToAccelerateRight, 1, 0, true));
        }
    }

    IEnumerator Accelerate(float time, float initialValue, float finalValue, bool hasToStop)
    {
        float elapsedTime = 0.0f;
        while(elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            moveDirection = Mathf.Lerp(initialValue, finalValue, elapsedTime / time);
            yield return null;
        }
        moveDirection = finalValue;
        if (hasToStop) StartCoroutine(Stopping(stopTime, -initialValue));
        if(!hasToStop)
        {
            yield return new WaitForSeconds(0.1f);
            accelerating = false;
            spinning = true;
        }
    }

    IEnumerator Stopping(float time, float finalValue)
    {
        yield return new WaitForSeconds(time);
        StartCoroutine(Accelerate((finalValue == 1) ? timeToAccelerateLeft : timeToAccelerateRight, 0.0f, finalValue, false));
    }
}
