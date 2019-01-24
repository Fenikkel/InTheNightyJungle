using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularMovement : MotionPlatform {

    [SerializeField]
    Transform rotationCenter;

    private float rotationRadius = 2f;

    public float initialAngle;

    float posX = 0f;
    float posY = 0f;
    float angle = 0f;
	
    void Start()
    {
        rotationRadius = (GetComponent<Transform>().position - rotationCenter.position).magnitude;
        angle = initialAngle;
    }

	// Update is called once per frame
	void Update () {
        angle = angle + Time.deltaTime * speed;

        posX = rotationCenter.position.x + Mathf.Cos(Mathf.Deg2Rad * angle) * rotationRadius;
        posY = rotationCenter.position.y + Mathf.Sin(Mathf.Deg2Rad * angle) * rotationRadius;

        distance = new Vector2(posX, posY) - ((Vector2)transform.position); //Esto lo necesita al jugador para cuando está en contacto con una plataforma móvil

        transform.position = new Vector2(posX, posY);

        if(angle >= 360f)
        {
            angle = 0f;
        }

    }
}
