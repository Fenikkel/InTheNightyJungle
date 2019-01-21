using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChamberManager : MonoBehaviour {

    public int chamberSizeType;
    private float cameraSize;
    public GameObject[] thingsToDeactivate;
    public bool initialChamber;

	// Use this for initialization
	void Awake()
    {
        print(gameObject.name);
        switch(chamberSizeType)
        {
            case 1: cameraSize = CameraSizes.smallChamberSize;
                    break;
            case 2: cameraSize = CameraSizes.mediumChamberSize;
                    break;
            case 3: cameraSize = CameraSizes.bigChamberSize;
                    break;
        } 
    }
    
    void Start () {

        if (initialChamber)
        {
            ActiveChamber();
        }
        else
        {
            DeActiveChamber();
        }
    }
	
	// Update is called once per frame
	void Update () {

    }

    public void ActiveChamber(){
        ActivateDeactivateThings(true);
    }
     
    public void DeActiveChamber()
    {
        ActivateDeactivateThings(false);

    }

    public void ActivateDeactivateThings(bool param)
    {
        for(int i = 0; i < thingsToDeactivate.Length; i++)
        {
            thingsToDeactivate[i].SetActive(param);
        }
    }

    public float GetCameraSize()
    {
        return cameraSize;   
    }
}
