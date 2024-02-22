using UnityEngine;
using System.Collections.Generic;

public class Prefabs : MonoBehaviour
{
    public GameObject prefabToSpawn; //El prefab que será instanciado
    public int initialPrefabCount = 4; //Número inicial de prefabs
    public float spawnRadius = 9f; //Radio dentro del cual se generarán los prefabs
    public float destroyTime = 59f; //Tiempo en segundos antes de destruir los prefabs
    public float timeToAdd = 5f; //Tiempo a añadir al contador cuando se eliminen todos los prefabs

    private List<GameObject> spawnedPrefabs = new List<GameObject>(); //Lista de prefabs instanciados
    private float timer = 0f; //Contador de tiempo

    private void Start()
    {
        SpawnPrefabs(initialPrefabCount); //Instanciar los prefabs al inicio
    }

    private void Update()
    {
        timer += Time.deltaTime; //Incrementar el temporizador
        if (timer >= destroyTime)
        {
            DestroyPrefabs(); //Destruir los prefabs al alcanzar el tiempo límite
            SpawnPrefabs(spawnedPrefabs.Count + 1); //Incrementar el número de prefabs
            timer = 0f; //Reiniciar el temporizador
        }
    }

    private void SpawnPrefabs(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Vector3 randomPosition = GetRandomPosition(); //Obtener una posición aleatoria
            GameObject prefab = Instantiate(prefabToSpawn, randomPosition, Quaternion.identity); //Instanciar el prefab
            spawnedPrefabs.Add(prefab); //Agregar el prefab a la lista
        }
    }

    private void DestroyPrefabs()
    {
        foreach (GameObject prefab in spawnedPrefabs)
        {
            Destroy(prefab); //Destruir todos los prefabs
        }
        spawnedPrefabs.Clear(); //Limpiar la lista de prefabs
    }

    private Vector3 GetRandomPosition()
    {
        Vector3 randomDirection = Random.insideUnitSphere * spawnRadius; //Obtener una dirección aleatoria dentro del radio
        randomDirection += transform.position; //Ajustar la posición relativa al objeto que contiene el script
        randomDirection.y = 0; //Mantener la posición en el plano XY
        Vector3 newPosition = randomDirection; //Nueva posición del prefab
        Collider[] colliders = Physics.OverlapBox(newPosition, new Vector3(1, 1, 1)); //Verificar colisiones
        if (colliders.Length > 0)
        {
            return GetRandomPosition(); //Si hay colisiones, obtener otra posición aleatoria
        }
        return newPosition; //Devolver la posición si no hay colisiones
    }
}