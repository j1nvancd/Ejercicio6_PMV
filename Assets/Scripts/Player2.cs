using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class Player2 : MonoBehaviour
{
    //Velocidad del jugador
    public float speed;
    private Rigidbody rb;

    private float movementX;
    private float movementY;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    private void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);
        rb.AddForce(movement * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            Destroy(other.gameObject);
            GameManager.instance.AddScore(1);
        }
    }
}
