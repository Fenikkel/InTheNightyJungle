using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevoradorEffectZoneBehaviour : MonoBehaviour {

    public DevoradorBehaviour devorador;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag.Equals("Player"))
            devorador.SetInside(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player"))
        {
            devorador.SetInside(false);
            collision.GetComponent<PlayerPlatformController>().SpeedToOriginal();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player"))
            devorador.Whispering(collision.gameObject);
    }
}
