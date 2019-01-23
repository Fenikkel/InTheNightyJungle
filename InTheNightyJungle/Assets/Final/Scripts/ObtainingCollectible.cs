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

    [SerializeField]
    private AudioSource collectibleSound;

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
            collectibleSound.Play();

            catched = true; //Para que no se pongan dos veces en el inventario
            
            GameObject collectibleObject = Instantiate(collectiblePrefab);
            collectibleObject.GetComponent<CollectibleInfo>().InitializeInfo(collectibleInInventory, name, description);

            GeneralUIController.Instance.GetComponent<InventoryUI>().AddItem(collectibleObject.GetComponent<CollectibleInfo>(), GameManager.Instance.IsCindyPlaying());

            collectibleObject.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
            Destroy(gameObject);
        }
    }
}
