using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour {

    public Transform target; //our player
    public Transform leftBounds; //donde chocara la camara
    public Transform rightBounds;

    public Transform upperBounds; //donde chocara la camara
    public Transform downerBounds;

    public float smoothDampTime = 0.15f;
    private Vector3 smoothDampVelocity = Vector3.zero;

    private float camWidth, camHeight, levelMinX, levelMaxX;
    private float levelMinY, levelMaxY;
    
    
    
    // Use this for initialization
	void Start () {

        camHeight = Camera.main.orthographicSize * 2;
        camWidth = camHeight * Camera.main.aspect;

        float leftBoundsWidth = leftBounds.GetComponentInChildren<SpriteRenderer>().bounds.size.x / 2; //los atributos siempre son hijos del objeto?
        float rightBoundsWidth = rightBounds.GetComponentInChildren<SpriteRenderer>().bounds.size.x / 2; //dividido por dos poque el punto de anclaje esta al centro

        float upperBoundsWidth = upperBounds.GetComponentInChildren<SpriteRenderer>().bounds.size.y / 2; //los atributos siempre son hijos del objeto?
        float downerBoundsWidth = downerBounds.GetComponentInChildren<SpriteRenderer>().bounds.size.y / 2;


        levelMinX = leftBounds.position.x + leftBoundsWidth + (camWidth / 2); //to specify our minimum left most position
        levelMaxX = rightBounds.position.x - rightBoundsWidth - (camWidth / 2); //to specify our maximum right most position

        levelMinY = downerBounds.position.y + downerBoundsWidth + (camHeight / 2); //to specify our minimum left most position
        levelMaxY = upperBounds.position.y - upperBoundsWidth - (camHeight / 2); //to specify our maximum right most position
    }

    public void ReStard()
    {
        camHeight = Camera.main.orthographicSize * 2;
        camWidth = camHeight * Camera.main.aspect;

        float leftBoundsWidth = leftBounds.GetComponentInChildren<SpriteRenderer>().bounds.size.x / 2; //los atributos siempre son hijos del objeto?
        float rightBoundsWidth = rightBounds.GetComponentInChildren<SpriteRenderer>().bounds.size.x / 2; //dividido por dos poque el punto de anclaje esta al centro

        float upperBoundsWidth = upperBounds.GetComponentInChildren<SpriteRenderer>().bounds.size.y / 2; //los atributos siempre son hijos del objeto?
        float downerBoundsWidth = downerBounds.GetComponentInChildren<SpriteRenderer>().bounds.size.y / 2;


        levelMinX = leftBounds.position.x + leftBoundsWidth + (camWidth / 2); //to specify our minimum left most position
        levelMaxX = rightBounds.position.x - rightBoundsWidth - (camWidth / 2); //to specify our maximum right most position

        levelMinY = downerBounds.position.y + downerBoundsWidth + (camHeight / 2); //to specify our minimum left most position
        levelMaxY = upperBounds.position.y - upperBoundsWidth - (camHeight / 2); //to specify our maximum right most position

    }
	
	// Update is called once per frame
	void Update () {

        if (target) //if target has been instanciated
        {
            float targetX = Mathf.Max(levelMinX, Mathf.Min(levelMaxX, target.position.x)); //con esto, el personaje se podra desmarcar del centro de la pantalla? Esto sirve para que la camara nunca supere levelMax y levlMin

            float targetY = Mathf.Max(levelMinY, Mathf.Min(levelMaxY, target.position.y)); //con esto, el personaje se podra desmarcar del centro de la pantalla? Esto sirve para que la camara nunca supere levelMax y levlMin


            float x = Mathf.SmoothDamp(transform.position.x, targetX, ref smoothDampVelocity.x, smoothDampTime);

            float y = Mathf.SmoothDamp(transform.position.y, targetY, ref smoothDampVelocity.y, smoothDampTime);


            transform.position = new Vector3(x, y, transform.position.z);
        }

	}
}
