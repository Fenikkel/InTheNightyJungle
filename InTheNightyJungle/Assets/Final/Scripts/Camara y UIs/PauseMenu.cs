using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {
    
    public static bool GameIsPaused = false;

    [SerializeField]
    private GameObject initialScreen, optionsScreen, brightnessScreen, inventoryScreen, inputScreen;
    
    [SerializeField]
    private Slider volume, brightnessBar;

    [SerializeField]
	private Image overIcons;
    [SerializeField]
	private Image[] shadowSprites;
    [SerializeField]
	private int minValue, maxValue;

    private float brightnessValue;

    [SerializeField]
	private GameObject inventoryContentCindy, inventoryContentBrenda;
    [SerializeField]
	private GameObject itemImage, itemName, itemDescription;
    [SerializeField]
	private GameObject inventoryCindy, inventoryBrenda;

	private List<CollectibleInfo> CindyItemList;
	private List<CollectibleInfo> BrendaItemList;

	private bool cindyInventoryOpenned;

    [SerializeField]
    private GameObject inputImage, inputName, inputDescription;

    private string[] inputInfo;
    [SerializeField]
    private Sprite[] inputSprites;
    private int currentInput;

    [SerializeField]
    private TextAsset inputInfoText;

    [SerializeField]
    private AudioSource clickSound;

    void Awake()
    {
        CindyItemList = new List<CollectibleInfo>();
		BrendaItemList = new List<CollectibleInfo>();
    }

    void Start()
    {
        InitializeVolume();
        InitializeBrightnessScreen();
        UnshowItem();
        InitializeInputInfo();

        ChangeScreen(0);
    }

    // Update is called once per frame
    public void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }else
            {
                Pause();
            }
        }
		
	}

    private void ChangeScreen(int param)
    {
        initialScreen.SetActive(param == 0);
        optionsScreen.SetActive(param == 1);
        brightnessScreen.SetActive(param == 2);
        inventoryScreen.SetActive(param == 3);
        inputScreen.SetActive(param == 4);

        if(param == 3)
        {
            cindyInventoryOpenned = GameManager.Instance.IsCindyPlaying();
            inventoryCindy.SetActive(cindyInventoryOpenned);
            inventoryBrenda.SetActive(!cindyInventoryOpenned);
        }

        if(param == 4)
        {
            ChangeInput();
        }
    }

    //Inicializadores
    private void InitializeVolume()
    {
        volume.value = AudioManager.Instance.GetVolume();
    }

    private void InitializeBrightnessScreen()
    {
        brightnessValue = float.Parse(SettingsManager.Instance.Load("brightness"));

		overIcons.color = new Color(overIcons.color.r, overIcons.color.g, overIcons.color.b, brightnessValue);
		brightnessBar.value = (255 * brightnessValue - minValue)/(maxValue - minValue);
		for(int i = 0; i < shadowSprites.Length; i++)
		{
			shadowSprites[i].color = new Color(shadowSprites[i].color.r, shadowSprites[i].color.g, shadowSprites[i].color.b, brightnessValue);
		}
    }

    private void InitializeInputInfo()
    {
        inputInfo = new string[inputSprites.Length*2];
        inputInfo = inputInfoText.text.Split("\n"[0]);
    }

    //Métodos de listeners

    public void Pause() //Se pulsa la tecla Esc
    {
        GeneralUIController.Instance.PauseGame();
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    //Listeners de botones del menú inicial

    public void Resume() //Se pulsa el botón Jugar o se pulsa la tecla Esc
    {
        GeneralUIController.Instance.ResumeGame();
        Time.timeScale = 1f;
        GameIsPaused = false;
        ChangeScreen(0);

        clickSound.Play();
    }

    public void Options() //Se pulsa el botón Opciones
    {
        ChangeScreen(1);

        clickSound.Play();
    }

    public void Inventory() //Se pulsa el botón Inventario 
    {
        ChangeScreen(3);

        clickSound.Play();
    }

    public void LoadMenu() //Se pulsa el botón Salir al menú
    {
        clickSound.Play();

        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void Inputs()
    {
        clickSound.Play();
        ChangeScreen(4);
    }

    //Listeners del menú de opciones

    public void BackFromOptions() //Se pulsa el botón Atrás del menú de opciones, se vuelve al menú inicial
    {
        ChangeScreen(0);
        
        SettingsManager.Instance.Save("volume", AudioManager.Instance.GetVolume().ToString());

        clickSound.Play();
    }

    public void ModifyBrightness() //Se pulsa el botón de Ajustar el brillo
    {
        ChangeScreen(2);

        clickSound.Play();
    }

    public void TurnUpVolume() //Se cambia la barra de volumen
    {
        AudioManager.Instance.ChangeGeneralVolume(volume.value);
    }

    //Listeners del menú de ajuste de brillo

    public void ChangeBrightnessValue() //Se cambia la barra de brillo
	{
		brightnessValue = (minValue + (maxValue - minValue) * brightnessBar.value)/255;
		overIcons.color = new Color(overIcons.color.r, overIcons.color.g, overIcons.color.b, brightnessValue);
		for(int i = 0; i < shadowSprites.Length; i++)
		{
			shadowSprites[i].color = new Color(shadowSprites[i].color.r, shadowSprites[i].color.g, shadowSprites[i].color.b, brightnessValue);
		}
	}

    public void BackFromBrightness() //Se pulsa el botón Atrás del menú de ajuste de brillo, se vuelve a las opciones
	{
		SettingsManager.Instance.Save("brightness", brightnessValue.ToString());
		ChangeScreen(1);

		clickSound.Play();
	}

    //Listeners del menú del inventario

    public void BackFromInventory()
    {
        ChangeScreen(0);
        UnshowItem();

        clickSound.Play();
    }

	public void ShowItem(int id) //Se pulsa sobre uno de los objetos
	{
		itemImage.GetComponent<Image>().color = new Color(1,1,1,1);
		CollectibleInfo item = cindyInventoryOpenned ? CindyItemList[id] : BrendaItemList[id];
		
		itemImage.GetComponent<Image>().sprite = item.GetSpriteInInventory();
		itemName.GetComponent<Text>().text = item.GetName();
		itemDescription.GetComponent<Text>().text = item.GetDescription();

        clickSound.Play();
	}

	public void UnshowItem()
	{
		itemImage.GetComponent<Image>().sprite = null;
		itemName.GetComponent<Text>().text = "";
		itemDescription.GetComponent<Text>().text = "";

		itemImage.GetComponent<Image>().color = new Color(0,0,0,0);
	} 

    public void AddItem(CollectibleInfo item, bool cindy)
	{
		if(cindy) 
		{
			item.SetItemID(CindyItemList.Count);
			CindyItemList.Add(item);
            item.GetComponent<Transform>().SetParent(inventoryContentCindy.GetComponent<Transform>());
		}
		else
		{
			item.SetItemID(BrendaItemList.Count);
			BrendaItemList.Add(item);
            item.GetComponent<Transform>().SetParent(inventoryContentBrenda.GetComponent<Transform>());
		}
		item.GetComponent<Button>().onClick.AddListener(delegate { ShowItem(item.GetItemID()); } );	
	}

    //Listeners del menú de controles

    public void BackFromInputs()
    {
        ChangeScreen(0);
        currentInput = 0;

        clickSound.Play();
    }

    private void ChangeInput()
    {
        inputImage.GetComponent<Image>().sprite = inputSprites[currentInput];
        inputName.GetComponent<Text>().text = inputInfo[currentInput * 2];
        inputDescription.GetComponent<Text>().text = inputInfo[currentInput * 2 + 1];
    }

    public void LeftArrow()
    {
        currentInput--;
        if(currentInput < 0)
        {
            currentInput = inputSprites.Length - 1;
        }
        ChangeInput();
    }

    public void RightArrow()
    {
        currentInput++;
        if(currentInput == inputSprites.Length)
        {
            currentInput = 0;
        }
        ChangeInput();
    }

}
