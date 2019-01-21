using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObtainingCollectible : MonoBehaviour {

	private bool catched; //false
	public Sprite collectibleInInventory;
    public string name;
    public string description;
    public GameObject collectiblePrefab;

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
            
            GameObject collectibleObject = Instantiate(collectiblePrefab);
            collectibleObject.GetComponent<CollectibleInfo>().InitializeInfo(collectibleInInventory, name, description);

            if(GameManager.Instance.IsCindyPlaying())
                collectibleObject.transform.SetParent(PantallaPausa.Instance.ContenidoInventarioCindy.transform);
            else
                collectibleObject.transform.SetParent(PantallaPausa.Instance.ContenidoInventarioBrenda.transform);
            Destroy(gameObject);
        }
    }
}
