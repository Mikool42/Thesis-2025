using UnityEngine;

public class NewMovableObject : MonoBehaviour
{
    private Rigidbody rb;

    [Tooltip("How much force to use for level one")]
    [SerializeField] float forceAmount_L1 = 2f;
    [Tooltip("How much force to use for level two")]
    [SerializeField] float forceAmount_L2 = 5f;
    [Tooltip("How much force to use for level three")]
    [SerializeField] float forceAmount_L3 = 10f;

    private float appliedForceAmount = -1;
    private Vector3 appliedForceDir = Vector3.zero;
    private int pushPullMultiplier = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (rb != null && appliedForceAmount != -1 && appliedForceDir != Vector3.zero)
        {
            rb.AddForce(appliedForceAmount * appliedForceDir * pushPullMultiplier);

            appliedForceAmount = -1;
            appliedForceDir = Vector3.zero;
            pushPullMultiplier = 0;
        }
    }

    public void ApplyForceToObject(Vector3 forceDirection, int forceLevel, int _pushPullMultiplier)
    {
        if (forceLevel == 0) appliedForceAmount = forceAmount_L1;
        else if (forceLevel == 1) appliedForceAmount = forceAmount_L2;
        else if (forceLevel == 2) appliedForceAmount = forceAmount_L3;
        else appliedForceAmount = -1;

        appliedForceDir = forceDirection;

        pushPullMultiplier = _pushPullMultiplier;
    }
}
