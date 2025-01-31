using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float controlSpeed = 10f;

    [SerializeField] GameObject target;

    [SerializeField] float forceAmount = 2f;

    Vector3 movement;

    Vector3 moveVector = new Vector3(1f, 0, 0);

    Rigidbody rb;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    void Update()
    {
        ProcessTranslation();
        //ApplyMovementforce();
    }

    public void OnMove(InputValue value)
    {
        movement = value.Get<Vector3>();
    }

    public void OnFire()
    {
        Debug.Log("Fire");
        Rigidbody targetRB = target.GetComponent<Rigidbody>();
        if (targetRB != null)
        {
            Vector3 appliedForce = moveVector * forceAmount;
            Debug.Log(appliedForce);
            targetRB.AddForce(appliedForce, ForceMode.Impulse);
        }
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

    private void ApplyMovementforce()
    { 
        



    }

    
}
