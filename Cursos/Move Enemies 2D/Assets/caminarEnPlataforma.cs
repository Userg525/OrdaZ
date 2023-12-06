using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class caminarEnPlataforma : MonoBehaviour
{
    public Rigidbody2D rb2D;

    public float VelocidadDeMovimiento;

    public LayerMask capaAbajo;

    public LayerMask capaEnfrente;

    public float ditanciaAbajo;

    public float diatanciaEnfrente;

    public Transform controladorAbajo;

    public Transform controladorEnfrente;

    public bool informacionAbajo;

    public bool informacionEnfrente;

    public bool mirandoALaDerecha = true;

    private void Update()
    {
        //actualiza la velocidad del componente Rigidbody2D 
        rb2D.velocity = new Vector2(VelocidadDeMovimiento, rb2D.velocity.y);

        //Utiliza Physics2D.Raycast para realizar un rayo desde la posición del controladorEnfrente hacia la derecha (transform.right).
        informacionEnfrente = Physics2D.Raycast(controladorEnfrente.position, transform.right, diatanciaEnfrente, capaEnfrente);
        //para realizar un rayo desde la posición del controladorAbajo hacia arriba (transform.up * -1).
        informacionAbajo = Physics2D.Raycast(controladorAbajo.position, transform.up * -1, ditanciaAbajo, capaAbajo);

        if (informacionEnfrente || !informacionAbajo)
        {
            Girar();
        }
    }

    private void Girar() 
    {
        mirandoALaDerecha = !mirandoALaDerecha;
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 180, 0);
        VelocidadDeMovimiento *= -1;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(controladorAbajo.transform.position, controladorAbajo.transform.position + transform.up * -1 * ditanciaAbajo);
        Gizmos.DrawLine(controladorEnfrente.transform.position, controladorEnfrente.transform.position + transform.right * diatanciaEnfrente);
    }
}


