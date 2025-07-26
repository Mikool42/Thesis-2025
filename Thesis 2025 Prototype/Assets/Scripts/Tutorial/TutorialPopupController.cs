using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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

    public Image player1;
    public Image player2;
    public Image group;

    [SerializeField] private List<popupIteration> popupIterationsOne = new List<popupIteration>();
    private int popupIterator = 0;

    private void Start()
    {
        //TriggerPopup(popupIterationsOne);
    }

    public void TriggerPopup(List<popupIteration> popupIterations)
    {
        Debug.Log("in popup");

        popupIterator = 0;

        popupIteration curr = popupIterations[popupIterator];

        InstantiateCanvasPopup(curr.popupImage);
        
    }

    public void AButtonPressed()
    {
        Debug.Log("a is triggered");
    }
    public void BButtonPressed()
    {
        Debug.Log("b is triggered");
    }
    public void ShoulderButtonPressed()
    {
        Debug.Log("shoulder is triggered");
    }
    public void AnyButtonPressed()
    {
        Debug.Log("any is triggered");
    }


    private void InstantiateCanvasPopup(Sprite popupSprite)
    {
        player1.gameObject.SetActive(true);
        player2.gameObject.SetActive(true);
        group.gameObject.SetActive(true);

        player1.sprite = popupSprite;
        player2.sprite = popupSprite;
        group.sprite = popupSprite;
        //Instantiate(popupPrefab, can.transform.position, can.transform.rotation, can.transform);
    }
}
