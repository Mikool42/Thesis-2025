using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PlayerAbilityBehaviour : MonoBehaviour
{

    enum ForceTypes { Force, Impulse };

    [SerializeField] float forceAmount = 2f;
    [SerializeField] float AOEForceAmplifier = 2f;

    [SerializeField] ForceTypes forceType = ForceTypes.Force;

    [SerializeField] GameObject AOESphere;
    [SerializeField] float AOEAnimationTime = 0.5f;
    [SerializeField] float AOEAnimationTimeStep = 0.01f;

    private bool isFiring = false;

    private List<GameObject> insideAOERadius = new List<GameObject>();

    private IEnumerator animationCoroutine;

    private GameObject abilityTarget = null;

    [SerializeField] PlayerMovement pm;

    // Update is called once per frame
    void Update()
    {
        if (isFiring && (forceType == ForceTypes.Force))
        {
            OnFireStart();
        }
    }

    public void OnFireStart()
    {
        isFiring = true;

        abilityTarget = pm.GetTarget(); // Temporary, still need to setup for the targeting

        Rigidbody targetRB = null;
        if (abilityTarget != null)
        {
            targetRB = abilityTarget.GetComponent<Rigidbody>();
        }
        if (targetRB != null)
        {
            Vector3 forceDir = Vector3.Normalize(abilityTarget.transform.position - transform.position);
            Vector3 appliedForce = forceDir * forceAmount;

            if (forceType == ForceTypes.Force)
            {
                targetRB.AddForce(appliedForce, ForceMode.Force);
            }
            else if (forceType == ForceTypes.Impulse)
            {
                targetRB.AddForce(appliedForce, ForceMode.Impulse);
            }
        }
    }

    public void OnFireStop()
    {
        isFiring = false;
    }

    public void OnAOETrigger()
    {

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
}
