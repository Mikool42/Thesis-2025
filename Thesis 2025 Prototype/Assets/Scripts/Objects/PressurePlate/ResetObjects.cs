using UnityEngine;
using System.Collections.Generic;

public class ResetObjects : MonoBehaviour
{
    [Tooltip("List of objects to reset")]
    [SerializeField] List<GameObject> objectsToReset = new List<GameObject>();

    private List<Vector3> startPositions = new List<Vector3>();

    void Start()
    {
        for (int i = 0; i < objectsToReset.Count; i++)
        {
            startPositions.Add(objectsToReset[i].transform.position);
        }
    }

    public void Reset()
    {
        for (int i = 0; i < objectsToReset.Count; i++)
        {
            objectsToReset[i].transform.position = startPositions[i];
        }
    }
}
