using UnityEngine;

public class SlidingDoorDoorStopper : MonoBehaviour
{
    public GameObject invisibleWall;

    void OnTriggerExit(Collider other)
    {
        if (invisibleWall != null && other.gameObject.name == "Door")
        {
            invisibleWall.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (invisibleWall != null && other.gameObject.name == "Door")
        {
            invisibleWall.SetActive(true);
        }
    }
}
