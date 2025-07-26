using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TutorialPopupTrigger : MonoBehaviour
{
    private TutorialPopupController tpc;

    [SerializeField] private List<popupIteration> popupSequence = new List<popupIteration>();

    private bool hasBeenTriggered = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tpc = GameObject.FindGameObjectWithTag("TutorialPopupController").GetComponent<TutorialPopupController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasBeenTriggered) return;
        if (popupSequence == null || other.gameObject.tag != "Player") return;

        hasBeenTriggered = true;
        tpc.TriggerPopup(popupSequence);
    }
}
