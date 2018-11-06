using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlatformController : PhysicsObject {

    public float maxSpeed = 7;
    public float jumpTakeOffSpeed = 7;

    private SpriteRenderer spriteRenderer;

    private bool inputActivated;

    // Use this for initialization
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        inputActivated = true;
    }

    protected override void ComputeVelocity()
    {
        Vector2 move = Vector2.zero;

        if (inputActivated)
        {
            move.x = Input.GetAxis("Horizontal");

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

        bool flipSprite = (spriteRenderer.flipX ? (move.x > 0.01f) : (move.x < 0.01f));
        if (flipSprite)
        {
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }

        targetVelocity = move * maxSpeed;
    }

    public bool GetInputActivated()
    {
        return inputActivated;
    }

    public void SetInputActivated(bool param)
    {
        inputActivated = param;
    }

    public void SetPosition(Vector2 param)
    {
        GetComponent<Transform>().position = param;
    }

    public void MoveToLeftRightChamber(DoorBehaviour door)
    {
        Sprite s = GetComponent<SpriteRenderer>().sprite;
        GetComponent<CapsuleCollider2D>().enabled = false;
        GetComponent<SpriteRenderer>().sprite = null;
        StartCoroutine(InterpolatePositionChangingChamber(0.5f, door.nextDoor.playerPosition.position, s));
    }

    IEnumerator InterpolatePositionChangingChamber(float time, Vector2 finalPosition, Sprite s)
    {
        float elapsedTime = 0.0f;
        Vector2 initialPosition = GetComponent<Transform>().position;
        while(elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            GetComponent<Transform>().position = Vector2.Lerp(initialPosition, finalPosition, elapsedTime/time);
            yield return null;
        }
        GetComponent<Transform>().position = finalPosition;
        GetComponent<SpriteRenderer>().sprite = s;
        GetComponent<CapsuleCollider2D>().enabled = true;
        SetInputActivated(true);
    }
}
