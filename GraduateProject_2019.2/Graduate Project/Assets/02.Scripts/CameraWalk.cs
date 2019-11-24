using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraWalk : MonoBehaviour
{
    private float mouseY = 0.0f;
    [SerializeField]
    private float rotationSpeed = 1.0f;

    [SerializeField]
    private float mouseYMin= 1.0f;
    [SerializeField]
    private float mouseYMax = 100.0f;

    [SerializeField]
    private float currentMouseY = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        mouseY = Input.GetAxis("Mouse Y");
        currentMouseY += mouseY;
        currentMouseY = Mathf.Clamp(currentMouseY, mouseYMin, mouseYMax);
        
        Vector3 rotation = new Vector3(-currentMouseY, 0, 0);

        transform.localEulerAngles = (rotation * rotationSpeed);
    }
}
