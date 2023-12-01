using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class Oleadas : MonoBehaviour
{
    private float minX, maxX, minY, maxY;

    [SerializeField] private Transform[] puntos;

    [SerializeField] private GameObject zombiePrincipalPrefab;
    private GameObject zombiePrincipalInstance;
    private int zombiesPorOleada = 5; // Número de zombies por oleada
    private int totalOleadas = 3; // Total de oleadas
    private List<GameObject> zombiesActivos = new List<GameObject>();

    [SerializeField] private float tiempoEsperaEntreZombies = 2f; // Tiempo de espera entre la generación de zombies
    [SerializeField] private float tiempoEsperaEntreOleadas = 10f; // Tiempo de espera entre oleadas

    public Canvas nivel2Canvas; // Arrastra y suelta el Canvas del Nivel 2 en el Inspector
    private int oleadasGeneradas = 0;
    private int zombiesRestantes;
    private Score Instance;

    private void Awake()
    {
        Instance = Score.ScoreInstance;
    }

    private bool nivelGanado = false;

    private void Start()
    {
        maxX = puntos.Max(punto => punto.position.x);
        minX = puntos.Min(punto => punto.position.x);
        maxY = puntos.Max(punto => punto.position.y);
        minY = puntos.Min(punto => punto.position.y);

        // Calcula el total de zombies para todas las oleadas
        zombiesRestantes = zombiesPorOleada * totalOleadas;

        // Desactiva el Canvas del Nivel 2 al inicio
        if (nivel2Canvas != null)
        {
            nivel2Canvas.gameObject.SetActive(false);
        }

        // Crea la instancia del zombie principal y rastrea la referencia
        zombiePrincipalInstance = Instantiate(zombiePrincipalPrefab, Vector3.zero, Quaternion.identity);
        zombiePrincipalInstance.SetActive(false);

        StartCoroutine(GenerarOleadas());
    }

    private IEnumerator GenerarOleadas()
    {
        while (oleadasGeneradas < totalOleadas)
        {
            for (int i = 0; i < zombiesPorOleada; i++)
            {
                CrearZombie();
                zombiesRestantes--;
                yield return new WaitForSeconds(tiempoEsperaEntreZombies);
            }

            oleadasGeneradas++;

            // Espera entre oleadas
            if (oleadasGeneradas < totalOleadas)
            {
                yield return new WaitForSeconds(tiempoEsperaEntreOleadas);
            }
        }
    }

    private void CrearZombie()
    {
        Vector2 posicionAleatoria = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
        GameObject zombieClone = Instantiate(zombiePrincipalPrefab, posicionAleatoria, Quaternion.identity);

        // Agrega el zombie clon a la lista de zombies activos
        zombiesActivos.Add(zombieClone);
    }

    private void Update()
    {
        // Verifica si todos los zombies han sido eliminados y el nivel aún no se ha ganado
        if (zombiesRestantes <= 0 && !nivelGanado && !ZombiesActivos())
        {
            nivelGanado = true; // Marca el nivel como ganado para evitar repeticiones
            JohnGanoNivel();
        }
    }

    private bool ZombiesActivos()
    {
        // Filtra los zombies activos en la lista
        zombiesActivos = zombiesActivos.Where(zombie => zombie != null && zombie.activeSelf).ToList();

        // Retorna true si hay zombies activos, false si no hay ninguno
        return zombiesActivos.Count > 0;
    }

    // Método para manejar el evento cuando John gana el nivel
    private void JohnGanoNivel()
    {
        Debug.Log("¡John ha ganado el nivel!");

        // Activa el Canvas del Nivel 2
        if (nivel2Canvas != null)
        {
            nivel2Canvas.gameObject.SetActive(true);
            // Buscar el score actual del jagador (necesitamos el nombre del juegador)

            MenuPrincipal MPInstance = MenuPrincipal.MPInstance;
            DBMongo datos = DBMongo.DBMongoInstance;
            datos.Registrar(MPInstance.NombreJugador, Instance.score);
        }

        // Desactiva el Zombie Principal
        if (zombiePrincipalInstance != null)
        {
            zombiePrincipalInstance.SetActive(false);
        }
    }
}