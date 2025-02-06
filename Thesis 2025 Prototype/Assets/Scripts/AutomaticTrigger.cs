using UnityEngine;

public class AutomaticTrigger : MonoBehaviour
{

    [SerializeField] Animation animations;
    [SerializeField] string firstAnimation = "";
    [SerializeField] string secondAnimation = "";

    public void TriggerFirstAnimation()
    {
        animations.Play(firstAnimation);
    }

    public void TriggerSecondAnimation()
    {
        animations.Play(secondAnimation);
    }
}
