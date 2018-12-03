using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChamberManager : MonoBehaviour {

    public int cameraSize;
    public GameObject desactivados;
    public bool initialChamber;
	// Use this for initialization
	void Start () {
        //activados = false;
        //transform.GetChild(0); //ponemos un game object a la estacia que sera los que se desactivan
        if (initialChamber)
        {
            desactivados.SetActive(true);
        }else
        {
            desactivados.SetActive(false);
        }
    }
	
	// Update is called once per frame
	void Update () {
        /*if (activados)
        {
            desactivados.SetActive(activados);
            SetCameraSize(cameraSize);
        }*/
    }
    public void ActiveChamber(){
        desactivados.SetActive(true);
        SetCameraSize(cameraSize);
    }
     
    public void DeActiveChamber()
    {
        desactivados.SetActive(false);

    }
    private void SetCameraSize(int size){
        print("CameraModificada");
        switch(size){
            
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
