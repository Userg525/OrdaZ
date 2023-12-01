using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletScript : MonoBehaviour
{
    public float Speed;
    public AudioClip Sound;

    private Rigidbody2D Rigidbody2D;
    private Vector3 Direction;

    private void Start()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Camera.main.GetComponent<AudioSource>().PlayOneShot(Sound);
    }

    private void FixedUpdate()
    {
        Rigidbody2D.velocity = Direction * Speed;
    }

    public void SetDirection(Vector3 direction)
    {
        Direction = direction;
    }

    public void DestroyBullet()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verifica si el objeto colisionado tiene el script del zombie (zombiec)
        zombiec zombie = other.GetComponent<zombiec>();

        // Si el objeto colisionado es un zombie, llama a su función RecibirImpacto
        if (zombie != null)
        {
            zombie.RecibirImpacto();

            // Accede a la clase JohnMovement en el objeto con tag "Player"
            JohnMovement john = GameObject.FindGameObjectWithTag("Player").GetComponent<JohnMovement>();

            // Si encontramos la clase JohnMovement, suma puntos
            if (john != null)
            {
                john.SumarPuntosPorMatarZombie();
            }
        }
        else
        {
            // Si no es un zombie, verifica si es el jugador o un enemigo
            GruntScript grunt = other.GetComponent<GruntScript>();
            JohnMovement john = other.GetComponent<JohnMovement>();

            // Llama a la función Hit del enemigo o del jugador correspondiente
            if (grunt != null)
            {
                grunt.Hit();
            }
            if (john != null)
            {
                john.Hit();
            }
        }

        // Destruye la bala después de la colisión
        DestroyBullet();
    }
}
