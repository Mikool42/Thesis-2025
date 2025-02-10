using UnityEngine;

public class DiscMovementController : MonoBehaviour
{
    [SerializeField] GameObject discSurface;

    // Update is called once per frame
    void Update()
    {
        if (discSurface.transform.position != transform.position)
        {
            discSurface.transform.position = transform.position;
        }
    }
}
