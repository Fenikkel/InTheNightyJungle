using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Depurando : MonoBehaviour {

	public GameObject coso;
	public GameObject cosoA;
	public GameObject cosoB;
	public Vector2 velocidadCoso;

	private float height;

	private float elapsedTime;
	private bool cosoActivado;

	// Use this for initialization
	void Start () {
		cosoActivado = false;
		CalculatingHigh();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Depurar"))
		{
			cosoActivado = true;
		}
		if(cosoActivado)
		{
			ParabollicMovement(elapsedTime);
			elapsedTime += Time.deltaTime;
		}
	}

	private void CalculatingHigh()
	{
		float timeToReachHighestPoint = -velocidadCoso.y/Physics2D.gravity.y;
		height = velocidadCoso.y * timeToReachHighestPoint + (1/2) * Physics2D.gravity.y * Mathf.Pow(timeToReachHighestPoint, 2);
	}

	private void ParabollicMovement(float time)
	{
		coso.transform.position = Parabola(cosoA.transform.position, cosoB.transform.position, height, time);
	}

	public Vector2 Parabola(Vector2 start, Vector2 end, float height, float t)
    {
        float f = -4 * height * t * t + 4 * height * t;

        Vector2 mid = Vector2.Lerp(start, end, t);

        return new Vector2(mid.x, f + Mathf.Lerp(start.y, end.y, t));
    }
}
