using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularMovement : MotionPlatform {

    [SerializeField]
    Transform rotationCenter;

    [SerializeField]
    float rotationRadius = 2f;

    float posX = 0f;
    float posY = 0f;
    float angle = 0f;
	
	// Update is called once per frame
	void FixedUpdate () {
        posX = rotationCenter.position.x + Mathf.Cos(angle) * rotationRadius;
        posY = rotationCenter.position.y + Mathf.Sin(angle) * rotationRadius;

        distance = new Vector2(posX, posY) - ((Vector2)transform.position); //Esto lo necesita al jugador para cuando está en contacto con una plataforma móvil

        transform.position = new Vector2(posX, posY);
        angle = angle + Time.deltaTime * speed;

        if(angle >= 360f)
        {
            angle = 0f;
        }

    }
}
