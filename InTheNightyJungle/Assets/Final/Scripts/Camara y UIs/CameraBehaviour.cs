﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour {

    private GameObject player;
    /*public Transform leftBounds; //donde chocara la camara
    public Transform rightBounds;

    public Transform upperBounds; //donde chocara la camara
    public Transform downerBounds;*/

    public float smoothDampTime = 0.15f;
    private Vector3 smoothDampVelocity = Vector3.zero;

    /*private float camWidth, camHeight, levelMinX, levelMaxX;
    private float levelMinY, levelMaxY;*/

    private float boundDistanceMaxX, boundDistanceMaxY;
    private float distanceToLeft, distanceToRight, distanceToTop, distanceToBottom;

    private bool followTarget;

    private float x, y, targetX, targetY;

    private Transform playerPosition;

    public float initialSize;


    void Start () {

        initialSize = Camera.main.orthographicSize;

        boundDistanceMaxY = GetComponent<Camera>().orthographicSize * 0.9f;
        boundDistanceMaxX = GetComponent<Camera>().orthographicSize * GetComponent<Camera>().aspect * 0.9f;
        
        followTarget = false;

        RestartCamera();
        SetFollowTarget(true);
    }

    public void RestartCamera()
    {
        boundDistanceMaxY = GetComponent<Camera>().orthographicSize * 0.9f;
        boundDistanceMaxX = GetComponent<Camera>().orthographicSize * GetComponent<Camera>().aspect * 0.9f;

        /*camHeight = Camera.main.orthographicSize * 2;
        camWidth = camHeight * Camera.main.aspect;

        float leftBoundsWidth = leftBounds.GetComponentInChildren<SpriteRenderer>().bounds.size.x / 2; //los atributos siempre son hijos del objeto?
        float rightBoundsWidth = rightBounds.GetComponentInChildren<SpriteRenderer>().bounds.size.x / 2; //dividido por dos porque el punto de anclaje esta al centro

        float upperBoundsWidth = upperBounds.GetComponentInChildren<SpriteRenderer>().bounds.size.y / 2; //los atributos siempre son hijos del objeto?
        float downerBoundsWidth = downerBounds.GetComponentInChildren<SpriteRenderer>().bounds.size.y / 2;


        levelMinX = leftBounds.position.x + leftBoundsWidth + (camWidth / 2); //to specify our minimum left most position
        levelMaxX = rightBounds.position.x - rightBoundsWidth - (camWidth / 2); //to specify our maximum right most position

        levelMinY = downerBounds.position.y + downerBoundsWidth + (camHeight / 2); //to specify our minimum left most position
        levelMaxY = upperBounds.position.y - upperBoundsWidth - (camHeight / 2); //to specify our maximum right most position*/

    }

    void Update () {

        if (followTarget && player) //if target has been instanciated
        {
            CheckingBoundaries();
            if (distanceToLeft > 0 && distanceToRight > 0) //Considerar hacer zoom (disminuir el size de la camara) en caso de que se produzca este caso
            {
                targetX = ((player.GetComponent<Transform>().position.x + distanceToLeft) + (player.GetComponent<Transform>().position.x - distanceToRight))/2;
            }
            else if(distanceToLeft > 0)
            {
                targetX = player.GetComponent<Transform>().position.x + distanceToLeft;
            }
            else if(distanceToRight > 0)
            {
                targetX = player.GetComponent<Transform>().position.x - distanceToRight;
            }
            else
            {
                targetX = player.GetComponent<Transform>().position.x;
            }

            //if (player.GetComponent<PlayerPlatformController>().GetGrounded() == true) {

                if (distanceToBottom > 0 && distanceToTop > 0)
                {
                    targetY = ((player.GetComponent<Transform>().position.y + distanceToBottom) + (player.GetComponent<Transform>().position.y - distanceToTop)) / 2;
                }
                else if (distanceToBottom > 0)
                {
                    targetY = player.GetComponent<Transform>().position.y + distanceToBottom;
                }
                else if (distanceToTop > 0)
                {
                    targetY = player.GetComponent<Transform>().position.y - distanceToTop;
                }
                else
                {
                    targetY = player.GetComponent<Transform>().position.y;//+camHeight/2//target.position.y //con esto, el personaje se podra desmarcar del centro de la pantalla? Esto sirve para que la camara nunca supere levelMax y levlMin
                }

                y = Mathf.SmoothDamp(transform.position.y, targetY, ref smoothDampVelocity.y, smoothDampTime);

            //}


            x = Mathf.SmoothDamp(transform.position.x, targetX, ref smoothDampVelocity.x, smoothDampTime);



            transform.position = new Vector3(x, y, transform.position.z);
        }

	}

    private void CheckingBoundaries()
    {
        RaycastHit2D hitUp = Physics2D.Raycast(player.GetComponent<Transform>().position, Vector2.up, Mathf.Infinity, (1 << LayerMask.NameToLayer("CameraBoundaries")));
        RaycastHit2D hitDown = Physics2D.Raycast(player.GetComponent<Transform>().position, Vector2.down, Mathf.Infinity, (1 << LayerMask.NameToLayer("CameraBoundaries")));
        RaycastHit2D hitLeft = Physics2D.Raycast(player.GetComponent<Transform>().position, Vector2.left, Mathf.Infinity, (1 << LayerMask.NameToLayer("CameraBoundaries")));
        RaycastHit2D hitRight = Physics2D.Raycast(player.GetComponent<Transform>().position, Vector2.right, Mathf.Infinity, (1 << LayerMask.NameToLayer("CameraBoundaries")));

        distanceToLeft = boundDistanceMaxX - hitLeft.distance;
        distanceToRight = boundDistanceMaxX - hitRight.distance;
        distanceToTop = boundDistanceMaxY - hitUp.distance;
        distanceToBottom = boundDistanceMaxY - hitDown.distance;

        /*
        if (hitUp.distance <= boundDistanceMaxY || hitDown.distance <= boundDistanceMaxY)
            collidingY= true;
        else
            collidingY = false;

        if (hitLeft.distance <= boundDistanceMaxX || hitRight.distance <= boundDistanceMaxX)
            collidingX = true;
        else
            collidingX = false;*/
    }

    public float GetInitialSize()
    {
        return initialSize;
    }

    public float GetSize()
    {
        return Camera.main.orthographicSize;
    }

    public void SetSize(float size)
    {
        Camera.main.orthographicSize = size;
        RestartCamera();
    }

    public void SetFollowTarget(bool param)
    {
        followTarget = param;
    }

    public void SetTarget(GameObject param)
    {
        followTarget = false;
        player = param;
        SetPosition(param.GetComponent<Transform>().position);
    }

    public void SetPosition(Vector2 param)
    {
        GetComponent<Transform>().position = new Vector3 (param.x, param.y, GetComponent<Transform>().position.z);
        RestartCamera();
        SetFollowTarget(true);
    }

    public void MoveToLeftRightChamber(DoorBehaviour door)
    {
        Vector3 finalPosition = new Vector3(door.nextDoor.cameraPosition.position.x, GetComponent<Transform>().position.y, GetComponent<Transform>().position.z);
        StartCoroutine(InterpolatePositionChangingChamber(finalPosition, door.nextDoor.chamber.GetCameraSize(), 0.5f));
    }

    private IEnumerator InterpolatePositionChangingChamber(Vector3 finalPosition, float finalSize, float time)
    {
        StartCoroutine(MoveSizeTo(finalPosition, finalSize, time));
        yield return new WaitForSeconds(time);

        RestartCamera();
        SetFollowTarget(true);
    }

    public IEnumerator MoveSizeTo(Vector3 finalPosition, float finalSize, float time)
    {
        float elapsedTime = 0.0f;
        Vector3 initialPosition = GetComponent<Transform>().position;
        finalPosition = new Vector3(finalPosition.x, finalPosition.y, initialPosition.z);
        float initialSize = GetComponent<Camera>().orthographicSize;

        while(elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            GetComponent<Transform>().position = Vector3.Lerp(initialPosition, finalPosition, elapsedTime / time);
            GetComponent<Camera>().orthographicSize = Mathf.Lerp(initialSize, finalSize, elapsedTime / time);
            yield return null;
        }
        GetComponent<Transform>().position = finalPosition;
        GetComponent<Camera>().orthographicSize = finalSize;

        RestartCamera();
    }

}

public static class CameraSizes {
    public const float conversationSize    = 2.624054f;
    public const float drinkingTestSize    = 1.5f;
    public const float dancingTestSize     = 1.5f;
    public const float dancingTestZoomSize = 1.3f;
    public const float checkpointSize      = 2f;
    public const float smallChamberSize    = 3.21849f;
    public const float mediumChamberSize   = 4f;
    public const float bigChamberSize      = 5f;
}
