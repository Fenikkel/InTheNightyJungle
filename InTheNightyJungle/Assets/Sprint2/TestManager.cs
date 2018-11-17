using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestManager : MonoBehaviour {

    public GameObject challenger; //Temporal, hay que crear un sistema para tener mas de un challenger. En plan el trigger de hablar con un challenger te empieza con StartTurnSistem(nombreDelChallenger)
    public Canvas UITest;
    public GameObject player;
    public GameObject[] danceLife;

    //public GameObject[] allChallengers;

    private bool pruebaTerminada = false;
    public bool challengerTurn = true;
    public bool playerTurn = false;
    private string[] allBlocks;
    private int currentMovement;
    private string currentBlock;
    private bool blockDanced;
    private int totalDanceLife;
    private bool victory;
    public float timeLeft;
    public float extraTime;


    void Start () {
        blockDanced = false;
        victory = true;

        allBlocks = challenger.GetComponent<ChallengerBehaviour>().allDanceBlocks; //esto con el challenger de turno

        UITest.transform.Find("RemainingChallenges").gameObject.GetComponent<Text>().text = "Remaining challenges: " + allBlocks.Length;

        currentMovement = 0;
        StartCoroutine(StartTurnSistem());
        totalDanceLife = danceLife.Length -1;
	}
	
	void Update () {
        if(playerTurn)
        {

            timeLeft -= Time.deltaTime;
            UITest.transform.Find("TimeLeft").gameObject.GetComponent<Text>().text = "Time Left: " + System.Math.Round(timeLeft,1);
            if (timeLeft< 0.0f)
            {
                //SE HACE ANIMACION DE DERROTA
                for(int i =0; i<danceLife.Length; i++)
                {
                    danceLife[i].SetActive(false);

                }

                totalDanceLife = 0;
                victory = false;
                pruebaTerminada = true;
                blockDanced = true;//para salir de los bucles
                currentMovement = 0;
                playerTurn = false;
                UITest.transform.Find("TimeLeft").gameObject.SetActive(false);
                UITest.transform.Find("RemainingChallenges").gameObject.SetActive(false);

            }

            string currentInput = Input.inputString.ToLower();
            
            if (!Input.inputString.Equals(""))
            {
                
                print("CurrentBlock: " + currentBlock);
                print("CurrentInput: " + currentInput);
                print("Correct: " + currentBlock[currentMovement]);


                if (currentBlock[currentMovement].ToString().Equals(currentInput)) //string[index] devuelve un char no un string
                {
                    print("BIEN");
                    timeLeft += extraTime; 
                    //SE HACE ANIMACION DE BAILE
                    player.GetComponent<Animator>().Play("jumping - flying");
                    //player.GetComponent<Animator>().Play("currentInput");

                    //(mejor cuando acierte todo el bloque?)SE HACE ANIMACION DE BIEN (no debe penalizar tiempo o directamente)
                    currentMovement++;
                    if (currentBlock.Length <= currentMovement)
                    {
                       
                        blockDanced = true;
                        print("Bloque completado");
                        currentMovement = 0;
                        playerTurn = false;


                        
                    }
                }
                else
                {
                    //SE HACE ANIMACION DE BAILE
                    //SE HACE ANIMACION DE MAL 
                    
                    player.GetComponent<Animator>().Play("knockback frontal");
                    danceLife[totalDanceLife].SetActive(false);
                   
                    --totalDanceLife;
                    
                    if (totalDanceLife<0)
                    {
                        victory = false;
                        pruebaTerminada = true;
                        blockDanced = true;//para salir de los bucles
                        //SE HACE ANIMACION DE DERROTA
                    }
                    print("MAL");
                    currentMovement = 0;
                    playerTurn = false;
                }

            }

        }
    }


    public IEnumerator StartTurnSistem()
    {
        player.GetComponent<PlayerPlatformController>().SetInputActivated(false);
        
        while (!pruebaTerminada)
        {
            //StartCoroutine(challenger.GetComponent<ChallengerBehaviour>().ChallengerTurn("ABCD")) ;

            for(int i = 0; i< allBlocks.Length; i++)
            {
                blockDanced = false;
                while (!blockDanced)
                {
                    challengerTurn = true;

                    StartCoroutine(challenger.GetComponent<ChallengerBehaviour>().ChallengerTurn(allBlocks[i]));

                    yield return new WaitUntil(() => challengerTurn == false);
                    currentBlock = allBlocks[i];

                    playerTurn = true;
                    UITest.transform.Find("TimeLeft").gameObject.SetActive(true);
                    //timeLeft = 10.0f; //si para cada bloque queremos un tiempo distinto, sino un tiempo para todos los bloques
                    yield return new WaitUntil(() => playerTurn == false);

                }
                if (pruebaTerminada)
                {
                    break;
                }
                else //si se ha acertado el bloque pero no ha terminado la partida
                {
                    UITest.transform.Find("RemainingChallenges").gameObject.GetComponent<Text>().text = "Remaining challenges: " + (allBlocks.Length-(i+1)) ;

                }

            }



            //StartCoroutine(WaitChallenger());
            //cuenta atras
            //habilitar teclas

            for (int i = 0; i < danceLife.Length; i++)
            {
                danceLife[i].SetActive(false);

            }
            totalDanceLife = 0;

            print("PRUEBA TERMINADA");
            //HACER ANIMACION VICTORIA
            UITest.transform.Find("TimeLeft").gameObject.SetActive(false);
            UITest.transform.Find("RemainingChallenges").gameObject.SetActive(false);
            print("Victory: " + victory);
            pruebaTerminada = true;

        }
        player.GetComponent<PlayerPlatformController>().SetInputActivated(true);  //antes que esto se deberia salir de la prueba de baile y toda la mandanga


    }
}
