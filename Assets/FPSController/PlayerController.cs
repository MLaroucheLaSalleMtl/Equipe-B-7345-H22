using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float baseSpeed = 40f;
    private Vector2 moveInput = Vector2.zero;
    private Vector3 playerMovement;

    [SerializeField] private float runMultiplier = 1.5f;
    private bool isRunning;

    [SerializeField] private float crouchMultiplier = 0.5f;
    [SerializeField] private float slideSpeed = 12f;

    [SerializeField] private float jumpForce = 5f;
    private bool jumpInput = false;
    private float airDrag = 1f;
    
    private bool isGrounded;
    private float groundDrag = 6f;

    private bool fireInput = false;
    private bool aimDownSightsInput = false;
    private bool reloadInput = false;

    private CapsuleCollider capsule;
    private float capsuleScale;

    public bool FireInput { get => fireInput; set => fireInput = value; }
    public bool AimDownSightsInput { get => aimDownSightsInput; set => aimDownSightsInput = value; }
    public bool ReloadInput { get => reloadInput; set => reloadInput = value; }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        capsule = GetComponentInChildren<CapsuleCollider>();
        capsuleScale = capsule.height;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Moving();
        CheckIfGrounded();
        Jumping();
        DragValue();
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        jumpInput = context.performed;
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded)
        {
            moveSpeed *= runMultiplier;
            isRunning = true;
        }
        else if (context.canceled)
        {
            moveSpeed = baseSpeed;
            isRunning = false;
        }
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded)
        {
            moveSpeed *= crouchMultiplier;
            transform.localPosition = new Vector3(transform.position.x, transform.position.y * 0.5f, transform.position.z);
            transform.localScale = new Vector3(1f, 0.5f, 1f);
            if (isRunning)
            {
                rb.AddForce(transform.forward * slideSpeed, ForceMode.VelocityChange);
                isRunning = false;
            }
        }
        else if (context.canceled)
        {
            moveSpeed = baseSpeed;
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        FireInput = context.performed;
    }

    public void OnAimDownSights(InputAction.CallbackContext context)
    {
        AimDownSightsInput = context.performed;
    }

    public void OnReload(InputAction.CallbackContext context)
    {
        ReloadInput = context.performed;
    }

    void Moving()
    {
        playerMovement = transform.forward * moveInput.y + transform.right * moveInput.x;
        if (isGrounded)
        {
            rb.AddForce(playerMovement.normalized * moveSpeed, ForceMode.Acceleration);
        }
        else
        {
            rb.AddForce(playerMovement.normalized * moveSpeed * 0.1f, ForceMode.Acceleration);
        }
    }

    void CheckIfGrounded()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, capsuleScale * 0.5f + 0.1f);
    }

    void DragValue()
    {
        if (isGrounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = airDrag;
        }
    }

    void Jumping()
    {
        if (jumpInput && isGrounded)
        {
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            jumpInput = false;
        }
    }
}
