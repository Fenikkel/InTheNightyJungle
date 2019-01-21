using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    private static GameManager instance;

    public static GameManager Instance{
        get{
            if(instance == null) instance = GameObject.FindObjectOfType<GameManager>();
            return instance;
        }
    }

    public GeneralUIController UIController;
    public PantallaPausa UIPause;
    public GameObject blackScreen;
    public GameObject mainCamera;
    private GameObject player;

    public GameObject BrendaLevels;
    public GameObject CindyLevels;
    public GameObject Brenda;
    public GameObject Cindy;

    public bool initialCutscene;

    private int aux;

    private bool cindyEnabled;
    private bool canChangePlayer;
    
    // Use this for initialization
	void Start () {

        UIPause.enabled = false;
        canChangePlayer = true;

        blackScreen.GetComponent<Image>().enabled = true;

        if(initialCutscene) StartCoroutine(StartInitialCutscene(2f));
        else StartGame();
	}

    public bool IsCindyPlaying()
    {
        return cindyEnabled;
    }

    private IEnumerator StartInitialCutscene(float time)
    {
        
        CindyLevels.SetActive(true);
        BrendaLevels.SetActive(true);

        Cindy.SetActive(true);
        Brenda.SetActive(true);

        Cindy.GetComponent<PlayerPlatformController>().SetInputActivated(false);
        Brenda.GetComponent<PlayerPlatformController>().SetInputActivated(false);

        mainCamera.GetComponent<CameraBehaviour>().SetTarget(Cindy);
        
        StartCoroutine(FadeOut(time, false));
        yield return new WaitForSeconds(time);
        GetComponent<InitialCutscene>().BeginCutscene(Cindy, Brenda, blackScreen, mainCamera.GetComponent<CameraBehaviour>());
    }

    public void StartGame()
    {
        UIPause.enabled = true;
        cindyEnabled = (Random.value > 0.5f) ? true : false;

        Cindy.SetActive(cindyEnabled);
        Brenda.SetActive(!cindyEnabled);

        aux = 0;

        ChangePlayer();

        StartCoroutine(FadeOut(0.5f, true));
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
        if (!cindyEnabled)
        {
            if (Cindy.GetComponent<PlayerPlatformController>().GetLastDoor() != null)
            {
                Cindy.transform.position = Cindy.GetComponent<PlayerPlatformController>().GetLastDoor().GetPosition() + new Vector3(0.01f, 0.01f, 0.01f);        
                Cindy.GetComponent<PlayerPlatformController>().SetCurrentChamber(Cindy.GetComponent<PlayerPlatformController>().GetLastDoor().chamber);
            }
            mainCamera.GetComponent<CameraBehaviour>().SetSize(Cindy.GetComponent<PlayerPlatformController>().GetCurrentChamber().GetCameraSize());
        }
        else
        {
            if (Brenda.GetComponent<PlayerPlatformController>().GetLastDoor() != null)
            {
                Brenda.transform.position = Brenda.GetComponent<PlayerPlatformController>().GetLastDoor().GetPosition() + new Vector3(0.01f, 0.01f, 0.01f);
                Brenda.GetComponent<PlayerPlatformController>().SetCurrentChamber(Brenda.GetComponent<PlayerPlatformController>().GetLastDoor().chamber);
            }
            mainCamera.GetComponent<CameraBehaviour>().SetSize(Brenda.GetComponent<PlayerPlatformController>().GetCurrentChamber().GetCameraSize());
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
        aux++;
        if(aux == 2) Application.Quit();
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
            if(door != null)
            {
                player.GetComponent<PlayerPlatformController>().SetPosition(door.nextDoor.playerPosition.position);
                mainCamera.GetComponent<CameraBehaviour>().SetPosition(door.nextDoor.cameraPosition.position);
                mainCamera.GetComponent<CameraBehaviour>().SetSize(door.nextDoor.chamber.GetCameraSize());
            } 
            yield return new WaitForSeconds(0.1f);
            StartCoroutine(FadeOut(time, transition, door));
        }

    }

    IEnumerator FadeOut(float time, bool transition, DoorBehaviour door)
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
            if(door != null) door.TurnOnTurnOff();
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
                player.GetComponent<PlayerPlatformController>().SetLastDoor(door.nextDoor);
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
        player.GetComponent<PlayerPlatformController>().SetPosition(player.GetComponent<PlayerPlatformController>().GetCheckPoint().GetPlayerPosition().position);
        player.GetComponent<PlayerPlatformController>().GetCurrentChamber().DeActiveChamber();
        player.GetComponent<PlayerPlatformController>().GetCheckPoint().GetChamber().ActiveChamber();
        mainCamera.GetComponent<CameraBehaviour>().SetSize(player.GetComponent<PlayerPlatformController>().GetCheckPoint().GetChamber().GetCameraSize());
        player.GetComponent<PlayerPlatformController>().SetCurrentChamber(player.GetComponent<PlayerPlatformController>().GetCheckPoint().GetChamber());
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
