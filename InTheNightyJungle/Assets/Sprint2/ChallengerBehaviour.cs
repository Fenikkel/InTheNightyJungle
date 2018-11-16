using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChallengerBehaviour : MonoBehaviour {

    //si faig una referencia al TestManager despres no servira per a mostrar la tecla per iniciar conversació
    public GameObject testManager;
    public string[] allDanceBlocks; //esta aqui por si hay mas de una prueba en un nivel
    private bool espera;

	// Use this for initialization
	void Start () {
        espera = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public IEnumerator Dance(char key)
    {
        //yield return new WaitForSeconds(1);

        transform.Find(key.ToString()).GetComponent<KeyShowBehaviour>().ShowIn();
        //do the animation


        yield return new WaitForSeconds(2); //WaitUntil ChallengerDanceAndimation ends

        transform.Find(key.ToString()).GetComponent<KeyShowBehaviour>().ShowOut();

        yield return new WaitForSeconds(1);

    }

    public IEnumerator ChallengerTurn(string keysBlock)
    {
        print("Bloque "+keysBlock);
        string loweredKeysBlock = keysBlock.ToLower();
        for (int i = 0; i< loweredKeysBlock.Length; i++)
        {
           
            print("Inicio");
            yield return new WaitForSeconds(1);

            print(keysBlock[i]);
            
            StartCoroutine(Dance(loweredKeysBlock[i]));

            //WaitUntil KeyFadeOut ends

            yield return new WaitForSeconds(1);
            print("Final");
            yield return new WaitForSeconds(1);
        }
        
        testManager.GetComponent<TestManager>().challengerTurn = false;
        
    }
}
