using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAround : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;

    private float XMouse = 0f;
    private float YMouse = 0f;
    private float sensitivity = 2f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MouseTracking();
    }

    private void MouseTracking()
    {
        XMouse += Input.GetAxis("Mouse X") * sensitivity;
        YMouse -= Input.GetAxis("Mouse Y") * sensitivity;

        YMouse = Mathf.Clamp(YMouse, -90f, 90f);

        transform.localEulerAngles = new Vector3(YMouse, 0, 0);
        playerTransform.localEulerAngles = new Vector3(0, XMouse, 0);
    }
}
