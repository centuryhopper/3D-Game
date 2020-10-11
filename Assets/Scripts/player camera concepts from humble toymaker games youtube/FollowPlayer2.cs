using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer2 : MonoBehaviour
{
    public Transform target;
    public Vector3 cameraOffset;
    public float rotateSpeed;
    public Transform pivot;

    // Start is called before the first frame update
    void Start()
    {
        cameraOffset = target.position - transform.position;

        pivot.transform.position = target.position;
        pivot.parent = target;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float horizontal = Input.GetAxis("Mouse X") * rotateSpeed;
        float vertical = Input.GetAxis("Mouse Y")  * rotateSpeed;

        target.Rotate(0, horizontal, 0);
        pivot.Rotate(vertical, 0, 0);

        float desiredAngleY = target.eulerAngles.y;
        float desiredAngleX = pivot.eulerAngles.x;

        Quaternion rotation = Quaternion.Euler(desiredAngleX, desiredAngleY, 0);

        transform.position = target.position - (rotation * cameraOffset);

        transform.LookAt(target);
    }
}
