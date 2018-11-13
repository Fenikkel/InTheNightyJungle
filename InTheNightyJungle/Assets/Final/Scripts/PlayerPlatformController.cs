﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlatformController : PhysicsObject {

    public float initialMaxSpeed = 7;
    public float initialJumpTakeOffSpeed = 7;
    private float maxSpeed;
    private float jumpTakeOffSpeed;

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

    private bool dash;
    private bool dashCooldown;
    private float dashSpeed;
    public float initialDashSpeed;
    public float dashTime;
    public float dashCooldownTime;
    public ParticleSystem dashEffect;

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
        dash = false;
        dashCooldown = false;

        maxSpeed = initialMaxSpeed;
        jumpTakeOffSpeed = initialJumpTakeOffSpeed;
        dashSpeed = initialDashSpeed;
    }

    protected override void ComputeVelocity()
    {
        Vector2 move = Vector2.zero;

        if (inputActivated)
        {
            if(Input.GetKeyDown(KeyCode.Z) && !dashCooldown)
            {
                Dash();
            }
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
            if(!dash)
            {
                targetVelocity = move * maxSpeed;
            }
            else
            {
                targetVelocity = new Vector2(GetComponent<Transform>().localScale.x, 0).normalized * dashSpeed;
            }
        }
    }

    private void Dash()
    {
        inputActivated = false; //No se puede mover
        dash = true; //Se está haciendo el dash
        dashCooldown = true; //No se va a poder gastar en un ratete

        velocity.y = 0; //Quitar la velocidad teórica vertical del siguiente frame (para si se hace el dash saltando)
        gravityModifier = 0; //Quitar el modificador de la gravedad para que el script no provoque caída
        rb2d.velocity = Vector2.zero; //Quitar la velocidad residual del Rigidbody en el momento del dash
        rb2d.isKinematic = true; //Quitar la influencia de la gravedad en el Rigidbody

        dashEffect.Play(); //Emisión de partículas

        Invulnerable(true); //No le tienen que hacer daño y debe poder traspasar los enemigos

        StartCoroutine(WhileDashActivated(dashTime)); //Iniciar la duración del dash
    }

    IEnumerator WhileDashActivated(float time)
    {
        yield return new WaitForSeconds(time);

        dash = false;
        inputActivated = true;

        gravityModifier = initialGravityModifier;
        rb2d.isKinematic = false;

        dashEffect.Stop();

        Invulnerable(false);

        StartCoroutine(WhileDashCooldownActivated(dashCooldownTime));
    }

    IEnumerator WhileDashCooldownActivated(float time)
    {
        yield return new WaitForSeconds(time);
        dashCooldown = false;
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
        if (!invulnerabity && other.tag.Equals("Enemy") && other.layer == LayerMask.NameToLayer("PhysicalEnemy"))
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
        knockback = true;
        targetVelocity = Vector2.zero;
        Invulnerable(true);

        float xDirection = 0.0f;
        float yDirection = 0.0f;

        foreach (ContactPoint2D contact in contacts)
        {
            xDirection += contact.normal.x;
            yDirection += contact.normal.y;
        }
        knockbackDirection = new Vector2(xDirection / contacts.Length, yDirection / contacts.Length).normalized;

        StartCoroutine(ReduceKnockback(0.5f));
        StartCoroutine(InvulnerabilityTime(2f, 0.1f));
    }

    private void Invulnerable(bool param)
    {
        if(param)
        {
            invulnerabity = true;
            gameObject.layer = LayerMask.NameToLayer("UntargetedPlayer");
            contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer) & ~(1 << LayerMask.NameToLayer("Enemy")));
        }
        else
        {
            invulnerabity = false;
            gameObject.layer = LayerMask.NameToLayer("Player");
            contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer) | (1 << LayerMask.NameToLayer("Enemy")));
        }
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
        Invulnerable(false);
    }

    private void Blink()
    {
        for (int i = 0; i < bodyParts.Length; i++)
        {
            bodyParts[i].enabled = blink;
        }
        blink = !blink;
    }

    public void SlowDown(float slowDownFactor, float time)
    {
        if (time == 0)
        {
            maxSpeed *= slowDownFactor;
            jumpTakeOffSpeed *= slowDownFactor; //Esta operación reduce la fuerza inicial de salto, de manera que es imposible saltar hasta una plataforma estando sobre el efecto del Devorador o habiendo sido afectado por un hielo. Cabe considerar, si a nivel de diseño, supone mucho inconveniente para el jugador o no, sobretodo en el caso de Cindy
        }
        else
        {
            StartCoroutine(SlowDownForTime(slowDownFactor, time));
        }
    }

    public void SpeedToOriginal()
    {
        maxSpeed = initialMaxSpeed;
        jumpTakeOffSpeed = initialJumpTakeOffSpeed;
    }

    IEnumerator SlowDownForTime(float slowDownFactor, float time)
    {
        maxSpeed *= slowDownFactor;
        jumpTakeOffSpeed *= slowDownFactor;
        yield return new WaitForSeconds(time);
        SpeedToOriginal();
    }

    public bool GetGrounded()
    {
        return grounded;
    }
}
