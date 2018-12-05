using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsController : MonoBehaviour {

    public StatsUIController statsUI;

    private float bladderTiredness;
    private float patience;
    private int fame;
    private int money;

    public float bladderTirednessIncreaseSpeed;

    public GameObject[] stars;
    //public Text temporal; //retirar una vez se pongan estrellas a la fama
    
    // Use this for initialization
	void Start () {
        patience = 1;
        bladderTiredness = 0;
        fame = 0;
        money = 0;
	}
	
	// Update is called once per frame
	void Update () {

        if(GetComponent<PlayerPlatformController>().GetInputActivated() && !ChangeBladderTiredness((bladderTirednessIncreaseSpeed / 100) * Time.deltaTime))
            StartCoroutine(GetComponent<PlayerPlatformController>().ChangePlayer());
	}
    public void IncreaseFame()
    {
        fame++;
        statsUI.OneMoreStar();
    }
    public void DecreaseFame() //no creo que haga falta
    {
        fame--;
        //desActivar tantas estrellas como fama haya
    }

    public bool ChangeBladderTiredness(float value)
    {
        bladderTiredness = ((bladderTiredness + value) > 1) ? 1 : ((bladderTiredness + value) < 0) ? 0 : bladderTiredness + value;
        statsUI.ChangeValueBladderTirednessBar(value);
        return bladderTiredness != 1;
    }

    public bool ChangePatience(float value)
    {
        //StartCoroutine(ChangeBarValue(pacienciaBar.value - value, pacienciaBar, 0.5f));
        patience = ((patience + value) > 1) ? 1 : ((patience + value) < 0) ? 0 : patience + value;
        statsUI.ChangeValuePatienceBar(value);
        return patience != 0;
    }

    public void ChangeMoney(int value)
    {
        money += value;
        statsUI.ChangeMoney(value);
    }

    public float GetBladderTiredness()
    {
        return bladderTiredness;
    }

    public float GetPatience()
    {
        return patience;
    }

    public int GetFame()
    {
        return fame;
    }

    public int GetMoney()
    {
        return money;
    }
}
