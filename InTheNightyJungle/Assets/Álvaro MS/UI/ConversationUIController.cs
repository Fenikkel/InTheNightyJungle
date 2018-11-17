using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConversationUIController : MonoBehaviour {

	public Image conversationBox;
	public Text conversationText;
	public Image optionsBox;
	public Text[] optionsTexts;

	public Vector2 opennedBoxPosition;
	public Vector2 opennedBoxSize;
	public Vector2 closedBoxPosition;
	public Vector2 closedBoxSize;

	// Use this for initialization
	void Start () {
		InitializeConversationBox();
	}
	
	public void InitializeConversationBox()
	{
		conversationBox.GetComponent<RectTransform>().anchoredPosition = closedBoxPosition;
		conversationBox.GetComponent<RectTransform>().sizeDelta = closedBoxSize;
	}

	// Update is called once per frame
	void Update () {
		
	}

	public void ShowConversationBox(string firstMessage)
	{
		StartCoroutine(OpenConversationBox(0.2f, firstMessage));
	}

	public void FinishedConversation()
	{
		StartCoroutine(CloseConversationBox(0.2f));
	}

	public void SpamText(string message)
	{
		StartCoroutine(WriteMessage(0.05f, message, 0));
	}

	IEnumerator OpenConversationBox(float time, string firstMessage)
	{
		float elapsedTime = 0.0f;
		Vector2 initialPosition = conversationBox.GetComponent<RectTransform>().anchoredPosition;
		Vector2 initialSize = conversationBox.GetComponent<RectTransform>().sizeDelta;

		while(elapsedTime < time)
		{
			elapsedTime += Time.deltaTime;
			conversationBox.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(initialPosition, opennedBoxPosition, elapsedTime/time);
			conversationBox.GetComponent<RectTransform>().sizeDelta = Vector2.Lerp(initialSize, opennedBoxSize, elapsedTime/time);
			yield return null;
		}

		conversationBox.GetComponent<RectTransform>().anchoredPosition = opennedBoxPosition;
		conversationBox.GetComponent<RectTransform>().sizeDelta = opennedBoxSize;

		conversationText.text = "";
		StartCoroutine(WriteMessage(0.05f, firstMessage, 0));
	}

	IEnumerator CloseConversationBox(float time)
	{
		conversationText.text = "";

		float elapsedTime = 0.0f;
		Vector2 initialPosition = conversationBox.GetComponent<RectTransform>().anchoredPosition;
		Vector2 initialSize = conversationBox.GetComponent<RectTransform>().sizeDelta;

		while(elapsedTime < time)
		{
			elapsedTime += Time.deltaTime;
			conversationBox.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(initialPosition, closedBoxPosition, elapsedTime/time);
			conversationBox.GetComponent<RectTransform>().sizeDelta = Vector2.Lerp(initialSize, closedBoxSize, elapsedTime/time);
			yield return null;
		}

		conversationBox.GetComponent<RectTransform>().anchoredPosition = closedBoxPosition;
		conversationBox.GetComponent<RectTransform>().sizeDelta = closedBoxSize;
	}

	IEnumerator WriteMessage(float timeBetweenLetters, string message, int i)
	{
		if(i < message.Length)
		{
			yield return new WaitForSeconds(timeBetweenLetters);
			conversationText.text += message[i];
			StartCoroutine(WriteMessage(timeBetweenLetters, message, i+1));
		}
	}
}
