using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InitialCutscene : MonoBehaviour {

	private GameObject Cindy;
	private GameObject Brenda;
    private GameObject blackScreen;
	private CameraBehaviour mainCamera;

	public Transform CindyPositionInBuilding;
	public Transform BrendaPositionInBuilding;
	public Transform CentralPositionInBuilding;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void BeginCutscene(GameObject param0, GameObject param1, GameObject param2, CameraBehaviour param3)
	{
		Cindy = param0;
		Brenda = param1;
		blackScreen = param2;
		mainCamera = param3;

		StartCoroutine(Cutscene());
	}

	private IEnumerator Cutscene()
	{
		mainCamera.SetFollowTarget(false);

		Vector3 newPosition = new Vector3(Cindy.GetComponent<Transform>().position.x, Cindy.GetComponent<Transform>().position.y, mainCamera.GetComponent<Transform>().position.z);
		StartCoroutine(mainCamera.MoveSizeTo(newPosition, 2f, 1f));
		yield return new WaitForSeconds(0.5f);

		PlayPhoneRinging();
		yield return new WaitForSeconds(2f);

		Cindy.GetComponent<Animator>().Play("answeringPhone");
		Brenda.GetComponent<Animator>().Play("talkingPhone");
		yield return new WaitForSeconds(1.5f);

		StartCoroutine(BuildingTransition(true));
	}

	private IEnumerator BuildingTransition(bool toBrenda)
	{
		Vector3 firstPosition;
		Vector3 secondPosition = new Vector3(CentralPositionInBuilding.position.x, CentralPositionInBuilding.position.y, mainCamera.GetComponent<Transform>().position.z);
		Vector3 thirdPosition;
		Vector3 finalPosition;
		if(toBrenda)
		{
			firstPosition = new Vector3(CindyPositionInBuilding.position.x, CindyPositionInBuilding.position.y, mainCamera.GetComponent<Transform>().position.z);
			thirdPosition = new Vector3(BrendaPositionInBuilding.position.x, BrendaPositionInBuilding.position.y, mainCamera.GetComponent<Transform>().position.z);
			finalPosition = new Vector3(Brenda.GetComponent<Transform>().position.x, Brenda.GetComponent<Transform>().position.y, mainCamera.GetComponent<Transform>().position.z);
		}
		else
		{
			firstPosition = new Vector3(BrendaPositionInBuilding.position.x, BrendaPositionInBuilding.position.y, mainCamera.GetComponent<Transform>().position.z);
			thirdPosition = new Vector3(CindyPositionInBuilding.position.x, CindyPositionInBuilding.position.y, mainCamera.GetComponent<Transform>().position.z);
			finalPosition = new Vector3(Cindy.GetComponent<Transform>().position.x, Cindy.GetComponent<Transform>().position.y, mainCamera.GetComponent<Transform>().position.z);
		}
		StartCoroutine(FadeIn(1f));
		StartCoroutine(mainCamera.MoveSizeTo(mainCamera.GetComponent<Transform>().position, 10f, 1f));
		yield return new WaitForSeconds(1f);

		mainCamera.GetComponent<Transform>().position = firstPosition;

		StartCoroutine(FadeOut(1f));
		StartCoroutine(mainCamera.MoveSizeTo(secondPosition, 25f, 1f));
		yield return new WaitForSeconds(1.5f);

		StartCoroutine(mainCamera.MoveSizeTo(thirdPosition, 10f, 1f));
		StartCoroutine(FadeIn(1f));
		yield return new WaitForSeconds(1f);

		mainCamera.GetComponent<Transform>().position = finalPosition;
		
		StartCoroutine(FadeOut(1f));
		StartCoroutine(mainCamera.MoveSizeTo(mainCamera.GetComponent<Transform>().position, 2f, 1f));
	}

	public void PlayPhoneRinging()
	{
		GetComponent<AudioSource>().Play();
	}

	public void StopPhoneRinging()
	{
		GetComponent<AudioSource>().Stop();
	}

	IEnumerator FadeOut(float time)
    {
        Color c = blackScreen.GetComponent<Image>().color;
        Color initialColor = c;
        Color finalColor = new Color(0, 0, 0, 0);
        float elapsedTime = 0.0f;
        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            c = Color.Lerp(initialColor, finalColor, elapsedTime/time);
            blackScreen.GetComponent<Image>().color = c;
            yield return null;
        }
        blackScreen.GetComponent<Image>().color = finalColor;
    }

	IEnumerator FadeIn(float time)
    {
        Color c = blackScreen.GetComponent<Image>().color;
        Color initialColor = c;
        Color finalColor = new Color(0, 0, 0, 1);
        float elapsedTime = 0.0f;
        while(elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            c = Color.Lerp(initialColor, finalColor, elapsedTime/time);
            blackScreen.GetComponent<Image>().color = c;
            yield return null;
        }
        blackScreen.GetComponent<Image>().color = finalColor;
    }
}
