using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PlayerAbilityBehaviourOld : MonoBehaviour
{

    enum ForceTypes { Force, Impulse };

    [Header("Ability General")]
    [Tooltip("Which ability type")]
    [SerializeField] PlayerAbilityBehaviour.AbilityType abilityType = PlayerAbilityBehaviour.AbilityType.PULL;

    [Header("Targeted Ability")]
    [Tooltip("How much force to use for level one")]
    [SerializeField] float forceAmount_L1 = 2f;
    [Tooltip("How much force to use for level two")]
    [SerializeField] float forceAmount_L2 = 5f;
    [Tooltip("How much force to use for level three")]
    [SerializeField] float forceAmount_L3 = 10f;

    //[Tooltip("The type of force to use for targeted ability (Force = gradual force | Impulse = instant force)")]
    private ForceTypes targetAbilityForceType = ForceTypes.Force;
    //[Tooltip("Indicator for what force level is currently in use")]
    private PlayerAbilityBehaviour.ForceLevel targetAbilityLevel = PlayerAbilityBehaviour.ForceLevel.L1;

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
    [Tooltip("The Radius of the AOE Ability")]
    [SerializeField] float AOERadius = 8.0f;

    [Tooltip("Indicator for what force level is currently in use")]
    [SerializeField] PlayerAbilityBehaviour.ForceLevel aoeAbilityLevel = PlayerAbilityBehaviour.ForceLevel.L1;
    
    //
    //private List<GameObject> insideAOERadius = new List<GameObject>();
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
        pat.ChangeLineThickness(0.3f);

        /*GameObject _pp = GameObject.FindGameObjectWithTag("InGameUI");
        if (_pp != null)
        {
            _powerHUDScript = _pp.GetComponent<PowerHUDScript>();
            _powerHUDScript.ChangeAbilityType(this.gameObject, abilityType);
        }
        else
        {
            Debug.LogWarning("power HUD Script not found");
        }*/
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
            if (targetAbilityLevel == PlayerAbilityBehaviour.ForceLevel.L1) { forceAmount = forceAmount_L1; }
            if (targetAbilityLevel == PlayerAbilityBehaviour.ForceLevel.L2) { forceAmount = forceAmount_L2; }
            if (targetAbilityLevel == PlayerAbilityBehaviour.ForceLevel.L3) { forceAmount = forceAmount_L3; }

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
        if (targetAbilityLevel == PlayerAbilityBehaviour.ForceLevel.L1)
        {
            targetAbilityLevel = PlayerAbilityBehaviour.ForceLevel.L2;
            pat.ChangeLineThickness(0.6f);
        }
        else if (targetAbilityLevel == PlayerAbilityBehaviour.ForceLevel.L2)
        {
            targetAbilityLevel = PlayerAbilityBehaviour.ForceLevel.L3;
            pat.ChangeLineThickness(1.0f);
        }
        else if (targetAbilityLevel == PlayerAbilityBehaviour.ForceLevel.L3)
        {
            targetAbilityLevel = PlayerAbilityBehaviour.ForceLevel.L1;
            pat.ChangeLineThickness(0.3f);
        }
        else
        {
            targetAbilityLevel = PlayerAbilityBehaviour.ForceLevel.L1;
            pat.ChangeLineThickness(0.3f);
        }

        //_powerHUDScript.ChangeAbilityPowerLevel(this.gameObject, targetAbilityLevel);
    }

    public void OnAOETrigger()
    {
        if (AOESphere.activeSelf)
            return;

        Collider[] hitColliders = Physics.OverlapSphere(gameObject.transform.position, AOERadius);
        Debug.Log(hitColliders.Length);

        float forceAmount = 0f;
        if (aoeAbilityLevel == PlayerAbilityBehaviour.ForceLevel.L1) { forceAmount = forceAmount_L1; }
        if (aoeAbilityLevel == PlayerAbilityBehaviour.ForceLevel.L2) { forceAmount = forceAmount_L2; }
        if (aoeAbilityLevel == PlayerAbilityBehaviour.ForceLevel.L3) { forceAmount = forceAmount_L3; }

        foreach (Collider col in hitColliders)
        {
            Debug.Log(col.gameObject.name);
            if (col.gameObject.tag != "MovableObject") continue;

            Rigidbody targetRB = col.gameObject.GetComponent<Rigidbody>();

            Vector3 forceDir = Vector3.Normalize(col.transform.position - transform.position);
            Vector3 appliedForce = forceDir * forceAmount * AOEForceAmplifier;

            targetRB.AddForce(appliedForce, ForceMode.Impulse);
        }

        animationCoroutine = AOEAnimation();
        StartCoroutine(animationCoroutine);
    }

    public void OnAOELevelSwitch()
    {
        if (aoeAbilityLevel == PlayerAbilityBehaviour.ForceLevel.L1)
        {
            aoeAbilityLevel = PlayerAbilityBehaviour.ForceLevel.L2;
        }
        else if (aoeAbilityLevel == PlayerAbilityBehaviour.ForceLevel.L2)
        {
            aoeAbilityLevel = PlayerAbilityBehaviour.ForceLevel.L3;
        }
        else if (aoeAbilityLevel == PlayerAbilityBehaviour.ForceLevel.L3)
        {
            aoeAbilityLevel = PlayerAbilityBehaviour.ForceLevel.L1;
        }
        else
        {
            aoeAbilityLevel = PlayerAbilityBehaviour.ForceLevel.L1;
        }

        _powerHUDScript.ChangeAbilityPowerLevel(this.gameObject, aoeAbilityLevel, true);
    }

    /*void OnTriggerEnter(Collider other)
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
    }*/

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

    public void SetPlayerAbility(PlayerAbilityBehaviour.AbilityType _abilityType)
    {
        abilityType = _abilityType;
        if (_powerHUDScript != null)
        {
            _powerHUDScript.ChangeAbilityType(this.gameObject, abilityType);
        }
        UpdateForceAccordingToAbility();
    }

    public PlayerAbilityBehaviour.AbilityType GetPlayerAbility()
    {
        return abilityType;
    }

    private void UpdateForceAccordingToAbility()
    {
        if (abilityType == PlayerAbilityBehaviour.AbilityType.PULL)
        {
            playerMesh.material = pullMaterial;

            forceAmount_L1 = Mathf.Abs(forceAmount_L1) * -1;
            forceAmount_L2 = Mathf.Abs(forceAmount_L2) * -1;
            forceAmount_L3 = Mathf.Abs(forceAmount_L3) * -1;
        }
        else if (abilityType == PlayerAbilityBehaviour.AbilityType.PUSH)
        {
            playerMesh.material = pushMaterial;

            forceAmount_L1 = Mathf.Abs(forceAmount_L1);
            forceAmount_L2 = Mathf.Abs(forceAmount_L2);
            forceAmount_L3 = Mathf.Abs(forceAmount_L3);
        }
    }
}
