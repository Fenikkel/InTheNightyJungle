using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour {

	public GameObject inventory;
	public GameObject inventoryContentCindy;
	public GameObject inventoryContentBrenda;

	public GameObject itemImage;
	public GameObject itemName;
	public GameObject itemDescription;
	public GameObject inventoryCindy;
	public GameObject inventoryBrenda;

	private List<CollectibleInfo> CindyItemList;
	private List<CollectibleInfo> BrendaItemList;

	private bool cindyInventoryOpenned;

	// Use this for initialization
	void Awake()
	{
		CindyItemList = new List<CollectibleInfo>();
		BrendaItemList = new List<CollectibleInfo>();
	}

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public GameObject GetInventoryContent(bool param)
	{
		return param ? inventoryContentCindy : inventoryContentBrenda;
	}

	public void AddItem(CollectibleInfo item, bool cindy)
	{
		if(cindy) 
		{
			item.SetItemID(CindyItemList.Count);
			CindyItemList.Add(item);
		}
		else
		{
			item.SetItemID(BrendaItemList.Count);
			BrendaItemList.Add(item);
		}
		item.GetComponent<Button>().onClick.AddListener(delegate { ShowItem(item.GetItemID()); } );	
	}

	public void ShowInventory(bool cindy)
	{
		inventory.SetActive(true);
		cindyInventoryOpenned = cindy;
		if(cindyInventoryOpenned) {
			inventoryCindy.SetActive(true);
			inventoryBrenda.SetActive(false);
		}
		else
		{
			inventoryCindy.SetActive(false);
			inventoryBrenda.SetActive(true);
		}
	}

	public void UnshowInventory()
	{
		inventoryBrenda.SetActive(false);
		inventoryCindy.SetActive(false);
		UnshowItem();
		inventory.SetActive(false);
	}

	public void ShowItem(int id)
	{
		itemImage.GetComponent<Image>().color = new Color(1,1,1,1);
		CollectibleInfo item = cindyInventoryOpenned ? CindyItemList[id] : BrendaItemList[id];
		
		itemImage.GetComponent<Image>().sprite = item.GetSpriteInInventory();
		itemName.GetComponent<Text>().text = item.GetName();
		itemDescription.GetComponent<Text>().text = item.GetDescription();
	}

	public void UnshowItem()
	{
		itemImage.GetComponent<Image>().sprite = null;
		itemName.GetComponent<Text>().text = "";
		itemDescription.GetComponent<Text>().text = "";

		itemImage.GetComponent<Image>().color = new Color(0,0,0,0);
	}
}
