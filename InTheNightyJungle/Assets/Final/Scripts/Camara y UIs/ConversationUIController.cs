using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConversationUIController : MonoBehaviour {

	private GeneralUIController UIController;
	
	public Image conversationBox; //Cuadro de texto para el mensaje
	public Text conversationText; //El texto del mensaje
	public Image optionsBox; //Cuadro de las opciones
	public Text[] optionsTexts; //Los dos huecos de texto para poner Sí y No
	public GameObject[] arrowPositions; //Las posibles posiciones de la flecha
	public Image pointingArrow; //La flecha

	public GameObject opennedConversationBoxPlace; //Sitio y tamaño donde se debe posicionar el cuadro de texto cuando esté abierto
	private Vector2 opennedConversationBoxPosition;
	private Vector2 opennedConversationBoxSize;
	private Vector2 closedConversationBoxPosition; //Sitio donde se debe posicionar el cuadro de texto cuando esté cerrado
	private Vector2 closedConversationBoxSize; //Tamaño que debe tomar el cuadro de texto cuando esté cerrado

	public GameObject opennedOptionsBoxPlace; //Lo mismo pero para el cuadro de opciones
	private Vector2 opennedOptionsBoxPosition;
	private Vector2 opennedOptionsBoxSize;
	private Vector2 closedOptionsBoxPosition;
	private Vector2 closedOptionsBoxSize;

	private bool conversationBoxOpenned; //El cuadro de texto ya está abierto (así no se abrirá más de una vez)
	private bool preparedForNewText; //Variable super importante porque le dice al ConversationalBehaviour cuando puede volver a enviar un mensaje para que se muestre
	private bool optionsBoxReady; //El cuadro de opciones está abierto

	private int pointingArrowPos; //Posición en la que está colocada la flecha ahora: 0 arriba, 1 abajo
	private int optionSelected; //Opción seleccionada al pulsar Enter con las opciones abiertas: -1 no se ha seleccionado aún una opción, 0 primera opción, 1 segunda opción

	// Use this for initialization
	void Start () {
		UIController = GetComponent<GeneralUIController>();
		//InitializeConversationBox(); //Esta llamada debería realizarse desde el ConversationalBehaviour
	}
	
	public void InitializeConversationBox()
	{
		UIController.ChangeMode(UILayer.Conversation);

		opennedConversationBoxPosition = opennedConversationBoxPlace.GetComponent<RectTransform>().anchoredPosition; //Guardamos en las variables Vector2 las posiciones y tamaños que nos interesan
		opennedConversationBoxSize = opennedConversationBoxPlace.GetComponent<RectTransform>().sizeDelta;
		closedConversationBoxPosition = Vector2.zero;
		closedConversationBoxSize = Vector2.zero;

		opennedOptionsBoxPosition = opennedOptionsBoxPlace.GetComponent<RectTransform>().anchoredPosition;
		opennedOptionsBoxSize = opennedOptionsBoxPlace.GetComponent<RectTransform>().sizeDelta;
		closedOptionsBoxPosition = Vector2.zero;
		closedOptionsBoxSize = Vector2.zero;
		
		conversationBox.GetComponent<RectTransform>().anchoredPosition = closedConversationBoxPosition; //Inicializamos las posiciones y tamaños de los cuadros
		conversationBox.GetComponent<RectTransform>().sizeDelta = closedConversationBoxSize;
		optionsBox.GetComponent<RectTransform>().anchoredPosition = closedOptionsBoxPosition;
		optionsBox.GetComponent<RectTransform>().sizeDelta = closedOptionsBoxSize;

		conversationBoxOpenned = false; //El cuadro de texto está cerrado al principio
		optionsBoxReady = false; //Igual con el cuadro de opciones
		conversationText.text = ""; //Vacíamos la variable de texto
		preparedForNewText = true; //Está preparado para lanzar un nuevo texto

		optionsTexts[0].text = "Sí"; //Opción 1
		optionsTexts[1].text = "No"; //Opción 2

		pointingArrowPos = 0; //Teóricamente colocada en la opción 1
		pointingArrow.GetComponent<RectTransform>().anchoredPosition = arrowPositions[pointingArrowPos].GetComponent<RectTransform>().anchoredPosition; //Visualmente colocada en la opción 1

		optionSelected = -1; //No se ha seleccionada ninguna opción aún
	}

	// Update is called once per frame
	void Update () {
		if(optionsBoxReady) //Controlamos el input de la selección de opciones desde este script, mientras que desde el ConversationalBehaviour deberíamos estar comprobando qué opción se guarda en la variable optionSelected para determinar cuando cerrar el cuadro de opciones y continuar con la conversación
		{
			if(Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
			{
				pointingArrowPos = pointingArrowPos == 0 ? 1 : 0;
				pointingArrow.GetComponent<RectTransform>().anchoredPosition = arrowPositions[pointingArrowPos].GetComponent<RectTransform>().anchoredPosition;
			}
			if(Input.GetKeyDown(KeyCode.Z))
			{
				optionSelected = pointingArrowPos;
			}
		}

		/* Depurando
		if(preparedForNewText)
		{
			if(Input.GetKeyDown(KeyCode.A))
			{
				SpamText("Empecemos...", false);
			}
			if(Input.GetKeyDown(KeyCode.S))
			{
				SpamText("A ver", true);
			}
		}

		if(optionsBoxReady && optionSelected != -1)
		{
			UnshowOptionsBox();
		}*/
	}

	public bool GetPreparedForNewText() //Este método es el que debe llamar (de manera continua) el ConversationalBehaviour para saber si puede escribir el siguiente mensaje o no (considerar también en ese script el input del jugador para saber qué quiere pasar el mensaje)
	{
		return preparedForNewText;
	}

	public void SetPreparedForNewText(bool param)
	{
		preparedForNewText = param;
	}

	public int GetOptionSelected() //Este método es el que debe llamar (de manera continua) el ConversationalBehaviour cuando la caja de opciones esté puesta en marcha
	{
		return optionSelected;
	}

	public bool GetOptionsBoxReady() //Cabe comprobar si el jugador ya puede elegir opciones para que desde el ConversationalBehaviour se compruebe la opción por el jugador solo en ese caso
	{
		return optionsBoxReady;
	}

	private void ShowConversationBox(string firstMessage, bool options) //Abre el cuadro de texto con el mensaje que se le indica. Si options está en true, tras enseñar el mensaje, abrirá el cuadro de opciones
	{
		preparedForNewText = false;
		StartCoroutine(OpenConversationBox(0.2f, firstMessage, options));
	}

	public void FinishedConversation() //Debe ser llamado desde el ConversationalBehaviour cuando se termina la conversación
	{
		StartCoroutine(CloseConversationBox(0.2f));
	}

	private void ShowOptionsBox() //Abre el cuadro de opciones
	{
		StartCoroutine(OpenOptionsBox(0.2f));
	}

	public void UnshowOptionsBox() //Debe ser llamado desde el ConversationalBehaviour cuando ha recibido un valor distinto a -1 en la variable optionSelected
	{
		StartCoroutine(CloseOptionsBox(0.2f));
	}

	public void SpamText(string message, bool options)  //Lo llama el ConversationalBehavoiur cada vez que puede mostrar el siguiente mensaje
	{
		if(!conversationBoxOpenned) ShowConversationBox(message, options); //Si es el primero, abrirá el cuadro de texto primero
		else
		{
			preparedForNewText = false;
			conversationText.text = "";
			StartCoroutine(WriteMessage(0.05f, message, 0, options)); //Escribe el mensaje con una pequeña espera por cada letra
		}
	}

	IEnumerator OpenConversationBox(float time, string firstMessage, bool options) //Abre el cuadro de texto con una interpolación de posición y tamaño
	{
		float elapsedTime = 0.0f;
		Vector2 initialPosition = conversationBox.GetComponent<RectTransform>().anchoredPosition;
		Vector2 initialSize = conversationBox.GetComponent<RectTransform>().sizeDelta;

		while(elapsedTime < time)
		{
			elapsedTime += Time.deltaTime;
			conversationBox.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(initialPosition, opennedConversationBoxPosition, elapsedTime/time);
			conversationBox.GetComponent<RectTransform>().sizeDelta = Vector2.Lerp(initialSize, opennedConversationBoxSize, elapsedTime/time);
			yield return null;
		}

		conversationBox.GetComponent<RectTransform>().anchoredPosition = opennedConversationBoxPosition;
		conversationBox.GetComponent<RectTransform>().sizeDelta = opennedConversationBoxSize;

		conversationBoxOpenned = true;
		SpamText(firstMessage, options); //Llama a SpamText con ese primer mensaje después de haber abierto el cuadro de texto
	}

	IEnumerator CloseConversationBox(float time) //Más de lo mismo pero para cerrar el cuadro de texto
	{
		conversationText.text = "";

		float elapsedTime = 0.0f;
		Vector2 initialPosition = conversationBox.GetComponent<RectTransform>().anchoredPosition;
		Vector2 initialSize = conversationBox.GetComponent<RectTransform>().sizeDelta;

		while(elapsedTime < time)
		{
			elapsedTime += Time.deltaTime;
			conversationBox.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(initialPosition, closedConversationBoxPosition, elapsedTime/time);
			conversationBox.GetComponent<RectTransform>().sizeDelta = Vector2.Lerp(initialSize, closedConversationBoxSize, elapsedTime/time);
			yield return null;
		}

		conversationBox.GetComponent<RectTransform>().anchoredPosition = closedConversationBoxPosition;
		conversationBox.GetComponent<RectTransform>().sizeDelta = closedConversationBoxSize;

		UIController.BackToStats();
	}

	IEnumerator OpenOptionsBox(float time) //Para abrir el cuadro de opciones
	{
		float elapsedTime = 0.0f;
		Vector2 initialPosition = optionsBox.GetComponent<RectTransform>().anchoredPosition;
		Vector2 initialSize = optionsBox.GetComponent<RectTransform>().sizeDelta;

		while(elapsedTime < time)
		{
			elapsedTime += Time.deltaTime;
			optionsBox.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(initialPosition, opennedOptionsBoxPosition, elapsedTime/time);
			optionsBox.GetComponent<RectTransform>().sizeDelta = Vector2.Lerp(initialSize, opennedOptionsBoxSize, elapsedTime/time);
			yield return null;
		}

		optionsBox.GetComponent<RectTransform>().anchoredPosition = opennedOptionsBoxPosition;
		optionsBox.GetComponent<RectTransform>().sizeDelta = opennedOptionsBoxSize;

		optionsBoxReady = true;
	}

	IEnumerator CloseOptionsBox(float time) //Para cerrar el cuadro de opciones
	{
		optionsBoxReady = false;

		optionSelected = -1; //Se reinicializa la optionSelected
		
		float elapsedTime = 0.0f;
		Vector2 initialPosition = optionsBox.GetComponent<RectTransform>().anchoredPosition;
		Vector2 initialSize = optionsBox.GetComponent<RectTransform>().sizeDelta;

		while(elapsedTime < time)
		{
			elapsedTime += Time.deltaTime;
			optionsBox.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(initialPosition, closedOptionsBoxPosition, elapsedTime/time);
			optionsBox.GetComponent<RectTransform>().sizeDelta = Vector2.Lerp(initialSize, closedOptionsBoxSize, elapsedTime/time);
			yield return null;
		}

		optionsBox.GetComponent<RectTransform>().anchoredPosition = closedOptionsBoxPosition;
		optionsBox.GetComponent<RectTransform>().sizeDelta = closedOptionsBoxSize;
	}

	IEnumerator WriteMessage(float timeBetweenLetters, string message, int i, bool options) //Corutina recursiva!!! Qué guay!
	{
		if(conversationText.text.Length < message.Length)
		{
			yield return new WaitForSeconds(timeBetweenLetters);
			conversationText.text += message[i];
			StartCoroutine(WriteMessage(timeBetweenLetters, message, i+1, options));
		}
		else
		{
			conversationText.text = message;
			if(options) //Si al terminar de escribir, options está en true, lanzará el cuadro de opciones
			{
				ShowOptionsBox();
			}
			else
				preparedForNewText = true; //Si está en false, indicará que ya se puede lanzar el siguiente texto
		}
	}
}

/* Punto importante a considerar:
	La manera de interpretar la variable preparedForNewText es que cuando esta esté en true, en el script ConversationalBehaviour, se debe permitir el Input del jugador para poder pasar de mensaje es decir:

	void Update() {
		[...]
		if(conversationUI.GetPreparedForNewText())
		{
			if(Input.GetKeyDown(KeyCode.Z))
			{
				SpamText(); //Se llama a este método local que se encargará de pedir a la UI que muestre el mensaje actual y de pasar al siguiente mensaje si el jugador no tiene que elegir entre las dos opciones
			}
			if(options && conversationUI.GetOptionsBoxReady())
			{
				int optionSelected = conversationUI.GetOptionSelected();
				if(optionSelected != -1) 
				
				Este caso es algo más complicado y la razón de porque existe el método local SpamText: cuando se elige una opción se debe interpretar como que el jugador ha elegido también pasar al siguiente mensaje.
				Es decir, se debe enseñar el siguiente mensaje automáticamente después de haber elegido el mensaje que se debía mostrar en función elegido. Esto es porque si no existen varias opciones (el caso normal),
				automáticamente se apunta al siguiente mensaje, pero al tener que esperar el Input del jugador para saber qué mensaje el siguiente y, dado que no tiene sentido que, una vez el jugador ha elegido una opción, 
				tener que volver esperar a que el jugador decida cambiar de mensaje, entonces, una vez elegida que rama de la conversación se va a seguir, se lanza el mensaje siguiente de manera directa.
				Si después de eso, ese mensaje vuelve a tener opciones a elegir, técnicamente debería reproducirse de nuevo esta parte del código sin ningún tipo de problema.
				Si en cambio, no es un mensaje con opciones a elegir, entonces se volverá a pedir al jugador el pasar a otro mensaje como es común.

				{
					NextMessage(optionSelected);
					SpamText();
				}
			}
		}
		[...]
	}

	private void SpamText()
	{
		conversationUI.SpamText(currentMessage, options) //options debería tener guardado si después de ese mensaje se tienen que elegir cosas o no
		if(!options) NextMessage(0); //Leería el siguiente nodo del ConversationalTree si no hay una bifurcación. El 0 indica que debe ir por la rama izquierda del árbol porque por defecto es donde van los mensajes encadenados sin bifurcación		
	}
 */
