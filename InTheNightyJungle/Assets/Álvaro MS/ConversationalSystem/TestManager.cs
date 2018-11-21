﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestManager : MonoBehaviour {

    public DancingTestUIController UI;
    public ChallengerBehaviour challenger;
    private PlayerPlatformController player;
    private CameraBehaviour camera;

    public Transform insidePlayerPosition;
    public Transform insideCameraPosition;
    public Transform outsidePlayerPosition;
    public Transform outsideCameraPosition;
    public float cameraSize;
    public LittleSilhouettoOfAMan[] foregroundSilhouettes;

    private bool testStarted;
    private bool turnActivated;
    private bool playerTurn;

    private int remainingMistakes;
    private int victory;

    public KeyCode[] possibleKeys;
    
    public SpriteRenderer[] visualKeys;
    public float visualKeyDistanceOverHead;
    public float visualKeyRotationAngle;

    public string[] blocks;
    private int currentBlock;
    private int currentKey;

    private float remainingTime;
    public float initialTime;

    public ParticleSystem[] playerCelebration;
    public ParticleSystem[] challengerCelebration;

    private void Start()
    {
        testStarted = false;
    }

    public void StartTest(PlayerPlatformController param0, CameraBehaviour param1)
    {
        player = param0;
        camera = param1;

        RestartTest();

        StartCoroutine(Introduction(0.5f, 0.5f, 0.5f));
    }

    public void RestartTest()
    {
        turnActivated = false;
        playerTurn = false;

        remainingMistakes = 3;
        victory = -1;

        currentBlock = 0;
        currentKey = 0;

        remainingTime = initialTime;
    }

    private IEnumerator Introduction(float time1, float time2, float time3)
    {
        StartCoroutine(camera.MoveSizeTo(insideCameraPosition.position, camera.GetSize(), time1));
        yield return new WaitForSeconds(time1);

        StartCoroutine(player.MoveTo(insidePlayerPosition.position, false, time2));
        yield return new WaitForSeconds(time2);

        StartCoroutine(camera.MoveSizeTo(insideCameraPosition.position, cameraSize, time3));
        FadeOutCrowd(time3);
        yield return new WaitForSeconds(time3);

        testStarted = true;
        UI.InitializeUI(blocks.Length, initialTime);
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
        UI.GetComponent<GeneralUIController>().ChangeMode("001");
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

    private void Update()
    {
        if(testStarted)
        {
            if(!turnActivated) //No se está ejecutando nada dentro de la prueba
            {
                if(victory == -1)
                {                    
                    if(playerTurn) //Turno del jugador
                    {
                        UpdateTimer();
                        if(Input.anyKeyDown) //Miramos solo si se está pulsando algo
                        {
                            for(int i = 0; i < possibleKeys.Length; i++) //Recorremos las posibles teclas que se pueden pulsar por si alguna de ellas se está pulsando
                            {
                                if(Input.GetKeyDown(possibleKeys[i]))
                                {
                                    turnActivated = true; //Sea cual sea la tecla dentro de la posibles, si se ha pulsado, no se podrá hacer nada hasta que termine la acción que desencadena
                                    bool right = TryKey(possibleKeys[i]); //Le pasamos la que se está pulsando a un método que nos comprobará si es correcta en función de cuál toca pulsar ahora
                                    if(right)
                                    {
                                        StartCoroutine(AppearPlayerCorrectKey(visualKeys[i], 0.75f));
                                    }
                                    else
                                    {
                                        StartCoroutine(AppearPlayerIncorrectKey(visualKeys[i], 0.75f));
                                    }
                                }
                            }
                        }
                    }
                    else //Turno del oponente
                    {
                        turnActivated = true;
                        NextDancingMovement();
                    }
                }
                else
                {
                    if(victory == 0) StartCoroutine(Victory(3f));
                    else StartCoroutine(Defeat(3f));
                }
            }
            else
            {
                if(playerTurn) { //Lo único que para el turno desde el jugador es la propia animación de personaje, por lo que cabe comprobar cuando el personaje vuelve al idle
                    UpdateTimer();
                    turnActivated = !player.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("preIdle");
                }
                else 
                    turnActivated = !challenger.HasFinished();
            }
        }
    }  

    private void UpdateTimer()
    {
        remainingTime -= Time.deltaTime;
        if(remainingTime >= 0)
        {
            UI.SetCurrentTime(remainingTime);
        }
        else
        {
            WrongKey();
        }
    }

    private bool TryKey(KeyCode key)
    {        
        bool result = true; //Del case que salga, comprobará si no coincide la tecla que se pulsar en ese momento con la versión codificada con la tecla realmente pulsada, poniendo el valor de result a false en caso de ser verdadera la condición
        
        switch(key)
        {
            case KeyCode.W:
                if(!blocks[currentBlock][currentKey].Equals('w')) result = false;
                break;
            case KeyCode.A:
                if(!blocks[currentBlock][currentKey].Equals('a')) result = false;
                break;
            case KeyCode.S:
                if(!blocks[currentBlock][currentKey].Equals('s')) result = false;
                break;
            case KeyCode.D:
                if(!blocks[currentBlock][currentKey].Equals('d')) result = false;
                break;
            case KeyCode.UpArrow:
                if(!blocks[currentBlock][currentKey].Equals('1')) result = false;
                break;
            case KeyCode.LeftArrow:
                if(!blocks[currentBlock][currentKey].Equals('2')) result = false;
                break;
            case KeyCode.DownArrow:
                if(!blocks[currentBlock][currentKey].Equals('3')) result = false;
                break;
            case KeyCode.RightArrow:
                if(!blocks[currentBlock][currentKey].Equals('4')) result = false;
                break;
        }

        if(!result) //Si se ha fallado, se debe indicar que el turno pasa al oponente y no se debe aumentar el currentKey
        {
            WrongKey();
        }
        else //Si se ha acertado, se aumenta el valor del currentKey
        {
            currentKey++;
            if(currentKey == blocks[currentBlock].Length) //Se comprueba que no se haya llegado al final del bloque
            {
                currentBlock++;
                currentKey = 0;

                UI.CompletedBlock(); //Se le indica a la UI que se ha completado un bloque

                if(currentBlock == blocks.Length) //Se comprueba si se han completado todos los bloques
                {
                    victory = 0; //El valor 0 indica victoria, el valor 1 indica derrota. El valor -1 es el valor predefinido y para el cual no se tiene que hacer nada
                }
                else //Si no hay victoria, al acabar un bloque se cambia el turno al oponente
                {
                    playerTurn = false;
                    
                    remainingTime = initialTime;
                    UI.SetCurrentTime(remainingTime);
                }
            }
        }
        //Tanto si se acierta como si no, el personaje del jugador deberá realizar la animación correspondiente a la tecla pulsada

        player.DancingMovement(key);

        return result;
    }

    private void WrongKey()
    {
        currentKey = 0;
        playerTurn = false;

        remainingMistakes--;
        UI.OneMistakeMore();

        if(remainingMistakes == 0)
            victory = 1;
        else
        {
            remainingTime = initialTime;
            UI.SetCurrentTime(remainingTime);
        }
    }

    private void NextDancingMovement()
    {
        if(currentKey != blocks[currentBlock].Length)
        {
            switch(blocks[currentBlock][currentKey].ToString())
            {
                case "w":
                    challenger.DancingMovement("W");
                    challenger.ShowKey(0);
                    break;
                case "a":
                    challenger.DancingMovement("A");
                    challenger.ShowKey(1);
                    break;
                case "s":
                    challenger.DancingMovement("S");
                    challenger.ShowKey(2);
                    break;
                case "d":
                    challenger.DancingMovement("D");
                    challenger.ShowKey(3);
                    break;
                case "1":
                    challenger.DancingMovement("Up");
                    challenger.ShowKey(4);
                    break;
                case "2":
                    challenger.DancingMovement("Left");
                    challenger.ShowKey(5);
                    break;
                case "3":
                    challenger.DancingMovement("Down");
                    challenger.ShowKey(6);
                    break;
                case "4":
                    challenger.DancingMovement("Right");
                    challenger.ShowKey(7);
                    break;
            } 
            currentKey++;
        }
        else
        {
            turnActivated = false;
            playerTurn = true;
            currentKey = 0;
        }
    }

    private IEnumerator AppearPlayerCorrectKey(SpriteRenderer key, float time)
    {
        float elapsedTime = 0.0f;
        Vector2 initialPosition = new Vector2(player.GetComponent<Transform>().position.x, key.GetComponent<Transform>().position.y);
        Vector2 finalPosition = initialPosition + new Vector2(0, visualKeyDistanceOverHead/2);

        Color initialColor = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        Color finalColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);

        key.color = initialColor;
        key.GetComponent<Transform>().position = initialPosition;

        while(elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            key.GetComponent<Transform>().position = Vector2.Lerp(initialPosition, finalPosition, elapsedTime / time);
            key.color = Color.Lerp(initialColor, finalColor, elapsedTime / time);
            yield return null;
        }
        key.color = finalColor;
        key.GetComponent<Transform>().position = finalPosition;

        StartCoroutine(DisappearPlayerCorrectKey(key, time, initialPosition));
    }

    private IEnumerator DisappearPlayerCorrectKey(SpriteRenderer key, float time, Vector2 originalPosition)
    {
        float elapsedTime = 0.0f;
        Vector2 initialPosition = new Vector2(player.GetComponent<Transform>().position.x, key.GetComponent<Transform>().position.y);
        Vector2 finalPosition = initialPosition + new Vector2(0, visualKeyDistanceOverHead/2);

        Color initialColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        Color finalColor = new Color(1.0f, 1.0f, 1.0f, 0.0f);

        key.color = initialColor;
        key.GetComponent<Transform>().position = initialPosition;

        while(elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            key.GetComponent<Transform>().position = Vector2.Lerp(initialPosition, finalPosition, elapsedTime / time);
            key.color = Color.Lerp(initialColor, finalColor, elapsedTime / time);
            yield return null;
        }
        key.color = finalColor;
        key.GetComponent<Transform>().position = originalPosition;
    } 

    private IEnumerator AppearPlayerIncorrectKey(SpriteRenderer key, float time)
    {
        float elapsedTime = 0.0f;
        Vector2 initialPosition = new Vector2(player.GetComponent<Transform>().position.x, key.GetComponent<Transform>().position.y);
        Vector2 finalPosition = initialPosition + new Vector2(0, visualKeyDistanceOverHead/2);

        Color initialColor = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        Color finalColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);

        key.color = initialColor;
        key.GetComponent<Transform>().position = initialPosition;

        while(elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            key.GetComponent<Transform>().position = Vector2.Lerp(initialPosition, finalPosition, elapsedTime / time);
            key.color = Color.Lerp(initialColor, finalColor, elapsedTime / time);
            yield return null;
        }
        key.color = finalColor;
        key.GetComponent<Transform>().position = finalPosition;

        StartCoroutine(DisappearPlayerIncorrectKey(key, time, initialPosition));

    }

    private IEnumerator DisappearPlayerIncorrectKey(SpriteRenderer key, float time, Vector2 originalPosition)
    {
        float elapsedTime = 0.0f;
        Vector2 initialPosition = new Vector2(player.GetComponent<Transform>().position.x, key.GetComponent<Transform>().position.y);
        Vector2 finalPosition = (Vector2)player.GetComponent<Transform>().position;

        Color initialColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        Color finalColor = new Color(1.0f, 1.0f, 1.0f, 0.0f);

        float initialAngle = 0;
        float finalAngle = visualKeyRotationAngle;

        key.color = initialColor;
        key.GetComponent<Transform>().rotation = Quaternion.Euler(key.GetComponent<Transform>().rotation.x, key.GetComponent<Transform>().rotation.y, initialAngle);
        key.GetComponent<Transform>().position = initialPosition;

        while(elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            key.GetComponent<Transform>().position = Vector2.Lerp(initialPosition, finalPosition, elapsedTime / time);
            key.color = Color.Lerp(initialColor, finalColor, elapsedTime / time);
            key.GetComponent<Transform>().rotation = Quaternion.Euler(key.GetComponent<Transform>().rotation.x, key.GetComponent<Transform>().rotation.y, Mathf.Lerp(initialAngle, finalAngle, elapsedTime/time));
            yield return null;
        }
        key.color = finalColor;

        key.GetComponent<Transform>().position = originalPosition;
        key.GetComponent<Transform>().rotation = Quaternion.Euler(key.GetComponent<Transform>().rotation.x, key.GetComponent<Transform>().rotation.y, initialAngle);
    }

    private IEnumerator Victory(float time)
    {
        testStarted = false;
        RestartTest();

        UI.GetComponent<GeneralUIController>().ChangeMode("000");
        for(int i = 0; i < playerCelebration.Length; i++)
        {
            playerCelebration[i].Play();
        }

        player.GetComponent<Animator>().SetBool("victory", true);
        challenger.GetComponent<Animator>().SetBool("defeat", true);

        yield return new WaitForSeconds(time);

        player.GetComponent<Animator>().SetBool("victory", false);
        challenger.GetComponent<Animator>().SetBool("defeat", false);

        StartCoroutine(Ending(0.5f, 0.5f, 0.5f));
    }

    private IEnumerator Defeat(float time)
    {
        testStarted = false;
        RestartTest();
        
        UI.GetComponent<GeneralUIController>().ChangeMode("000");
        for(int i = 0; i < challengerCelebration.Length; i++)
        {
            challengerCelebration[i].Play();
        }

        player.GetComponent<Animator>().SetBool("defeat", true);
        challenger.GetComponent<Animator>().SetBool("victory", true);

        yield return new WaitForSeconds(time);

        player.GetComponent<Animator>().SetBool("defeat", false);
        challenger.GetComponent<Animator>().SetBool("victory", false);

        StartCoroutine(Ending(0.5f, 0.5f, 0.5f));
    }
}
