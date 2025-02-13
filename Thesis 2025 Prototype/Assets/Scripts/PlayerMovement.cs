using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    enum ForceTypes { Force, Impulse };

    [SerializeField] float controlSpeed = 10f;

    [SerializeField] GameObject target = null;

    Vector3 movement;

    Rigidbody rb;

    Vector3 moveVector = new Vector3(0,0,0);

    public float moveSpeed;
    private Vector3 m_Move;
    private Vector3 m_Rotation;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();

        GameObject[] tmpObj = GameObject.FindGameObjectsWithTag("MovableObject"); // will be replaced with targeting component
        target = tmpObj[0];
    }

    void Update()
    {
        //ProcessTranslation();
        Move(m_Move);
        Look(m_Move);
    }

    public void OnMove(InputValue value)
    {
        m_Move = value.Get<Vector3>();
    }

    private void ProcessTranslation()
    {
        float xOffset = movement.x * controlSpeed * Time.deltaTime;
        float rawXPos = transform.localPosition.x + xOffset;


        float zOffset = movement.z * controlSpeed * Time.deltaTime;
        float rawZPos = transform.localPosition.z + zOffset;

        Vector3 newPos = new Vector3(rawXPos, transform.localPosition.y, rawZPos);

        if (newPos != transform.localPosition)
        {
            moveVector = newPos - transform.localPosition;
            transform.rotation = Quaternion.LookRotation(moveVector);
        }

        transform.localPosition = newPos;

    }

    private void Move(Vector3 direction)
    {
        if (direction.sqrMagnitude < 0.01)
            return;
        var scaledMoveSpeed = moveSpeed * Time.deltaTime;
        // For simplicity's sake, we just keep movement in a single plane here. Rotate
        // direction according to world Y rotation of player.
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

    public GameObject GetTarget() // Will be replaced wit ha targetting component
    {
        if (target == null)
        {
            return GameObject.FindGameObjectsWithTag("MovableObject")[0];
        }
        return target;
    }

}
