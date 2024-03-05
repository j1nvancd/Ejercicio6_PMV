using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn_Obj : MonoBehaviour
{
    public GameObject prefab; //Prefab a instanciar
    public float margen = 1f; //Margen para evitar los bordes del plano
    public float tiempoParaGenerar = 59f; //Tiempo en segundos para generar nuevos prefabs
    public int cantidadInicial = 4; //Cantidad inicial de prefabs
    public float distanciaMinimaEntrePrefabs = 2f; //Distancia mínima entre prefabs
    private List<GameObject> prefabsInstanciados = new List<GameObject>(); //Lista para rastrear prefabs instanciados

    private float tiempoTranscurrido = 0f;
    private int cantidadActual = 4;

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

        //Si el tiempo transcurrido supera el tiempo para generar, generar nuevos prefabs
        if (tiempoTranscurrido >= tiempoParaGenerar || prefabsInstanciados.Count == 0)
        {
            tiempoTranscurrido = 0f;
            cantidadActual++;
            GeneratePrefabs(cantidadActual);
        }
    }

    //Método para calcular los límites del plano de juego
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

    // Método llamado cuando se destruye un prefab
    public void PrefabDestroyed(GameObject prefab)
    {
        prefabsInstanciados.Remove(prefab); // Remover el prefab de la lista de prefabs instanciados
        Destroy(prefab); // Destruir el prefab
    }

    // Método para destruir todos los prefabs existentes
    private void DestroyExistingPrefabs()
    {
        foreach (GameObject existingPrefab in prefabsInstanciados)
        {
            Destroy(existingPrefab);
        }
        prefabsInstanciados.Clear(); // Limpiar la lista de prefabs instanciados
    }
}
