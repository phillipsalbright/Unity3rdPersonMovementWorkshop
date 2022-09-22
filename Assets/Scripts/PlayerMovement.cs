using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class PlayerMovement : MonoBehaviour
{
    private InputControls controls;
    private Rigidbody rb;
    private Collider playerCollider;
    private Transform cameraTransform;

    private Vector2 moveDir = Vector2.zero;
    [SerializeField]
    private float speed = 5f;

    [SerializeField]
    private float jumpForce = 100f;
    private float distToGround;

    // Start is called before the first frame update
    void Start()
    {
        controls = new InputControls();
        rb = GetComponent<Rigidbody>();
        playerCollider = GetComponent<Collider>();
        cameraTransform = Camera.main.transform;

        controls.CharacterControls.Movement.performed += MovementInput;
        controls.CharacterControls.Movement.canceled += StopMovement;
        controls.CharacterControls.Movement.Enable();
        controls.CharacterControls.Jump.performed += Jump;
        controls.CharacterControls.Jump.Enable();

        distToGround = playerCollider.bounds.extents.y;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, cameraTransform.eulerAngles.y, transform.eulerAngles.z);
        moveDir = moveDir.normalized * speed;
        Vector3 velocity = new Vector3(0, rb.velocity.y, 0);
        velocity += transform.forward * moveDir.y;
        velocity += transform.right * moveDir.x;
        rb.velocity = velocity;
    }

    void Jump(CallbackContext ctx)
    {
        if (IsGrounded())
            rb.AddForce(new Vector3(0, jumpForce, 0));
    }

    void MovementInput(CallbackContext ctx)
    {
        moveDir = ctx.ReadValue<Vector2>();
    }

    void StopMovement(CallbackContext ctx)
    {
        moveDir = Vector2.zero;
    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, distToGround + .01f);
    }
}
