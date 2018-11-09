using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObtainingMoney : MonoBehaviour {

    public float moneyValue;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            collision.GetComponent<PlayerPlatformController>().DecreaseCansancio(moneyValue);
            gameObject.SetActive(false);
        }
    }
}
