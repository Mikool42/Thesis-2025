using UnityEngine;
using System.Collections;
using static PlayerAbilityBehaviour;
using Unity.VisualScripting;

public class TimedTriggerController : MonoBehaviour
{

    [Header("Needed refernences")]
    [Tooltip("All pressure plates to be used, and in the order they should be pressed.")]
    [SerializeField] GameObject[] pressurePlates;
    [Tooltip("Time that you need to press the next pressure plate in.")]
    [SerializeField] float timer = 5f;
    [Tooltip("Similar to the target to trigger on pressure plates, reference to Automatic trigger component.")]
    [SerializeField] AutomaticTrigger TargetToTrigger;

    private int gameobjectIndex = 0;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pressurePlates[0].GetComponentInChildren<PressurePlateController>().Unlock();

        for (int i = 1; i < pressurePlates.Length; i++)
        {
            pressurePlates[i].GetComponentInChildren<PressurePlateController>().Lock();
        }
    }

    public void Trigger(GameObject triggerObject)
    {
        if (pressurePlates[gameobjectIndex] == triggerObject)
        {
            if (gameobjectIndex == pressurePlates.Length - 1)
            {
                gameobjectIndex++; //Just to stop the last coroutine
                TargetToTrigger.TriggerButtonDown();
                return;
            }

            gameobjectIndex++;
            StartCoroutine(TriggerTimer(gameobjectIndex - 1));
        }
    }

    private IEnumerator TriggerTimer(int _index)
    {
        float _timer = timer;
        pressurePlates[_index + 1].GetComponentInChildren<PressurePlateController>().Unlock();
        while (0f < _timer)
        {
            if (_index + 1 != gameobjectIndex) 
            {
                yield break; 
            }

            _timer = _timer - Time.deltaTime;

            yield return new WaitForFixedUpdate();
        }
        ResetPressurePlates();
        yield return null;
    }

    private void ResetPressurePlates()
    {
        foreach (GameObject pp in pressurePlates)
        {
            pp.GetComponentInChildren<PressurePlateController>().UnToggle();
            pp.GetComponentInChildren<PressurePlateController>().Lock();
        }
        pressurePlates[0].GetComponentInChildren<PressurePlateController>().Unlock();
        
        gameobjectIndex = 0;
    }


}
