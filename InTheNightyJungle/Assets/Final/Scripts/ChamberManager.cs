using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChamberManager : MonoBehaviour {

    public int cameraSize;
    public GameObject[] thingsToDeactivate;
    public bool initialChamber;
	// Use this for initialization
	void Start () {
        //activados = false;
        //transform.GetChild(0); //ponemos un game object a la estacia que sera los que se desactivan
        if (initialChamber)
        {
            ActiveChamber();
        }else
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
        switch(cameraSize){
            
            case 1:
                return 3.21849f;
                break;

            case 2:
                return 4f;
                break;


            case 3:
                return 5f;
                break;

            default:
                return 0f;
                break;
        }
    }

    public void SetCameraSize(){
        //print("CameraModificada");
        switch(cameraSize){
            
            case 1:

                Camera.main.orthographicSize = 3.21849f;

                break;

            case 2:
                Camera.main.orthographicSize = 4f;

                break;


            case 3:
                Camera.main.orthographicSize = 5f;
                break;

            default:

                break;
        }

    }
}
