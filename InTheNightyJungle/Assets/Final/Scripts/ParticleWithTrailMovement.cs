using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleWithTrailMovement : MonoBehaviour {

	public Vector2 velocity;
	public float topLimit;
	public float bottomLimit;
	public float leftLimit;
	public float rightLimit;

	// Use this for initialization
	void Start () {
		
		StartCoroutine(Movement());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private IEnumerator Movement()
	{
		Vector2 finalPosition = new Vector2(Random.Range(leftLimit, rightLimit), Random.Range(bottomLimit, topLimit));
		float smoothTime = (finalPosition.magnitude - transform.position.magnitude) / velocity.magnitude;

		while(!transform.position.Equals(finalPosition))
		{
			transform.position = Vector2.SmoothDamp(transform.position, finalPosition, ref velocity, smoothTime);
			yield return null;
		}
	}
}
