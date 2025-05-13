using UnityEngine;

public class animationStateController : MonoBehaviour
{
    Animator animator;
    string isRunning = "isRunning";
    string isJumping = "isJumping";

    [Header("For Animations")]
    [Tooltip("Reference to movement script")]
    [SerializeField] PlayerMovement playerMovementScript;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        bool isRunningAnimState = animator.GetBool(isRunning);
        //bool isJumpingAnimState = animator.GetBool(isJumping);

        Vector3 movement = playerMovementScript.GetMoveVector();

        bool isMoving = false;
        if ( !(movement.sqrMagnitude < 0.01) )
        {
            isMoving = true;
        }

        bool jumpPressed = playerMovementScript.GetJumpBool();

        //runs
        if (!isRunningAnimState && isMoving)
        {
            animator.SetBool(isRunning, true);
        }
        else if (isRunningAnimState && !isMoving)
        {
            animator.SetBool(isRunning, false);
        }
    }
}
