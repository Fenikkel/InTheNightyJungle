using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinealMotionPlatform : MonoBehaviour {

	public Transform target;

    private Vector2 initialPosition;
    private Vector2 finalPosition;
    
	public float movingTime;
	public float stoppingTime;

	// Use this for initialization
	void Start()
	{
		initialPosition = transform.position;
		finalPosition = target.position;
	}

	void OnEnable () {
		
        if(initialPosition != Vector2.zero) transform.position = initialPosition;
		if(finalPosition != Vector2.zero) target.position = finalPosition;
		StartCoroutine(Move());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private IEnumerator Move()
    {
        float elapsedTime = 0.0f;

		initialPosition = transform.position;
		finalPosition = target.position;

        while(elapsedTime < movingTime)
        {
            elapsedTime += Time.deltaTime;
            transform.position = Vector2.Lerp(initialPosition, finalPosition, elapsedTime / movingTime);
            yield return null;
        }

        transform.position = finalPosition;
		target.position = initialPosition;

        yield return new WaitForSeconds(stoppingTime);

        StartCoroutine(Move());
    }
}

