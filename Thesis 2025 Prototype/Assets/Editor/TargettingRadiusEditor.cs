using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (PlayerAbilityTargeting))]
public class TargettingRadiusEditor : Editor
{
    void OnSceneGUI()
    {
        PlayerAbilityTargeting pat = (PlayerAbilityTargeting) target;

        Handles.color = Color.white;
        Handles.DrawWireArc(pat.transform.position, Vector3.up, Vector3.forward, 360, pat.GetTargettingRadius());
    }
}
