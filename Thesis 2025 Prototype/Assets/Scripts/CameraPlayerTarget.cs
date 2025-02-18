using UnityEngine;
using Unity.Cinemachine;

public class CameraPlayerTarget : MonoBehaviour
{
    [Tooltip("Reference to the Target group component")]
    [SerializeField] private CinemachineTargetGroup targetGroup;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
