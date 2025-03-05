using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PlayerAbilityBehaviour : MonoBehaviour
{

    enum ForceTypes { Force, Impulse };
    public enum ForceLevel { L1, L2, L3 };
    public enum AbilityType { PULL, PUSH };

    [Header("Ability General")]
    [Tooltip("Which ability type")]
    [SerializeField] AbilityType abilityType = AbilityType.PULL;

    [Header("Targeted Ability")]
    [Tooltip("How much force to use for level one")]
    [SerializeField] float forceAmount_L1 = 2f;
    [Tooltip("How much force to use for level two")]
    [SerializeField] float forceAmount_L2 = 5f;
    [Tooltip("How much force to use for level three")]
    [SerializeField] float forceAmount_L3 = 10f;

    [Tooltip("The type of force to use for targeted ability (Force = gradual force | Impulse = instant force)")]
    [SerializeField] ForceTypes targetAbilityForceType = ForceTypes.Force;
    [Tooltip("Indicator for what force level is currently in use")]
    [SerializeField] ForceLevel targetAbilityLevel = ForceLevel.L1;

    private bool isFiring = false;
    private GameObject abilityTarget = null;
    

    [Header("AOE Ability")]
    [Tooltip("The GameObject that is animated when AOE is triggered")]
    [SerializeField] GameObject AOESphere;
    [Tooltip("The amplifier that indicates how much more force is used in AOE compared to targeted (force used in AOE = force used in targeted * amplifier)")]
    [SerializeField] float AOEForceAmplifier = 2f;
    [Tooltip("The time it takes for the animation to run")]
    [SerializeField] float AOEAnimationTime = 0.5f;
    [Tooltip("How many 'frames' there are in the animation (smoothness)")]
    [SerializeField] float AOEAnimationTimeStep = 0.01f;

    [Tooltip("Indicator for what force level is currently in use")]
    [SerializeField] ForceLevel aoeAbilityLevel = ForceLevel.L1;
    
    private List<GameObject> insideAOERadius = new List<GameObject>();
    private IEnumerator animationCoroutine;
    

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

        GameObject _pp = GameObject.FindGameObjectWithTag("InGameUI");
        if (_pp != null)
        {
            _powerHUDScript = _pp.GetComponent<PowerHUDScript>();
            _powerHUDScript.ChangeAbilityType(this.gameObject, abilityType);
        }
        else
        {
            Debug.LogWarning("power HUD Script not found");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isFiring && (targetAbilityForceType == ForceTypes.Force))
        {
            OnFireStart();
        }
    }

    public void OnFireStart()
    {
        isFiring = true;

        abilityTarget = pat.GetTarget(); // Temporary, still need to setup for the targeting

        Rigidbody targetRB = null;
        if (abilityTarget != null)
        {
            targetRB = abilityTarget.GetComponent<Rigidbody>();
        }
        if (targetRB != null)
        {
            float forceAmount = 0f;
            if (targetAbilityLevel == ForceLevel.L1) { forceAmount = forceAmount_L1; }
            if (targetAbilityLevel == ForceLevel.L2) { forceAmount = forceAmount_L2; }
            if (targetAbilityLevel == ForceLevel.L3) { forceAmount = forceAmount_L3; }

            Vector3 forceDir = Vector3.Normalize(abilityTarget.transform.position - transform.position);
            Vector3 appliedForce = forceDir * forceAmount;

            if (targetAbilityForceType == ForceTypes.Force)
            {
                targetRB.AddForce(appliedForce, ForceMode.Force);
            }
            else if (targetAbilityForceType == ForceTypes.Impulse)
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
        if (targetAbilityLevel == ForceLevel.L1)
        {
            targetAbilityLevel = ForceLevel.L2;
        }
        else if (targetAbilityLevel == ForceLevel.L2)
        {
            targetAbilityLevel = ForceLevel.L3;
        }
        else if (targetAbilityLevel == ForceLevel.L3)
        {
            targetAbilityLevel = ForceLevel.L1;
        }
        else
        {
            targetAbilityLevel = ForceLevel.L1;
        }

        _powerHUDScript.ChangeAbilityPowerLevel(this.gameObject, targetAbilityLevel);
    }

    public void OnAOETrigger()
    {
        if (AOESphere.activeSelf)
            return;

        float forceAmount = 0f;
        if (aoeAbilityLevel == ForceLevel.L1) { forceAmount = forceAmount_L1; }
        if (aoeAbilityLevel == ForceLevel.L2) { forceAmount = forceAmount_L2; }
        if (aoeAbilityLevel == ForceLevel.L3) { forceAmount = forceAmount_L3; }

        foreach (GameObject go in insideAOERadius)
        {
            Rigidbody targetRB = go.GetComponent<Rigidbody>();

            Vector3 forceDir = Vector3.Normalize(go.transform.position - transform.position);
            Vector3 appliedForce = forceDir * forceAmount * AOEForceAmplifier;

            targetRB.AddForce(appliedForce, ForceMode.Impulse);
        }

        animationCoroutine = AOEAnimation();
        StartCoroutine(animationCoroutine);
    }

    public void OnAOELevelSwitch()
    {
        if (aoeAbilityLevel == ForceLevel.L1)
        {
            aoeAbilityLevel = ForceLevel.L2;
        }
        else if (aoeAbilityLevel == ForceLevel.L2)
        {
            aoeAbilityLevel = ForceLevel.L3;
        }
        else if (aoeAbilityLevel == ForceLevel.L3)
        {
            aoeAbilityLevel = ForceLevel.L1;
        }
        else
        {
            aoeAbilityLevel = ForceLevel.L1;
        }

        _powerHUDScript.ChangeAbilityPowerLevel(this.gameObject, aoeAbilityLevel, true);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "MovableObject")
        {
            insideAOERadius.Add(other.gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "MovableObject")
        {
            insideAOERadius.Remove(other.gameObject);
        }
    }

    private IEnumerator AOEAnimation()
    {
        float animationTime = AOEAnimationTime;
        Vector3 originalScale = AOESphere.transform.localScale;

        AOESphere.SetActive(true);

        while (0f < animationTime)
        {
            animationTime = animationTime - AOEAnimationTimeStep;

            AOESphere.transform.localScale = originalScale * (1 - (animationTime / AOEAnimationTime));

            yield return new WaitForSeconds(AOEAnimationTimeStep);
        }
        AOESphere.SetActive(false);
        yield return null;
    }

    public void SetPlayerAbility(AbilityType _abilityType)
    {
        abilityType = _abilityType;
        if (_powerHUDScript != null)
        {
            _powerHUDScript.ChangeAbilityType(this.gameObject, abilityType);
        }
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
