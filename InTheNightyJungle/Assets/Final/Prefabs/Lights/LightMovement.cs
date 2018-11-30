using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightMovement : MonoBehaviour {

	public float angleMin;
	public float angleMax;
	public float speed;

	private float currentAngle;
	private bool direction;

	private Transform tf;

	// Use this for initialization
	void Start () {
		tf = GetComponent<Transform>();
		currentAngle = tf.eulerAngles.z;
		direction = true;
	}
	
	// Update is called once per frame
	void Update () {
		if(direction)
		{
			currentAngle += speed * Time.deltaTime;
			if(currentAngle >= angleMax)
			{
				currentAngle = angleMax;
				direction = false;
			}
		}
		else
		{
			currentAngle -= speed * Time.deltaTime;
			if(currentAngle <= angleMin)
			{
				currentAngle = angleMin;
				direction = true;
			}
		}
		tf.eulerAngles = new Vector3(tf.eulerAngles.x, tf.eulerAngles.y, currentAngle);
	}
}
