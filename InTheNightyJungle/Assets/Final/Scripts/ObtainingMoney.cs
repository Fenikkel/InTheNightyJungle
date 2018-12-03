using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObtainingMoney : MonoBehaviour {

    public float moneyValue;
    bool catched; //false

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player") && !catched)
        {
            catched = true; //Para que no se pongan dos veces en el inventario
            collision.GetComponent<PlayerPlatformController>().DecreaseCansancio(moneyValue);
            GameObject g = new GameObject();
            g.AddComponent<Image>();
            g.GetComponent<Image>().sprite = GetComponent<SpriteRenderer>().sprite;
            g.transform.parent = PantallaPausa.Instance.ContenidoInventario.transform;
            Destroy(gameObject);
        }
    }
}
