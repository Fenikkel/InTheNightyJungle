using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObtainingMoney : MonoBehaviour {

    public int moneyValue;
    private bool aux;

	// Use this for initialization
	void Start () {
        aux = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            if(!aux)
            {
                collision.GetComponent<PlayerStatsController>().ChangeMoney(moneyValue);
                aux = true;
            }
            gameObject.SetActive(false);
        }
    }
}
