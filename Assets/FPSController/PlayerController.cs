using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;

    [SerializeField] private float moveSpeed;
    private Vector3 playerVelocity;

    private Vector2 move = Vector2.zero;



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        playerVelocity = new Vector3(move.x, 0, move.y).normalized;

        Vector3 destination = rb.position + playerVelocity * Time.fixedDeltaTime;
        rb.MovePosition(destination);

    }
    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }
}
