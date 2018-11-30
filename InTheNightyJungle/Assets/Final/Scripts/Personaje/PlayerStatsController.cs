using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsController : MonoBehaviour {

    public Slider changenessBar;
    public Slider pacienciaBar;
    public float changenessIncreaseSpeed;
    private int fame;
    public GameObject[] stars;
    //public Text temporal; //retirar una vez se pongan estrellas a la fama
    
    // Use this for initialization
	void Start () {
        changenessBar.value = 0;
        pacienciaBar.value = 1;
        fame = 0;
	}
	
	// Update is called once per frame
	void Update () {

        changenessBar.value += (changenessIncreaseSpeed / 100) * Time.deltaTime;
	}
    public void IncreaseFame()
    {
        fame++;
        for (int i = 0; i<fame && fame<=stars.Length; i++)
        {
            stars[i].SetActive(true);
            print("Hola");
        }
        //Activar tantas estrellas como fama haya
    }
    public void DecreaseFame() //no creo que haga falta
    {
        fame--;
        //desActivar tantas estrellas como fama haya
    }

    public void DecreaseChangenessStat(float value)
    {
        StartCoroutine(ChangeBarValue(changenessBar.value - value, changenessBar, 0.5f));
    }

    public void DecreasePaciencia(float value)
    {
        StartCoroutine(ChangeBarValue(pacienciaBar.value - value, pacienciaBar, 0.5f));
    }

    IEnumerator ChangeBarValue(float finalValue, Slider bar, float time)
    {
        float initialValue = bar.value;
        float elapsedTime = 0.0f;
        while(elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            bar.value = Mathf.Lerp(initialValue, finalValue, elapsedTime / time);
            yield return null;
        }
        bar.value = finalValue;
    }
}
