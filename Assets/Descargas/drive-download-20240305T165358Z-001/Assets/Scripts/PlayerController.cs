using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    // Speed at which the player moves.
    public float speed;

    public ParticleSystem coinParticles; // Referencia al sistema de partículas de la moneda
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI tiempoText; // Referencia al texto de tiempo en el HUD

    private Rigidbody rb;
    
    // Movement along X and Y axes.
    private float movementX;
    private float movementY;

    //Number of collected PickUps
    private int count;

    // Start is called before the first frame update
    void Start()
    {
        // Get and store the Rigidbody component attached to the player.
        rb = GetComponent<Rigidbody>();
    }

    // This function is called when a move input is detected.
    private void OnMove(InputValue movementValue)
    {
        // Convert the input value into a Vector2 for movement.
        Vector2 movementVector = movementValue.Get<Vector2>();

        // Store the X and Y components of the movement.
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    // FixedUpdate is called once per fixed frame-rate frame.
    void FixedUpdate()
    {
        // Create a 3D movement vector using the X and Y inputs.
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);

        // Apply force to the Rigidbody to move the player.
        rb.AddForce(movement * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            // Activate the particle system
            coinParticles.Play();

            Destroy(other.gameObject);
            count++;
            scoreText.text = $"Score : {count * 125:0000}";
        }
    }
}