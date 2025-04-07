using UnityEngine;

public class MovableObjectTargetColorSwitch : MonoBehaviour
{
    [Tooltip("Default Movable Object Material Material")]
    [SerializeField] private Material material1;

    [Tooltip("Outline material if applicable")]
    /*[SerializeField]*/ private Material outline;
    /*[SerializeField]*/ private bool hasOutline = false; // Add again if we decide to use the outline stuff
    
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

        if (hasOutline)
        {
            Material[] matArray = mr.materials;
            matArray[0] = material1;
            matArray[1] = outline;
            mr.materials = matArray;
        }
        else
        {
            mr.material = material1;
        }

        
    }

    public void SetAsTarget(bool isTarget, PlayerAbilityBehaviour.AbilityType abilityType)
    {
        if (isTarget)
        {
            SetMaterialBool(abilityType, 1);
            if (hasOutline) SetOutlineBool(1);
        }
        else
        {
            SetMaterialBool(abilityType, 0);
            if (hasOutline) SetOutlineBool(0);
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

    private void SetOutlineBool(int _bool)
    {
        Material[] matArray = mr.materials;
        if (_bool == 0)
        {
            if (matArray[0].GetInt("_PullTargeting") == 0 && matArray[0].GetInt("_PushTargeting") == 0)
            {
                matArray[1].SetInt("_Outline", _bool);
            }
        }
        else
        {
            matArray[1].SetInt("_Outline", _bool);
        }
    }
}
