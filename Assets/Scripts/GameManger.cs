using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; //Instancia singleton del GameManager

    //Variables para el tiempo
    public TextMeshProUGUI tiempoText; //Referencia al texto de tiempo en el HUD
    public float tiempoParaTerminar = 59f; //Tiempo en segundos para finalizar el juego
    public float tiempoTranscurrido = 0f; //Tiempo en segundos transcurrido del juego

    public int cantidadInicial = 4; //Cantidad inicial  de prefabs

    //Lista para rastrear prefabs instanciados
    public List<GameObject> prefabsInstanciados = new List<GameObject>();

    // Variables para el score
    private int score = 0;
    public TextMeshProUGUI scoreText;

    public GameObject endGameMenu;
    

    private void Start()
    {
        endGameMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    void Awake()
    {
        // Configurar la instancia singleton del GameManager
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        //Actualizar el tiempo transcurrido
        tiempoTranscurrido += Time.deltaTime;

        //Actualizar el tiempo en el HUD
        tiempoText.text = "Tiempo: " + Mathf.RoundToInt(tiempoParaTerminar - tiempoTranscurrido).ToString();

        //Verificar si el tiempo ha llegado a cero
        if (tiempoTranscurrido >= tiempoParaTerminar)
        {    
            showEndGameMenu();
        }
    }

    private void showEndGameMenu()
    {
        Time.timeScale = 0f;
        endGameMenu.SetActive(true);
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
    }

    //Método para agregar puntos al score
    public void AddScore(int points)
    {
        score += points;
        UpdateScoreText();
    }

    //Método para actualizar el texto del score
    private void UpdateScoreText()
    {
        scoreText.text = $"Score : {score:0000}";
    }

    //Método para salir de la aplicación o del editor
    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false; //Salir del editor
        #else
            Application.Quit(); //Salir de la aplicación
        #endif
    }
}
