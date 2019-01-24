using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObtainingMoney : MonoBehaviour {

    public int moneyValue;
    private bool catched;

	// Use this for initialization
	void Start () {
        catched = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void StolenByPlayer()
    {
        StartCoroutine(Scale(GetComponent<Transform>().localScale, new Vector3(0,0,0), 1.0f));
        StartCoroutine(MoveTo(GetComponent<Transform>().localPosition, new Vector3(0,0,0), 1.0f));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player") && !catched)
        {
            catched = true;
            ObtainMoney();
        }
    }

    private void ObtainMoney()
    {
        PlayerStatsController player = GameManager.Instance.IsCindyPlaying() ? GameManager.Instance.Cindy.GetComponent<PlayerStatsController>() : GameManager.Instance.Brenda.GetComponent<PlayerStatsController>();
        player.ChangeMoney(moneyValue);
        Destroy(gameObject);
    }

    private IEnumerator Scale(Vector3 initialScale, Vector3 finalScale, float time)
    {
        float elapsedTime = 0.0f;
        while(elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            GetComponent<Transform>().localScale = Vector3.Lerp(initialScale, finalScale, elapsedTime / time);
            yield return null;
        }
        GetComponent<Transform>().localScale = finalScale;

        ObtainMoney();
    }

    private IEnumerator MoveTo(Vector3 initialPosition, Vector3 finalPosition, float time)
    {
        float elapsedTime = 0.0f;
        while(elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            GetComponent<Transform>().localPosition = Vector3.Lerp(initialPosition, finalPosition, elapsedTime / time);
            yield return null;
        }
        GetComponent<Transform>().localPosition = finalPosition;
    }
}
