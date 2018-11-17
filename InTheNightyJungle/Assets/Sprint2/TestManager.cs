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
    public bool challengerTurn = true; //public necesario para ChallengerBehaviour
    private bool playerTurn = false;
    private string[] allBlocks;
    private int currentMovement;
    private string currentBlock;
    private bool blockDanced;
    private int totalDanceLife;
    private bool victory;
    public float timeLeft;
    public float extraTime;
    private bool isDancing;
    public Text textoTiempo;
    public Text textBloques;



    void Start () {
        isDancing = false;
        blockDanced = false;
        victory = true;

        allBlocks = challenger.GetComponent<ChallengerBehaviour>().allDanceBlocks; //esto con el challenger de turno
        textoTiempo.text = timeLeft.ToString();//Mathf.Round(timeLeft).ToString(); 

        //UITest.transform.Find("BloquesCompletados").gameObject.GetComponent<Text>().text = "Bloques completados: 0 / 5"; //+ allBlocks.Length;
        textBloques.text = "Bloques completados: 0/5"; //+ allBlocks.Length;

        currentMovement = 0;
        StartCoroutine(StartTurnSistem());
        totalDanceLife = danceLife.Length -1;
	}
	
	void Update () {
        if(playerTurn)
        {
            if (!isDancing)
            {
                PlayerDance();
                UpdateTime();
            }

        }
    }

    private void UpdateTime()
    {
        timeLeft -= Time.deltaTime;
        textoTiempo.text = Mathf.Round(timeLeft).ToString(); //"Time Left: " + System.Math.Round(timeLeft, 1);
        if (timeLeft < 0.0f)
        {
            //SE HACE ANIMACION DE DERROTA
            for (int i = 0; i < danceLife.Length; i++)
            {
                danceLife[i].SetActive(false);

            }

            totalDanceLife = 0;
            victory = false;
            pruebaTerminada = true;
            blockDanced = true;//para salir de los bucles
            currentMovement = 0;
            playerTurn = false;
            //UITest.transform.Find("TimeLeft").gameObject.SetActive(false);
            //UITest.transform.Find("RemainingChallenges").gameObject.SetActive(false);

        }
    }

    private IEnumerator DanceAnimation(string animationName)
    {
        player.GetComponent<Animator>().Play(animationName);



        //print("Animation Start: " + Time.deltaTime);
        yield return new WaitForSeconds(player.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);// + player.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime);
        //print("Animation Done: "+ Time.deltaTime);
        isDancing = false;
    }

    private void PlayerDance()
    {

        string currentInput = Input.inputString.ToLower();

        if (!Input.inputString.Equals(""))
        {

            print("CurrentBlock: " + currentBlock);
            print("CurrentInput: " + currentInput);
            print("Correct: " + currentBlock[currentMovement]);

            //en este if se puede hacer un OR para comprobar cada una de las teclas de la cruceta
            if (currentBlock[currentMovement].ToString().Equals(currentInput)) //string[index] devuelve un char no un string
            {
                print("BIEN");
                timeLeft += extraTime;
                //SE HACE ANIMACION DE BAILE
                isDancing = true; 
                StartCoroutine(DanceAnimation("dashCindy"));
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

                StartCoroutine(DanceAnimation("knockback frontal"));

                //player.GetComponent<Animator>().Play("knockback frontal");
                danceLife[totalDanceLife].SetActive(false);

                --totalDanceLife;

                if (totalDanceLife < 0)
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
                    //UITest.transform.Find("TimeLeft").gameObject.SetActive(true);
                    //timeLeft = 10.0f; //si para cada bloque queremos un tiempo distinto, sino un tiempo para todos los bloques
                    yield return new WaitUntil(() => playerTurn == false);

                }
                if (pruebaTerminada)
                {
                    break;
                }
                else //si se ha acertado el bloque pero no ha terminado la partida
                {
                    textBloques.text = "Bloques completados: "+(i+1)+"/5"; //+ (allBlocks.Length-(i+1)) ;

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
            textoTiempo.gameObject.SetActive(false);
            textBloques.gameObject.SetActive(false);
            //print("Victory: " + victory);
            UITest.transform.Find("TheEnd").gameObject.SetActive(true);
            if (victory)
            {
                UITest.transform.Find("TheEnd").gameObject.GetComponent<Text>().text = "VICTORY";
            }
            else
            {
                UITest.transform.Find("TheEnd").gameObject.GetComponent<Text>().text = "DEFEAT";

            }

            pruebaTerminada = true;

        }
        player.GetComponent<PlayerPlatformController>().SetInputActivated(true);  //antes que esto se deberia salir de la prueba de baile y toda la mandanga


    }
}
