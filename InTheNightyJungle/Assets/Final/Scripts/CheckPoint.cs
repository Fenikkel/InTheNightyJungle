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

    private void Start()
    {
        playerAnimator = PlayerPlatformController.Instance.GetComponent<Animator>();
    }

    private void OnTriggerStay2D()
    {
        if (insideCheckPoint && Input.GetKeyDown(KeyCode.Z) && !PlayerPlatformController.Instance.Descansando)
        {
            PlayerPlatformController.Instance.SetInputActivated(false);
            playerAnimator.SetBool("Sitting", true);
            StartCoroutine(DisappearZKey(0.5f));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            PlayerPlatformController.Instance.SetDashActivated(false);
            if (!shownZkey)
                StartCoroutine(AppearZKey(0.5f));


            insideCheckPoint = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
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

}
