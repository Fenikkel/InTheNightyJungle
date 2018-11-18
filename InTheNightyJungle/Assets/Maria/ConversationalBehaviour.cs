using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversationalBehaviour : MonoBehaviour {

    bool activeCon; //Activa la conversación
    bool conEnd; //Para saber si la conversación ha acabado

    public PlayerPlatformController player;

	// Use this for initialization
	void Start () {

        player = PlayerPlatformController.playerInstance;

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
