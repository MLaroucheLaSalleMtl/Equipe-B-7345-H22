using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;

    [SerializeField] private float moveSpeed = 5f;
    private Vector2 moveInput = Vector2.zero;
    private Vector3 playerMovement;

    [SerializeField] private float runMultiplier = 2f;
    private bool runInput = false;

    [SerializeField] private float jumpForce = 5f;
    private bool jumpInput = false;
    
    private bool isGrounded;

    private CapsuleCollider capsule;
    private float capsuleScale;

    //add by steven **
    private int hp = 100;

    public int Hp { get => hp; set => hp = value; }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        capsule = GetComponentInChildren<CapsuleCollider>();
        capsuleScale = capsule.height;
        Hp = 100;
    }

    // Update is called once per frame
    void Update()
    {
        Moving();
        CheckIfGrounded();
        Jumping();
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        jumpInput = context.performed;
    }

    void Moving()
    {
        playerMovement = transform.forward * moveInput.y + transform.right * moveInput.x;
        rb.AddForce(playerMovement.normalized * moveSpeed, ForceMode.Acceleration);
    }

    void CheckIfGrounded()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, capsuleScale * 0.5f + 0.1f);
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
