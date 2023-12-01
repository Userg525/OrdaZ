using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarraDeVida : MonoBehaviour
{
    private Slider slider;

    private void Start()
    {
        slider = GetComponent<Slider>();
    }

    public void CambiarVidaMaxima(float vidaMaxima)
    {
        slider.value = vidaMaxima;
    }

    public void CambiarVidaActual(float cantidadVida)
    {
        slider.value = cantidadVida;
    }

    public void CambiarVidaPorcentaje(float porcentaje)
    {
        porcentaje = Mathf.Clamp01(porcentaje); // Asegurarse de que el porcentaje esté entre 0 y 1
        float vidaMaxima = slider.maxValue;
        float nuevaVida = vidaMaxima * porcentaje;
        CambiarVidaActual(nuevaVida);
    }

    public void InicializarBarraDeVida(float cantidadVida)
    {
        CambiarVidaMaxima(cantidadVida);
        CambiarVidaActual(cantidadVida);
    }
}
