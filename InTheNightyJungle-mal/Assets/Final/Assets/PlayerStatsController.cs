using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsController : MonoBehaviour {

    public Slider cansancioBar;
    public float cansancioIncreaseSpeed;
    
    // Use this for initialization
	void Start () {
        cansancioBar.value = 0;
	}
	
	// Update is called once per frame
	void Update () {

        cansancioBar.value += cansancioIncreaseSpeed * Time.deltaTime;
	}

    public void DecreaseCansancio(float value)
    {
        cansancioBar.value -= value;
    }
}
