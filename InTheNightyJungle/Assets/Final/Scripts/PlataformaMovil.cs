using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlataformaMovil : MotionPlatform {

    public Transform target;

    private Vector3 start;
    private Vector3 end;

	// Use this for initialization
	void Start () {
		if(target != null)
        {
            start = transform.position;
            end = target.position;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void FixedUpdate()
    {
        if(target != null)
        {
            float fixedSpeed = speed * Time.deltaTime;
            Vector2 oldPosition = transform.position;
            transform.position = Vector3.MoveTowards(transform.position, target.position, fixedSpeed);
            
            distance = ((Vector2)transform.position) - oldPosition; //Esto lo necesita al jugador para cuando está en contacto con una plataforma móvil
        }

        if(transform.position == target.position)
        {
            target.position = (target.position == start) ? end : start;
        }
    }
}
