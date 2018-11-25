using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrinkingTestManager : MonoBehaviour {

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

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
