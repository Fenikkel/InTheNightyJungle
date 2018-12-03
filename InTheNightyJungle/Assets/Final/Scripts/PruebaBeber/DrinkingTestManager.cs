using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrinkingTestManager : MonoBehaviour {

	public DrinkingTestUIController UI;
    public DrinkingChallengerBehaviour challenger;
    private PlayerPlatformController player;
    public BarmanBehaviour barman;
    private CameraBehaviour cameraObject;

    public Transform insidePlayerPosition;
    public Transform insideCameraPosition;
    public Transform outsidePlayerPosition;
    public Transform outsideCameraPosition;
    public float cameraSize;
    public DrinkingShadow[] foregroundSilhouettes;

    private bool testStarted;
    private int victory;
    private bool augmentFama;

    public int totalNumDrinks;
    private int playerCurrentNumDrinks;
    private int challengerCurrentNumDrinks;

    private bool playerDrinking;
    private bool challengerDrinking;
    private bool playerGlassOnTable;
    private bool playerBarmanTurn; //true significa que es el turno del jugador y false, el del camarero
    private bool challengerGlassOnTable;
    private bool challengerBarmanTurn; //true significa que es el turno del oponente y false, el del camarero

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

    public ParticleSystem[] playerCelebration;
    public ParticleSystem[] challengerCelebration;

	// Use this for initialization
	void Start () {
		testStarted = false;
	}

    public void StartTest(PlayerPlatformController param0, CameraBehaviour param1)
    {
        player = param0;
        cameraObject = param1;

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

        leftSideGlass.InitializeGlass(leftSideGlassPlaceOnBar, positionInHand, GetComponent<Transform>());
        rightSideGlass.InitializeGlass(rightSideGlassPlaceOnBar, positionInHand, GetComponent<Transform>());
        leftSideGlass.SetSortingLayer(barman.GetSortingLayer());
        rightSideGlass.SetSortingLayer(barman.GetSortingLayer());

        if(playerSide)
        {
            player.SetGlass(leftSideGlass);
            challenger.SetGlass(rightSideGlass);
        }
        else
        {
            player.SetGlass(rightSideGlass);
            challenger.SetGlass(leftSideGlass);
        }

        elapsedTimeWithoutDrinking = elapsedTimeWithoutIncreasing = 0.0f;
    }

    private IEnumerator Introduction(float time1, float time2, float time3, float time4)
    {
        StartCoroutine(cameraObject.MoveSizeTo(insideCameraPosition.position, cameraObject.GetSize(), time1));
        yield return new WaitForSeconds(time1);

        StartCoroutine(player.MoveTo(insidePlayerPosition.position, false, time2));
        yield return new WaitForSeconds(time2);

        StartCoroutine(cameraObject.MoveSizeTo(insideCameraPosition.position, cameraSize, time3));
        FadeOutCrowd(time3);
        yield return new WaitForSeconds(time3);

        UI.InitializeUI();

        StartCoroutine(UI.ShowIntroductionTexts(time4));
        yield return new WaitForSeconds(time4);
        
        testStarted = true;
    }

    private IEnumerator Ending(float time1, float time2, float time3)
    {
        StartCoroutine(cameraObject.MoveSizeTo(insideCameraPosition.position, cameraObject.GetInitialSize(), time1));
        FadeInCrowd(time1);
        yield return new WaitForSeconds(time1);

        StartCoroutine(player.MoveTo(outsidePlayerPosition.position, true, time2));
        yield return new WaitForSeconds(time2);

        StartCoroutine(cameraObject.MoveSizeTo(outsideCameraPosition.position, cameraObject.GetSize(), time3));
        yield return new WaitForSeconds(time3);

        player.SetInputActivated(true);
        cameraObject.SetFollowTarget(true);
        UI.GetComponent<GeneralUIController>().ChangeMode(UILayer.BrendaStats);
       
        if (augmentFama)
        {
            //player.DecreaseCansancio(0.66f);
            player.DecreasePatience(0.5f);
            //print("victoria" + victory);
            player.IncreaseFame();
        }
        else
        {
            player.DecreaseCansancio(0.1f);
            player.DecreasePatience(0.5f);
            player.IncreaseFame();

            //print("derrota" + victory);

        }
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
            if(victory == -1)
            {
                if(playerDrinking)
                {
                    if(Input.GetKeyDown(KeyCode.Space))
                    {
                        elapsedTimeWithoutDrinking = 0.0f;
                        if(!player.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("drinking"))
                        {
                            player.PlayDrinking(true);
                        }
                        else
                        {
                            if(UI.IncreaseDrinkingBar(playerSide))
                            {
                                playerCurrentNumDrinks++;
                                playerDrinking = false;
                                player.PlayDrinking(false);
                                if(playerCurrentNumDrinks == totalNumDrinks)
                                {
                                    challengerDrinking = false;
                                    challenger.PlayDrinking(false);
                                    victory = 0;
                                }
                            }
                        }
                    }
                    else
                    {
                        elapsedTimeWithoutDrinking += Time.deltaTime;
                        if(elapsedTimeWithoutDrinking >= minTimeToDrink)
                        {
                            player.PlayDrinking(false);
                        }
                    }
                }
                else
                {
                    if(playerGlassOnTable) //El vaso está sobre la mesa
                    {
                        if(playerBarmanTurn) //Es el turno del jugador, deberá ejecutar la animación de coger el vaso y se deberá comprobar cuando lo tiene ya que entonces se puede volver a beber
                        {
                            if(player.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("idle"))
                            {
                                player.PlayAimGlass();
                            }
                            else if(player.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("drinkingIdle"))
                            {
                                playerDrinking = true;
                                playerGlassOnTable = false;
                            }
                        }
                        else //Es el turno del camarero, por lo tanto deberá recoger el vaso y en ese proceso estará ocupado
                        {
                            if((playerSide && !barman.GetRightBusyBarman()) || (!playerSide && !barman.GetLeftBusyBarman())) //Puede que esté ya ocupado y por tanto, no podrá hacerlo
                            {
                                if(playerSide) barman.SetLeftBusyBarman(true);
                                else barman.SetRightBusyBarman(true);
                                
                                StartCoroutine(barman.PlayChangeGlass(playerSide, 0.5f));
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
                                player.PlayLeaveGlass();
                            }
                            else if(player.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("preIdle"))
                            {
                                playerGlassOnTable = true;
                                playerBarmanTurn = false;
                            }
                        }
                        else //Es el turno del camerero, lo que quiere decir estamos mirando si ha terminado de sustituir los vasos
                        {
                            if((playerSide && !barman.GetLeftBusyBarman()) || (!playerSide && !barman.GetRightBusyBarman()))
                            {
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
                            if(challengerCurrentNumDrinks == totalNumDrinks)
                            {   
                                playerDrinking = false;
                                player.PlayDrinking(false);
                                victory = 1;
                            }
                        }
                    }
                }
                else
                {
                    if(challengerGlassOnTable) //El vaso está sobre la mesa
                    {
                        if(challengerBarmanTurn) //Es el turno del jugador, deberá ejecutar la animación de coger el vaso y se deberá comprobar cuando lo tiene ya que entonces se puede volver a beber
                        {
                            if(challenger.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("idle"))
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
                            if((playerSide && !barman.GetLeftBusyBarman()) || (!playerSide && !barman.GetRightBusyBarman())) //Puede que esté ya ocupado y por tanto, no podrá hacerlo
                            {
                                if(!playerSide) barman.SetLeftBusyBarman(true);
                                else barman.SetRightBusyBarman(true);

                                StartCoroutine(barman.PlayChangeGlass(!playerSide, 0.5f));
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
                            if((playerSide && !barman.GetRightBusyBarman()) || (!playerSide && !barman.GetLeftBusyBarman()))
                            {
                                challengerGlassOnTable = true;
                                challengerBarmanTurn = true;
                            }
                        }
                    }
                }

            }
            else
            {
                if(victory == 0) StartCoroutine(Victory(2f, 3f));
                else StartCoroutine(Defeat(2f, 3f));
            }
        }
	}
    
    private IEnumerator Victory(float time1, float time2)
    {
        testStarted = false;

        player.PlayLeaveGlass();
        challenger.PlayLeaveGlass();

        StartCoroutine(UI.ShowFinalText(time1));
        yield return new WaitForSeconds(time1);

        RestartTest();

        UI.GetComponent<GeneralUIController>().ChangeMode(UILayer.Empty);
        for(int i = 0; i < playerCelebration.Length; i++)
        {
            playerCelebration[i].Play();
        }

        player.GetComponent<Animator>().SetBool("victory", true);
        challenger.GetComponent<Animator>().SetBool("defeat", true);

        yield return new WaitForSeconds(time2);

        player.GetComponent<Animator>().SetBool("victory", false);
        challenger.GetComponent<Animator>().SetBool("defeat", false);

        augmentFama = true;
        StartCoroutine(Ending(0.5f, 0.5f, 0.5f));
    }

    private IEnumerator Defeat(float time1, float time2)
    {
        testStarted = false;

        player.PlayLeaveGlass();
        challenger.PlayLeaveGlass();

        StartCoroutine(UI.ShowFinalText(time1));
        yield return new WaitForSeconds(time1);

        RestartTest();
        
        UI.GetComponent<GeneralUIController>().ChangeMode(UILayer.Empty);
        for(int i = 0; i < challengerCelebration.Length; i++)
        {
            challengerCelebration[i].Play();
        }

        player.GetComponent<Animator>().SetBool("defeat", true);
        challenger.GetComponent<Animator>().SetBool("victory", true);

        yield return new WaitForSeconds(time2);

        player.GetComponent<Animator>().SetBool("defeat", false);
        challenger.GetComponent<Animator>().SetBool("victory", false);

        augmentFama = false;
        StartCoroutine(Ending(0.5f, 0.5f, 0.5f));
    }
}
