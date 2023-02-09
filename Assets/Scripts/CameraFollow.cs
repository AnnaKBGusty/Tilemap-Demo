using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform _player;
    public float smoothSpeed = 0.05f;
    public Vector3 locationOffset;

    void FixedUpdate()
    {
        Vector3 desiredPosition = _player.position + locationOffset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
