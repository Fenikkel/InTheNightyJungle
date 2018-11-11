using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlatformController : PhysicsObject {

    public float maxSpeed = 7;
    public float jumpTakeOffSpeed = 7;

    private bool inputActivated;

    private bool invulnerabity;
    private bool knockback;
    private Vector2 knockbackDirection;
    public float knockbackSpeed = 2;

    private PlayerStatsController stats;
    private Animator anim;

    private float lastMove;

    public SkinnedMeshRenderer[] bodyParts;
    private bool blink;

    // Use this for initialization
    void Awake()
    {
        stats = GetComponent<PlayerStatsController>();
        anim = GetComponent<Animator>();
    }


    protected override void initialization()
    {
        base.initialization();
        inputActivated = true;
        invulnerabity = false;
        blink = false;
    }

    protected override void ComputeVelocity()
    {
        Vector2 move = Vector2.zero;

        if (inputActivated)
        {
            move.x = Input.GetAxis("Horizontal");

            //anim.SetBool("movement", move.x != lastMove || move.x != 0);

            //lastMove = move.x;

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
            targetVelocity = move * maxSpeed;
        }
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
        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            GetComponent<Transform>().position = Vector2.Lerp(initialPosition, finalPosition, elapsedTime / time);
            yield return null;
        }
        GetComponent<Transform>().position = finalPosition;
        GetComponent<SpriteRenderer>().sprite = s;
        GetComponent<CapsuleCollider2D>().enabled = true;
        SetInputActivated(true);
    }

    public void DecreaseCansancio(float value)
    {
        stats.DecreaseChangenessStat(value);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject other = collision.gameObject;
        if (!invulnerabity && other.tag.Equals("Enemy"))
        {
            EnemyBehaviour enemy = other.GetComponent<EnemyBehaviour>();
            stats.DecreasePaciencia(enemy.GetDamage() / 100);
            enemy.CollideWithPlayer();

            KnockBack(collision.contacts);
        }

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        OnCollisionEnter2D(collision);
    }

    private void /*Proto*/KnockBack(ContactPoint2D[] contacts)
    {
        inputActivated = false;
        invulnerabity = true;
        knockback = true;
        targetVelocity = Vector2.zero;

        float xDirection = 0.0f;
        float yDirection = 0.0f;

        foreach (ContactPoint2D contact in contacts)
        {
            xDirection += contact.normal.x;
            yDirection += contact.normal.y;
        }
        knockbackDirection = new Vector2(xDirection / contacts.Length, yDirection / contacts.Length).normalized;

        gameObject.layer = LayerMask.NameToLayer("UntargetedPlayer");
        contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer) & ~(1 << LayerMask.NameToLayer("Enemy")));

        StartCoroutine(ReduceKnockback(0.5f));
        StartCoroutine(InvulnerabilityTime(2f, 0.1f));
    }

    IEnumerator ReduceKnockback(float time)
    {
        float elapsedTime = 0.0f;
        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            float xNewDirection = Mathf.Lerp(knockbackDirection.x, 0, elapsedTime / time);
            float yNewDirection = Mathf.Lerp(knockbackDirection.y, 0, elapsedTime / time);
            knockbackDirection = new Vector2(xNewDirection, yNewDirection);
            yield return null;
        }
        knockbackDirection = new Vector2(0, 0);
        knockback = false;
        inputActivated = true;
    }

    IEnumerator InvulnerabilityTime(float time, float blinkTime)
    {
        float elapsedBlinkTime = 0.0f;
        float elapsedTotalTime = 0.0f;
        
        while (elapsedTotalTime < time)
        {
            elapsedTotalTime += Time.deltaTime;
            elapsedBlinkTime += Time.deltaTime;
            if (elapsedBlinkTime >= blinkTime)
            {
                Blink();
                elapsedBlinkTime = 0.0f;
            }
            yield return null;
        }
        if (blink) Blink();
        invulnerabity = false;
        gameObject.layer = LayerMask.NameToLayer("Player");
        contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer) | (1 << LayerMask.NameToLayer("Enemy")));
    }

    private void Blink()
    {
        for (int i = 0; i < bodyParts.Length; i++)
        {
            bodyParts[i].enabled = blink;
        }
        blink = !blink;
    }
}
