using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DancingTestUIController : MonoBehaviour {

    private GeneralUIController UIController;
    public RectTransform canvas;
    public Image[] introductionTexts;
    public Image finalText;

    //Variables de los bloques completados
    public Text completedBlocksText; //Texto que especifica los bloques completados con respecto a los totales
    private int numCompletedBlocks; //Número de bloques completados actualmente
    private int numTotalBlocks; //Número de bloques totales
    private string completedBlocksTextBase = "Bloques completados: ";

    //Variables de la barra de tiempo
    private float timeBarValue; //Valor entre 0 y 1 que rellena la barra de tiempo
    public Image timeBar; //La barra de tiempo
    public float increaseTimeBarSize; //Porcentaje de tamaño que ha de aumentar la barra cuando su valor sea aumentado (se acierta una nota)
    private bool activatedTimer; //Si está en true quiere decir que el temporizador está en marcha, si está en false no, ya que, por ejemplo se está aumentando el tiempo que se tiene
    public Color initialColor; //Color que muestra la barra de tiempo cuando el temporizador está en marcha
    public Color increasingColor; //Color que muestra la barra cuando el tiempo aumenta
    public Text timeText; //Texto del tiempo en segundos
    private float totalTime; //Tiempo total del que se dispone
    private float currentTime = 60; //Tiempo actual del que se dispone

    //Variables de los fallos permitidos
    private int currentRemainingMistakes;
    public Image[] unusedRemainingMistakes;
    public Image[] usedRemainingMistakes;

	// Use this for initialization
	void Start () {
        UIController = GetComponent<GeneralUIController>();
	}

    public IEnumerator ShowIntroductionTexts(float totalTime)
	{
		StartCoroutine(MoveToCenter(introductionTexts[0], true, totalTime/12));
		yield return new WaitForSeconds(totalTime/6);

		for(int i = 0; i < introductionTexts.Length; i++)
		{
			StartCoroutine(ScaleAndFade(introductionTexts[i], 1.5f, totalTime/6));
			yield return new WaitForSeconds(totalTime/6);
		}
	}

    public IEnumerator ShowFinalText(float totalTime)
    {
        StartCoroutine(MoveToCenter(finalText, false, totalTime/3));
        yield return new WaitForSeconds(2 * totalTime/3);

        StartCoroutine(ScaleAndFade(finalText, 1.5f, totalTime/3));
        yield return new WaitForSeconds(totalTime/3);
    }

	private IEnumerator MoveToCenter(Image text, bool fromRight, float time)
	{
		int direction = (fromRight) ? 1 : -1;
		text.GetComponent<RectTransform>().localPosition = new Vector2(direction * (canvas.rect.width + text.GetComponent<RectTransform>().rect.width) / 2, 0);
		text.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
		Vector2 initialPosition = text.GetComponent<RectTransform>().localPosition;
		Vector2 finalPosition = new Vector2(0,0);

		float elapsedTime = 0.0f;
		while(elapsedTime < time)
		{
			elapsedTime += Time.deltaTime;
			text.GetComponent<RectTransform>().localPosition = Vector2.Lerp(initialPosition, finalPosition, elapsedTime/time);
			yield return null;
		}
		text.GetComponent<RectTransform>().localPosition = finalPosition;
	}

	private IEnumerator ScaleAndFade(Image text, float increaseScale, float time)
	{
		text.GetComponent<RectTransform>().localPosition = Vector2.zero;
        
        Color initialColor = text.color;
		Color finalColor = new Color(1.0f, 1.0f, 1.0f, 0.0f);

		Vector2 initialScale = text.GetComponent<RectTransform>().sizeDelta;
		Vector2 finalScale = text.GetComponent<RectTransform>().sizeDelta * increaseScale;

		float elapsedTime = 0.0f;

		if(text.color.a == 1.0f)
		{
            while(elapsedTime < time)
			{
				elapsedTime += Time.deltaTime;
				text.GetComponent<RectTransform>().sizeDelta = Vector2.Lerp(initialScale, finalScale, elapsedTime / time);
				text.color = Color.Lerp(initialColor, finalColor, elapsedTime / time);
				yield return null;
			}
			text.GetComponent<RectTransform>().sizeDelta = finalScale;
			text.color = finalColor;
		}
		else
		{
			StartCoroutine(FadeInTwoParts(text, initialColor, new Color(1.0f, 1.0f, 1.0f, 1.0f), true, time/2));
			while(elapsedTime < time)
			{
				elapsedTime += Time.deltaTime;
				text.GetComponent<RectTransform>().sizeDelta = Vector2.Lerp(initialScale, finalScale, elapsedTime / time);
				yield return null;
			}
			text.GetComponent<RectTransform>().sizeDelta = finalScale;
		}
	}

	private IEnumerator FadeInTwoParts(Image text, Color initialColor, Color finalColor, bool firstTime, float time)
	{
		float elapsedTime = 0.0f;

		while(elapsedTime < time)
		{
			elapsedTime += Time.deltaTime;
			text.color = Color.Lerp(initialColor, finalColor, elapsedTime / time);
			yield return null;
		}
		text.color = finalColor;

		if(firstTime) StartCoroutine(FadeInTwoParts(text, finalColor, initialColor, false, time));
	}

	public void InitializeUI(int param0, float param1) //Esta función debe ser llamada por el TestManager cuando se inicia el sistema de turnos de la prueba
    {
        UIController.ChangeMode(UILayer.DancingTest);
        
        activatedTimer = true;
        timeBar.color = initialColor;
        
        numTotalBlocks = param0;
        numCompletedBlocks = 0;
        UpdateCompletedBlocksText();
        print("hola");
        timeBarValue = 1;
        timeBar.fillAmount = 1;
      
        totalTime = param1;
        currentTime = totalTime;
        timeText.text = totalTime.ToString();

        for(int i = 0; i < introductionTexts.Length; i++)
        {
			introductionTexts[i].color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        }
        finalText.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);

        currentRemainingMistakes = 3;
        for(int i = 0; i < currentRemainingMistakes; i++)
        {
            unusedRemainingMistakes[i].color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            usedRemainingMistakes[i].color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        }
    }

	// Update is called once per frame
	void Update () {
        /* Depurando
        if(activatedTimer) 
        { 
            currentTime -= Time.deltaTime; //Esto en principio lo debería realizar el TestManager y cambiar la variable currentTime mediante SetCurrentTime a cada frame desde ese script
            UpdateTimeBar(currentTime/totalTime);
            
            if(Input.GetKeyDown(KeyCode.A))
            {
                currentTime += 10;
                if (currentTime > 60) currentTime = totalTime;
                UpdateTimeBar(currentTime/totalTime);
                if(currentRemainingMistakes > 0)OneMistakeMore();
            }
            if(Input.GetKeyDown(KeyCode.B))
                if(currentRemainingMistakes < 3)OneMistakeLess();
            
            timeText.text = ((int) currentTime).ToString();
        }*/

        //Versión buena del código cuando esté implementado el TestManager
        if(activatedTimer)
        {
            UpdateTimeBar(currentTime/totalTime);
            if(currentTime >= 0) timeText.text = ((int) currentTime).ToString();
        }
        //Dado que será el TestManager el que gestione la bajada del tiempo y cuando se debe aumentar (cuando se pulsa una tecla correctamente), no es necesario considerarlo desde aquí 
        
	}

    public void CompletedBlock() //Se debe llamar desde el TestManager cada vez que se complete un bloque y así poder updatear la UI con nueva información
    {
        numCompletedBlocks++;
        UpdateCompletedBlocksText();
    }

    public void OneMistakeMore() //Debe ser llamado por el TestManager cuando se falle una tecla
    {
        currentRemainingMistakes--;
        UseMistake();
    }

    public void OneMistakeLess() //Esto será útil cuando se añadan las invocaciones
    {
        UnusedMistake();
        currentRemainingMistakes++;
    }

    public bool GetActivatedTimer() //Al TestManager le interesa saber si el temporizador está en marcha para restar tiempo a su cantidad de tiempo o no
    {
        return activatedTimer;
    }

    public float GetCurrentTime()
    {
        return currentTime;
    }

    public void SetCurrentTime(float param) //El TestManager deberá realizar llamada a este método cada vez que actualice esta variable de tiempo, es decir, cada frame
    {
        currentTime = param;
    }

    private void UpdateCompletedBlocksText() //Updatea el texto de los bloques completados
    {
        completedBlocksText.text = completedBlocksTextBase + numCompletedBlocks + "/" + numTotalBlocks;
    }

    private void UpdateTimeBar(float newValue) //Updatea la barra de tiempo
    {
        if(newValue <= timeBarValue) //Si el nuevo valor que se utiliza para actualizar es menor o igual al actual valor, quiere decir que es el temporizador haciendo su función, por lo que, simplemente se actualiza la barra, y dado que es una disminución pequeña entre valores, no es necesaria interpolación
        {
            timeBarValue = newValue; //Updateamos a nivel teórica
            timeBar.fillAmount = timeBarValue; //Y a nivel visual
        }
        else //Si el nuevo valor es mayor al actual, quiere decir que se ha aumentado el tiempo, y que ejecutar las corutinas
        { 
            StartCoroutine(IncreaseTime(newValue, 1f, 0.1f));
        }
    }

    private void UseMistake() //Hace desaparecer el fallo sin utilizar y aparece el fallo utilizado
    {
        StartCoroutine(FadeInOutMistake(0.5f, 10f, unusedRemainingMistakes[currentRemainingMistakes], usedRemainingMistakes[currentRemainingMistakes]));
    }

    private void UnusedMistake() //Hace desaparecer el fallo utilizado y aparece el fallo sin utilizar
    {
        StartCoroutine(FadeInOutMistake(0.5f, -10f, usedRemainingMistakes[currentRemainingMistakes], unusedRemainingMistakes[currentRemainingMistakes]));
    }

    IEnumerator IncreaseTime(float newValue, float time, float colorTime) //Parámetros: el nuevo valor a expresar, el tiempo que se emplea para la interpolación y el porcentaje del tiempo total que se destina a los cambios de color de la barra
    {
        activatedTimer = false; //Se indica que el temporizador debe dejar de funcionar
        StartCoroutine(ResizeTimeBar(time / 2, timeBar.GetComponent<RectTransform>().sizeDelta * increaseTimeBarSize, true)); //Llamamos a la corutina que aumentará el tamaño de la barra en la mitad del tiempo total empleado en la corutina global

        StartCoroutine(ChangeColor(time * colorTime, increasingColor)); //Lo mismo con la corutina para cambiar el color y se utiliza el porcentaje del tiempo indicado en el parámetro colorTime
        bool changedColor = false; //Esta variable se emplea para que posteriormente, se cambie el color al original de nuevo una única vez

        float originalValue = timeBarValue;
        float elapsedTime = 0.0f;
        while(elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;

            if(!changedColor && elapsedTime >= (time - (time*colorTime)))
            {
                changedColor = true;
                StartCoroutine(ChangeColor(time * colorTime, initialColor)); //Una vez pasado el tiempo total de la corutina menos el porcentaje del tiempo para cambiar el color, volvemos a cambiar el color ahora al original de la barra
            }

            timeBarValue = Mathf.Lerp(originalValue, newValue, elapsedTime / time);
            timeBar.fillAmount = timeBarValue;
            yield return null;
        }
        timeBar.fillAmount = newValue;
        activatedTimer = true; //Cuando se acaba esta corutina, se puede retomar el temporizador con normalidad
    }

    IEnumerator ResizeTimeBar(float time, Vector2 sizeTo, bool getBack) //Tiempo empleado en la corutina, tamaño a tomar al final y si se debe repetir para posteriormente volver a tomar el tamaño original en la llamada a esta corutina
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
        if (getBack) StartCoroutine(ResizeTimeBar(time, originalSize, false));
    }

    IEnumerator ChangeColor(float time, Color toColor) //No hace falta explicar demasiado aquí uwu
    {
        float elapsedTime = 0.0f;
        Color originalColor = timeBar.color;
        while(elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            timeBar.color = Color.Lerp(originalColor, toColor, elapsedTime/time);
            yield return null;
        }
        timeBar.color = toColor;
    }

    IEnumerator FadeInOutMistake(float time, float distance, Image disappear, Image appear)
    {
        float elapsedTime = 0.0f;
        Vector2 initialPosition = disappear.GetComponent<RectTransform>().anchoredPosition;
        Vector2 finalPosition = new Vector2(initialPosition.x, initialPosition.y + distance);

        Color initialColor1 = disappear.color;
        Color finalColor1 = new Color(1.0f, 1.0f, 1.0f, 0.0f);

        Color initialColor2 = appear.color;
        Color finalColor2 = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        
        while(elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            disappear.GetComponent<RectTransform>().anchoredPosition = 
                Vector2.Lerp(initialPosition, finalPosition, elapsedTime/time);

            disappear.color = Color.Lerp(initialColor1, finalColor1, elapsedTime/time);
            appear.color = Color.Lerp(initialColor2, finalColor2, elapsedTime/time);

            yield return null;
        }
        disappear.GetComponent<RectTransform>().anchoredPosition = finalPosition;
        disappear.color = finalColor1;
        appear.color = finalColor2;

        disappear.GetComponent<RectTransform>().anchoredPosition = initialPosition;
    }
}
