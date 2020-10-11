using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    Transform target;


    void Awake()
    {
        target = GameObject.FindWithTag("Player").transform;
    }

    [SerializeField] float smoothSpeed = 0.125f;
    [SerializeField] Vector3 cameraOffset;
    Vector3 velocity;

    void FixedUpdate()
    {
        // offset the camera pos
        Vector3 desiredPosition = target.position + cameraOffset;
        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed);
        transform.position = smoothedPosition;

        // look at player
        transform.LookAt(target);
    }
}
