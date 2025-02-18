using UnityEngine;

public class MovableObjectTargetColorSwitch : MonoBehaviour
{
    [Tooltip("Default Material")]
    [SerializeField] private Material material1;

    [Tooltip("When targeted Material")]
    [SerializeField] private Material material2;
    
    [Tooltip("MeshRenderer to cahnge color on, if not assigned will use default MeshRenderer")]
    [SerializeField] MeshRenderer presetMeshRenderer;

    private MeshRenderer mr;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (presetMeshRenderer != null)
        {
            mr = presetMeshRenderer;

        }
        else
        {
            mr = GetComponent<MeshRenderer>();
        }
        mr.material = material1;
    }

    public void SetAsTarget(bool isTarget)
    {
        if (isTarget)
        {
            mr.material = material2;
        }
        else
        {
            mr.material = material1;
        }
    }
}
