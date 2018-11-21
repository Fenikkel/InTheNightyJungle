using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChallengerBehaviour : MonoBehaviour {
    
    private bool dancing;
    private bool shownKey;

    private Animator anim;

    public SpriteRenderer[] visualKeys;
	private SpriteRenderer currentKey;
    public float distanceOverHead;

    void Start()
    {
        dancing = false;
        shownKey = false;

        anim = GetComponent<Animator>();
    }

    void Update()
    {

    }

    public void DancingMovement(string trigger)
    {
        dancing = true;
        anim.SetTrigger(trigger);
    }

    public void DancingMovementStops()
    {
        dancing = false;
		StartCoroutine(DisappearKey(currentKey, 0.5f));
    }

    public void ShowKey(int i)
    {
        StartCoroutine(AppearKey(visualKeys[i], 0.5f));
		currentKey = visualKeys[i];
    }

    public bool HasFinished()
    {
        return !dancing && !shownKey;
    }

    private IEnumerator AppearKey(SpriteRenderer key, float time)
	{
		shownKey = true;

		float elapsedTime = 0.0f;
		Vector2 initialPosition = new Vector2(GetComponent<Transform>().position.x, key.GetComponent<Transform>().position.y);
		Vector2 finalPosition = new Vector2(initialPosition.x, initialPosition.y + distanceOverHead);

		Color initialColor = new Color(1.0f, 1.0f, 1.0f, 0.0f);
		Color finalColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);

        key.color = initialColor;

		while(elapsedTime < time)
		{
			elapsedTime += Time.deltaTime;
			key.GetComponent<Transform>().position = Vector2.Lerp(initialPosition, finalPosition, elapsedTime/time);
			key.color = Color.Lerp(initialColor, finalColor, elapsedTime/time);
			yield return null;
		}
		key.color = finalColor;
		key.GetComponent<Transform>().position = finalPosition;
	}

	private IEnumerator DisappearKey(SpriteRenderer key, float time)
	{
		float elapsedTime = 0.0f;
		Vector2 initialPosition = key.GetComponent<Transform>().position;
		Vector2 finalPosition = new Vector2(initialPosition.x, initialPosition.y - distanceOverHead);

		Color initialColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
		Color finalColor = new Color(1.0f, 1.0f, 1.0f, 0.0f);

        key.color = initialColor;

		while(elapsedTime < time)
		{
			elapsedTime += Time.deltaTime;
			key.GetComponent<Transform>().position = Vector2.Lerp(initialPosition, finalPosition, elapsedTime/time);
			key.color = Color.Lerp(initialColor, finalColor, elapsedTime/time);
			yield return null;
		}
		key.color = finalColor;
		key.GetComponent<Transform>().position = finalPosition;

		shownKey = false;

		currentKey = null;
	}
}
