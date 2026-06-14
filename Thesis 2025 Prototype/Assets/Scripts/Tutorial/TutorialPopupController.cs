using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    public float secondsToTrigger;
}

public class TutorialPopupController : MonoBehaviour
{

    public Image player1;
    public Image player2;
    public Image group;

    [SerializeField] private List<popupIteration> popupIterationsOne = new List<popupIteration>();
    private List<popupIteration> currentIterationList = null;
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
        currentIterationList = popupIterations;

        popupIterator = 0;

        popupIteration curr = currentIterationList[popupIterator];

        InstantiateCanvasPopup(curr.popupImage);

        WaitForResponse(curr.buttonToDismissPopup);
        
    }

    private void TriggerNextIteration()
    {
        Debug.Log("in next trigger");
        if (currentIterationList.Count <= popupIterator) return;

        popupIteration curr = currentIterationList[popupIterator];

        InstantiateCanvasPopup(curr.popupImage);

        WaitForResponse(curr.buttonToDismissPopup);
    }

    private void StartCountdownForNextPopup()
    {
        popupIteration curr = currentIterationList[popupIterator];

        popupIterator++;

        IEnumerator coroutine = CountdownToNextPopup(curr.secondsToTrigger);
        StartCoroutine(coroutine);
    }

    public void AButtonPressed(GameObject _playerThatPressed)
    {
        AnyButtonPressed(_playerThatPressed);
        if (!waitingForAPress) return;

        if(RemoveCanvasPopup(_playerThatPressed))
        {
            waitingForAPress = false;
            StartCountdownForNextPopup();
        }
    }
    public void BButtonPressed(GameObject _playerThatPressed)
    {
        AnyButtonPressed(_playerThatPressed);
        if (!waitingForBPress) return;

        if(RemoveCanvasPopup(_playerThatPressed))
        {
            waitingForBPress = false;
            StartCountdownForNextPopup();
        }
    }
    public void ShoulderButtonPressed(GameObject _playerThatPressed)
    {
        AnyButtonPressed(_playerThatPressed);
        if (!waitingForShoulderPress) return;

        if(RemoveCanvasPopup(_playerThatPressed))
        {
            waitingForShoulderPress = false;
            StartCountdownForNextPopup();
        }
    }
    public void AnyButtonPressed(GameObject _playerThatPressed)
    {
        if (!waitingForAnyPress) return;

        if (RemoveCanvasPopup(_playerThatPressed))
        {
            waitingForAnyPress = false;
            StartCountdownForNextPopup();
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
            player1.gameObject.SetActive(false);
            playerOneHasDismissed = true;
        }
        else if (followerTwo != null && followerTwo.gameObject.name == "PlayerFollower2")
        {
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

    IEnumerator CountdownToNextPopup(float sec)
    {
        yield return new WaitForSeconds(sec);
        //Start next iteration
        TriggerNextIteration();
    }
}
