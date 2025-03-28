using UnityEngine;

public class MovableObjectTargetColorSwitch : MonoBehaviour
{
    [Tooltip("Default Movable Object Material Material")]
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

    public void SetAsTarget(bool isTarget, PlayerAbilityBehaviour.AbilityType abilityType)
    {
        if (isTarget)
        {
            SetMaterialBool(abilityType, 1);
        }
        else
        {
            SetMaterialBool(abilityType, 0);
        }
    }

    private void SetMaterialBool(PlayerAbilityBehaviour.AbilityType abilityType, int _bool) //false = 0, true = 1
    {
        if (abilityType == PlayerAbilityBehaviour.AbilityType.PUSH)
        {
            mr.material.SetInt("_PushTargeting", _bool);
        }
        else if (abilityType == PlayerAbilityBehaviour.AbilityType.PULL)
        {
            mr.material.SetInt("_PullTargeting", _bool);
        }
    }
}
