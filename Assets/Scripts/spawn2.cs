using UnityEngine;
using TMPro;

public class spawn2 : MonoBehaviour
{
    #region Variables de los prefabs
    public GameObject prefab; //Prefab a instanciar
    public float distanciaMinimaEntrePrefabs = 2f; //Distancia mínima entre prefabs
    #endregion
    
    #region Variables del plano de spawn
    public float margen = 1f; //Margen para evitar los bordes
    private float minX, maxX, minZ, maxZ; //Dimensiones del plano de juego  
    #endregion

    private void Start()
    {
        CalculateBounds(); //Calcular los límites del plano del juego
        GeneratePrefabs(GameManager.instance.cantidadInicial); //Instanciar los prefabs inniciales
    }

    private void Update()
    {
        //Verificar si todos los prefabs han sido destruidos
        if (AreAllPrefabsDestroyed())
        {          
            GameManager.instance.cantidadInicial++; //Añade un prefab más a generar 
            GeneratePrefabs(GameManager.instance.cantidadInicial); //Generar nuevos prefabs
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
        GameManager.instance.tiempoTranscurrido = 0f;
        
        DestroyExistingPrefabs(); //Destruye todos los prefabs existentes (en caso de que hubiese elimina posibles fallos)

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
