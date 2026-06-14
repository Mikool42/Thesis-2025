using UnityEngine;

public class SendToMenuTrigger : MonoBehaviour
{
    public MenuController mc;
    public int MainMenuLevelIndex = 0;

    private void OnTriggerEnter(Collider other)
    {
        mc.SwitchLevel(MainMenuLevelIndex);
    }
}
