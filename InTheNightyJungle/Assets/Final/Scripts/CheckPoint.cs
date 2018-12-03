using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckPoint : MonoBehaviour {

    bool insideCheckPoint;
    Animator playerAnimator;
    bool shownZkey;
    public SpriteRenderer Zkey;
    public float distanceOverHead;

    private PlayerPlatformController player;

    public Transform playerPosition;
    public Transform cameraPosition;
    public float cameraSize;

    private bool framedCheckPoint;
    private bool aux;

    private void Start()
    {
        RestartCheckpoint();
    }

    public void RestartCheckpoint()
    {
        framedCheckPoint = false;
        aux = false;
    }

    private void Update()
    {
        if(framedCheckPoint)
        {
            if(!aux)
            {
                player.GetComponent<Animator>().SetBool("Sitting", true);
                aux = true;
            }
            if(player.GetInputActivated())
            {
                UnframeCheckPoint(0.8f);
            }
        }
    }

    private void OnTriggerStay2D()
    {
        if (insideCheckPoint && Input.GetKeyDown(KeyCode.Z) && !player.Descansando)
        {
            StartCoroutine(DisappearZKey(0.5f));
            StartCoroutine(FrameCheckPoint(0.8f));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            player = collision.GetComponent<PlayerPlatformController>();
            player.SetDashActivated(false);
            player.SetBreathActivated(false);
            if (!shownZkey)
                StartCoroutine(AppearZKey(0.5f));

            insideCheckPoint = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            player.SetDashActivated(true);
            player.SetBreathActivated(true);
			if(shownZkey)
				StartCoroutine(DisappearZKey(0.5f));

            insideCheckPoint = false;
        }
    }

    private IEnumerator AppearZKey(float time)
    {
        shownZkey = true;

        float elapsedTime = 0.0f;
        Vector2 initialPosition = Zkey.GetComponent<Transform>().position;
        Vector2 finalPosition = new Vector2(initialPosition.x, initialPosition.y + distanceOverHead);

        Color initialColor = Zkey.color;
        Color finalColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);

        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            Zkey.GetComponent<Transform>().position = Vector2.Lerp(initialPosition, finalPosition, elapsedTime / time);
            Zkey.color = Color.Lerp(initialColor, finalColor, elapsedTime / time);
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

        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            Zkey.GetComponent<Transform>().position = Vector2.Lerp(initialPosition, finalPosition, elapsedTime / time);
            Zkey.color = Color.Lerp(initialColor, finalColor, elapsedTime / time);
            yield return null;
        }
        Zkey.GetComponent<Transform>().position = finalPosition;
        Zkey.color = finalColor;
    }

    private IEnumerator FrameCheckPoint(float time)
	{
		GetComponent<PolygonCollider2D>().enabled = false;
        
        player.SetInputActivated(false);
		player.SetDashActivated(true);
        player.SetBreathActivated(true);
		Camera.main.GetComponent<CameraBehaviour>().SetFollowTarget(false);

		StartCoroutine(player.MoveTo(playerPosition.position, true, time));
		StartCoroutine(Camera.main.GetComponent<CameraBehaviour>().MoveSizeTo(cameraPosition.position, cameraSize, time));

		yield return new WaitForSeconds(time);

		framedCheckPoint = true;
	}

    private void UnframeCheckPoint(float time)
	{
		StartCoroutine(Camera.main.GetComponent<CameraBehaviour>().MoveSizeTo(Camera.main.GetComponent<CameraBehaviour>().GetComponent<Transform>().position, Camera.main.GetComponent<CameraBehaviour>().GetInitialSize(), time));
		player.SetInputActivated(true);
		Camera.main.GetComponent<CameraBehaviour>().SetFollowTarget(true);

        RestartCheckpoint();

		GetComponent<PolygonCollider2D>().enabled = true;
	}

}
