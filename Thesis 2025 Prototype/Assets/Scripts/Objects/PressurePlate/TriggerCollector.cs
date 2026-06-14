using UnityEngine;
using UnityEngine.Events;

public class TriggerCollector : MonoBehaviour
{
    [Header("Ability General")]
    [Tooltip("How many triggers to collect to trigger Event")]
    [SerializeField] int numberOfTriggers = 1;

    [Tooltip("The Event to")]
    [SerializeField] private UnityEvent unityEvent;



    public int currentNumberOfTriggers = 0;


    public void AddTrigger()
    {
        currentNumberOfTriggers++;

        if (currentNumberOfTriggers == numberOfTriggers)
        {
            unityEvent?.Invoke();
        }
    }

    public void RemoveTrigger()
    {
        currentNumberOfTriggers--;
    }
}
