using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using static PlayerAbilityBehaviour;
using Unity.VisualScripting;

public class TimedTriggerController : MonoBehaviour
{

    [Header("Needed refernences")]
    [Tooltip("All pressure plates to be used, and in the order they should be pressed.")]
    [SerializeField] GameObject[] pressurePlates;
    [Tooltip("Similar to the target to trigger on pressure plates, reference to Automatic trigger component.")]
    [SerializeField] AutomaticTrigger TargetToTrigger;

    [Header("Editable fields")]
    [Tooltip("Time that you need to press the next pressure plate in.")]
    [SerializeField] float timer = 5f;

    [Header("Button blinking options")]
    [Tooltip("Maximum amount of time between blinks.")]
    [SerializeField] float maxInterval = 1.2f;
    [Tooltip("Minnimum amount of time between blinks.")]
    [SerializeField] float minInterval = 0.2f;
    [Tooltip("Blinking color")]
    [SerializeField] Color blinkColor = new Color(0, 0, 50);
    private List<Coroutine> CurrentBlinkingCoroutine = new List<Coroutine>(); // To keep track of the blinking buttons
    private List<GameObject> CurrentBlinkingObject = new List<GameObject>(); // To keep track of the blinking buttons
    private List<Color> CurrentBlinkingObjectColor = new List<Color>(); // To keep track of the blinking buttons
    private Color fallbackColor = new Color(0, 0, 255);

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
            if (gameobjectIndex == 0)
            {
                //first pressure plate pressed

            }

            if (gameobjectIndex == pressurePlates.Length - 1)
            {
                // last pressure plate pressed
                gameobjectIndex++; //Just to stop the last coroutine
                StopBlinking();
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
        StopBlinking();
        StartBlinking(timer);
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
        StopBlinking();

        foreach (GameObject pp in pressurePlates)
        {
            pp.GetComponentInChildren<PressurePlateController>().UnToggle();
            pp.GetComponentInChildren<PressurePlateController>().Lock();
        }
        pressurePlates[0].GetComponentInChildren<PressurePlateController>().Unlock();
        
        gameobjectIndex = 0;
    }

    public void StartBlinking(float totalBlinkTime)
    {
        foreach (GameObject go in pressurePlates)
        {
            Transform blinkingTransform = go.transform.Find("Pressure plate/Button");
            if (blinkingTransform == null) { Debug.LogWarning("pressure plate and button hierarchy broken"); return; }

            GameObject blinkingObject = blinkingTransform.gameObject;

            CurrentBlinkingObject.Add(blinkingObject);
            CurrentBlinkingObjectColor.Add(blinkingObject.GetComponent<Renderer>().material.color);
            CurrentBlinkingCoroutine.Add(StartCoroutine(Blinking(blinkingObject, totalBlinkTime)));
        }
    }

    private void StopBlinking()
    {
        for (int i = 0; i < CurrentBlinkingCoroutine.Count; i++)
        {
            Coroutine c = CurrentBlinkingCoroutine[i];
            if (c != null)
            {
                StopCoroutine(c);
                CurrentBlinkingObject[i].GetComponent<Renderer>().material.color = CurrentBlinkingObjectColor[i];
            }

        }
    }

    private IEnumerator Blinking(GameObject _go, float totalBlinkTime)
    {
        float interval = 1.0f;
        float _timer = totalBlinkTime;

        Color originalColor = _go.GetComponent<Renderer>().material.color;

        while (0f < _timer)
        {
            interval = minInterval + _timer / totalBlinkTime * (maxInterval - minInterval);
            _timer -= Time.deltaTime;
            if (_timer < 0.0f)
            {
                _go.GetComponent<Renderer>().material.color = originalColor;
                yield break;
            }

            if (Mathf.PingPong(Time.time, interval) > (interval / 2.0f))
            {
                _go.GetComponent<Renderer>().material.color = originalColor;
            }
            else
            {
                _go.GetComponent<Renderer>().material.color = blinkColor;
            }

            yield return new WaitForFixedUpdate();
        }
    }

}
