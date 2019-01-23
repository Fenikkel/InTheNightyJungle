﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Anima2D;

public class PlayerPlatformController : MonoBehaviour {

    public static PlayerPlatformController Instance;
    public GameManager GM;

    public float initialMaxSpeed = 60;
    public float initialJumpForce;
    private float maxSpeed;

    private float move;
    private bool jump;

    public bool cindy;

    private DoorBehaviour lastDoor;
    private ChamberManager currentChamber;
    public ChamberManager initialChamber;

    //Maria
    [HideInInspector]
    public bool Descansando;
    public CheckPoint initialCheckPoint;
    private CheckPoint lastCheckPoint;

    private bool inputActivated;
    
    protected Transform onMotionPlatform;
    protected Rigidbody2D rb2d;

    private bool invulnerabity;
    private Coroutine invulnerableCoroutine;
    private bool knockback;
    private Vector2 knockbackDirection;
    public float knockbackSpeed = 2;

    private ContactFilter2D enemyContactFilter;
    private ContactFilter2D motionPlatformContactFilter;

    private PlayerStatsController stats;
    private Animator anim;
    private CharacterController2D controller;

    private float lastMove;

    public SkinnedMeshRenderer[] bodyParts;
    private bool blink;

    private bool breath;
    private bool breathCooldown;
    public float breathTime;
    public float breathCooldownTime;
    public ParticleSystem breathEffect;
    public GameObject breathLight;

    private bool dash;
    private bool dashCooldown;
    private float dashSpeed;
    public float initialDashSpeed;
    public float dashTime;
    public float dashCooldownTime;
    public ParticleSystem dashEffect;

    private DrinkingTestGlassBehaviour glass;
    public Transform rightHandBone;
    private string sortingLayer;

    public AudioSource jumpSound;
    public AudioSource breathSound;
    public AudioSource dashSound;

    private void Awake()
    {
        Instance = this;
        
        rb2d = GetComponent<Rigidbody2D> ();
        stats = GetComponent<PlayerStatsController>();
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController2D>();

        sortingLayer = bodyParts[0].GetComponent<SpriteMeshInstance>().sortingLayerName;

        lastCheckPoint = initialCheckPoint;
        currentChamber = initialChamber;

        initialization();
    }

    private void initialization()
    {
        RestartPlayer();

        ContactFilterInitialization();
    }

    public void RestartPlayer()
    {
        inputActivated = true;
        Invulnerable(false);
        Blink(true);
        dash = false;
        breath = false;
        dashCooldown = false;
        breathCooldown = false;
        if(!cindy) breathLight.SetActive(false);

        anim.Play("idle");

        maxSpeed = initialMaxSpeed;
        GetComponent<CharacterController2D>().SetJumpForce(initialJumpForce);
        dashSpeed = initialDashSpeed;
    }

    private void ContactFilterInitialization()
    {
        enemyContactFilter = new ContactFilter2D();
        enemyContactFilter.SetLayerMask(1 << LayerMask.NameToLayer("PhysicalEnemy"));
        enemyContactFilter.useLayerMask = true;

        motionPlatformContactFilter = new ContactFilter2D();
        motionPlatformContactFilter.SetLayerMask(1 << LayerMask.NameToLayer("PlatformCollider"));
        motionPlatformContactFilter.useLayerMask = true;
    }

    private void Update()
    {
        if(!dash) move = 0;

        if (inputActivated)
        {            
            move = Input.GetAxisRaw("Horizontal") * maxSpeed;

            if(cindy && Input.GetKeyDown(KeyCode.Z) && !dashCooldown)
            {
                Dash();
            }
            else if (!cindy && controller.GetGrounded() && Input.GetKeyDown(KeyCode.Z) && !breathCooldown)
            {
                Breath();
            }

            if(Input.GetKeyDown(KeyCode.I))
            {
                StartCoroutine(ChangePlayer());
            }

            anim.SetBool("movement", move != lastMove || move != 0);
            anim.SetBool("grounded", controller.GetGrounded());
            anim.SetFloat("yVel", rb2d.velocity.y);

            lastMove = move;

            if (Input.GetButtonDown("Jump"))
            {
                jump = true;
            }
            else if(Input.GetButtonUp("Jump"))
            {
                jump = false;
            }
        }

        if ((move > 0.01f && GetComponent<Transform>().localScale.x < 0) || (move < -0.01f && GetComponent<Transform>().localScale.x > 0))
        {
            GetComponent<Transform>().localScale = new Vector3(-GetComponent<Transform>().localScale.x, GetComponent<Transform>().localScale.y, GetComponent<Transform>().localScale.z);
        }

        /*if (knockback)
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
        }*/
    }

    private void FixedUpdate()
    {
        DetectingEnemies();
        DetectingMotionPlatform();
        
        if(knockback)
        {
            controller.Move(knockbackDirection.x * knockbackSpeed * Time.fixedDeltaTime, false, false);
            rb2d.velocity = new Vector2(rb2d.velocity.x, knockbackDirection.y * knockbackSpeed * 0.2f);
        }
        else
        {
            if(dash)
            {
                controller.Move(move * Time.fixedDeltaTime, false, false);
            }
            else
            {
                controller.Move(move * Time.fixedDeltaTime, false, jump);
		        jump = false;
            }
        }
    }

    private void Dash()
    {

        inputActivated = false; //No se puede mover
        dash = true; //Se está haciendo el dash
        anim.SetBool("dash", dash);
        dashCooldown = true; //No se va a poder gastar en un ratete

        rb2d.gravityScale = 0;
        rb2d.velocity = Vector2.zero; //Quitar la velocidad residual del Rigidbody en el momento del dash
        move = Mathf.Sign(GetComponent<Transform>().localScale.x) * dashSpeed;

        dashEffect.Play(); //Emisión de partículas
        dashSound.Play();

        if(invulnerableCoroutine != null) StopCoroutine(invulnerableCoroutine);
        invulnerableCoroutine = StartCoroutine(InvulnerabilityTime(dashTime + dashCooldownTime, 0.1f));

        StartCoroutine(WhileDashActivated(dashTime)); //Iniciar la duración del dash
    }

    private void Breath()
    {

        //inputActivated = false; //No se puede mover
        breath = true; //Se está haciendo el breath
        breathCooldown = true; //No se va a poder gastar en un ratete

        //El salto + aliento queda palero si le quitamos la velocidad residual
        //rb2d.velocity = Vector2.zero; //Quitar la velocidad residual del Rigidbody en el momento del dash
        anim.Play("fireBrenda"); //Hace la animacion, que esta ya se ocupa de activar el sistema de particulas

        if(invulnerableCoroutine != null) StopCoroutine(invulnerableCoroutine);
        invulnerableCoroutine = StartCoroutine(InvulnerabilityTime(breathTime + breathCooldownTime/4, 0.1f));

        StartCoroutine(WhileBreathActivated(breathTime)); //Iniciar la duración del dash
    }

    public void PlayBreath()
    {
        breathSound.Play();
        breathEffect.Play();
        breathLight.SetActive(true);
    }
    public void StopBreath()
    {
        breathEffect.Stop();
        breathLight.SetActive(false);
    }

    public void SetDashActivated(bool param)
    {
        dashCooldown = !param;
    }

    public void SetBreathActivated(bool param)
    {
        breathCooldown = !param;
    }

    //Maria
    public void StartProgressBar()
    {
        Descansando = true;
        StartCoroutine(ProgressPaciencia());
    }

    public void EndProgressBar()
    {
        inputActivated = true;
        anim.SetBool("WakeUp", false);
    }

    IEnumerator ProgressPaciencia()
    {
        stats.ChangePatience(1);
        yield return new WaitForSeconds(1);

        Descansando = false;
        anim.SetBool("Sitting", false);
        anim.SetBool("WakeUp", true);
    }

    public CheckPoint GetCheckPoint()
    {
        return lastCheckPoint;
    }

    public void SetCheckPoint(CheckPoint param)
    {
        lastCheckPoint = param;
    }

    IEnumerator WhileDashActivated(float time)
    {
        yield return new WaitForSeconds(time);

        dash = false;
        anim.SetBool("dash", dash);
        inputActivated = true;

        rb2d.gravityScale = 3;

        dashEffect.Stop();

        GetComponent<PlayerStatsController>().ChangeBladderTiredness(0.05f);
        if(GetComponent<PlayerStatsController>().CheckBladderTiredness())
        {
            StartCoroutine(WhileDashCooldownActivated(dashCooldownTime));

            yield return new WaitForSeconds(10 * time);
        }
        else
        {
            StartCoroutine(ChangePlayer());
        }
    }

    IEnumerator WhileBreathActivated(float time)
    {
        yield return new WaitForSeconds(time); //aci se deuria esperar a que la animacio acabara

        breath = false;
        //inputActivated = true;
        //gravityModifier = initialGravityModifier;

        GetComponent<PlayerStatsController>().ChangeBladderTiredness(0.05f);
        if(GetComponent<PlayerStatsController>().CheckBladderTiredness())
        {            
            StartCoroutine(WhileBreathCooldownActivated(breathCooldownTime));

            yield return new WaitForSeconds(10 * time);
        }
        else
        {
            StartCoroutine(ChangePlayer());
        }

    }

    IEnumerator WhileDashCooldownActivated(float time)
    {
        yield return new WaitForSeconds(time);
        dashCooldown = false;
    }
    IEnumerator WhileBreathCooldownActivated(float time)
    {
        yield return new WaitForSeconds(time);
        breathCooldown = false;
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
        Blink(false);
        //GetComponent<CapsuleCollider2D>().enabled = false;
        StartCoroutine(InterpolatePositionChangingChamber(0.5f, door.nextDoor.playerPosition.position, door));
        lastDoor = door.nextDoor;
    }

    IEnumerator InterpolatePositionChangingChamber(float time, Vector2 finalPosition, DoorBehaviour door)
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
        Blink(true);
        GetComponent<CapsuleCollider2D>().enabled = true;
        SetInputActivated(true);
        
        SetCurrentChamber(door.nextDoor.chamber);
        door.TurnOnTurnOff();
    }

    public IEnumerator MoveTo(Vector2 finalPosition, bool hasToFlip, float time)
    {
        float elapsedTime = 0.0f;
        Vector2 initialPosition = GetComponent<Transform>().position;
        Vector3 vectorAux = new Vector3(-GetComponent<Transform>().localScale.x, GetComponent<Transform>().localScale.y, GetComponent<Transform>().localScale.z);

        if(finalPosition.x < initialPosition.x && GetComponent<Transform>().localScale.x > 0) GetComponent<Transform>().localScale = vectorAux;
        else if(finalPosition.x > initialPosition.x && GetComponent<Transform>().localScale.x < 0) GetComponent<Transform>().localScale = vectorAux;

        anim.SetBool("movement", true);
        while(elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            GetComponent<Transform>().position = Vector2.Lerp(initialPosition, finalPosition, elapsedTime / time);
            yield return null;
        }
        GetComponent<Transform>().position = finalPosition;

        anim.SetBool("movement", false);
        vectorAux = new Vector3(-GetComponent<Transform>().localScale.x, GetComponent<Transform>().localScale.y, GetComponent<Transform>().localScale.z);
        if(GetComponent<Transform>().localScale.x > 0 && hasToFlip || GetComponent<Transform>().localScale.x < 0 && !hasToFlip) GetComponent<Transform>().localScale = vectorAux;
    }

    private void DetectingMotionPlatform()
    {
        RaycastHit2D[] results = new RaycastHit2D[16];
        int count = Physics2D.CapsuleCast(GetComponent<Transform>().position, GetComponent<CapsuleCollider2D>().size * 1.05f, CapsuleDirection2D.Vertical, 0, Vector2.zero, motionPlatformContactFilter, results);
        
        if(count > 0)
        {
            int j = -1;
            for(int i = 0; i < count; i++)
            {
                if(results[i].normal.y > 0.65f)
                    j = i;
            }
            if(j != -1)
            {
                onMotionPlatform = results[j].collider.gameObject.GetComponent<Transform>();
                GetComponent<Transform>().parent = onMotionPlatform;
            }
        }
        else {
            GetComponent<Transform>().parent = null;
            onMotionPlatform = null;
        }
    }

    private void DetectingEnemies()
    {
        if(!invulnerabity)
        {
            RaycastHit2D[] results = new RaycastHit2D[16];
            List<Vector2> normals = new List<Vector2>();
            int count = Physics2D.CapsuleCast(GetComponent<Transform>().position, GetComponent<CapsuleCollider2D>().size * 1.05f, CapsuleDirection2D.Vertical, 0, Vector2.zero, enemyContactFilter, results);
            
            if(count > 0)
            {
                GameObject firstEnemy = results[0].collider.gameObject;
                for(int i = 0; i < count; i++)
                {
                    if(results[i].collider.gameObject.Equals(firstEnemy))
                    {
                        normals.Add(results[i].normal);
                    }
                }

                EnemyBehaviour enemy = firstEnemy.GetComponent<EnemyBehaviour>();
                bool param = ChangePatience(-enemy.GetDamage() / 100);
                enemy.CollideWithPlayer();

                inputActivated = false;
                Invulnerable(true);
                if(param) KnockBack(normals);
            }
        }
    }

    public bool ChangePatience(float increaseValue)
    {
        bool aux = stats.ChangePatience(increaseValue); 
        if(!aux)
            Death();
        return aux;
    }
    /*
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
    }*/

    private void OnParticleCollision(GameObject other)
    {
        if(other.tag.Equals("Puke"))
        {
            List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();
            int numCollisionEvents = ParticlePhysicsExtensions.GetCollisionEvents(other.GetComponent<ParticleSystem>(), gameObject, collisionEvents);

            int i = 0;
            List<Vector2> contactNormals = new List<Vector2>();

            while (i < numCollisionEvents)
            {
                contactNormals.Add(new Vector2(- Mathf.Sign(collisionEvents[i].normal.x) * 1, 1));
                i++;
            }

            bool param = ChangePatience(-other.GetComponent<Transform>().parent.GetComponent<GeiserBehaviour>().GetDamage() / 100);

            Invulnerable(true);
            if(param) KnockBack(contactNormals);
        }
    }

    private void KnockBack(List<Vector2> contactNormals)
    {
        knockback = true;

        float xDirection = 0.0f;
        float yDirection = 0.0f;

        foreach (Vector2 normal in contactNormals)
        {
            xDirection += normal.x;
            yDirection += normal.y;
        }
        knockbackDirection = new Vector2(xDirection / contactNormals.Count, yDirection / contactNormals.Count).normalized;

        if (Mathf.Sign(xDirection) == Mathf.Sign(GetComponent<Transform>().localScale.x)) anim.SetTrigger("knockbackTrasero");
        else anim.SetTrigger("knockbackFrontal");

        StartCoroutine(ReduceKnockback(0.5f));
        if(invulnerableCoroutine != null) StopCoroutine(invulnerableCoroutine);
        invulnerableCoroutine = StartCoroutine(InvulnerabilityTime(2f, 0.1f));
    }

    public void Invulnerable(bool param)
    {
        if(param)
        {
            invulnerabity = true;
            gameObject.layer = LayerMask.NameToLayer("UntargetedPlayer");
            //contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer) & ~(1 << LayerMask.NameToLayer("Enemy")));
        }
        else
        {
            invulnerabity = false;
            gameObject.layer = LayerMask.NameToLayer("Player");
            //contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer) | (1 << LayerMask.NameToLayer("Enemy")));
        }
    }

    IEnumerator InvulnerabilityTime(float time, float blinkTime)
    {
        float elapsedBlinkTime = 0.0f;
        float elapsedTotalTime = 0.0f;
        Invulnerable(true);
        
        while (elapsedTotalTime < time)
        {
            elapsedTotalTime += Time.deltaTime;
            elapsedBlinkTime += Time.deltaTime;
            if (elapsedBlinkTime >= blinkTime)
            {
                if(!knockback) Blink(blink);
                elapsedBlinkTime = 0.0f;
            }
            yield return null;
        }
        if(blink) Blink(blink);
        Invulnerable(false);
    }

    public IEnumerator InvulnerableInTime(float time)
    {
        Invulnerable(true);
        yield return new WaitForSeconds(time);
        Invulnerable(false);
    }

    private void Blink(bool param)
    {
        for (int i = 0; i < bodyParts.Length; i++)
        {
            bodyParts[i].enabled = param;
        }
        blink = !param;
    }

    public IEnumerator ChangePlayer()
    {
        inputActivated = false;
        while(!controller.GetGrounded())
        {
            yield return null;
        }
        
        GetComponent<PlayerStatsController>().ChangeBladderTiredness(-1);

        anim.SetTrigger("ChangePlayer");
        yield return new WaitForSeconds(1.0f);
        GM.BeginChangePlayer();
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

    public void SlowDown(float slowDownFactor, float time)
    {
        if (time == 0)
        {
            maxSpeed *= slowDownFactor;
            GetComponent<CharacterController2D>().SetJumpForce(initialJumpForce * slowDownFactor * 5);
        }
        else
        {
            StartCoroutine(SlowDownForTime(slowDownFactor, time));
        }
    }

    public void SpeedToOriginal()
    {
        maxSpeed = initialMaxSpeed;
        GetComponent<CharacterController2D>().SetJumpForce(initialJumpForce);
    }

    IEnumerator SlowDownForTime(float slowDownFactor, float time)
    {
        maxSpeed *= slowDownFactor;
        GetComponent<CharacterController2D>().SetJumpForce(initialJumpForce * slowDownFactor * 5);
        yield return new WaitForSeconds(time);
        SpeedToOriginal();
    }

    public bool GetGrounded()
    {
        return controller.GetGrounded();
    }

    public void DancingMovement(KeyCode key)
    {
        switch(key)
        {
            case KeyCode.W:
                anim.SetTrigger("W");
                break;
            case KeyCode.A:
                anim.SetTrigger("A");
                break;
            case KeyCode.S:
                anim.SetTrigger("S");
                break;
            case KeyCode.D:
                anim.SetTrigger("D");
                break;
            case KeyCode.UpArrow:
                anim.SetTrigger("Up");
                break;
            case KeyCode.LeftArrow:
                anim.SetTrigger("Left");
                break;
            case KeyCode.DownArrow:
                anim.SetTrigger("Down");
                break;
            case KeyCode.RightArrow:
                anim.SetTrigger("Right");
                break;
        }
    }

    public void SetGlass(DrinkingTestGlassBehaviour param)
	{
		glass = param;
		glass.SetRightHandBone(rightHandBone);
	}

	private void AimGlass()
	{
		glass.SetRightHandBone(rightHandBone);
		glass.GlassInHand(sortingLayer);
	}

	private void LeaveGlass()
	{
		glass.GlassOverBar();
	}

    public void PlayDrinking(bool param)
    {
        anim.SetBool("drinking", param);
        anim.ResetTrigger("aimGlass");
    }

    public void PlayAimGlass()
    {
        anim.SetTrigger("aimGlass");
        anim.ResetTrigger("leaveGlass");
    }

    public void PlayLeaveGlass()
    {
        anim.SetTrigger("leaveGlass");
    }
    public void IncreaseFame()
    {
        stats.IncreaseFame();
    }

    public void Death()
    {
        anim.SetTrigger("Death");
    }

    private void EndDeath()
    {
        //StartCoroutine(GM.DeathTransition(1f));
    }

    public DoorBehaviour GetLastDoor()
    {
        return lastDoor;
    }

    public void SetLastDoor(DoorBehaviour param)
    {
        lastDoor = param;
    }

    public ChamberManager GetCurrentChamber()
    {
        return currentChamber;
    }

    public void SetCurrentChamber(ChamberManager param)
    {
        currentChamber = param;
    }

    public void StopPhoneRinging()
    {
        GM.GetComponent<InitialCutscene>().StopPhoneRinging();
    }
}
