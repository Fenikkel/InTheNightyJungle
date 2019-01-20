﻿using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCBehaviour : MonoBehaviour {

	private PlayerPlatformController player;
	private CameraBehaviour mainCamera;

	private ChamberManager chamberLocation;

	public Transform conversationPlayerPosition;
	public Transform conversationCameraPosition;
	public bool hasToFlip;

	public TextAsset conversationalTreeText;
	private string[] conversationalTreeLines;

	private List<ConversationalNode> nodeList;
	private ConversationalTree ct;

	public SpriteRenderer Zkey;
	private bool shownZkey;
	public float distanceOverHead;

	private bool framedConversation;
	private bool conversationTime;
	
	private ConversationalNode currentNode;
	private bool options;
	private bool stopConversation;

	public GameObject nextThingToDo;

	public ConversationUIController UI;

    private string actual;

	public int moneyRequired;
	private bool purchasedTicket;

	private bool interactable;

	// Use this for initialization
	void Start () {
		CreateConversationalTree();

		mainCamera = Camera.main.GetComponent<CameraBehaviour>();
		chamberLocation = GetComponentInParent<ChamberManager>();

		framedConversation = false;
		conversationTime = false;
		options = false;
		stopConversation = false;
		purchasedTicket = false;
		interactable = true;
	}

	private void RestartConversation()
	{
		currentNode = ct.GetRoot();

		framedConversation = false;
		conversationTime = false;
		options = false;
		stopConversation = false;

		GetComponent<PolygonCollider2D>().enabled = true;
	}
	
	// Update is called once per frame
	void Update () {
		if(conversationTime)
		{
            if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Space))
            {
                UI.conversationText.text = actual;
            }

            if (UI.GetPreparedForNewText())
			{
				if(Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Space))
				{
					stopConversation = currentNode.GetMessage() != null && currentNode.GetMessage().StartsWith("_");
					if(!stopConversation) SpamText();
				}
			}

			if(options)
			{
				//print(UI.GetOptionSelected() + " " + currentNode.GetMessage());
				if(UI.GetOptionSelected() != -1)
				{
					if(UI.GetOptionSelected() == 0) {

						if(currentNode.GetSomethingToDo() != null && currentNode.GetSomethingToDo().StartsWith("_S_"))
							StartCoroutine(currentNode.GetSomethingToDo().Substring(3));

						NextNode(currentNode.GetChildNode1());
					}
					else {

						if(currentNode.GetSomethingToDo() != null && currentNode.GetSomethingToDo().StartsWith("_N_"))
							StartCoroutine(currentNode.GetSomethingToDo().Substring(3));
							
						NextNode(currentNode.GetChildNode2());
					}
					UI.UnshowOptionsBox();
					stopConversation = currentNode.GetMessage() != null && currentNode.GetMessage().StartsWith("_");
					if(!stopConversation) SpamText();
				}
			}

			if(stopConversation)
			{
				switch(currentNode.GetMessage())
				{
					case "_dance":
						UI.FinishedConversation();
						RestartConversation();
						nextThingToDo.GetComponent<DancingTestManager>().StartTest(player, mainCamera); 
						break;
					case "_drink":
						UI.FinishedConversation();
						RestartConversation();
						nextThingToDo.GetComponent<DrinkingTestManager>().StartTest(player, mainCamera); 
						break;
					case "_endLevel":
						UI.FinishedConversation();
						RestartConversation();
						StartCoroutine(mainCamera.MoveSizeTo(mainCamera.GetComponent<Transform>().position, chamberLocation.GetCameraSize(), 0.5f));
						nextThingToDo.GetComponent<GameManager>().PlayerDone();
						break;
					case "_cancel":
						StartCoroutine(CancelConversation());
						break;
				}
				conversationTime = false;
			}
		}
		else
		{
			if(framedConversation && !stopConversation)
			{
				StartConversation();
			}
		}		
	}

	private void CreateConversationalTree()
	{
		conversationalTreeLines = conversationalTreeText.text.Split("\n"[0]);
		nodeList = new List<ConversationalNode>();
		string node1 = null, node2 = null, message = null, extraCondition = null, somethingToDo = null;
		int line = 0;

		for(int i = 0; i < conversationalTreeLines.Length; i++)
		{
			if(conversationalTreeLines[i][0] == '#' && !conversationalTreeLines[i].StartsWith("#0"))
			{
				nodeList.Add(new ConversationalNode(node1, node2, message, extraCondition, somethingToDo));
				line = 0;
				node1 = node2 = message = extraCondition = somethingToDo = null;
			}
			else
			{
				switch(line)
				{
					case 1:
						if(conversationalTreeLines[i][0] != '-')
							node1 = conversationalTreeLines[i].Substring(0, conversationalTreeLines[i].Length - 1);
						break;
					case 2:
						if(conversationalTreeLines[i][0] != '-')
							node2 = conversationalTreeLines[i].Substring(0, conversationalTreeLines[i].Length - 1);
						break;
					case 3:
						if(conversationalTreeLines[i][0] != '-')
							message = conversationalTreeLines[i].Substring(0, conversationalTreeLines[i].Length - 1);
						break;
					case 4:
						if(conversationalTreeLines[i][0] != '-')
							extraCondition = conversationalTreeLines[i].Substring(0, conversationalTreeLines[i].Length - 1);
						break;
					case 5:
						if(conversationalTreeLines[i][0] != '-')
							somethingToDo = conversationalTreeLines[i].Substring(0, conversationalTreeLines[i].Length - 1);
						break;
				}
			}
			line++;
		}
		nodeList.Add(new ConversationalNode(node1, node2, message, extraCondition, somethingToDo));

		ct = new ConversationalTree(nodeList[0]);

		for(int i = 0; i < nodeList.Count; i++)
		{
			if(nodeList[i].GetNode1() != null) nodeList[i].SetChildNode1( nodeList[ Convert.ToInt32( nodeList[i].GetNode1() ) ] );
			if(nodeList[i].GetNode2() != null) nodeList[i].SetChildNode2( nodeList[ Convert.ToInt32( nodeList[i].GetNode2() ) ] );
		}

		currentNode = ct.GetRoot();
		options = currentNode.GetChildNode2() != null && currentNode.GetExtraCondition() == null;
	}

	public void StartConversation()
	{
		conversationTime = true;

		UI.InitializeConversationBox();
		SpamText();
	}

	private void SpamText()
	{
		actual = currentNode.GetMessage();

		string variableToLocate = "";
		string finalMessage = "";
		bool readingVariable = false;
	
		for(int i = 0; i < actual.Length; i++)
		{
			if(actual[i] == '}')
			{
				finalMessage += Format(variableToLocate);
				variableToLocate = "";
				readingVariable = false;
			}
			else if(actual[i] == '{')
			{
				readingVariable = true;
			}
			else if(readingVariable)
			{
				variableToLocate += actual[i];
			}
			else
			{
				finalMessage += actual[i];
			}
		}
	    currentNode.SetMessage(finalMessage);
		actual = finalMessage;

        UI.SpamText(currentNode.GetMessage(), options);
		if(currentNode.GetSomethingToDo() != null && !(currentNode.GetSomethingToDo().StartsWith("_S_") || currentNode.GetSomethingToDo().StartsWith("_N_")))
			StartCoroutine(currentNode.GetSomethingToDo());

		if(currentNode.GetExtraCondition() != null)
		{
			switch(currentNode.GetExtraCondition())
			{
				case "hasTicket":
					if(purchasedTicket)
					{
						NextNode(currentNode.GetChildNode2());
					}
					else
					{
						NextNode(currentNode.GetChildNode1());
					}
					break;
				case "checkFame":
					if (player.GetComponent<PlayerStatsController>().GetFame() == 3)
					{
						NextNode(currentNode.GetChildNode1());
					}
					else
					{
						NextNode(currentNode.GetChildNode2());
					}
					break;
				case "checkMoney":
					if(purchasedTicket)
					{
						NextNode(currentNode.GetChildNode1());
					}
					else
					{
						NextNode(currentNode.GetChildNode2());
					}
					break;

			}
		}
		else
		{
			if(currentNode.GetChildNode2() == null) 
			{
				NextNode(currentNode.GetChildNode1());
			}
		}
	}

	private void NextNode(ConversationalNode next)
	{
		currentNode = next;
		options = currentNode.GetChildNode2() != null && currentNode.GetExtraCondition() == null && currentNode.GetMessage() != null && !currentNode.GetMessage().StartsWith("_");
	}

	private void PurchaseTicket()
	{
		//Aquí haríamos la disminución de money correspondiente y tal
		if(player.GetComponent<PlayerStatsController>().GetMoney() >= moneyRequired)
		{
			purchasedTicket = true;
			player.GetComponent<PlayerStatsController>().ChangeMoney(-moneyRequired);
		}
	}

	private IEnumerator CancelConversation()
	{
		UI.FinishedConversation();
		StartCoroutine(mainCamera.MoveSizeTo(mainCamera.GetComponent<Transform>().position, chamberLocation.GetCameraSize(), 0.5f));
		yield return new WaitForSeconds(0.5f);
		player.SetInputActivated(true);
		mainCamera.SetFollowTarget(true);

		RestartConversation();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.gameObject.tag.Equals("Player")) 
		{
			player = collision.GetComponent<PlayerPlatformController>();
			if(interactable  && player.GetInputActivated())
			{
				player.SetDashActivated(false);
				player.SetBreathActivated(false);
				if(!shownZkey)
					StartCoroutine(AppearZKey(0.5f));
			}
		}
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		if(interactable && player.GetInputActivated() && shownZkey && collision.gameObject.tag.Equals("Player") && Input.GetKeyDown(KeyCode.Z) && player.GetGrounded())
		{
			GetComponent<PolygonCollider2D>().enabled = false;
			StartCoroutine(DisappearZKey(0.5f));
			StartCoroutine(FrameConversation(0.8f));
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if(interactable && collision.gameObject.tag.Equals("Player") && player.GetInputActivated())
		{
			player.SetDashActivated(true);
			player.SetBreathActivated(true);
			if(shownZkey)
				StartCoroutine(DisappearZKey(0.5f));
		}
	}

	public void SetInteractable(bool param)
	{
		interactable = param;
	}

	private IEnumerator FrameConversation(float time)
	{
		player.SetInputActivated(false);
		player.SetDashActivated(true);
		player.SetBreathActivated(true);
		mainCamera.SetFollowTarget(false);		

		StartCoroutine(player.MoveTo(conversationPlayerPosition.position, hasToFlip, time));
		StartCoroutine(mainCamera.MoveSizeTo(conversationCameraPosition.position, CameraSizes.conversationSize, time));

		yield return new WaitForSeconds(time);

		framedConversation = true;
	}

	private IEnumerator AppearZKey(float time)
	{
		shownZkey = true;

		float elapsedTime = 0.0f;
		Vector2 initialPosition = Zkey.GetComponent<Transform>().position;
		Vector2 finalPosition = new Vector2(initialPosition.x, initialPosition.y + distanceOverHead);

		Color initialColor = Zkey.color;
		Color finalColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);

		while(elapsedTime < time)
		{
			elapsedTime += Time.deltaTime;
			Zkey.GetComponent<Transform>().position = Vector2.Lerp(initialPosition, finalPosition, elapsedTime/time);
			Zkey.color = Color.Lerp(initialColor, finalColor, elapsedTime/time);
			yield return null;
		}
		Zkey.GetComponent<Transform>().position = finalPosition;
		Zkey.color = finalColor;
	}

	private IEnumerator DisappearZKey(float time)
	{
		shownZkey = false;

		float elapsedTime = 0.0f;
		Vector2 initialPosition = Zkey.GetComponent<Transform>().position;
		Vector2 finalPosition = new Vector2(initialPosition.x, initialPosition.y - distanceOverHead);

		Color initialColor = Zkey.color;
		Color finalColor = new Color(1.0f, 1.0f, 1.0f, 0.0f);

		while(elapsedTime < time)
		{
			elapsedTime += Time.deltaTime;
			Zkey.GetComponent<Transform>().position = Vector2.Lerp(initialPosition, finalPosition, elapsedTime/time);
			Zkey.color = Color.Lerp(initialColor, finalColor, elapsedTime/time);
			yield return null;
		}
		Zkey.GetComponent<Transform>().position = finalPosition;
		Zkey.color = finalColor;
	}

	//Este método es lo más contra mis principios que he hecho en mi vida :(
	private string Format(string variableToLocate)
	{
		switch(variableToLocate)
		{
			case "moneyRequired":
				return moneyRequired.ToString();
			default:
				return "";
		}
	}
}
