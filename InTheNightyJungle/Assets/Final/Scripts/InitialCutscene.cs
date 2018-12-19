using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class InitialCutscene : MonoBehaviour {

	private GameObject Cindy;
	private GameObject Brenda;
    private GameObject blackScreen;
	private CameraBehaviour mainCamera;

	public Transform CindyPositionInBuilding;
	public Transform BrendaPositionInBuilding;
	public Transform CentralPositionInBuilding;

	public TextAsset conversationalTreeText;
	private string[] conversationalTreeLines;

	private List<ConversationalNode> nodeList;
	private ConversationalTree ct;

	private bool conversationTime;
	
	private ConversationalNode currentNode;
	private bool options;
	private bool stopConversation;
	private bool transitionDone;
	private string transitionToDo;
	private bool duringTransition;

	public ConversationUIController UI;

    private string actual;

	// Use this for initialization
	void Start () {
		CreateConversationalTree();
		RestartConversation();
		transitionDone = true;
		duringTransition = false;
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
					if(!duringTransition && !transitionDone && transitionToDo != null)
					{
						switch(transitionToDo)
						{
							case "buildingTransitionToBrenda":
								StartCoroutine(BuildingTransition(true));
								break;
							case "buildingTransitionToCindy":
								StartCoroutine(BuildingTransition(false));
								break;
							case "normalTransitionToBrenda":
								StartCoroutine(NormalTransition(true));
								break;
							case "normalTransitionToCindy":
								StartCoroutine(NormalTransition(false));
								break;
						}
					}
					else
					{
						if(transitionToDo == null && transitionDone)
						{
							stopConversation = currentNode.GetMessage() != null && currentNode.GetMessage().StartsWith("_");
							if(!stopConversation) SpamText();
						}
					}
				}

				if(transitionDone)
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
					case "_finished":
						UI.FinishedConversation();
						RestartConversation();
						FinishedCutscene();
						break;
				}
				conversationTime = false;
			}
		}
	}

	public void BeginCutscene(GameObject param0, GameObject param1, GameObject param2, CameraBehaviour param3)
	{
		Cindy = param0;
		Brenda = param1;
		blackScreen = param2;
		mainCamera = param3;

		StartCoroutine(Cutscene());
	}

	private void FinishedCutscene()
	{
		GetComponent<GameManager>().StartGame();
		Cindy.GetComponent<Animator>().Play("idle");
		Brenda.GetComponent<Animator>().Play("idle");
	}

	private IEnumerator Cutscene()
	{
		mainCamera.SetFollowTarget(false);

		Vector3 newPosition = new Vector3(Cindy.GetComponent<Transform>().position.x, Cindy.GetComponent<Transform>().position.y, mainCamera.GetComponent<Transform>().position.z);
		StartCoroutine(mainCamera.MoveSizeTo(newPosition, 2f, 1f));
		yield return new WaitForSeconds(0.5f);

		PlayPhoneRinging();
		yield return new WaitForSeconds(2f);

		Cindy.GetComponent<Animator>().Play("answeringPhone");
		Brenda.GetComponent<Animator>().Play("talkingPhone");
		yield return new WaitForSeconds(1.5f);

		StartConversation();

		//StartCoroutine(BuildingTransition(true));
	}

	private IEnumerator BuildingTransition(bool toBrenda)
	{		
		duringTransition = true;

		Vector3 firstPosition;
		Vector3 secondPosition = new Vector3(CentralPositionInBuilding.position.x, CentralPositionInBuilding.position.y, mainCamera.GetComponent<Transform>().position.z);
		Vector3 thirdPosition;
		Vector3 finalPosition;
		if(toBrenda)
		{
			firstPosition = new Vector3(CindyPositionInBuilding.position.x, CindyPositionInBuilding.position.y, mainCamera.GetComponent<Transform>().position.z);
			thirdPosition = new Vector3(BrendaPositionInBuilding.position.x, BrendaPositionInBuilding.position.y, mainCamera.GetComponent<Transform>().position.z);
			finalPosition = new Vector3(Brenda.GetComponent<Transform>().position.x, Brenda.GetComponent<Transform>().position.y, mainCamera.GetComponent<Transform>().position.z);
		}
		else
		{
			firstPosition = new Vector3(BrendaPositionInBuilding.position.x, BrendaPositionInBuilding.position.y, mainCamera.GetComponent<Transform>().position.z);
			thirdPosition = new Vector3(CindyPositionInBuilding.position.x, CindyPositionInBuilding.position.y, mainCamera.GetComponent<Transform>().position.z);
			finalPosition = new Vector3(Cindy.GetComponent<Transform>().position.x, Cindy.GetComponent<Transform>().position.y, mainCamera.GetComponent<Transform>().position.z);
		}
		StartCoroutine(FadeIn(1f));
		StartCoroutine(mainCamera.MoveSizeTo(mainCamera.GetComponent<Transform>().position, 10f, 1f));
		yield return new WaitForSeconds(1f);

		mainCamera.GetComponent<Transform>().position = firstPosition;

		StartCoroutine(FadeOut(1f));
		StartCoroutine(mainCamera.MoveSizeTo(secondPosition, 25f, 1f));
		yield return new WaitForSeconds(1.5f);

		StartCoroutine(mainCamera.MoveSizeTo(thirdPosition, 10f, 1f));
		StartCoroutine(FadeIn(1f));
		yield return new WaitForSeconds(1f);

		mainCamera.GetComponent<Transform>().position = finalPosition;
		
		StartCoroutine(FadeOut(1f));
		StartCoroutine(mainCamera.MoveSizeTo(mainCamera.GetComponent<Transform>().position, 2f, 1f));
		yield return new WaitForSeconds(1.5f);

		transitionDone = true;
		duringTransition = false;
	}

	private IEnumerator NormalTransition(bool toBrenda)
	{
		duringTransition = true;

		Vector3 finalPosition;

		if(toBrenda)
		{
			finalPosition = new Vector3(Brenda.GetComponent<Transform>().position.x, Brenda.GetComponent<Transform>().position.y, mainCamera.GetComponent<Transform>().position.z);
		}
		else
		{
			finalPosition = new Vector3(Cindy.GetComponent<Transform>().position.x, Cindy.GetComponent<Transform>().position.y, mainCamera.GetComponent<Transform>().position.z);
		}

		StartCoroutine(FadeIn(1f));
		StartCoroutine(mainCamera.MoveSizeTo(mainCamera.GetComponent<Transform>().position, 10f, 1f));
		yield return new WaitForSeconds(1f);

		mainCamera.GetComponent<Transform>().position = finalPosition;

		StartCoroutine(FadeOut(1f));
		StartCoroutine(mainCamera.MoveSizeTo(mainCamera.GetComponent<Transform>().position, 2f, 1f));
		yield return new WaitForSeconds(1.5f);

		transitionDone = true;
		duringTransition = false;
	}

	public void PlayPhoneRinging()
	{
		GetComponent<AudioSource>().Play();
	}

	public void StopPhoneRinging()
	{
		GetComponent<AudioSource>().Stop();
	}

	IEnumerator FadeOut(float time)
    {
        Color c = blackScreen.GetComponent<Image>().color;
        Color initialColor = c;
        Color finalColor = new Color(0, 0, 0, 0);
        float elapsedTime = 0.0f;
        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            c = Color.Lerp(initialColor, finalColor, elapsedTime/time);
            blackScreen.GetComponent<Image>().color = c;
            yield return null;
        }
        blackScreen.GetComponent<Image>().color = finalColor;
    }

	IEnumerator FadeIn(float time)
    {
        Color c = blackScreen.GetComponent<Image>().color;
        Color initialColor = c;
        Color finalColor = new Color(0, 0, 0, 1);
        float elapsedTime = 0.0f;
        while(elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            c = Color.Lerp(initialColor, finalColor, elapsedTime/time);
            blackScreen.GetComponent<Image>().color = c;
            yield return null;
        }
        blackScreen.GetComponent<Image>().color = finalColor;
    }

	private void RestartConversation()
	{
		currentNode = ct.GetRoot();

		conversationTime = false;
		options = false;
		stopConversation = false;
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

        UI.SpamText(currentNode.GetMessage(), options);
		if(currentNode.GetSomethingToDo() != null && !(currentNode.GetSomethingToDo().StartsWith("_S_") || currentNode.GetSomethingToDo().StartsWith("_N_")))
		{
			transitionDone = false;
			transitionToDo = currentNode.GetSomethingToDo();
		}
		else
		{
			transitionToDo = null;
		}

		if(currentNode.GetExtraCondition() != null)
		{
			switch(currentNode.GetExtraCondition())
			{
				default:
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

	//Este método es lo más contra mis principios que he hecho en mi vida :(
	/* private string Format(string variableToLocate)
	{
		switch(variableToLocate)
		{
			default:
				return "";
		}
	}*/
}
