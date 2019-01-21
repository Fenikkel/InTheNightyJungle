﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectibleInfo : MonoBehaviour {

	private Sprite spriteInInventory;
	private string name;
	private string description;
	private int itemID;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void InitializeInfo(Sprite param0, string param1, string param2)
	{
		spriteInInventory = param0;
		GetComponent<Image>().sprite = spriteInInventory;
		name = param1;
		description = param2;
	}

	public Sprite GetSpriteInInventory()
	{
		return spriteInInventory;
	}

	public string GetName()
	{
		return name;
	}

	public string GetDescription()
	{
		return description;
	}

	public int GetItemID()
	{
		return itemID;
	}

	public void SetItemID(int param)
	{
		itemID = param;
	}
}
