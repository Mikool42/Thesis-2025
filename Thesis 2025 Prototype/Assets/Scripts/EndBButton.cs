using UnityEngine;
using System.Collections;

public class EndBButton : MonoBehaviour
{
    public void OnEndButtonPress()
    {
        StopAllCoroutines();
        Debug.Log("end");
        //Application.Quit();
    #if UNITY_EDITOR
        // Application.Quit() does not work in the editor so
        // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }
}
