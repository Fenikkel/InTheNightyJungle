using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBehaviour : MonoBehaviour {

	private float launchSpeed, timeToReachPlayer, height;
	private Vector3 originPosition, targetPosition;

	private float elapsedTime;

	private bool launched;

	private float damage, slowDownTime, slowDownFactor;

	private AudioSource iceSound;

	// Use this for initialization
	void Start () {
		launched = false;
		elapsedTime = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
		if(launched)
		{
			ParabollicMovement(elapsedTime);
			elapsedTime += Time.deltaTime;
			if(elapsedTime > 10.0f) Destroy(gameObject);
		}
	}

	public void Launch(Vector3 targetPosition, float launchSpeed, float timeToReachPlayer, float damage, float slowDownTime, float slowDownFactor, AudioSource iceSound)
	{
		this.targetPosition = targetPosition;
		this.launchSpeed = launchSpeed;
		this.timeToReachPlayer = timeToReachPlayer;
		this.damage = damage;
		this.slowDownFactor = slowDownFactor;
		this.slowDownTime = slowDownTime;
		this.iceSound = iceSound;

		originPosition = GetComponent<Transform>().position;

		CalculatingHigh();

		launched = true;
	}

	private void CalculatingHigh()
	{
		float speedX = (targetPosition.x - GetComponent<Transform>().position.x) / timeToReachPlayer;
		float speedY = Mathf.Sqrt(launchSpeed * launchSpeed - speedX * speedX);

		float timeToReachHighestPoint = -speedY/Physics2D.gravity.y;
		height = speedY * timeToReachHighestPoint + (1/2) * Physics2D.gravity.y * Mathf.Pow(timeToReachHighestPoint, 2);
	}

	private void ParabollicMovement(float time)
	{
		GetComponent<Transform>().position = Parabola(originPosition, targetPosition, height, time);
	}

	private Vector2 Parabola(Vector2 start, Vector2 end, float height, float t)
    {
        float f = -4 * height * t * t + 4 * height * t;

        Vector2 mid = Vector2.Lerp(start, end, t);

        return new Vector2(mid.x, f + Mathf.Lerp(start.y, end.y, t));
    }

	public void CollideWithPlayer()
	{
		iceSound.Play();
		Destroy(gameObject);
	}

	public float GetDamage()
	{
		return damage;
	}

	public float GetSlowDownTime()
	{
		return slowDownTime;
	}

	public float GetSlowDownFactor()
	{
		return slowDownFactor;
	}
}
