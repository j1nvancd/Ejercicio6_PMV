using UnityEngine;
using TMPro;

public class spawn2 : MonoBehaviour
{
    //Prefab a instanciar
    public GameObject prefab;
    //Margen para evitar los bordes
    public float margen = 1f;
    //Distancia mínima entre prefabs
    public float distanciaMinimaEntrePrefabs = 2f;

    //Dimensiones del plano de juego
    private float minX, maxX, minZ, maxZ;
    

    private void Start()
    {
        //Calcular los límites del plano del juego
        CalculateBounds();
        //Instanciar los prefabs inniciales
        GeneratePrefabs(GameManager.instance.cantidadInicial);
    }

    private void Update()
    {
        //Verificar si todos los prefabs han sido destruidos
        if (AreAllPrefabsDestroyed())
        {
            //Añade un prefab más a generar
            GameManager.instance.cantidadInicial++;
            //Generar nuevos prefabs
            GeneratePrefabs(GameManager.instance.cantidadInicial);
        }

        //Verifica si el tiempo ha llegado a 0
       /* if (GameManager.instance.tiempoTranscurrido >= GameManager.instance.tiempoParaTerminar)
        {
            //Salir del juego
            GameManager.instance.QuitGame();
        }*/
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
        GameManager.instance.tiempoTranscurrido = 0f;
        //Destruye todos los prefabs existentes (en caso de que hubiese elimina posibles fallos)
        DestroyExistingPrefabs();

        //Genera los nuevos prefabs
        for (int i = 0; i < cantidad; i++)
        {
            Vector3 posicionAleatoria = GetRandomPosition();
            GameObject newPrefab = Instantiate(prefab, posicionAleatoria, Quaternion.identity);
            GameManager.instance.prefabsInstanciados.Add(newPrefab);
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

    //Método para destruir todos los prefabs existentes y limpiar la lista
    private void DestroyExistingPrefabs()
    {
        foreach (GameObject existingPrefab in GameManager.instance.prefabsInstanciados)
        {
            Destroy(existingPrefab);
        }
        //Limpia la lisita de los prefabs 
        GameManager.instance.prefabsInstanciados.Clear();
    }

    //Método para verificar si todos los prefabs han sido destruidos
    private bool AreAllPrefabsDestroyed()
    {
        foreach (GameObject prefabInstance in GameManager.instance.prefabsInstanciados)
        {
            if (prefabInstance != null)
            {
                return false; //Aún hay prefabs activos
            }
        }
        return true; //Todos los prefabs fueron destruidos
    }
}
