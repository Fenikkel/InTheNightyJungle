using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tempScript : MonoBehaviour {

	public Transform[] positions;

	private int currentPosition;

	// Use this for initialization
	void Start () {
		currentPosition = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.A))
		{
			transform.position = positions[currentPosition].position;
			currentPosition++;
			if(currentPosition == positions.Length) currentPosition = 0;
		}
	}
}
