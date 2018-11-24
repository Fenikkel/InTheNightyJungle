using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionPlatform : PhysicsObject {

	//protected Vector2 distance;
	public float speed;

	// Use this for initialization
	protected override void initialization()
	{
		base.initialization();
		gravityModifier = 0;
	}
	

	public Vector2 GetDistance()
	{
		return move * distance;
	}
}
