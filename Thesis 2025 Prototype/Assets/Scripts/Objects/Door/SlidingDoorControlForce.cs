using UnityEngine;

public class SlidingDoorControlForce : MonoBehaviour
{
    public enum Axis { Xaxis, Zaxis };

    Rigidbody rb;
    public SlidingDoorController controller;

    [Tooltip("When the angle is correct (its technically opening the door) what is the minnimum amount of force being applied to open the door (without taking the angle into account).")]
    public float minnimumForceApplied = 0.2f;
    public float maximumForceApplied = 3.0f;

    public Axis slidingDoorMovementAxis = Axis.Xaxis;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rb == null) return;
        if (controller == null) return;

        if (Vector3.Dot(Vector3.Normalize(rb.linearVelocity), controller.GetCurrentDirection()) < 0)
        {
            float _magnitude = rb.linearVelocity.magnitude;
            if (_magnitude < minnimumForceApplied) {
                _magnitude = minnimumForceApplied; 
            }
            else if (_magnitude > maximumForceApplied)
            {
                _magnitude = maximumForceApplied;
            }
            rb.linearVelocity = -controller.GetCurrentDirection() * _magnitude;
        }
    }
}
