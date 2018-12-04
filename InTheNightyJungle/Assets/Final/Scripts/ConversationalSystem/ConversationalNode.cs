using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversationalNode {

	private ConversationalNode childNode1;
	private ConversationalNode childNode2;

	private string node1;
	private string node2;
	private string message;
	private string extraCondition;
	private string somethingToDo;

	public ConversationalNode(string param0, string param1, string param2, string param3, string param4)
	{
		childNode1 = null;
		childNode2 = null;
		
		node1 = param0;
		node2 = param1;
		message = param2;
		extraCondition = param3;
		somethingToDo = param4;
	}

	public ConversationalNode GetChildNode1()
	{
		return childNode1;
	}

	public ConversationalNode GetChildNode2()
	{
		return childNode2;
	}

	public string GetNode1()
	{
		return node1;
	}

	public string GetNode2()
	{
		return node2;
	}

	public string GetMessage()
	{
		return message;
	}

	public string GetExtraCondition()
	{
		return extraCondition;
	}

	public string GetSomethingToDo()
	{
		return somethingToDo;
	}

	public void SetMessage(string param)
	{
		message = param;
	}

	public void SetChildNode1(ConversationalNode param1)
	{
		childNode1 = param1;
	}

	public void SetChildNode2(ConversationalNode param)
	{
		childNode2 = param;
	}

	public string PrintNode()
	{
		return "LeftNode: " + node1 + "\n" +
				"RightNode: " +  node2 + "\n" + 
				"Message: " + message + "\n" + 
				"ExtraCondition: " + extraCondition + "\n" + 
				"SomethingToDo: " + somethingToDo;
	}
}
