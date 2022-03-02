using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LookAround : MonoBehaviour
{
    private Camera cam;
    private Rigidbody rb;

    [SerializeField] private float sensitivity = 10f;
    private float multiplier = 0.01f;
    private float xMouse;
    private float yMouse;
    private float xRotation;
    private float yRotation;
    private float smoothTime = 5f;

    private Vector2 lookInput;

   

    private void Awake()
    {
        StartCoroutine(PostUpdateRb());
    }

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponentInChildren<Camera>();
        rb = GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    

    // Update is called once per frame
    void Update()
    {
        Look();

        cam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    private void Look()
    {
        xMouse = lookInput.x * sensitivity * multiplier;
        yMouse = lookInput.y * sensitivity * multiplier;

        yRotation += xMouse;
        xRotation -= yMouse;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
    }

    IEnumerator PostUpdateRb()
    {
        YieldInstruction waitForFixedUpdate = new WaitForFixedUpdate();
        while (true)
        {
            yield return waitForFixedUpdate;

            rb.MoveRotation(Quaternion.AngleAxis(xMouse, Vector3.up));
        }
    }
    


}
