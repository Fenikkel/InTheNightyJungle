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
            
            string currentInput = Input.inputString.ToLower();
            
            if (!Input.inputString.Equals(""))
            {
                
                print("CurrentBlock: " + currentBlock);
                print("CurrentInput: " + currentInput);
                print("Correct: " + currentBlock[currentMovement]);


                if (currentBlock[currentMovement].ToString().Equals(currentInput)) //string[index] devuelve un char no un string
                {
                    print("BIEN");
                    //SE HACE ANIMACION DE BAILE
                    //SE HACE ANIMACION DE BIEN (no debe penalizar tiempo o directamente)
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
                    danceLife[totalDanceLife].SetActive(false);
                    print(totalDanceLife);

                    --totalDanceLife;
                    print(totalDanceLife);
                    if (totalDanceLife<0)
                    {
                        victory = false;
                        pruebaTerminada = true;
                        blockDanced = true;//para salir de los bucles
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

            print("PRUEBA TERMINADA");
            print("Victory: " + victory);
            pruebaTerminada = true;
        }
        player.GetComponent<PlayerPlatformController>().SetInputActivated(true);


    }
}
