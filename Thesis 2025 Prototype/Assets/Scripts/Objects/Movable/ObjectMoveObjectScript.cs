using UnityEngine;

public class ObjectMoveObjectScript : MonoBehaviour
{

    [Header("Editable settings")]
    [Tooltip("How much does the child have to move so that the parent moves.")]
    [SerializeField] float moveMagnitudeTreshhold = 0.05f;

    [Header("References")]
    [Tooltip("Object that will move when this object moves")]
    [SerializeField] GameObject _object;


    private Vector3 prevPosition;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        prevPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (prevPosition != transform.position && (prevPosition - transform.position).magnitude > moveMagnitudeTreshhold)
        {
            Debug.Log("Child moved and crossed the threshhold");
            Debug.Log(prevPosition);
            Debug.Log(transform.position);
        }
    }
}
