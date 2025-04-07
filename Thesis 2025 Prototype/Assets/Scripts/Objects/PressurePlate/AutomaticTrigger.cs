using UnityEngine;
using UnityEngine.Events;

public class AutomaticTrigger : MonoBehaviour
{
    enum TriggerTypes { Animation, ScriptFunction };

    [Header("Trigger General")]
    [Tooltip("Trigger type")]
    [SerializeField] TriggerTypes type = TriggerTypes.Animation;

    [Header("Animation Trigger options")]
    [Tooltip("The Animation to trigger")]
    [SerializeField] Animation animations;
    [Tooltip("The animation sequence to play on button down")]
    [SerializeField] string firstAnimation = "";
    [Tooltip("The animation sequence to play on button up")]
    [SerializeField] string secondAnimation = "";

    [Header("Script Function Trigger options")]
    [Tooltip("The Event to trigger on button down")]
    [SerializeField] private UnityEvent firstScriptFunction;
    [Tooltip("The Event to trigger on button up")]
    [SerializeField] private UnityEvent secondScriptFunction;


    public void TriggerButtonDown()
    {
        if (type == TriggerTypes.Animation)
        {
            TriggerFirstAnimation();
        }
        else if (type == TriggerTypes.ScriptFunction)
        {
            TriggerFirstFunction();
        }
    }

    public void TriggerButtonUp()
    {
        if (type == TriggerTypes.Animation)
        {
            TriggerSecondAnimation();
        }
        else if (type == TriggerTypes.ScriptFunction)
        {
            TriggerSecondFunction();
        }
    }

    private void TriggerFirstFunction()
    {
        firstScriptFunction?.Invoke();
    }

    private void TriggerSecondFunction()
    {
        secondScriptFunction?.Invoke();
    }

    private void TriggerFirstAnimation()
    {
        animations.Play(firstAnimation);
    }

    private void TriggerSecondAnimation()
    {
        animations.Play(secondAnimation);
    }
}
