using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Spawn_Obj : MonoBehaviour
{
    public GameObject prefab; //Prefab a instanciar
    public float margen = 1f; //Margen para evitar los bordes del plano
    public float tiempoParaGenerar = 59f; //Tiempo en segundos para finalizar el juego
    public int cantidadInicial = 4; //Cantidad inicial de prefabs
    public float distanciaMinimaEntrePrefabs = 2f; //Distancia mínima entre prefabs
    public TextMeshProUGUI tiempoText; // Referencia al texto de tiempo en el HUD

    public List<GameObject> prefabsInstanciados = new List<GameObject>(); //Lista para rastrear prefabs instanciados
    private float tiempoTranscurrido = 0f;

    // Dimensiones del plano de juego
    private float minX, maxX, minZ, maxZ;

    void Start()
    {
        //Calcular los límites del plano de juego
        CalculateBounds();

        //Instanciar prefabs iniciales
        GeneratePrefabs(cantidadInicial);
    }

    void Update()
    {
        //Actualizar el tiempo transcurrido
        tiempoTranscurrido += Time.deltaTime;

        // Actualizar el tiempo en el HUD
        tiempoText.text = "Tiempo: " + Mathf.RoundToInt(tiempoParaGenerar - tiempoTranscurrido).ToString();

        // Verificar si todos los prefabs han sido destruidos
        if (AreAllPrefabsDestroyed())
        {
            cantidadInicial ++;
            // Generar nuevos prefabs
            GeneratePrefabs(cantidadInicial);
        }

        // Verificar si el tiempo ha llegado a cero
        if (tiempoTranscurrido >= tiempoParaGenerar)
        {
            // Salir de la aplicación o del editor
            QuitGame();
        }
    }

    // Método para calcular los límites del plano de juego
    private void CalculateBounds()
    {
        //Obtener los límites del plano
        Renderer renderer = GetComponent<Renderer>();
        minX = transform.position.x - renderer.bounds.size.x / 2 + margen;
        maxX = transform.position.x + renderer.bounds.size.x / 2 - margen;
        minZ = transform.position.z - renderer.bounds.size.z / 2 + margen;
        maxZ = transform.position.z + renderer.bounds.size.z / 2 - margen;
    }

    //Método para generar prefabs en posiciones aleatorias sin superponerse
    private void GeneratePrefabs(int cantidad)
    {
         tiempoTranscurrido = 0f;

        //Destruir todos los prefabs existentes
        DestroyExistingPrefabs();

        //Generar nuevos prefabs
        for (int i = 0; i < cantidad; i++)
        {
            Vector3 posicionAleatoria = GetRandomPosition();
            GameObject newPrefab = Instantiate(prefab, posicionAleatoria, Quaternion.identity);
            prefabsInstanciados.Add(newPrefab); // Agregar el nuevo prefab a la lista de prefabs instanciados   
        }
    }

    //Método para obtener una posición aleatoria dentro del rango del plano de juego sin superponerse
    private Vector3 GetRandomPosition()
    {
        Vector3 randomPosition;
        bool positionValid = false;

        do
        {
            randomPosition = new Vector3(Random.Range(minX, maxX), 1f, Random.Range(minZ, maxZ));
            Collider[] colliders = Physics.OverlapBox(randomPosition, new Vector3(distanciaMinimaEntrePrefabs, 0.1f, distanciaMinimaEntrePrefabs));
            positionValid = colliders.Length == 0;
        } while (!positionValid);

        return randomPosition;
    }

    // Método para destruir todos los prefabs existentes y limpiar la lista
    private void DestroyExistingPrefabs()
    {
        foreach (GameObject existingPrefab in prefabsInstanciados)
        {
            Destroy(existingPrefab);
        }
        prefabsInstanciados.Clear(); // Limpiar la lista de prefabs instanciados
    }

    // Método para verificar si todos los prefabs han sido destruidos
    private bool AreAllPrefabsDestroyed()
    {
        foreach (GameObject prefabInstance in prefabsInstanciados)
    {
        if (prefabInstance != null)
        {
            return false; // Aún hay prefabs activos
        }
    }
    return true; // Todos los prefabs han sido destruidos
    }

    private void QuitGame()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false; // Salir del editor
        #else
                Application.Quit(); // Salir de la aplicación
        #endif
    }
}