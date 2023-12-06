using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movimiento : MonoBehaviour
{
    //Referencia rigidbody 2d para movimiento del personaje.
    private Rigidbody2D rb2D;

    //Crea un encabezado en el Inspector de Unity llamado "Movimiento" para organizar visualmente las siguientes variables.
    [Header("Movimiento")]

    //Almacena el valor del movimiento horizontal del objeto.
    private float movimientoHorizontal = 0f;

    //Variable visible en el Inspector que representa la velocidad de movimiento del objeto.
    [SerializeField] private float velocidadDeMovimiento;

    //Variable visible en el Inspector que puede estar relacionada con la suavización del movimiento.
    [SerializeField] private float suavizadoDeMovimiento;

    //velocidad tridimensional del objeto.
    private Vector3 velocidad = Vector3.zero;

    //Indica si el objeto está inicialmente mirando hacia la derecha (valor booleano).
    private bool mirandoDerecha = true;

    [Header("salto")]

    //Fuerza salto
    [SerializeField] private float fuerzaDeSalto;
    //identificar el suelo
    [SerializeField] private LayerMask queEsSuelo;
    //detectar si el personaje esta en contacto con el suelo
    [SerializeField] private Transform controladorSuelo;
    // Se utiliza para detectar el suelo
    [SerializeField] private Vector3 dimensionesCaja;
    //Identifica si el objeto controlado está en contacto con el suelo.
    [SerializeField] private bool enSuelo;

    private bool salto = false;

    //se utiliza para acceder al componente de físicas 2D adjunto al objeto
    private void Start() 
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    //devuelve un valor -1, 0 o 1 según si las teclas de dirección izquierda, derecha o ninguna están siendo presionadas
    private void Update() 
    {
        movimientoHorizontal = Input.GetAxisRaw("Horizontal") * velocidadDeMovimiento;

        if (Input.GetButtonDown("Jump"))
        {
            salto = true;
        }
    }

    //Este método se llama en intervalos regulares de tiempo fijos y se encarga de llamar al método Mover()
    //Esto asegura que el movimiento sea suave y consistente incluso si los cuadros por segundo varían.
    private void FixedUpdate() 
    {
        enSuelo = Physics2D.OverlapBox(controladorSuelo.position, dimensionesCaja, 0f, queEsSuelo);

        Mover(movimientoHorizontal * Time.fixedDeltaTime, salto);

        salto = false;
    }

    //Este método se encarga de manejar el movimiento del objeto. 
    private void Mover(float mover, bool saltar) 
    {
       Vector3 velocidadObjetivo = new Vector2(mover, rb2D.velocity.y);
        rb2D.velocity = Vector3.SmoothDamp(rb2D.velocity, velocidadObjetivo, ref velocidad, suavizadoDeMovimiento);

        if (mover > 0 && !mirandoDerecha)
        {
            Girar();
        }

        else if (mover < 0 && mirandoDerecha)
        {
            Girar();
        }

        if (enSuelo && saltar)
        {
            enSuelo = false;
            rb2D.AddForce(new Vector2(0f, fuerzaDeSalto));
        }
    }

    //Este método invierte la orientación del objeto horizontalmente al cambiar el signo de la escala en el eje x.
    private void Girar() 
    {
        mirandoDerecha = !mirandoDerecha;
        Vector3 escala = transform.localScale;
        escala.x *= -1;
        transform.localScale = escala;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(controladorSuelo.position, dimensionesCaja);
    }

}
