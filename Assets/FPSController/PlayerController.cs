using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;

    private MovingPlatforms platforms;
    private Rigidbody platformRBody;
    private bool isOnPlatform = false;

    private Camera cam;
    [SerializeField] private Transform orientation;

    private CapsuleCollider capsule;
    private float capsuleScale;

    [Header("Player Sound")]
    private AudioSource audioS;
    [SerializeField] private AudioClip footstepSound, runningSound, jumpingSound, slidingSound;

    [Header("Player movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float baseSpeed = 40f;
    private Vector2 moveInput = Vector2.zero;
    private Vector3 playerMovement;

    [SerializeField] private float runMultiplier = 1.5f;
    private bool runInput = false;

    [Header("Player crouch")]
    [SerializeField] private float crouchMultiplier = 0.5f;
    [SerializeField] private float slideSpeed = 12f;
    private bool crouchInput = false;
    private bool isCrouched = false;
    private Vector3 halfHeight;

    [Header("Player jump")]
    [SerializeField] private float jumpForce = 5f;
    private bool jumpInput = false;
    private float airDrag = 1f;

    [Header("Player ground detection")]
    [SerializeField] private LayerMask groundCheck;
    private bool isGrounded = false;
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
    private bool interactInput = false;

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
    public bool InteractInput { get => interactInput; set => interactInput = value; }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        capsule = GetComponent<CapsuleCollider>();
        capsuleScale = capsule.height;
        cam = GetComponentInChildren<Camera>();
        audioS = GetComponent<AudioSource>();
        platforms = GetComponent<MovingPlatforms>();
    }

    private void Awake()
    {
        rayStepUpper.transform.position = new Vector3(rayStepUpper.transform.position.x, stepHeight, rayStepUpper.transform.position.z);
    }

    private void Update()
    {
        PlayFootstep();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Moving();
        CheckIfGrounded();
        CheckIfCeiling();
        Jumping();
        DragValue();
        Stepping();
        OnSlopes();
        Crouching();
        Running();
        OnPlatforms();
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        jumpInput = context.performed;
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        interactInput = context.performed;
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.performed) //&& isGrounded)
        {
            //moveSpeed *= runMultiplier;
            runInput = true;
        }
        else if (context.canceled) //&& !isCrouched)
        {
            //moveSpeed = baseSpeed;
            runInput = false;
        }
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {

        crouchInput = context.performed;
    
        //if (context.performed)
        //{
            //transform.localPosition = new Vector3(transform.position.x, transform.position.y * 0.5f, transform.position.z);
            //moveSpeed *= crouchMultiplier;
            //capsule.height = capsuleScale * 0.5f;
            //isCrouched = true;
            //if (isRunning)
            //{
            //    rb.AddForce(transform.forward * slideSpeed, ForceMode.VelocityChange);
            //    isRunning = false;
            //    moveSpeed = baseSpeed * crouchMultiplier;
            //}
            //if (context.performed && OnSlopes())
            //{
            //    cam.transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y - 0.4f, cam.transform.position.z);
            //}
        //}
        //else if (context.performed && OnSlopes())
        //{
        //    cam.transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y - 0.4f, cam.transform.position.z);
        //    if (context.canceled)
        //    {
        //        cam.transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y + 0.4f, cam.transform.position.z);
        //    }
        
        //else if (context.canceled)
        //{
            //if (context.canceled && OnSlopes()) { 
            //    cam.transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y + 0.4f, cam.transform.position.z);
            //}
            //moveSpeed = baseSpeed;
            //capsule.height = capsuleScale;
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
        playerMovement = orientation.forward * moveInput.y + orientation.right * moveInput.x;
        if (isGrounded && !OnSlopes())
        {
            rb.AddForce(playerMovement.normalized * moveSpeed, ForceMode.Acceleration);
        }
        else if (isGrounded && OnSlopes())
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
        if (crouchInput && isGrounded)
        {
            isCrouched = true;
            moveSpeed = baseSpeed * crouchMultiplier;
            capsule.height = capsuleScale * 0.5f;
            if (runInput)
            {
                rb.AddForce(orientation.forward * slideSpeed, ForceMode.VelocityChange);
                audioS.PlayOneShot(slidingSound);
                runInput = false;
                moveSpeed = baseSpeed * crouchMultiplier;
            }
        }
        else if (!crouchInput && !CheckIfCeiling())
        {
            isCrouched = false;
            ReturnBaseState();
        }
    }

    void Running()
    {
        if (runInput && isGrounded && !CheckIfCeiling())
        {
            moveSpeed = baseSpeed * runMultiplier;
            PlayRunningFootstep();
        }
        else if (!runInput && !crouchInput && !isCrouched)
        {
            moveSpeed = baseSpeed;
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
        if (Physics.Raycast(transform.localPosition, Vector3.down, out slopeHit, capsuleScale * 0.5f + 0.3f))
        {
            if (slopeHit.normal != Vector3.up)
            {
                if (moveInput.magnitude == 0)
                {
                    rb.velocity = Vector3.zero;
                    rb.drag = 100f;
                }
                print("On slopes");
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
        halfHeight = new Vector3(0f, capsule.height * 0.5f, 0f);
        isGrounded = Physics.CheckSphere(capsule.transform.position - halfHeight, groundDistance, groundCheck);
    }

    bool CheckIfCeiling()
    {
        halfHeight = new Vector3(0f, capsule.height * 0.5f , 0f);
        return Physics.Raycast(cam.transform.position, Vector3.up, 1f);
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawWireSphere(capsule.transform.position - halfHeight, groundDistance);
    //    Gizmos.DrawWireSphere(capsule.transform.position + halfHeight, groundDistance);
    //}

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
        //rb.useGravity = !OnSlopes();
    }

    void Jumping()
    {
        if (jumpInput && isGrounded && !CheckIfCeiling())
        {
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            audioS.PlayOneShot(jumpingSound);
            jumpInput = false;
        }
    }
    void PlayRunningFootstep()
    {
        if (rb.velocity.magnitude > 0.5f && audioS.isPlaying == false)
        {
            LoopingAudioClip(runningSound);
        }
    }
    void PlayFootstep()
    {
        if(isGrounded && rb.velocity.magnitude > 1f && audioS.isPlaying == false)
        {
            LoopingAudioClip(footstepSound);
        }
    }
    void LoopingAudioClip(AudioClip clip)
    {
        if (!audioS.isPlaying)
        {
            audioS.PlayOneShot(clip);
            audioS.Play();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Platform")
        {
            platformRBody = platforms.gameObject.GetComponent<Rigidbody>();
            isOnPlatform = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.tag == "Platform")
        {
            isOnPlatform = false;
            platformRBody = null;
        }
    }

    void OnPlatforms()
    {
        if (isOnPlatform)
        {
            rb.velocity = rb.velocity + platformRBody.velocity;
        }
    }
}
