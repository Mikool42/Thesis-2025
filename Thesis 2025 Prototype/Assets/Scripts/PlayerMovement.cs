using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float controlSpeed = 10f;

    Vector3 movement;


    void Update()
    {
        ProcessTranslation();

    }

    public void OnMove(InputValue value)
    {
        movement = value.Get<Vector3>();
    }

    private void ProcessTranslation()
    {
        float xOffset = movement.x * controlSpeed * Time.deltaTime;
        float rawXPos = transform.localPosition.x + xOffset;


        float zOffset = movement.z * controlSpeed * Time.deltaTime;
        float rawZPos = transform.localPosition.z + zOffset;

        transform.localPosition = new Vector3(rawXPos, transform.localPosition.y, rawZPos);
    }

    
}
