using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private Camera cam;

    private CapsuleCollider capsule;
    private float capsuleScale;

    [Header("Player movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float baseSpeed = 40f;
    private Vector2 moveInput = Vector2.zero;
    private Vector3 playerMovement;

    [SerializeField] private float runMultiplier = 1.5f;
    private bool isRunning;

    [Header("Player crouch")]
    [SerializeField] private float crouchSpeed = 20f;
    [SerializeField] private float slideSpeed = 12f;
    private bool isCrouched;

    [Header("Player jump")]
    [SerializeField] private float jumpForce = 5f;
    private bool jumpInput = false;
    private float airDrag = 1f;

    [Header("Player ground detection")]
    [SerializeField] private LayerMask groundCheck;
    private bool isGrounded;
    private float groundDistance = 0.4f;
    private float groundDrag = 6f;

    [Header("Player steps")]
    [SerializeField] private float stepHeight = 0.3f;
    [SerializeField] private float stepSmooth = 0.1f;
    [SerializeField] private GameObject rayStepUpper;
    [SerializeField] private GameObject rayStepLower;

    [Header("Player slopes handling")]
    RaycastHit slopeHit;
    private Vector3 movementOnSlopes;

    private bool fireInput = false;
    private bool aimDownSightsInput = false;
    private bool reloadInput = false;

    private bool firstWeaponInput = false;
    private bool secondWeaponInput = false;
    private bool thirdWeaponInput = false;
    private bool fourthWeaponInput = false;

    public bool FireInput { get => fireInput; set => fireInput = value; }
    public bool AimDownSightsInput { get => aimDownSightsInput; set => aimDownSightsInput = value; }
    public bool ReloadInput { get => reloadInput; set => reloadInput = value; }
    public bool FirstWeaponInput { get => firstWeaponInput; set => firstWeaponInput = value; }
    public bool SecondWeaponInput { get => secondWeaponInput; set => secondWeaponInput = value; }
    public bool ThirdWeaponInput { get => thirdWeaponInput; set => thirdWeaponInput = value; }
    public bool FourthWeaponInput { get => fourthWeaponInput; set => fourthWeaponInput = value; }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        capsule = GetComponent<CapsuleCollider>();
        capsuleScale = capsule.height;
        cam = GetComponentInChildren<Camera>();
    }

    private void Awake()
    {
        rayStepUpper.transform.position = new Vector3(rayStepUpper.transform.position.x, stepHeight, rayStepUpper.transform.position.z);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Moving();
        CheckIfGrounded();
        Jumping();
        DragValue();
        Stepping();
        OnSlopes();
        Crouching();
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
        isCrouched = context.performed;
        //if (context.performed && isGrounded && !OnSlopes())
        //{
        //    //transform.localPosition = new Vector3(transform.position.x, transform.position.y * 0.5f, transform.position.z);
        //    moveSpeed *= crouchMultiplier;
        //    capsule.height = capsuleScale * 0.5f;
        //    isCrouched = true;
        //    if (isRunning)
        //    {
        //        rb.AddForce(transform.forward * slideSpeed, ForceMode.VelocityChange);
        //        isRunning = false;
        //        moveSpeed = baseSpeed * crouchMultiplier;
        //    }
        //}
        //else if (context.canceled && !Physics.Raycast(transform.localPosition, Vector3.up, 2f))
        //{
        //    moveSpeed = baseSpeed;
        //    capsule.height = capsuleScale;
        //    isCrouched = false;
        //}
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

    public void OnFirstWeaponSwitch(InputAction.CallbackContext context)
    {
        FirstWeaponInput = context.performed;
    }
    public void OnSecondWeaponSwitch(InputAction.CallbackContext context)
    {
        SecondWeaponInput = context.performed;
    }
    public void OnThirdWeaponSwitch(InputAction.CallbackContext context)
    {
        ThirdWeaponInput = context.performed;
    }
    public void OnFourthWeaponSwitch(InputAction.CallbackContext context)
    {
        FourthWeaponInput = context.performed;
    }

    void ReturnBaseState()
    {
        moveSpeed = baseSpeed;
        capsule.height = capsuleScale;
    }

    void Moving()
    {
        playerMovement = transform.forward * moveInput.y + transform.right * moveInput.x;
        if (isGrounded || isCrouched)
        {
            rb.AddForce(playerMovement.normalized * moveSpeed, ForceMode.Acceleration);
        }
        else if (OnSlopes())
        {
            rb.AddForce(movementOnSlopes.normalized * moveSpeed, ForceMode.Acceleration);
        }
        else
        {
            rb.AddForce(playerMovement.normalized * moveSpeed * 0.1f, ForceMode.Acceleration);
        }
    }

    void Crouching()
    {
        //if(isCrouched && OnSlopes())
        //{
        //    ReturnBaseState();
        //}

        if (isCrouched)
        {
            moveSpeed = crouchSpeed;
            capsule.height = capsuleScale * 0.5f;
            //capsule.center = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
            if (isRunning)
            {
                rb.AddForce(transform.forward * slideSpeed, ForceMode.VelocityChange);
                rb.AddForce(Vector3.down * 3f, ForceMode.Impulse);
                isRunning = false;
                moveSpeed = crouchSpeed;
            }
        }
        else if (!isCrouched && !Physics.Raycast(transform.localPosition, Vector3.up, 2f))
        {
            ReturnBaseState();
        }
    }

    void Stepping()
    {
        //RaycastHit lowHit;
        //if (Physics.Raycast(rayStepLower.transform.position, transform.TransformDirection(Vector3.forward), out lowHit, 0.1f))
        //{
        //    RaycastHit upperHit;
        //    if (!Physics.Raycast(rayStepUpper.transform.position, transform.TransformDirection(Vector3.forward), out upperHit, 0.2f))
        //    {
        //        rb.position += new Vector3(0f, stepSmooth, 0f);
        //    }
        //}
    }

    bool OnSlopes()
    {
        movementOnSlopes = Vector3.ProjectOnPlane(playerMovement, slopeHit.normal).normalized;
        if (Physics.Raycast(capsule.transform.position, Vector3.down, out slopeHit, capsuleScale * 0.5f + 0.3f))
        {
            if (slopeHit.normal != Vector3.up)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    void CheckIfGrounded()
    {
        Vector3 halfHeight = new Vector3(0f, capsuleScale * 0.5f, 0f);
        isGrounded = Physics.CheckSphere(transform.position - halfHeight, groundDistance, groundCheck);  
    }

    void DragValue()
    {
        if (isGrounded || isCrouched)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = airDrag;
        }
        rb.useGravity = !OnSlopes();
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
