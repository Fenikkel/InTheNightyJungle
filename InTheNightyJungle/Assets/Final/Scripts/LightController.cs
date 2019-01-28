using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LightController : MonoBehaviour {

	public bool glowing;
	public float minIntensity;
	public float maxIntensity;
	public float glowSpeed;

	public bool rotate;
	public float minAngle;
	public float maxAngle;
	public float rotateSpeed;

	public bool scale;
	public float minSize;
	public float maxSize;
	public float scaleSpeed;

	public bool blink;
	public float timeBlinking;
	public float timeUnblinking;

	public bool changeColor;
	public Color[] colors;
	public float timeStayingColor;
	public float timeChangingColor;

	private float currentIntensity;
	private bool glowingDirection;

	private float currentAngle;
	private bool rotationDirection;

	private float currentSize;
	private Vector2 originalSize;
	private bool scalingDirection;

	private float currentBlinkingTime;
	private bool blinking;

	private bool changingColor;
	private int currentColor;

	private Transform tf;
	private Color lightColor;
	private SpriteMask mask;

	// Use this for initialization
	void Start () {
		tf = GetComponent<Transform>();
		lightColor = GetComponentInChildren<SpriteRenderer>().color;
		mask = GetComponent<SpriteMask>();

		currentIntensity = lightColor.a;
		currentAngle = tf.eulerAngles.z;
		currentSize = 1;
		originalSize = tf.localScale;
		currentColor = 0;
		changingColor = true;

		if(changeColor)
		{
			GetComponentInChildren<SpriteRenderer>().color = new Color(colors[currentColor].r, colors[currentColor].g, colors[currentColor].b, lightColor.a);
			StartCoroutine(ChangeColor());
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(rotate)
		{
			Rotate();
		}
		if(glowing)
		{
			Glow();
		}
		if(scale)
		{
			Scale();
		}
		if(blink)
		{
			Blink();
		}
	}

	private void Rotate()
	{
		if(rotationDirection)
		{
			currentAngle += rotateSpeed * Time.deltaTime;
			if(currentAngle >= maxAngle)
			{
				currentAngle = maxAngle;
				rotationDirection = false;
			}
		}
		else
		{
			currentAngle -= rotateSpeed * Time.deltaTime;
			if(currentAngle <= minAngle)
			{
				currentAngle = minAngle;
				rotationDirection = true;
			}
		}
		tf.eulerAngles = new Vector3(tf.eulerAngles.x, tf.eulerAngles.y, currentAngle);
	}

	private void Glow()
	{
		if(glowingDirection)
		{
			currentIntensity += glowSpeed * Time.deltaTime;
			if(currentIntensity >= maxIntensity)
			{
				currentIntensity = maxIntensity;
				glowingDirection = false;
			}
		}
		else
		{
			currentIntensity -= glowSpeed * Time.deltaTime;
			if(currentIntensity <= minIntensity)
			{
				currentIntensity = minIntensity;
				glowingDirection = true;
			}
		}
		lightColor = new Color(lightColor.r, lightColor.g, lightColor.b, currentIntensity);
		GetComponentInChildren<SpriteRenderer>().color = lightColor;
	}

	private void Scale()
	{
		if(scalingDirection)
		{
			currentSize += scaleSpeed * Time.deltaTime;
			if(currentSize >= maxSize)
			{
				currentSize = maxSize;
				scalingDirection = false;
			}
		}
		else
		{
			currentSize -= scaleSpeed * Time.deltaTime;
			if(currentSize <= minSize)
			{
				currentSize = minSize;
				scalingDirection = true;
			}
		}
		tf.localScale = new Vector2(originalSize.x * currentSize, originalSize.y * currentSize);
	}

	private void Blink()
	{
		if(!blinking)
		{
			currentBlinkingTime += Time.deltaTime;
			if(currentBlinkingTime >= timeUnblinking)
			{
				currentBlinkingTime = 0;
				blinking = true;
			}
		}
		else
		{
			currentBlinkingTime += Time.deltaTime;
			if(currentBlinkingTime >= timeBlinking)
			{
				currentBlinkingTime = 0;
				blinking = false;
			}
		}
		mask.enabled = !blinking;
	}

	private IEnumerator ChangeColor()
	{
		if(changingColor)
		{
			changingColor = false;
			StartCoroutine(InterpolateColor());
		}
		else
		{
			yield return new WaitForSeconds(timeChangingColor + timeStayingColor);
			changingColor = true;
		}
		StartCoroutine(ChangeColor());
	}

	private IEnumerator InterpolateColor()
	{
		float elapsedTime = 0.0f;
		Color initialColor = lightColor;
		Color finalColor = (currentColor < colors.Length - 1) ? colors[currentColor + 1] : colors[0];
		Color c;

		while(elapsedTime < timeChangingColor)
		{
			elapsedTime += Time.deltaTime;
			c = Color.Lerp(initialColor, finalColor, elapsedTime / timeChangingColor);
			GetComponentInChildren<SpriteRenderer>().color = new Color(c.r, c.g, c.b, lightColor.a);
			yield return null;
		}
		GetComponentInChildren<SpriteRenderer>().color = new Color(finalColor.r, finalColor.g, finalColor.b, lightColor.a);
		lightColor = GetComponentInChildren<SpriteRenderer>().color;
		currentColor = (currentColor < colors.Length - 1) ? currentColor + 1 : 0;
	}
}
