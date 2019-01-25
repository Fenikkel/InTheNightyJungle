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
    public PauseMenu UIPause;
    public GameObject blackScreen;
    public GameObject mainCamera;
    private GameObject player;

    [SerializeField]
    private GameObject BrendaLevelsContainer;
    [SerializeField]
    private GameObject CindyLevelsContainer;
    [SerializeField]
    private GameObject[] BrendaLevels;
    [SerializeField]
    private GameObject[] CindyLevels;
    [SerializeField]
    private GameObject[] BrendaFirstChambers;
    [SerializeField]
    private GameObject[] CindyFirstChambers;
    [SerializeField]
    private GameObject[] BrendaFirstCheckpoints;
    [SerializeField]
    private GameObject[] CindyFirstCheckpoints;

    public GameObject Brenda;
    public GameObject Cindy;

    public bool initialCutscene;

    private int aux;

    private bool cindyEnabled;
    private bool canChangePlayer;

    [SerializeField]
    private Transform[] CindyPlayerCameraBeginningLevelPositions;
    [SerializeField]
    private Transform[] BrendaPlayerCameraBeginningLevelPositions;

    private int CindyCurrentLevel;
    private int BrendaCurrentLevel;
    
    // Use this for initialization
	void Start () {

        UIPause.enabled = false;
        canChangePlayer = true;

        CindyCurrentLevel = 0;
        BrendaCurrentLevel = 0;

        blackScreen.GetComponent<Image>().enabled = true;

        ActiveLevel(true, CindyCurrentLevel);
        ActiveLevel(false, BrendaCurrentLevel);

        InitializeCharacterInLevel(true);
        InitializeCharacterInLevel(false);

        if(initialCutscene) StartCoroutine(StartInitialCutscene(2f));
        else StartGame();
	}

    public bool IsCindyPlaying()
    {
        return cindyEnabled;
    }

    private IEnumerator StartInitialCutscene(float time)
    {
        
        CindyLevelsContainer.SetActive(true);
        BrendaLevelsContainer.SetActive(true);

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
        AudioManager.Instance.TurnOnBackgroundMusic(cindyEnabled);

        BeginChangePlayer();

        //StartCoroutine(FadeOut(0.5f, true));
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
        AudioManager.Instance.ChangeBackgroundMusic(1.0f);

        StartCoroutine(FadeIn(0.5f, false));

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

        yield return new WaitForSeconds(0.1f);

        ChangePlayer();
        StartCoroutine(FadeOut(0.5f, false));
    }
    
    public void ChangePlayer()
    {
        if(canChangePlayer)
        {
            cindyEnabled = !cindyEnabled;

            CindyLevelsContainer.SetActive(cindyEnabled);
            BrendaLevelsContainer.SetActive(!cindyEnabled);

            Cindy.SetActive(cindyEnabled);
            Brenda.SetActive(!cindyEnabled);

            UIController.Initialize(cindyEnabled);

            player = cindyEnabled ? Cindy : Brenda;
            player.GetComponent<PlayerPlatformController>().RestartPlayer();
            StartCoroutine(player.GetComponent<PlayerPlatformController>().InvulnerableInTime(1.0f));
            
            mainCamera.GetComponent<CameraBehaviour>().SetTarget(player);
        }
    }

    private void ActiveLevel(bool cindy, int level)
    {
        if(cindy)
        {
            CindyLevelsContainer.SetActive(true);
            CindyLevels[level].SetActive(true);
            CindyLevelsContainer.SetActive(cindyEnabled);
        }
        else
        {
            //print("hola");
            BrendaLevelsContainer.SetActive(true);
            BrendaLevels[level].SetActive(true);
            BrendaLevelsContainer.SetActive(!cindyEnabled);
        }
    }

    private void DeactiveLevel(bool cindy, int level)
    {
        if(cindy)
        {
            CindyLevelsContainer.SetActive(true);
            CindyLevels[level].SetActive(false);
            CindyLevelsContainer.SetActive(cindyEnabled);
        }
        else
        {
            BrendaLevelsContainer.SetActive(true);
            BrendaLevels[level].SetActive(false);
            BrendaLevelsContainer.SetActive(!cindyEnabled);
        }
    }

    public void NextLevel()
    {
        StartCoroutine(FadeIn(0.5f, false));

        print("hola");

        if(cindyEnabled)
        {
            ActiveLevel(cindyEnabled, CindyCurrentLevel + 1);

            CindyCurrentLevel++;
 
            InitializeCharacterInLevel(cindyEnabled);

            DeactiveLevel(cindyEnabled, CindyCurrentLevel - 1);
        }
        else
        {
            ActiveLevel(cindyEnabled, BrendaCurrentLevel + 1);

            BrendaCurrentLevel++;
 
            InitializeCharacterInLevel(cindyEnabled);

            DeactiveLevel(cindyEnabled, BrendaCurrentLevel - 1);
        }


        StartCoroutine(FadeOut(0.5f, false));
    }

    private void InitializeCharacterInLevel(bool cindy)
    {
        if(cindy)
        {
            Cindy.GetComponent<Transform>().position = CindyPlayerCameraBeginningLevelPositions[CindyCurrentLevel * 2].position;
            mainCamera.GetComponent<Transform>().position = CindyPlayerCameraBeginningLevelPositions[CindyCurrentLevel * 2 + 1].position;

            Cindy.GetComponent<PlayerPlatformController>().SetCurrentChamber(CindyFirstChambers[CindyCurrentLevel].GetComponent<ChamberManager>());
            mainCamera.GetComponent<CameraBehaviour>().SetSize(Cindy.GetComponent<PlayerPlatformController>().GetCurrentChamber().GetCameraSize());
            
            Cindy.GetComponent<PlayerPlatformController>().SetCheckPoint(CindyFirstCheckpoints[CindyCurrentLevel].GetComponent<CheckPoint>());

            Cindy.GetComponent<PlayerPlatformController>().SetLastDoor(null);
        }
        else
        {
            Brenda.GetComponent<Transform>().position = BrendaPlayerCameraBeginningLevelPositions[BrendaCurrentLevel * 2].position;
            mainCamera.GetComponent<Transform>().position = BrendaPlayerCameraBeginningLevelPositions[BrendaCurrentLevel * 2 + 1].position;

            Brenda.GetComponent<PlayerPlatformController>().SetCurrentChamber(BrendaFirstChambers[BrendaCurrentLevel].GetComponent<ChamberManager>());
            mainCamera.GetComponent<CameraBehaviour>().SetSize(Brenda.GetComponent<PlayerPlatformController>().GetCurrentChamber().GetCameraSize());
            
            Brenda.GetComponent<PlayerPlatformController>().SetCheckPoint(BrendaFirstCheckpoints[BrendaCurrentLevel].GetComponent<CheckPoint>());

            Brenda.GetComponent<PlayerPlatformController>().SetLastDoor(null);
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

    IEnumerator FadeIn(float time, bool transition)
    {
        Color c = blackScreen.GetComponent<Image>().color;
        Color initialColor = c;
        Color finalColor = new Color(0, 0, 0, 1);
        float elapsedTime = 0.0f;
        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            c = Color.Lerp(initialColor, finalColor, elapsedTime / time);
            blackScreen.GetComponent<Image>().color = c;
            yield return null;
        }
        blackScreen.GetComponent<Image>().color = finalColor;
        if(transition)
        {
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
