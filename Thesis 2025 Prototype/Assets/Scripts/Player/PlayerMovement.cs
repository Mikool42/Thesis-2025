using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Collections;
using SmallHedge.SoundManager;

public class PlayerMovement : MonoBehaviour
{
    [Tooltip("Reference to floor detection script")]
    [SerializeField] private FloorDetection FDScript;

    [Tooltip("Player movement speed")]
    [SerializeField] float moveSpeed = 5f;

    [Tooltip("Jump Force")]
    [SerializeField] float jumpForce = 5f;

    [Tooltip("ShadowObject")]
    [SerializeField] GameObject shadow;

    private Rigidbody rb;
    private PlayerAbilityTargeting pat;
    private Vector3 m_Move;
    private bool m_Jump = false;
    private Vector3 m_Rotation;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        pat = gameObject.GetComponent<PlayerAbilityTargeting>();
    }

    void FixedUpdate()
    {
        Move(m_Move);

        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position, Vector3.down, out hitInfo, 10, /*LayerMask.GetMask("Ground")*/ ~0, QueryTriggerInteraction.Ignore))
        {
            Debug.DrawRay(transform.position, Vector3.down * hitInfo.distance, Color.yellow);
            shadow.transform.position = hitInfo.point;
        }
    }

    public void OnMove(InputValue value)
    {
        Debug.Log("in OnMove");
        m_Move = value.Get<Vector3>();
    }

    private void Move(Vector3 direction)
    {
        Debug.Log(direction);
        if (direction.sqrMagnitude < 0.01)
            return;
        Debug.Log(direction);
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
            SoundManager.PlaySound(SoundType.JUMP);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            FDScript.JustJumped();
        }
    }

    public Vector3 GetMoveVector()
    {
        return m_Move;
    }

    public bool GetJumpBool()
    {
        return m_Jump;
    }

    public void SetJumpBool(bool _jump)
    {
        m_Jump = _jump;
    }
}
