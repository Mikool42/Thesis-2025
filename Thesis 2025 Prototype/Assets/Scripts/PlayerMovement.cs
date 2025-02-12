using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    enum ForceTypes { Force, Impulse };

    [SerializeField] float controlSpeed = 10f;

    [SerializeField] GameObject target;

    [SerializeField] float forceAmount = 2f;
    [SerializeField] float AOEForceAmplifier = 2f;

    [SerializeField] ForceTypes forceType = ForceTypes.Force;

    [SerializeField] GameObject AOESphere;
    [SerializeField] float AOEAnimationTime = 0.5f;
    [SerializeField] float AOEAnimationTimeStep = 0.01f;

    Vector3 movement;

    Rigidbody rb;

    Vector3 moveVector = new Vector3(0,0,0);

    private bool isFiring = false;

    private List<GameObject> insideAOERadius = new List<GameObject>();

    private IEnumerator animationCoroutine;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();

        GameObject[] tmpObj = GameObject.FindGameObjectsWithTag("MovableObject");
        target = tmpObj[0];
    }

    void Update()
    {
        ProcessTranslation();
        //ApplyMovementforce();
        if (isFiring && (forceType == ForceTypes.Force))
        {
            OnFireStart();
        }
    }

    public void OnMove(InputValue value)
    {
        movement = value.Get<Vector3>();
    }

    public void OnFireStart()
    {
        isFiring = true;
        Rigidbody targetRB = target.GetComponent<Rigidbody>();
        if (targetRB != null)
        {
            Vector3 forceDir = Vector3.Normalize(target.transform.position - transform.position);
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

    private void ProcessTranslation()
    {
        float xOffset = movement.x * controlSpeed * Time.deltaTime;
        float rawXPos = transform.localPosition.x + xOffset;


        float zOffset = movement.z * controlSpeed * Time.deltaTime;
        float rawZPos = transform.localPosition.z + zOffset;

        Vector3 newPos = new Vector3(rawXPos, transform.localPosition.y, rawZPos);

        if (newPos != transform.localPosition)
        {
            moveVector = newPos - transform.localPosition;
            transform.rotation = Quaternion.LookRotation(moveVector);
        }

        transform.localPosition = newPos;

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
