using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObtainingCollectible : MonoBehaviour {

	private bool catched; //false
	public Sprite collectibleInInventory;

	// Use this for initialization
	void Start () {
		catched = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player") && !catched)
        {
            catched = true; //Para que no se pongan dos veces en el inventario
            GameObject g = new GameObject();
            g.AddComponent<Image>();
            g.GetComponent<Image>().sprite = collectibleInInventory;
            g.transform.SetParent(PantallaPausa.Instance.ContenidoInventario.transform);
            Destroy(gameObject);
        }
    }
}
