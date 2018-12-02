using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsUIController : MonoBehaviour {

	public Slider bladderTirednessStatBar; //Barra correspondiente a la vejiga/cansancio
	public Slider patienceStatBar; //Barra correspondiente a la paciencia
	public Image[] stars; //Imágenes de las 3 estrellas de fama
	public Image shine; //Imagen de brillo que se mostrará al conseguir una estrella
	public Text moneyText; //Texto que indica el dinero que se posee

	public Sprite emptyStar; //Sprite que marca una estrella vacía
	public Sprite fullStar; //Sprite que marca una estrella completa

	private int numStars; //Número actual de estrellas
	private Vector2 shineOriginalSize;

	// Use this for initialization
	void Start () {
		bladderTirednessStatBar.value = 0;
		patienceStatBar.value = 1;

		for(int i = 0; i < stars.Length; i++)
		{
			HideStar(i);
		}
		numStars = 0;
		shine.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
		shineOriginalSize = shine.GetComponent<RectTransform>().sizeDelta;

		moneyText.text = "";
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void ChangeValueBladderTirednessBar(float increaseValue) //Este método se debe llamar cuando se debe reducir o aumentar el valor de cansancio/vejiga. 
	{																//El argumento que recibe debe ser un valor positivo o negativo entre 0 y 1 en valor absoluto y debe significar el aumento o decremento que debe sufrir el valor actual, NO el valor final
		StartCoroutine(InterpolateBar(bladderTirednessStatBar, increaseValue, Mathf.Abs(increaseValue))); //El tiempo que tarda en cambiar ese valor es el mismo que se quiere aumentar o reducir
	}

	public void ChangeValuePatienceBar(float increaseValue) //Método que se debe llamar cuando se debe reducir o aumentar el valor de paciencia. El argumento, de nuevo, significa el aumento o decremento de la paciencia y debe estar entre 0 y 1 en valor absoluto
	{
		StartCoroutine(InterpolateBar(patienceStatBar, increaseValue, Mathf.Abs(increaseValue)));
	}

	private IEnumerator InterpolateBar(Slider bar, float increaseValue, float time) //Cambia el valor de una barra en un tiempo determinado
	{
		float elapsedTime = 0.0f;
		float initialValue = bar.value;
		float finalValue = (initialValue + increaseValue > 1) ? 1 : (initialValue + increaseValue < 0) ? 0 : initialValue + increaseValue; //El valor final no pasará el 0 ni el 1

		while(elapsedTime < time)
		{
			elapsedTime += Time.deltaTime;
			bar.value = Mathf.Lerp(initialValue, finalValue, elapsedTime / time);
			yield return null;
		}
		bar.value = finalValue;
	}

	public void OneMoreStar()
	{
		StartCoroutine(ShowShine(stars[numStars], 1.5f, new Color(1.0f, 1.0f, 1.0f, 1.0f), 1.0f / 2, true));
	}

	private IEnumerator ShowShine(Image star, float factorScale, Color finalColor, float time, bool turnBack)
	{
		shine.GetComponent<RectTransform>().anchoredPosition = star.GetComponent<RectTransform>().anchoredPosition;

		Color initialColor = shine.color;
		
		Vector2 initialSize = shine.GetComponent<RectTransform>().sizeDelta;
		Vector2 finalSize = shineOriginalSize * factorScale;

		float elapsedTime = 0.0f;

		while(elapsedTime < time)
		{
			elapsedTime += Time.deltaTime;
			shine.color = Color.Lerp(initialColor, finalColor, elapsedTime / time);
			shine.GetComponent<RectTransform>().sizeDelta = Vector2.Lerp(initialSize, finalSize, elapsedTime / time);
			yield return null;
		}
		shine.color = finalColor;
		shine.GetComponent<RectTransform>().sizeDelta = finalSize;

		if(turnBack)
		{
			ShowStar(numStars++);
			StartCoroutine(ShowShine(star, 1.0f, initialColor, time, false));
		}
	}

	private void ShowStar(int i)
	{
		stars[i].sprite = fullStar;
	}

	private void HideStar(int i)
	{
		stars[i].sprite = emptyStar;
	}
}

/*

	Comentario: el aumento automático de la barra de cansancio/vejiga viene dado por parte del personaje (que debe tener acceso a este script) y es recomendable que el factor de aumento sea pequeño multiplicándolo por Time.deltaTime
	Este aumento automático debe ser detenido en momentos concretos, por lo que debe existir un booleano que lo desactive.

 */
