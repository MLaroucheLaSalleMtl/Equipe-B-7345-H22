using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;

    [SerializeField] private float moveSpeed = 5f;
    private Vector2 moveInput = Vector2.zero;
    private Vector3 playerVelocity;

    [SerializeField] private float jumpForce = 5f;
    private bool jumpInput = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Moving();
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

    public void Moving()
    {
        playerVelocity = new Vector3(moveInput.x, 0, moveInput.y).normalized;
        Vector3 destination = rb.position + playerVelocity * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(destination);
    }

    public void Jumping()
    {
        if (jumpInput)
        {
            playerVelocity.y += jumpForce;
        }
    }
}
