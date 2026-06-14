using UnityEngine;
using System.Collections.Generic;

public class TurnOffDiscMovement : MonoBehaviour
{
    private Rigidbody rb;

    public bool canMove = true;

    private List<GameObject> players = new List<GameObject>();

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        string objTag = other.gameObject.tag;

        if (objTag == "Player")
        {
            rb.linearVelocity = Vector3.zero;
            
            players.Add(other.gameObject);
            canMove = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        string objTag = other.gameObject.tag;

        if (objTag == "Player")
        {
            players.Remove(other.gameObject);

            if(players.Count == 0) { canMove = true; }
        }
    }
}
