using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] float moveSpeed = 5f;

    private Rigidbody rb;
    private Vector3 m_Move;
    private Vector3 m_Rotation;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    void Update()
    {
        Move(m_Move);
        Look(m_Move);
    }

    public void OnMove(InputValue value)
    {
        m_Move = value.Get<Vector3>();
    }

    private void Move(Vector3 direction)
    {
        if (direction.sqrMagnitude < 0.01)
            return;
        var scaledMoveSpeed = moveSpeed * Time.deltaTime;
        var move = Quaternion.Euler(0, 0, 0) * direction;
        rb.AddForce(Vector3.Normalize(direction) * scaledMoveSpeed, ForceMode.Impulse);
    }

    private void Look(Vector3 direction)
    {
        if (direction.sqrMagnitude < 0.1)
            return;
        //var scaledRotateSpeed = rotateSpeed * Time.deltaTime;
        /*Debug.Log(rotate.x);
        Debug.Log(rotate.y);
        Debug.Log(rotate.z);
        m_Rotation.x = 0;
        m_Rotation.y = rotate.y;
        m_Rotation.z = 0;*/
        //m_Rotation.x = Mathf.Clamp(m_Rotation.x - rotate.y * scaledRotateSpeed, -89, 89);
        transform.rotation = Quaternion.LookRotation(direction);
    }
}
