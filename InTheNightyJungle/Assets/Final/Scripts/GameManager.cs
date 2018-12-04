﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public GeneralUIController UIController;
    public GameObject blackScreen;
    public GameObject mainCamera;
    private GameObject player;

    public GameObject BrendaLevels;
    public GameObject CindyLevels;
    public GameObject Brenda;
    public GameObject Cindy;

    private bool cindyEnabled;
    private bool canChangePlayer;
    
    // Use this for initialization
	void Start () {

        canChangePlayer = true;

        blackScreen.GetComponent<Image>().enabled = true;

        cindyEnabled = (Random.value > 0.5f) ? true : false;

        StartCoroutine(FadeOut(1f, false));

        ChangePlayer();
	}
	
	// Update is called once per frame
	void Update () {
        /*if(Input.GetKeyDown(KeyCode.I))
        {
            ChangePlayer();
        }*/
	}

    public void BeginChangePlayer()
    {
        StartCoroutine(Transition());
    }
    
    IEnumerator Transition()
    {
        Color c = blackScreen.GetComponent<Image>().color;
        Color initialColor = c;
        Color finalColor = new Color(0, 0, 0, 1);
        float elapsedTime = 0.0f;
        while (elapsedTime < 0.5f)
        {
            elapsedTime += Time.deltaTime;
            c = Color.Lerp(initialColor, finalColor, elapsedTime / 0.5f);
            blackScreen.GetComponent<Image>().color = c;
            yield return null;
        }
        blackScreen.GetComponent<Image>().color = finalColor;
        if (cindyEnabled)
        {
            if (Cindy.GetComponent<PlayerPlatformController>().GetLastDoor() != Vector3.zero)
            {
                Cindy.transform.position = Cindy.GetComponent<PlayerPlatformController>().GetLastDoor() + new Vector3(0.01f, 0.01f, 0.01f);
            }
        }
        else
        {
            if (Brenda.GetComponent<PlayerPlatformController>().GetLastDoor() != Vector3.zero)
            {
                Brenda.transform.position = Brenda.GetComponent<PlayerPlatformController>().GetLastDoor() + new Vector3(0.01f, 0.01f, 0.01f);
            }
        }
        ChangePlayer();
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(FadeOut(0.5f, true));
    }
    
    public void ChangePlayer()
    {
        if(canChangePlayer)
        {
            cindyEnabled = !cindyEnabled;

            CindyLevels.SetActive(cindyEnabled);
            BrendaLevels.SetActive(!cindyEnabled);

            Cindy.SetActive(cindyEnabled);
            Brenda.SetActive(!cindyEnabled);

            UIController.Initialize(cindyEnabled);

            player = cindyEnabled ? Cindy : Brenda;
            player.GetComponent<PlayerPlatformController>().RestartPlayer();
            StartCoroutine(player.GetComponent<PlayerPlatformController>().InvulnerableInTime(1.0f));
            
            mainCamera.GetComponent<CameraBehaviour>().SetTarget(player);
        }
    }

    public void PlayerDone()
    {
        BeginChangePlayer();
        canChangePlayer = false;
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

    public IEnumerator DeathTransition(float time)
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
        player.GetComponent<Animator>().SetTrigger("EndDeath");
        player.GetComponent<Animator>().ResetTrigger("Death");
        player.GetComponent<PlayerPlatformController>().SetPosition(player.GetComponent<PlayerPlatformController>().GetCheckPoint().position);
        player.GetComponent<PlayerPlatformController>().ChangePatience(1);
        player.GetComponent<PlayerPlatformController>().Invulnerable(false);
        yield return new WaitForSeconds(0.1f);

        initialColor = finalColor;
        finalColor = new Color(0,0,0,0);
        elapsedTime = 0.0f;
        while(elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            c = Color.Lerp(initialColor, finalColor, elapsedTime/time);
            blackScreen.GetComponent<Image>().color = c;
            yield return null;
        }
        blackScreen.GetComponent<Image>().color = finalColor;
        player.GetComponent<PlayerPlatformController>().SetInputActivated(true);
    }
}
