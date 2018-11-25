using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class NPCBehaviour : MonoBehaviour {

	private PlayerPlatformController player;
	private CameraBehaviour camera;

	public Transform conversationPlayerPosition;
	public Transform conversationCameraPosition;
	public float conversationCameraSize;
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

	// Use this for initialization
	void Start () {
		CreateConversationalTree();

		camera = Camera.main.GetComponent<CameraBehaviour>();

		framedConversation = false;
		conversationTime = false;
		options = false;
		stopConversation = false;
	}

	private void RestartConversation()
	{
		currentNode = ct.GetRoot();

		framedConversation = false;
		conversationTime = false;
		options = false;
		stopConversation = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(conversationTime)
		{
			if(UI.GetPreparedForNewText())
			{
				if(Input.GetKeyDown(KeyCode.Z))
				{
					SpamText();
				}
			}

			if(options)
			{
				if(UI.GetOptionSelected() != -1)
				{
					if(UI.GetOptionSelected() == 0) {

						if(currentNode.GetSomethingToDo() != null && currentNode.GetSomethingToDo().StartsWith("_S_"))
							Invoke(currentNode.GetSomethingToDo().Substring(3), 0);

						NextNode(currentNode.GetChildNode1());
					}
					else {

						if(currentNode.GetSomethingToDo() != null && currentNode.GetSomethingToDo().StartsWith("_N_"))
							Invoke(currentNode.GetSomethingToDo().Substring(3), 0);
							
						NextNode(currentNode.GetChildNode2());
					}
					UI.UnshowOptionsBox();
				}
			}

			if(stopConversation)
			{
				switch(currentNode.GetMessage())
				{
					case "_dance":
						UI.FinishedConversation();
						nextThingToDo.GetComponent<DancingTestManager>().StartTest(player, camera);
						break;
					case "_cancel":
						CancelConversation();
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
		GetComponent<PolygonCollider2D>().enabled = false;

		UI.InitializeConversationBox();
		SpamText();
	}

	private void SpamText()
	{
		UI.SpamText(currentNode.GetMessage(), options);
		if(currentNode.GetSomethingToDo() != null && !(currentNode.GetSomethingToDo().StartsWith("_S_") || currentNode.GetSomethingToDo().StartsWith("_N_")))
			Invoke(currentNode.GetSomethingToDo(), 0);

		if(currentNode.GetExtraCondition() != null)
		{
			switch(currentNode.GetExtraCondition())
			{
				case "hasTicket":
					NextNode(currentNode.GetChildNode2());
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
		options = currentNode.GetChildNode2() != null && currentNode.GetExtraCondition() == null;
		stopConversation = currentNode.GetMessage().StartsWith("_");
	}

	private void PurchaseTicket()
	{
		//Aquí haríamos la disminución de money correspondiente y tal
	}

	private void CancelConversation()
	{
		UI.FinishedConversation();
		StartCoroutine(camera.MoveSizeTo(camera.GetComponent<Transform>().position, camera.GetInitialSize(), 0.5f));
		player.SetInputActivated(true);
		camera.SetFollowTarget(true);

		RestartConversation();

		GetComponent<PolygonCollider2D>().enabled = true;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.gameObject.tag.Equals("Player"))
		{
			player = collision.GetComponent<PlayerPlatformController>();
			player.SetDashActivated(false);
			if(!shownZkey)
				StartCoroutine(AppearZKey(0.5f));
		}
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		if(shownZkey && collision.gameObject.tag.Equals("Player") && Input.GetKeyDown(KeyCode.Z) && player.GetGrounded())
		{
			StartCoroutine(DisappearZKey(0.5f));
			StartCoroutine(FrameConversation( 0.8f));
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if(collision.gameObject.tag.Equals("Player"))
		{
			player.SetDashActivated(true);
			if(shownZkey)
				StartCoroutine(DisappearZKey(0.5f));
		}
	}

	private IEnumerator FrameConversation(float time)
	{
		player.SetInputActivated(false);
		player.SetDashActivated(true);
		camera.SetFollowTarget(false);

		StartCoroutine(player.MoveTo(conversationPlayerPosition.position, hasToFlip, time));
		StartCoroutine(camera.MoveSizeTo(conversationCameraPosition.position, conversationCameraSize, time));

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
}
