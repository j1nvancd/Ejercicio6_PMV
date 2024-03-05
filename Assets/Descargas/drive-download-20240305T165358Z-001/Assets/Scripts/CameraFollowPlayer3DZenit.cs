using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer3DZenit : MonoBehaviour
{
    // camera follow player
    [SerializeField] private Transform player;
    [SerializeField] private float cameraSpeed = 5f;
    [SerializeField] private bool rotateCamera = false;
    
    // set constant delta distance
    private const float _deltaDistance = 0.1f;
    
    private void Start()
    {
        // get reference to the player
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }
    
    private void LateUpdate()
    {
        // follow the player on X and Z axis
        float cameraX = transform.position.x;
        float cameraZ = transform.position.z;
        // verify distance between the camera and the player
        if (Mathf.Abs(player.position.x - transform.position.x) > _deltaDistance)
        {
            cameraX = Mathf.Lerp(transform.position.x, player.position.x, cameraSpeed * Time.deltaTime);
        }
        if (Mathf.Abs(player.position.z - transform.position.z) > _deltaDistance)
        {
            cameraZ = Mathf.Lerp(transform.position.z, player.position.z, cameraSpeed * Time.deltaTime);
        }
        // set the camera position
        transform.position = new Vector3(cameraX, transform.position.y, cameraZ);
        // keep the rotation of the camera constant to -Y axis or rotate with the player in XZ plane
        if (rotateCamera)
        {
            transform.rotation = Quaternion.Euler(90, player.eulerAngles.y, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(90, 0, 0);
        }
    }
}
