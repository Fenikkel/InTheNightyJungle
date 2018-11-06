using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SliderScript : MonoBehaviour
{
    public int startingValue = 0;      //el valor inicial de la barra, es una variable por si quereis empezar con otro valor                      
    public int currentValue;           // el valor durante la ejecucion                        
    public Slider Slider1;              //el slider en forma de variable                     


    void Awake()
    {
        // Setting up the references.
        // Set the initial value.
        currentValue = startingValue;
    }



    public void ChangingVariable(int amount)  // a esta funcion la debereis llamar en el script que controle los valores de la barra
    {
        // Si esta llena no añadira mas
        //Si no, aumentara el valor.
        if (currentValue < 100)
        {
            // Aumenta el valor segun lo introducido en la funcion
            currentValue += amount;

            //Actualiza la barra para reflejar el cambio.
            Slider1.value = currentValue;

        }
        

    }
}
