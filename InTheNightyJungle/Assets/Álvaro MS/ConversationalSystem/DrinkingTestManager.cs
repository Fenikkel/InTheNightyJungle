using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrinkingTestManager : MonoBehaviour {

	public DancingTestUIController UI;
    public ChallengerBehaviour challenger;
    private PlayerPlatformController player;
    private CameraBehaviour camera;

    public Transform insidePlayerPosition;
    public Transform insideCameraPosition;
    public Transform outsidePlayerPosition;
    public Transform outsideCameraPosition;
    public float cameraSize;
    public DrinkingShadow[] foregroundSilhouettes;

    private bool testStarted;
    private int victory;

	// Use this for initialization
	void Start () {
		
	}

    public void StartTest(PlayerPlatformController param0, CameraBehaviour param1)
    {
        player = param0;
        camera = param1;

        RestartTest();

        StartCoroutine(Introduction(0.5f, 0.5f, 0.5f, 6.0f));
    }

    public void RestartTest()
    {
        victory = -1;
    }

    private IEnumerator Introduction(float time1, float time2, float time3, float time4)
    {
        StartCoroutine(camera.MoveSizeTo(insideCameraPosition.position, camera.GetSize(), time1));
        yield return new WaitForSeconds(time1);

        StartCoroutine(player.MoveTo(insidePlayerPosition.position, false, time2));
        yield return new WaitForSeconds(time2);

        StartCoroutine(camera.MoveSizeTo(insideCameraPosition.position, cameraSize, time3));
        FadeOutCrowd(time3);
        yield return new WaitForSeconds(time3);

        //UI.InitializeUI(blocks.Length, initialTime);

        StartCoroutine(UI.ShowIntroductionTexts(time4));
        yield return new WaitForSeconds(time4);
        
        testStarted = true;
    }

    private IEnumerator Ending(float time1, float time2, float time3)
    {
        StartCoroutine(camera.MoveSizeTo(insideCameraPosition.position, camera.GetInitialSize(), time1));
        FadeInCrowd(time1);
        yield return new WaitForSeconds(time1);

        StartCoroutine(player.MoveTo(outsidePlayerPosition.position, true, time2));
        yield return new WaitForSeconds(time2);

        StartCoroutine(camera.MoveSizeTo(outsideCameraPosition.position, camera.GetSize(), time3));
        yield return new WaitForSeconds(time3);

        player.SetInputActivated(true);
        camera.SetFollowTarget(true);
        UI.GetComponent<GeneralUIController>().ChangeMode(UILayer.Stats);
    }

    private void FadeOutCrowd(float time)
    {
        for(int i = 0; i < foregroundSilhouettes.Length; i++)
        {
            StartCoroutine(foregroundSilhouettes[i].FadeOut(time));
        }
    }

    private void FadeInCrowd(float time)
    {
        for(int i = 0; i < foregroundSilhouettes.Length; i++)
        {
            StartCoroutine(foregroundSilhouettes[i].FadeIn(time));
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
