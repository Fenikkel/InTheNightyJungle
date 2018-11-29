using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversationalTree {

	private ConversationalNode root;

	public ConversationalTree(ConversationalNode param)
	{
		root = param;
	}

	public ConversationalNode GetRoot()
	{
		return root;
	}
}
