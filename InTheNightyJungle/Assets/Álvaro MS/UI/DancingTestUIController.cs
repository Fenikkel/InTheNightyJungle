using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DancingTestUIController : MonoBehaviour {

    public Text completedBlocksText;
    private int numCompletedBlocks;
    private int numTotalBlocks;
    private string completedBlocksTextBase = "Bloques completados: ";

    private float timeBarValue;
    public Image timeBar;
    public float increaseTimeBarSize;

    private float totalTime = 60;
    private float aux = 60;

	// Use this for initialization
	void Start () {
        InitializeUI(10);
	}
	
	// Update is called once per frame
	void Update () {
        aux -= Time.deltaTime;
        UpdateTimeBar(aux/totalTime);

        if(Input.anyKey)
        {
            aux += 10;
            if (aux > 60) aux = totalTime;
            UpdateTimeBar(aux/totalTime);
        }
	}

    public void InitializeUI(int param0)
    {
        numTotalBlocks = param0;
        numCompletedBlocks = 0;
        timeBarValue = 1;
        timeBar.fillAmount = timeBarValue;
    }

    public void CompletedBlock()
    {
        numCompletedBlocks++;

    }

    public void UpdateCompletedBlocksText()
    {
        completedBlocksText.text = completedBlocksTextBase + numCompletedBlocks + "/" + numTotalBlocks;
    }

    public void UpdateTimeBar(float newValue)
    {
        if(newValue < timeBarValue)
        {
            timeBarValue = newValue;
            timeBar.fillAmount = timeBarValue;
        }
        else
        { 
            StartCoroutine(IncreaseTime(newValue, 1f));
        }
    }

    IEnumerator IncreaseTime(float newValue, float time)
    {
        ResizeTimeBar(time / 2, timeBar.GetComponent<RectTransform>().sizeDelta * increaseTimeBarSize, true);
        float elapsedTime = 0.0f;
        while(elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            timeBarValue = Mathf.Lerp(timeBarValue, newValue, elapsedTime / time);
            timeBar.fillAmount = timeBarValue;
            yield return null;
        }
        timeBar.fillAmount = newValue;
    }

    IEnumerator ResizeTimeBar(float time, Vector2 sizeTo, bool getBack)
    {
        float elapsedTime = 0.0f;
        Vector2 originalSize = timeBar.GetComponent<RectTransform>().sizeDelta;
        while(elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            timeBar.GetComponent<RectTransform>().sizeDelta = Vector2.Lerp(originalSize, sizeTo, elapsedTime / time);
            yield return null;
        }
        timeBar.GetComponent<RectTransform>().sizeDelta = sizeTo;
        if (getBack) ResizeTimeBar(time, originalSize, false);
    }
}
