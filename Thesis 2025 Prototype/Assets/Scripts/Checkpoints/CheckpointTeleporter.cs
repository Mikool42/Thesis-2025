using UnityEngine;

public class CheckpointTeleporter : MonoBehaviour
{

    [SerializeField] private MenuController mc;

    public void teleportToCheckpoint(GameObject checkpoint)
    {
        Vector3 teleportPosition = checkpoint.GetComponent<Transform>().position;

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject player in players)
        {
            player.GetComponent<Transform>().position = teleportPosition;
        }

        mc.OnMenuExit();
    }


    
}
