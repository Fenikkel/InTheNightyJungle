using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggeredConveration : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerStay2D(Collider2D other)
    {
        other.gameObject.GetComponent<PlayerPlatformController>().StartConversation();
    }
}
