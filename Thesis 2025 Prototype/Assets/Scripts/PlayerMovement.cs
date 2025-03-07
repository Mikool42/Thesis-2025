using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [Tooltip("Reference to floor detection script")]
    [SerializeField] private FloorDetection FDScript;

    [Tooltip("Player movement speed")]
    [SerializeField] float moveSpeed = 5f;

    [Tooltip("Jump Force")]
    [SerializeField] float jumpForce = 5f;

    private Rigidbody rb;
    private PlayerAbilityTargeting pat;
    private Vector3 m_Move;
    private Vector3 m_Rotation;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        pat = gameObject.GetComponent<PlayerAbilityTargeting>();
    }

    void FixedUpdate()
    {
        Move(m_Move);

        /*RaycastHit hitInfo;
        if (Physics.Raycast(transform.position, Vector3.down, out hitInfo, 10, LayerMask.GetMask("Default"), QueryTriggerInteraction.Ignore))
        {
            Debug.DrawRay(transform.position, Vector3.down * hitInfo.distance, Color.yellow);
        }*/
    }

    public void OnMove(InputValue value)
    {
        m_Move = value.Get<Vector3>();
    }

    private void Move(Vector3 direction)
    {
        if (direction.sqrMagnitude < 0.01)
            return;
        var scaledMoveSpeed = moveSpeed * Time.fixedDeltaTime;
        var move = Quaternion.Euler(0, 0, 0) * direction;

        rb.Move(transform.position + (Vector3.Normalize(direction) * scaledMoveSpeed), Quaternion.LookRotation(direction, Vector3.up));
        //rb.MovePosition(transform.position + direction * scaledMoveSpeed);

        pat.RenderLineOnTarget();
    }
    
    public void OnJump()
    {
        if (FDScript != null && FDScript.GetIsGrounded())
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            FDScript.JustJumped();
        }
    }
}
