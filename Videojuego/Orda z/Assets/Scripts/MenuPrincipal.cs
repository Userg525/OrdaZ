using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Agrega este using para usar UI

public class MenuPrincipal : MonoBehaviour
{
    public static MenuPrincipal MPInstance;
    public InputField nombreInputField; // Referencia al campo de entrada de texto
    public string NombreJugador;

    private void Awake()
    {
        if (MPInstance == null)
        {
            MPInstance = this;
            DontDestroyOnLoad(MPInstance);
        }
        else
        {
            Destroy(this.gameObject);
        }

    }

    public void jugar()
    {

        // Obtén el nombre ingresado por el jugador
        MPInstance.NombreJugador = nombreInputField.text;

        // Puedes hacer algo con el nombre, como guardarlo o pasarlo a otra escena
        // En este ejemplo, simplemente lo mostramos en la consola
        Debug.Log("Nombre del jugador: " + MPInstance.NombreJugador);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Salir() 
    {
        Debug.Log("Salir...");
        Application.Quit();
    }
}
