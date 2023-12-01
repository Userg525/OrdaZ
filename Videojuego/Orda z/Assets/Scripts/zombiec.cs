using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zombiec : MonoBehaviour
{
    public GameObject John;
    private float velocidadMinima = 0.6f;  // Velocidad mínima del zombie
    private float velocidadMaxima = 1.5f;  // Velocidad máxima del zombie
    public int salud = 2;  // Ajusta la salud inicial del zombie
    private float velocidadActual;  // Velocidad actual del zombie

    private void Start()
    {
        // Asigna una velocidad aleatoria al zombie al inicio
        velocidadActual = Random.Range(velocidadMinima, velocidadMaxima);
        ConfigurarVelocidad(velocidadActual);
    }

    void Update()
    {
        // Calcula la dirección hacia John
        if (John != null)
        {
            Vector3 direccion = John.transform.position - transform.position;

            // Orienta al zombie hacia John
            OrientarHaciaJohn(direccion);

            // Mueve al zombie hacia John
            MoverHaciaJohn(direccion);
        }
    }

    void OrientarHaciaJohn(Vector3 direccion)
    {
        // Orienta al zombie hacia John
        if (direccion.x >= 0.0f)
        {
            transform.localScale = new Vector3(2.0f, 2.0f, 2.0f);
        }
        else
        {
            transform.localScale = new Vector3(-2.0f, 2.0f, 2.0f);
        }
    }

    void MoverHaciaJohn(Vector3 direccion)
    {
        // Calcula la dirección normalizada hacia John
        Vector3 direccionNormalizada = direccion.normalized;

        // Mueve al zombie en la dirección de John con la velocidad configurada
        transform.Translate(direccionNormalizada * velocidadActual * Time.deltaTime);
    }

    public void RecibirImpacto()
    {
        // Resta salud al zombie
        salud--;

        // Verifica si el zombie ha perdido toda su salud
        if (salud <= 0)
        {
            // Lógica para morir
            Morir();
        }
    }

    private void Morir()
    {
        // Implementa aquí la lógica para la muerte del zombie
        // Puedes desactivar el GameObject, reproducir una animación, etc.
        gameObject.SetActive(false);
    }

    // Configura la velocidad del zombie
    private void ConfigurarVelocidad(float velocidad)
    {
        velocidadMinima = velocidad;
        velocidadMaxima = velocidad * 1.5f;  // Ajusta el factor de variabilidad según tus necesidades
    }

    // Obtiene la velocidad del zombie
    private float GetVelocidad()
    {
        return Random.Range(velocidadMinima, velocidadMaxima);
    }
}
