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

    public void InitializeInfo(Sprite param0, string param1, string param2)
    {
        collectibleInInventory = param0;
        name = param1;
        description = param2;
    }

    public void StolenByPlayer()
    {
        StartCoroutine(Scale(GetComponent<Transform>().localScale, new Vector3(0,0,0), 1.0f));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player") && !catched)
        {
            catched = true; //Para que no se pongan dos veces en el inventario
            
            ObtainCollectible();
        }
    }

    private void ObtainCollectible()
    {
        collectibleSound.Play();

        GameObject collectibleObject = Instantiate(collectiblePrefab);
        collectibleObject.GetComponent<CollectibleInfo>().InitializeInfo(collectibleInInventory, name, description);

        GeneralUIController.Instance.GetComponent<PauseMenu>().AddItem(collectibleObject.GetComponent<CollectibleInfo>(), GameManager.Instance.IsCindyPlaying());

        collectibleObject.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
        Destroy(gameObject);
    }

    private IEnumerator Scale(Vector3 initialScale, Vector3 finalScale, float time)
    {
        float elapsedTime = 0.0f;
        while(elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            GetComponent<Transform>().localScale = Vector3.Lerp(initialScale, finalScale, elapsedTime / time);
            yield return null;
        }
        GetComponent<Transform>().localScale = finalScale;

        ObtainCollectible();
    }
}
