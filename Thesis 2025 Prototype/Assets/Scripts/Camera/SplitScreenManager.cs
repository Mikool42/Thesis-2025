using UnityEngine;

public class SplitScreenManager : MonoBehaviour
{
    public Transform player1;
    public Transform player2;
    public Camera player1Camera;
    public Camera player2Camera;
    public Camera playerGroupCamera;

    public float mergeDistance = 5f;
    private float splitViewSize = 0.5f; // Half screen for each player

    void Update()
    {
        if (player1 == null || player2 == null) return;

        float distance = Vector3.Distance(player1.position, player2.position);
        float mergeRatio = Mathf.Clamp01((distance - mergeDistance) / mergeDistance); // 0 to 1 as they get closer

        // Adjust viewports based on merge ratio
        if (mergeRatio < 1)
        {
            playerGroupCamera.GetComponent<Camera>().enabled = true;
            // Merged view (full screen)
            player1Camera.rect = new Rect(0, 0, 0, 1);
            player2Camera.rect = new Rect(0, 0, 0, 1);
        }
        else
        {
            playerGroupCamera.GetComponent<Camera>().enabled = false;
            // Split view
            player1Camera.rect = new Rect(0, 0, splitViewSize, 1);
            player2Camera.rect = new Rect(splitViewSize, 0, splitViewSize, 1);
        }
    }
}
