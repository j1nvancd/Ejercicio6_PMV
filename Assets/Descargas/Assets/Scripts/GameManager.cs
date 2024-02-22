using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI  scoreText; //Texto para mostrar el marcador
    public TextMeshProUGUI timerText; //Texto para mostrar el tiempo restante

    private float score = 0f; //Puntaje del jugador
    private float timeLeft = 59f; //Tiempo restante

    private void Start()
    {
        UpdateUI(); //Actualizar el HUD al inicio del juego
    }

    private void Update()
    {
        timeLeft -= Time.deltaTime; //Reducir el tiempo restante
        if (timeLeft <= 0f)
        {
            //Juego terminado
            Debug.Log("Game Over");
        }
        UpdateUI(); //Actualizar el HUD en cada frame
    }

    private void UpdateUI()
    {
        scoreText.text = "Score: " + score.ToString(); //Mostrar el marcador actualizado
        timerText.text = "Time: " + Mathf.RoundToInt(timeLeft).ToString(); //Mostrar el tiempo restante
    }
}
