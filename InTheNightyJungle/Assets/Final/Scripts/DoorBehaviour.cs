using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBehaviour : MonoBehaviour {

    public Transform playerPosition;
    public Transform cameraPosition;
    public ChamberManager chamber;

    bool shownUPkey;
    bool shownDOWNkey;
    public float distanceOverHead;
    public SpriteRenderer key;

    public DoorBehaviour nextDoor;
    public int doorType; //0 hacia dentro, 1 derecha, 2 hacia fuera, 3 izquierda

    private GameManager GM;

    private bool justOne;
    private bool enabledDoor;

    [SerializeField]
    private AudioSource doorSound;
    
    // Use this for initialization
	void Start ()
    {
        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        chamber = GetComponentInParent<ChamberManager>();

        justOne = false;
        enabledDoor = doorType != 4;
        StartCoroutine(DisappearDOWNKey(0.5f));
        StartCoroutine(DisappearUPKey(0.5f));
    }
	
	// Update is called once per frame
	void Update () {
        		
	}

    public void TurnOnTurnOff()
    {
        chamber.DeActiveChamber();
        nextDoor.chamber.ActiveChamber();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player"))
        {
            if(doorType%2 == 1) { 
                doorSound.Play();
            }

            if (doorType == 0)
            {
                StartCoroutine(AppearUPKey(0.5f));
            }
            else if (doorType == 2)
            {
                StartCoroutine(AppearDOWNKey(0.5f));
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player"))
        {
            if (doorType == 0 && Input.GetKeyDown(KeyCode.UpArrow)) { //Si la puerta es hacia dentro, el jugador debe pulsar la flecha hacia arriba para pasar
                doorSound.Play();
                GM.ChangingChamber(this);
            }
            else if(doorType == 2 && Input.GetKeyDown(KeyCode.DownArrow)) { //Si la puerta es hacia fuera, el jugador debe pulsar la flecha hacia abajo para pasar
                doorSound.Play();
                GM.ChangingChamber(this);
            }
            else if(doorType%2 == 1) { //En cualquier otro caso, pasará sin necesidad de pulsar ningún botón
                GM.ChangingChamber(this);
            }
            else if(doorType == 4 && Input.GetKeyDown(KeyCode.UpArrow) && !justOne && enabledDoor)
            {
                justOne = true;
                doorSound.Play();
                GM.NextLevel();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (doorType == 0 && shownUPkey)
        {
            StartCoroutine(DisappearUPKey(0.5f));
        }
        else if(doorType==2 && shownDOWNkey)
        {
            StartCoroutine(DisappearDOWNKey(0.5f));
        }
        
    }

        private IEnumerator AppearUPKey(float time)
    {
        shownUPkey = true;

        float elapsedTime = 0.0f;
        Vector2 initialPosition = key.GetComponent<Transform>().position;
        Vector2 finalPosition = new Vector2(initialPosition.x, initialPosition.y + distanceOverHead);

        Color initialColor = key.color;
        Color finalColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);

        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            key.GetComponent<Transform>().position = Vector2.Lerp(initialPosition, finalPosition, elapsedTime / time);
            key.color = Color.Lerp(initialColor, finalColor, elapsedTime / time);
            yield return null;
        }
        key.GetComponent<Transform>().position = finalPosition;
        key.color = finalColor;
    }

    private IEnumerator DisappearUPKey(float time)
    {
        shownUPkey = false;

        float elapsedTime = 0.0f;
        Vector2 initialPosition = key.GetComponent<Transform>().position;
        Vector2 finalPosition = new Vector2(initialPosition.x, initialPosition.y - distanceOverHead);

        Color initialColor = key.color;
        Color finalColor = new Color(1.0f, 1.0f, 1.0f, 0.0f);

        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            key.GetComponent<Transform>().position = Vector2.Lerp(initialPosition, finalPosition, elapsedTime / time);
            key.color = Color.Lerp(initialColor, finalColor, elapsedTime / time);
            yield return null;
        }
        key.GetComponent<Transform>().position = finalPosition;
        key.color = finalColor;
    }

    private IEnumerator AppearDOWNKey(float time)
    {
        shownDOWNkey = true;

        float elapsedTime = 0.0f;
        Vector2 initialPosition = key.GetComponent<Transform>().position;
        Vector2 finalPosition = new Vector2(initialPosition.x, initialPosition.y + distanceOverHead);

        Color initialColor = key.color;
        Color finalColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);

        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            key.GetComponent<Transform>().position = Vector2.Lerp(initialPosition, finalPosition, elapsedTime / time);
            key.color = Color.Lerp(initialColor, finalColor, elapsedTime / time);
            yield return null;
        }
        key.GetComponent<Transform>().position = finalPosition;
        key.color = finalColor;
    }

    private IEnumerator DisappearDOWNKey(float time)
    {
        shownDOWNkey = false;

        float elapsedTime = 0.0f;
        Vector2 initialPosition = key.GetComponent<Transform>().position;
        Vector2 finalPosition = new Vector2(initialPosition.x, initialPosition.y - distanceOverHead);

        Color initialColor = key.color;
        Color finalColor = new Color(1.0f, 1.0f, 1.0f, 0.0f);

        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            key.GetComponent<Transform>().position = Vector2.Lerp(initialPosition, finalPosition, elapsedTime / time);
            key.color = Color.Lerp(initialColor, finalColor, elapsedTime / time);
            yield return null;
        }
        key.GetComponent<Transform>().position = finalPosition;
        key.color = finalColor;
    }

    public Vector3 GetPosition()
    {
        return playerPosition.position;
    }

    public void SetEnabledDoor(bool param)
    {
        enabledDoor = param;
    }
}
