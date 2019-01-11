using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Depurando : MonoBehaviour {

	public GameObject coso;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Depurar"))
		{
			coso.SetActive(!coso.activeSelf);
		}
	}
}
