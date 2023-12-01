using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JohnMovement : MonoBehaviour
{
    public float Speed;
    public float JumpForce;
    public GameObject BulletPrefab;

    private Rigidbody2D Rigidbody2D;
    private Animator Animator;
    private float Horizontal;
    private bool Grounded;
    private float LastShoot;
    private int Health = 5;
    private int disparosNecesarios = 5;
    private int contadorDisparos = 0;
    private int vidajugador = 5;
    public int puntos { get; set; } = 0; // Agrega esta línea para declarar la variable 'puntos'
    public Text puntosText; // Variable para referenciar el objeto Text que mostrará los puntos
    private Score ScoreInstance;

    private bool isDead = false;
    public GameObject gameOverCanvas; // Asigna el Canvas de Game Over desde el Inspector
    [SerializeField] private BarraDeVida barraDeVida;
    private Score Instance;

    private void Awake() 
    {
        Instance = Score.ScoreInstance;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isDead && collision.gameObject.CompareTag("zombie"))
        {
            StartCoroutine(AtaqueZombie());
            Debug.Log("vida = " + vidajugador);
        }
    }

    public IEnumerator AtaqueZombie()
    {
        vidajugador = vidajugador - 1;

        // Asegurarse de que la vida no sea menor que cero
        vidajugador = Mathf.Max(vidajugador, 0);

        // Calcular el porcentaje de vida actual
        float porcentajeVida = (float)vidajugador / 5.0f;

        // Actualizar la barra de vida con el porcentaje actual
        barraDeVida.CambiarVidaPorcentaje(porcentajeVida);

        Animator.SetBool("daño", true);
        yield return new WaitForSeconds(0.100f);
        Animator.SetBool("daño", false);

        if (vidajugador <= 0)
        {
            Morir();
            Destroy(gameObject);
        }
    }

    private void Morir()
    {
        isDead = true;
        Debug.Log("John ha muerto");

        // Activa el Canvas de Game Over
        if (gameOverCanvas != null)
        {
            gameOverCanvas.SetActive(true);
        }
        else
        {
            Debug.LogError("No se ha asignado el Canvas de Game Over en el Inspector.");
        }
    }

    private void Start()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
    }

    //sumar puntos al mator un zombie
    public void SumarPuntosPorMatarZombie()
    {
        puntos += 10; // O la cantidad de puntos que desees asignar por matar un zombie
        Instance.score += 10;
        Debug.Log("Puntos: " + puntos);

        // Actualizar el texto en la pantalla
        if (puntosText != null)
        {
            puntosText.text = "Score: " + puntos;
        }
    }

    private void Update()
    {
        // Movimiento
        Horizontal = Input.GetAxisRaw("Horizontal");

        if (Horizontal < 0.0f) transform.localScale = new Vector3(-2.0f, 2.0f, 2.0f);
        else if (Horizontal > 0.0f) transform.localScale = new Vector3(2.0f, 2.0f, 2.0f);

        Animator.SetBool("running", Horizontal != 0.0f);

        // Detectar Suelo
        // Debug.DrawRay(transform.position, Vector3.down * 0.1f, Color.red);
        if (Physics2D.Raycast(transform.position, Vector3.down, 0.2f))
        {
            Grounded = true;
        }
        else Grounded = false;

        // Salto
        if (Input.GetKeyDown(KeyCode.W) && Grounded)
        {
            Jump(4f); // Puedes ajustar el valor (20f) para cambiar la altura del salto
        }

        // ...

        void Jump(float jumpForce)
        {
            // Aplica la fuerza vertical al Rigidbody2D
            GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, jumpForce);
        }

        // Disparar
        if (Input.GetKey(KeyCode.Space) && Time.time > LastShoot + 0.25f)
        {
            Shoot();
            LastShoot = Time.time;
        }
        //destruir jugador
    }

    private void FixedUpdate()
    {
        Rigidbody2D.velocity = new Vector2(Horizontal * Speed, Rigidbody2D.velocity.y);
    }

    private void Jump()
    {
        Rigidbody2D.AddForce(Vector2.up * JumpForce);
    }

    private void Shoot()
    {
        Vector3 direction;
        if (transform.localScale.x == 2.0f) direction = Vector3.right;
        else direction = Vector3.left;

        GameObject bullet = Instantiate(BulletPrefab, transform.position + direction * 0.2f, Quaternion.identity);
        bullet.GetComponent<BulletScript>().SetDirection(direction);

        // Incrementar el contador de disparos
        contadorDisparos++;

        // Verificar si se alcanzó el número de disparos necesarios
        if (contadorDisparos >= disparosNecesarios)
        {
            // Lógica para matar al zombie
            MatarZombie();
        }
    }

    private void MatarZombie()
    {
        // Implementa la lógica para matar al zombie aquí
        // Puedes desactivar el GameObject del zombie, reproducir una animación, etc.
        // Asumamos que el zombie está referenciado en la variable zombieGameObject
        // zombieGameObject.SetActive(false);
    }

    public void Hit()
    {
        Health -= 2;
        if (Health == 0) Destroy(gameObject);
    }
}