﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBehaviour : MonoBehaviour {

    public Transform playerPosition;
    public Transform cameraPosition;
    public ChamberManager chamber;

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

    public Vector3 GetPosition()
    {
        return playerPosition.position;
    }

    public void SetEnabledDoor(bool param)
    {
        enabledDoor = param;
    }
}
