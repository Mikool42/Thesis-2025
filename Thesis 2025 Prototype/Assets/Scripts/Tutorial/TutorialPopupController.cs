using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public enum ButtonDismissPresses
{
    ANY,
    ABUTTON,
    BBUTTON,
    SHOULDER
}

[System.Serializable]
public struct popupIteration
{
    public Sprite popupImage;
    public ButtonDismissPresses buttonToDismissPopup;
    public int secondsToTrigger;
}

public class TutorialPopupController : MonoBehaviour
{

    public Canvas player1;
    public Canvas player2;
    public Canvas Group;

    [SerializeField] private List<popupIteration> popupIterationsOne = new List<popupIteration>();

    private void Start()
    {
        TriggerPopup(popupIterationsOne);
    }

    public void TriggerPopup(List<popupIteration> popupIterations)
    {
        Debug.Log("in popup");
        
    }

    private void InstantiateCanvasPopup(GameObject popupPrefab, Canvas can)
    {
        Instantiate(popupPrefab, can.transform.position, can.transform.rotation, can.transform);
    }
}
