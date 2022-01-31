using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAround : MonoBehaviour
{
    private CapsuleCollider cam;

    private float XMouse = 0f;
    private float YMouse = 0f;
    [SerializeField] private float sensitivity = 2f;

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponentInParent<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        Look();
    }


    private void Look()
    {
        YMouse -= Input.GetAxisRaw("Mouse Y") * sensitivity;
        YMouse = Mathf.Clamp(YMouse, -90f, 90f);
        XMouse += Input.GetAxisRaw("Mouse X") * sensitivity;
        cam.transform.localRotation = Quaternion.Euler(YMouse, XMouse, 0);
    }

    


}
