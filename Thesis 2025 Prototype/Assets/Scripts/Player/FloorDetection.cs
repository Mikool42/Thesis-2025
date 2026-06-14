using UnityEngine;
using System.Collections.Generic;

public class FloorDetection : MonoBehaviour
{
    [SerializeField] private bool isGrounded = false;
    private List<GameObject> currentGroundObjects = new List<GameObject>(); // used to hinder player becoming not grounded while grounded

    private PlayerMovement pm;

    void Start()
    {
        pm = transform.parent.gameObject.GetComponent<PlayerMovement>();
    }

    private void OnTriggerEnter(Collider other)
    {
        /*string objTag = other.gameObject.tag;

        if (objTag == "Ground" || objTag == "MovableObject")
        {
        }*/
        isGrounded = true;
        currentGroundObjects.Add(other.gameObject);
        pm.SetJumpBool(false);

        //Debug.Log(objTag);
    }

    private void OnTriggerExit(Collider other)
    {
        currentGroundObjects.Remove(other.gameObject);

        if (currentGroundObjects.Count == 0 )
        {
            isGrounded = false;
        }
    }
    
    /*private void OnTriggerExit(Collider other)
    {
        string objTag = other.gameObject.tag;

        if (objTag == "Ground" || objTag == "MovableObject" && currentGroundObject == other.gameObject)
        {
            isGrounded = false;
            currentGroundObject = null;
        }

        Debug.Log(objTag);
    }*/

    public bool GetIsGrounded() { return isGrounded; }

    public void JustJumped() { isGrounded = false; }
}
