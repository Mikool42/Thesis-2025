using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeapon : MonoBehaviour
{
    //[SerializeField] GameObject[] lasers;
    [SerializeField] GameObject laser;
    //[SerializeField] RectTransform crosshair;
    [SerializeField] Transform targetPoint;
    [SerializeField] float targetDistance = 100f;

    bool isFiring = false;

    private void Start()
    {
        //Cursor.visible = false;
    }

    private void Update()
    {
        ProcessFiring();
       // MoveCrosshair();
        MoveTargetPoint();
        AimLasers();
    }


    public void OnFire(InputValue value)
    {
        isFiring = value.isPressed;
    }

    void ProcessFiring()
    {
            var emmissionModule = laser.gameObject.GetComponent<ParticleSystem>().emission;
            emmissionModule.enabled = isFiring;
    }

    void MoveCrosshair()
    {
        //crosshair.position = Input.mousePosition;
    }

    private void MoveTargetPoint()
    {
        Vector3 targetPointPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, targetDistance);
        targetPoint.position = Camera.main.ScreenToWorldPoint(targetPointPosition);
    }

    void AimLasers()
    {
            Vector3 fireDirection = targetPoint.position - this.transform.position;
            Quaternion rotationToTarget = Quaternion.LookRotation(fireDirection);
            laser.transform.rotation = rotationToTarget;
    }
}