using UnityEngine;
using Unity.Cinemachine;

public class CameraPlayerTarget : MonoBehaviour
{
    [Tooltip("Reference to the Target group component")]
    [SerializeField] private CinemachineTargetGroup targetGroup;

    public void OnPlayerJoined()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        targetGroup.Targets.Clear();

        foreach (GameObject _player in players)
        {
            targetGroup.AddMember(_player.transform, 1, 1);
        }

        Debug.Log(players.Length);

    }
}
