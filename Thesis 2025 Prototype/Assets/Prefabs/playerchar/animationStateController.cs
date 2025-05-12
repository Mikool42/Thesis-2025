using UnityEngine;

public class animationStateController : MonoBehaviour
{
    Animator animator;
    int isRunningHash;
    int isJumpingHash;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        isRunningHash = Animator.StringToHash("isRunning");
        isJumpingHash = Animator.StringToHash("isJumping");
    }

    // Update is called once per frame
    void Update()
    {
        bool isRunning = animator.GetBool(isRunningHash);
        bool isJumping = animator.GetBool(isJumpingHash);

        bool forwardPressedW = Input.GetKey("w");
        bool forwardPressedA = Input.GetKey("a");
        bool forwardPressedS = Input.GetKey("s");
        bool forwardPressedD = Input.GetKey("d");

        bool jumpPressed = Input.GetKey("space");

        //runs
        if (!isRunning && forwardPressedW)
        {
            animator.SetBool(isRunningHash, true);
        }

        if (!isRunning && forwardPressedA)
        {
            animator.SetBool(isRunningHash, true);
        }

        if (!isRunning && forwardPressedS)
        {
            animator.SetBool(isRunningHash, true);
        }

        if (!isRunning && forwardPressedD)
        {
            animator.SetBool(isRunningHash, true);
        }

        if (isRunning && !forwardPressedW)
        {
            animator.SetBool(isRunningHash, false);
        }

        if (isRunning && !forwardPressedA)
        {
            animator.SetBool(isRunningHash, false);
        }

        if (isRunning && !forwardPressedS)
        {
            animator.SetBool(isRunningHash, false);
        }

        if (isRunning && !forwardPressedD)
        {
            animator.SetBool(isRunningHash, false);
        }

    }
}
