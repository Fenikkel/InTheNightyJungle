using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrancotiradorEffectZoneBehaviour : MonoBehaviour {

    public FrancotiradorBehaviour franco;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag.Equals("Player"))
            franco.SetTarget(collision.gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player"))
        {
            franco.SetTarget(null);
        }
    }
}
