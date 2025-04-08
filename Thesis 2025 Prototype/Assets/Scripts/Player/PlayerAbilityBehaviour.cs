using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PlayerAbilityBehaviour : MonoBehaviour
{

    public enum ForceTypes { Force, Impulse };
    public enum ForceLevel { L1, L2, L3 };
    public enum AbilityType { PULL, PUSH };

    [Header("Ability General")]
    [Tooltip("Which ability type")]
    [SerializeField] AbilityType abilityType = AbilityType.PULL;

    [Header("Ability")]
    [Tooltip("How much force to use for level one")]
    [SerializeField] float forceAmount_L1 = 2f;
    [Tooltip("What Type of force to use for level one")]
    [SerializeField] ForceTypes forceType_L1 = ForceTypes.Force;
    [Tooltip("How much force to use for level two")]
    [SerializeField] float forceAmount_L2 = 5f;
    [Tooltip("What Type of force to use for level one")]
    [SerializeField] ForceTypes forceType_L2 = ForceTypes.Force;
    [Tooltip("How much force to use for level three")]
    [SerializeField] float forceAmount_L3 = 10f;
    [Tooltip("What Type of force to use for level one")]
    [SerializeField] ForceTypes forceType_L3 = ForceTypes.Force;

    [Tooltip("The amplifier that indicates how much more force is used in AOE compared to targeted (force used in AOE = force used in targeted * amplifier)")]
    [SerializeField] float AOEForceAmplifier = 2f;

    private ForceTypes currentAbilityForceType = ForceTypes.Force;
    private ForceLevel abilityLevel = ForceLevel.L1;

    private bool isFiring = false;
    private bool isFiringAoe = false;
    private GameObject abilityTarget = null;
   
    [Header("Target finding")]
    [Tooltip("Reference to Player ability targeting script")]
    [SerializeField] PlayerAbilityTargeting pat;

    [Header("Player mesh")]
    [Tooltip("Reference to player mesh")]
    [SerializeField] MeshRenderer playerMesh;
    [Tooltip("Player Pull Material")]
    [SerializeField] private Material pullMaterial;
    [Tooltip("Player Push Material")]
    [SerializeField] private Material pushMaterial;


    private PowerHUDScript _powerHUDScript;

    void Start()
    {
        UpdateForceAccordingToAbility();
        pat.ChangeLineThickness(0.3f);
    }

    // Update is called once per frame
    void Update()
    {
        if (isFiring && (currentAbilityForceType == ForceTypes.Force))
        {
            OnFireStart();
        }
        else if (isFiringAoe && (currentAbilityForceType == ForceTypes.Force))
        {
            OnAOETrigger();
        }
    }

    public void OnFireStart()
    {
        isFiring = true;

        UpdateForceType();

        abilityTarget = pat.GetTarget();

        Rigidbody targetRB = null;
        if (abilityTarget != null)
        {
            if (abilityTarget.GetComponent<TurnOffDiscMovement>() != null &&
            !abilityTarget.GetComponent<TurnOffDiscMovement>().canMove)
            {
                return;
            }

            targetRB = abilityTarget.GetComponent<Rigidbody>();
        }

        if (targetRB != null)
        {
            float forceAmount = 0f;
            if (abilityLevel == ForceLevel.L1) { forceAmount = forceAmount_L1; }
            if (abilityLevel == ForceLevel.L2) { forceAmount = forceAmount_L2; }
            if (abilityLevel == ForceLevel.L3) { forceAmount = forceAmount_L3; }

            Vector3 forceDir = Vector3.Normalize(abilityTarget.transform.position - transform.position);
            Vector3 appliedForce = forceDir * forceAmount;

            if (currentAbilityForceType == ForceTypes.Force)
            {
                targetRB.AddForce(appliedForce, ForceMode.Force);
            }
            else if (currentAbilityForceType == ForceTypes.Impulse)
            {
                targetRB.AddForce(appliedForce, ForceMode.Impulse);
            }
        }
    }

    public void OnFireStop()
    {
        isFiring = false;
    }

    public void OnTargetLevelSwitch()
    {
        if (abilityLevel == ForceLevel.L1)
        {
            abilityLevel = ForceLevel.L2;
            pat.ChangeLineThickness(0.6f);
        }
        else if (abilityLevel == ForceLevel.L2)
        {
            abilityLevel = ForceLevel.L3;
            pat.ChangeLineThickness(1.0f);
        }
        else if (abilityLevel == ForceLevel.L3)
        {
            abilityLevel = ForceLevel.L1;
            pat.ChangeLineThickness(0.3f);
        }
        else
        {
            abilityLevel = ForceLevel.L1;
            pat.ChangeLineThickness(0.3f);
        }

        //_powerHUDScript.ChangeAbilityPowerLevel(this.gameObject, targetAbilityLevel);
    }

    public void OnAOETrigger()
    {
        Debug.Log("aoe started");
        isFiringAoe = true;

        UpdateForceType();

        float forceAmount = 0f;
        if (abilityLevel == ForceLevel.L1) { forceAmount = forceAmount_L1; }
        if (abilityLevel == ForceLevel.L2) { forceAmount = forceAmount_L2; }
        if (abilityLevel == ForceLevel.L3) { forceAmount = forceAmount_L3; }


        List<GameObject> targetList = pat.GetAoeTargetsList();
        for (int i = 0; i < targetList.Count; i++)
        {
            GameObject _t = targetList[i];
            if (_t.tag != "MovableObject") continue;

            Rigidbody targetRB = _t.GetComponent<Rigidbody>();

            Vector3 forceDir = Vector3.Normalize(_t.transform.position - transform.position);
            Vector3 appliedForce = forceDir * forceAmount * AOEForceAmplifier;

            if (currentAbilityForceType == ForceTypes.Force)
            {
                targetRB.AddForce(appliedForce, ForceMode.Force);
            }
            else if (currentAbilityForceType == ForceTypes.Impulse)
            {
                targetRB.AddForce(appliedForce, ForceMode.Impulse);
            }
        }
    }

    public void OnAOEStop()
    {
        Debug.Log("aoe stopped");

        isFiringAoe = false;
    }

    private void UpdateForceType()
    {
        if (abilityLevel == ForceLevel.L1) currentAbilityForceType = forceType_L1;
        else if (abilityLevel == ForceLevel.L2) currentAbilityForceType = forceType_L2;
        else if (abilityLevel == ForceLevel.L3) currentAbilityForceType = forceType_L3;
    }

    public void OnAOELevelSwitch()
    {
        if (abilityLevel == ForceLevel.L1)
        {
            abilityLevel = ForceLevel.L2;
        }
        else if (abilityLevel == ForceLevel.L2)
        {
            abilityLevel = ForceLevel.L3;
        }
        else if (abilityLevel == ForceLevel.L3)
        {
            abilityLevel = ForceLevel.L1;
        }
        else
        {
            abilityLevel = ForceLevel.L1;
        }
    }

    public void SetPlayerAbility(AbilityType _abilityType)
    {
        abilityType = _abilityType;
        UpdateForceAccordingToAbility();
    }

    public AbilityType GetPlayerAbility()
    {
        return abilityType;
    }

    private void UpdateForceAccordingToAbility()
    {
        if (abilityType == AbilityType.PULL)
        {
            playerMesh.material = pullMaterial;

            forceAmount_L1 = Mathf.Abs(forceAmount_L1) * -1;
            forceAmount_L2 = Mathf.Abs(forceAmount_L2) * -1;
            forceAmount_L3 = Mathf.Abs(forceAmount_L3) * -1;
        }
        else if (abilityType == AbilityType.PUSH)
        {
            playerMesh.material = pushMaterial;

            forceAmount_L1 = Mathf.Abs(forceAmount_L1);
            forceAmount_L2 = Mathf.Abs(forceAmount_L2);
            forceAmount_L3 = Mathf.Abs(forceAmount_L3);
        }
    }
}
