﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrinkingTestManager : MonoBehaviour {

	public DrinkingTestUIController UI;
    public DrinkingChallengerBehaviour challenger;
    private PlayerPlatformController player;
    public BarmanBehaviour barman;
    private CameraBehaviour camera;

    public Transform insidePlayerPosition;
    public Transform insideCameraPosition;
    public Transform outsidePlayerPosition;
    public Transform outsideCameraPosition;
    public float cameraSize;
    public DrinkingShadow[] foregroundSilhouettes;

    private bool testStarted;
    private int victory;

    public int totalNumDrinks;
    private int playerCurrentNumDrinks;
    private int challengerCurrentNumDrinks;

    private bool playerDrinking;
    private bool challengerDrinking;
    private bool playerGlassOnTable;
    private bool playerBarmanTurn; //true significa que es el turno del jugador y false, el del camarero
    private bool challengerGlassOnTable;
    private bool challengerBarmanTurn; //true significa que es el turno del oponente y false, el del camarero
    private bool busyBarman;

    public bool playerSide; //true significa left, false significa right

    public DrinkingTestGlassBehaviour leftSideGlass;
    public DrinkingTestGlassBehaviour rightSideGlass;
    public Transform leftSideGlassPlaceOnBar;
    public Transform rightSideGlassPlaceOnBar;
    public Vector2 positionInHand;

    public float minTimeToDrink;
    private float elapsedTimeWithoutDrinking;

    public float challengerTimeToIncreaseDrink;
    private float elapsedTimeWithoutIncreasing;

	// Use this for initialization
	void Start () {
		testStarted = false;
	}

    public void StartTest(PlayerPlatformController param0, CameraBehaviour param1)
    {
        player = param0;
        camera = param1;

        RestartTest();

        StartCoroutine(Introduction(0.5f, 0.5f, 0.5f, 6.0f));
    }

    public void RestartTest()
    {
        victory = -1;
        playerCurrentNumDrinks = challengerCurrentNumDrinks = 0;

        playerDrinking = challengerDrinking = false;
        playerGlassOnTable = challengerGlassOnTable = true;
        playerBarmanTurn = challengerBarmanTurn = true;
        busyBarman = false;

        leftSideGlass.InitializeGlass(leftSideGlassPlaceOnBar, positionInHand);
        rightSideGlass.InitializeGlass(rightSideGlassPlaceOnBar, positionInHand);

        if(playerSide)
        {
            //player.SetGlass(leftSideGlass);
            challenger.SetGlass(rightSideGlass);
        }
        else
        {
            //player.SetGlass(rightSideGlass);
            challenger.SetGlass(leftSideGlass);
        }

        elapsedTimeWithoutDrinking = elapsedTimeWithoutIncreasing = 0.0f;
    }

    private IEnumerator Introduction(float time1, float time2, float time3, float time4)
    {
        StartCoroutine(camera.MoveSizeTo(insideCameraPosition.position, camera.GetSize(), time1));
        yield return new WaitForSeconds(time1);

        StartCoroutine(player.MoveTo(insidePlayerPosition.position, false, time2));
        yield return new WaitForSeconds(time2);

        StartCoroutine(camera.MoveSizeTo(insideCameraPosition.position, cameraSize, time3));
        FadeOutCrowd(time3);
        yield return new WaitForSeconds(time3);

        UI.InitializeUI();

        StartCoroutine(UI.ShowIntroductionTexts(time4));
        yield return new WaitForSeconds(time4);
        
        testStarted = true;
    }

    private IEnumerator Ending(float time1, float time2, float time3)
    {
        StartCoroutine(camera.MoveSizeTo(insideCameraPosition.position, camera.GetInitialSize(), time1));
        FadeInCrowd(time1);
        yield return new WaitForSeconds(time1);

        StartCoroutine(player.MoveTo(outsidePlayerPosition.position, true, time2));
        yield return new WaitForSeconds(time2);

        StartCoroutine(camera.MoveSizeTo(outsideCameraPosition.position, camera.GetSize(), time3));
        yield return new WaitForSeconds(time3);

        player.SetInputActivated(true);
        camera.SetFollowTarget(true);
        UI.GetComponent<GeneralUIController>().ChangeMode(UILayer.Stats);
    }

    private void FadeOutCrowd(float time)
    {
        for(int i = 0; i < foregroundSilhouettes.Length; i++)
        {
            StartCoroutine(foregroundSilhouettes[i].FadeOut(time));
        }
    }

    private void FadeInCrowd(float time)
    {
        for(int i = 0; i < foregroundSilhouettes.Length; i++)
        {
            StartCoroutine(foregroundSilhouettes[i].FadeIn(time));
        }
    }
	
	// Update is called once per frame
	void Update () {
		if(testStarted)
        {
            if(playerDrinking)
            {
                if(Input.GetKeyDown(KeyCode.Space))
                {
                    elapsedTimeWithoutDrinking = 0.0f;
                    if(!player.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("drinking"))
                    {
                        //player.PlayDrinking(true);
                    }
                    else
                    {
                        if(UI.IncreaseDrinkingBar(playerSide))
                        {
                            playerCurrentNumDrinks++;
                            playerDrinking = false;
                        }
                    }
                }
                else
                {
                    elapsedTimeWithoutDrinking += Time.deltaTime;
                    if(elapsedTimeWithoutDrinking >= minTimeToDrink)
                    {
                        //player.PlayDrinking(false);
                    }
                }
            }
            else
            {
                if(playerGlassOnTable) //El vaso está sobre la mesa
                {
                    if(playerBarmanTurn) //Es el turno del jugador, deberá ejecutar la animación de coger el vaso y se deberá comprobar cuando lo tiene ya que entonces se puede volver a beber
                    {
                        if(player.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("preIdle"))
                        {
                            //player.PlayAimGlass();
                        }
                        else if(player.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("drinkingIdle"))
                        {
                            playerDrinking = true;
                            playerGlassOnTable = false;
                        }
                    }
                    else //Es el turno del camarero, por lo tanto deberá recoger el vaso y en ese proceso estará ocupado
                    {
                        if(!busyBarman) //Puede que esté ya ocupado y por tanto, no podrá hacerlo
                        {
                            busyBarman = true;
                            barman.PlayChangeGlass(playerSide);
                            playerGlassOnTable = false;
                        }
                    }
                }
                else //El vaso no está sobre la mesa
                {
                    if(playerBarmanTurn) //Es el turno del jugador, lo que quiere decir que acaba de terminar de beber y tiene que dejar el vaso sobre la barra
                    {
                        if(player.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("drinkingIdle"))
                        {
                            //player.PlayLeaveGlass();
                        }
                        else if(player.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("preIdle"))
                        {
                            playerGlassOnTable = true;
                            playerBarmanTurn = false;
                        }
                    }
                    else //Es el turno del camerero, lo que quiere decir estamos mirando si ha terminado de sustituir los vasos
                    {
                        if(barman.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("preIdle"))
                        {
                            busyBarman = false;
                            playerGlassOnTable = true;
                            playerBarmanTurn = true;
                        }
                    }
                }
            }

            if(challengerDrinking)
            {
                challenger.PlayDrinking(true);
                elapsedTimeWithoutIncreasing += Time.deltaTime;
                if(elapsedTimeWithoutIncreasing >= challengerTimeToIncreaseDrink)
                {
                    elapsedTimeWithoutIncreasing = 0.0f;
                    if(UI.IncreaseDrinkingBar(!playerSide)) 
                    {
                        challengerCurrentNumDrinks++;
                        challengerDrinking = false;
                        challenger.PlayDrinking(false);
                    }
                }
            }
            else
            {
                if(challengerGlassOnTable) //El vaso está sobre la mesa
                {
                    if(challengerBarmanTurn) //Es el turno del jugador, deberá ejecutar la animación de coger el vaso y se deberá comprobar cuando lo tiene ya que entonces se puede volver a beber
                    {
                        if(challenger.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("preIdle"))
                        {
                            challenger.PlayAimGlass();
                        }
                        else if(challenger.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("drinkingIdle"))
                        {
                            challengerDrinking = true;
                            challengerGlassOnTable = false;
                        }
                    }
                    else //Es el turno del camarero, por lo tanto deberá recoger el vaso y en ese proceso estará ocupado
                    {
                        if(!busyBarman) //Puede que esté ya ocupado y por tanto, no podrá hacerlo
                        {
                            busyBarman = true;
                            barman.PlayChangeGlass(!playerSide);
                            challengerGlassOnTable = false;
                        }
                    }
                }
                else //El vaso no está sobre la mesa
                {
                    if(challengerBarmanTurn) //Es el turno del jugador, lo que quiere decir que acaba de terminar de beber y tiene que dejar el vaso sobre la barra
                    {
                        if(challenger.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("drinkingIdle"))
                        {
                            challenger.PlayLeaveGlass();
                        }
                        else if(challenger.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("preIdle"))
                        {
                            challengerGlassOnTable = true;
                            challengerBarmanTurn = false;
                        }
                    }
                    else //Es el turno del camerero, lo que quiere decir estamos mirando si ha terminado de sustituir los vasos
                    {
                        if(barman.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("preIdle"))
                        {
                            busyBarman = false;
                            challengerGlassOnTable = true;
                            challengerBarmanTurn = true;
                        }
                    }
                }
            }
        }
	}
}
