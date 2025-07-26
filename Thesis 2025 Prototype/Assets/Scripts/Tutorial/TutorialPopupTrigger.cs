using Unity.VisualScripting;
using UnityEngine;

public class TutorialPopupTrigger : MonoBehaviour
{
    private TutorialPopupController tpc;

    [SerializeField] GameObject popupPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tpc = GameObject.FindGameObjectWithTag("TutorialPopupController").GetComponent<TutorialPopupController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (popupPrefab == null || other.gameObject.tag != "Player") return;

        //tpc.TriggerPopup(popupPrefab);
    }
}
