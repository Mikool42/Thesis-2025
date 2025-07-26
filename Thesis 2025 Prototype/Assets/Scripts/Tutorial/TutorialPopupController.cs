using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor.PackageManager;
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

    private bool waitingForAPress = false;
    private bool waitingForBPress = false;
    private bool waitingForShoulderPress = false;
    private bool waitingForAnyPress = false;

    private bool playerOneHasDismissed = false;
    private bool playerTwoHasDismissed = false;



    private void Start()
    {
        TriggerPopup(popupIterationsOne);
    }

    public void TriggerPopup(List<popupIteration> popupIterations)
    {
        popupIterator = 0;

        popupIteration curr = popupIterations[popupIterator];

        InstantiateCanvasPopup(curr.popupImage);

        WaitForResponse(curr.buttonToDismissPopup);
        
    }

    public void AButtonPressed(GameObject _playerThatPressed)
    {
        if (!waitingForAPress) return;

        if(RemoveCanvasPopup(_playerThatPressed))
        {
            waitingForAPress = false;
        }
    }
    public void BButtonPressed(GameObject _playerThatPressed)
    {
        if (!waitingForBPress) return;

        if(RemoveCanvasPopup(_playerThatPressed))
        {
            waitingForBPress = false;
        }
    }
    public void ShoulderButtonPressed(GameObject _playerThatPressed)
    {
        if (!waitingForShoulderPress) return;

        if(RemoveCanvasPopup(_playerThatPressed))
        {
            waitingForShoulderPress = false;
        }
    }
    public void AnyButtonPressed(GameObject _playerThatPressed)
    {
        if (!waitingForAnyPress) return;

        if (RemoveCanvasPopup(_playerThatPressed))
        {
            waitingForAnyPress = false;
        }
    }

    private void WaitForResponse(ButtonDismissPresses _button)
    {
        switch(_button)
        {
            case ButtonDismissPresses.ABUTTON:
                waitingForAPress = true;
                break;
            case ButtonDismissPresses.BBUTTON:
                waitingForBPress = true;
                break;
            case ButtonDismissPresses.SHOULDER:
                waitingForShoulderPress = true;
                break;
            case ButtonDismissPresses.ANY:
                waitingForAnyPress = true;
                break;
            default:
                waitingForAnyPress = true;
                break;
        }
    }

    private void InstantiateCanvasPopup(Sprite popupSprite)
    {
        player1.gameObject.SetActive(true);
        player2.gameObject.SetActive(true);
        group.gameObject.SetActive(true);

        player1.sprite = popupSprite;
        player2.sprite = popupSprite;
        group.sprite = popupSprite;

        playerOneHasDismissed = false;
        playerTwoHasDismissed = false;
        //Instantiate(popupPrefab, can.transform.position, can.transform.rotation, can.transform);
    }

    //Return true if all popups have been removed, else false
    private bool RemoveCanvasPopup(GameObject _player)
    {
        Transform followerOne = _player.transform.Find("PlayerFollower1");
        Transform followerTwo = _player.transform.Find("PlayerFollower2");

        if (followerOne != null && followerOne.gameObject.name == "PlayerFollower1")
        {
            Debug.Log("player one dismissed");
            player1.gameObject.SetActive(false);
            playerOneHasDismissed = true;
        }
        else if (followerTwo != null && followerTwo.gameObject.name == "PlayerFollower2")
        {
            Debug.Log("player two dismissed");
            player2.gameObject.SetActive(false);
            playerTwoHasDismissed = true;
        }
        else
        {
            playerOneHasDismissed = true;
            playerTwoHasDismissed = true;
            Debug.LogError("could not figure out which player pressed the button");
        }

        if (playerOneHasDismissed && playerTwoHasDismissed)
        {
            player1.gameObject.SetActive(false);
            player2.gameObject.SetActive(false);
            group.gameObject.SetActive(false);

            player1.sprite = null;
            player2.sprite = null;
            group.sprite = null;

            return true;
        }

        return false;
    }
}
