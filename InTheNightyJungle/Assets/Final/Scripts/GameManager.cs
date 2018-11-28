﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public GameObject blackScreen;
    public GameObject mainCamera;
    private GameObject player;

    public GameObject BrendaLevels;
    public GameObject CindyLevels;
    public GameObject Brenda;
    public GameObject Cindy;

    private bool cindyEnabled;
    
    // Use this for initialization
	void Start () {
        blackScreen.GetComponent<Image>().enabled = true;
        StartCoroutine(FadeOut(1f, false));

        cindyEnabled = true;

        EnableCindy(cindyEnabled);
	}
	
	// Update is called once per frame
	void Update () {

        if(Input.GetKeyDown(KeyCode.I))
        {
            EnableCindy(!cindyEnabled);
        }
	}

    public void EnableCindy(bool param)
    {
        CindyLevels.SetActive(param);
        BrendaLevels.SetActive(!param);

        Cindy.SetActive(param);
        Brenda.SetActive(!param);

        player = param ? Cindy : Brenda;
        
        mainCamera.GetComponent<CameraBehaviour>().SetTarget(player);

        cindyEnabled = param;
    }

    IEnumerator FadeIn(float time, bool transition, DoorBehaviour door)
    {
        Color c = blackScreen.GetComponent<Image>().color;
        Color initialColor = c;
        Color finalColor = new Color(0, 0, 0, 1);
        float elapsedTime = 0.0f;
        while(elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            c = Color.Lerp(initialColor, finalColor, elapsedTime/time);
            blackScreen.GetComponent<Image>().color = c;
            yield return null;
        }
        blackScreen.GetComponent<Image>().color = finalColor;
        if(transition)
        {
            player.GetComponent<PlayerPlatformController>().SetPosition(door.nextDoor.playerPosition.position);
            mainCamera.GetComponent<CameraBehaviour>().SetPosition(door.nextDoor.cameraPosition.position);
            yield return new WaitForSeconds(0.1f);
            StartCoroutine(FadeOut(time, transition));
        }

    }

    IEnumerator FadeOut(float time, bool transition)
    {
        Color c = blackScreen.GetComponent<Image>().color;
        Color initialColor = c;
        Color finalColor = new Color(0, 0, 0, 0);
        float elapsedTime = 0.0f;
        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            c = Color.Lerp(initialColor, finalColor, elapsedTime/time);
            blackScreen.GetComponent<Image>().color = c;
            yield return null;
        }
        blackScreen.GetComponent<Image>().color = finalColor;
        if(transition)
        {
            player.GetComponent<PlayerPlatformController>().SetInputActivated(true);
            mainCamera.GetComponent<CameraBehaviour>().SetFollowTarget(true);
        }
    }

    public void ChangingChamber(DoorBehaviour door)
    {
        if(player.GetComponent<PlayerPlatformController>().GetInputActivated())
        {
            player.GetComponent<PlayerPlatformController>().SetInputActivated(false);
            mainCamera.GetComponent<CameraBehaviour>().SetFollowTarget(false);
            //.GetComponent<CameraBehaviour>().SetBoundaries(door.nextChamber);
            if (door.doorType % 2 == 0) //Es front door o back door
            {
                StartCoroutine(FadeIn(0.5f, true, door));
            }
            else
            {
                player.GetComponent<PlayerPlatformController>().MoveToLeftRightChamber(door);
                mainCamera.GetComponent<CameraBehaviour>().MoveToLeftRightChamber(door);
            }
        }
    }
}
