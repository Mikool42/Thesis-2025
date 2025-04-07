using UnityEngine;

public class FloorDetection : MonoBehaviour
{
    [SerializeField] private bool isGrounded = false;
    private GameObject currentGroundObject = null; // used to hinder player becoming not grounded while grounded

    private void OnTriggerEnter(Collider other)
    {
        string objTag = other.gameObject.tag;

        if (objTag == "Ground" || objTag == "MovableObject")
        {
            isGrounded = true;
            currentGroundObject = other.gameObject;
        }

        //Debug.Log(objTag);
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
