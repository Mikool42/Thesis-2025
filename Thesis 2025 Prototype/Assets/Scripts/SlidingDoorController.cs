using UnityEngine;

public class SlidingDoorController : MonoBehaviour
{
    [SerializeField] float closingForce = 5f;

    [Tooltip("Don't cahnge, is a reference to the movable part of the door for the script.")]
    [SerializeField] Rigidbody DoorBody;

    private Vector3 _defaultDirectionVector = new Vector3(1, 0, 0);
    private Vector3 currentDirectionVector;

    void Start()
    {
        currentDirectionVector = transform.rotation * _defaultDirectionVector;

        if (Mathf.Abs(currentDirectionVector.x) < Mathf.Abs(currentDirectionVector.z))
        {
            DoorBody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
            currentDirectionVector = new Vector3(0, 0, currentDirectionVector.z);
        }
        else
        {
            DoorBody.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
            currentDirectionVector = new Vector3(currentDirectionVector.x, 0, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        DoorBody.AddForce(currentDirectionVector * closingForce, ForceMode.Force);


    }
}
