using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyShowBehaviour : MonoBehaviour {

    //public float interpolationTime;


	void Start () {
		
	}
	
	
	void Update () {
		
	}
    public void ShowIn()
    {
        transform.GetComponent<Animator>().Play("KeyFadeIn");

        //FALTA MOVERLO CON CODIGO
        /*
        Vector3 initialPosition = transform.GetComponent<Transform>().position;
        Vector3 finalPosition = transform.GetComponent<Transform>().position + new Vector3(0, 1, 0);

        float elapsedTime = 0.0f;
        while (elapsedTime < interpolationTime)
        {
            elapsedTime += Time.deltaTime;
            GetComponent<Transform>().position = Vector3.Lerp(initialPosition, finalPosition, elapsedTime / interpolationTime);
        }
        */
        
    }
    public void ShowOut()
    {
        transform.GetComponent<Animator>().Play("KeyFadeOut");
    }
}
