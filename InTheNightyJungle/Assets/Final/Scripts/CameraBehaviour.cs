using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour {

    public GameObject player;
    public Transform leftBounds; //donde chocara la camara
    public Transform rightBounds;

    public Transform upperBounds; //donde chocara la camara
    public Transform downerBounds;

    public float smoothDampTime = 0.15f;
    private Vector3 smoothDampVelocity = Vector3.zero;

    private float camWidth, camHeight, levelMinX, levelMaxX;
    private float levelMinY, levelMaxY;

    private bool followTarget;

    private float y;
    private float targetY;

    private Transform playerPosition; 


    void Start () {
        playerPosition = player.transform;
        RestartCamera();
    }

    public void RestartCamera()
    {
        followTarget = true;

        camHeight = Camera.main.orthographicSize * 2;
        camWidth = camHeight * Camera.main.aspect;

        float leftBoundsWidth = leftBounds.GetComponentInChildren<SpriteRenderer>().bounds.size.x / 2; //los atributos siempre son hijos del objeto?
        float rightBoundsWidth = rightBounds.GetComponentInChildren<SpriteRenderer>().bounds.size.x / 2; //dividido por dos porque el punto de anclaje esta al centro

        float upperBoundsWidth = upperBounds.GetComponentInChildren<SpriteRenderer>().bounds.size.y / 2; //los atributos siempre son hijos del objeto?
        float downerBoundsWidth = downerBounds.GetComponentInChildren<SpriteRenderer>().bounds.size.y / 2;


        levelMinX = leftBounds.position.x + leftBoundsWidth + (camWidth / 2); //to specify our minimum left most position
        levelMaxX = rightBounds.position.x - rightBoundsWidth - (camWidth / 2); //to specify our maximum right most position

        levelMinY = downerBounds.position.y + downerBoundsWidth + (camHeight / 2); //to specify our minimum left most position
        levelMaxY = upperBounds.position.y - upperBoundsWidth - (camHeight / 2); //to specify our maximum right most position

    }
    /*
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Boundary")
        {

        }
    }*/

    void Update () {

        if (followTarget && playerPosition) //if target has been instanciated
        {
            float targetX = Mathf.Max(levelMinX, Mathf.Min(levelMaxX, playerPosition.position.x)); //con esto, el personaje se podra desmarcar del centro de la pantalla? Esto sirve para que la camara nunca supere levelMax y levlMin



            if (player.GetComponent<PlayerPlatformController>().GetGrounded() == true) {

                targetY = Mathf.Max(levelMinY, Mathf.Min(levelMaxY, playerPosition.position.y));//+camHeight/2//target.position.y //con esto, el personaje se podra desmarcar del centro de la pantalla? Esto sirve para que la camara nunca supere levelMax y levlMin
                y = Mathf.SmoothDamp(transform.position.y, targetY, ref smoothDampVelocity.y, smoothDampTime);

            }


            float x = Mathf.SmoothDamp(transform.position.x, targetX, ref smoothDampVelocity.x, smoothDampTime);



            transform.position = new Vector3(x, y, transform.position.z);
        }

	}

    public void SetFollowTarget(bool param)
    {
        followTarget = param;
    }

    public void SetPosition(Vector2 param)
    {
        GetComponent<Transform>().position = new Vector3 (param.x, param.y, GetComponent<Transform>().position.z);
        RestartCamera();
    }

    public void MoveToLeftRightChamber(DoorBehaviour door)
    {
        Vector3 finalPosition = new Vector3(door.nextDoor.cameraPosition.position.x, GetComponent<Transform>().position.y, GetComponent<Transform>().position.z);
        StartCoroutine(InterpolatePositionChangingChamber(0.5f, finalPosition));
    }

    IEnumerator InterpolatePositionChangingChamber(float time, Vector3 finalPosition)
    {
        float elapsedTime = 0.0f;
        Vector3 initialPosition = GetComponent<Transform>().position;
        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            GetComponent<Transform>().position = Vector3.Lerp(initialPosition, finalPosition, elapsedTime/time);
            yield return null;
        }
        GetComponent<Transform>().position = finalPosition;
        RestartCamera();
        SetFollowTarget(true);
    }

    public void SetBoundaries(int chamber)
    {
        //print(chamber);
        rightBounds = GameObject.Find("RightBoundary" + chamber).transform;
        leftBounds = GameObject.Find("LeftBoundary" + chamber).transform;
        upperBounds = GameObject.Find("UpperBoundary" + chamber).transform;
        downerBounds = GameObject.Find("DownerBoundary" + chamber).transform;
    }
}
