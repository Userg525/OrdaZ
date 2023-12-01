using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CambioDeEscenaNivel2 : MonoBehaviour
{
    public void CambiarAEscenaNivel2()
    {
        SceneManager.LoadScene("Nivel2"); // Reemplaza con el nombre real de tu escena del Nivel 2
    }

    public void CambiarAEscenaMenu() 
    {
       SceneManager.LoadScene("Menuinicial"); // Reemplaza con el nombre real de tu escena del Nivel 2
    }
}
