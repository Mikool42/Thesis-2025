using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using SmallHedge.SoundManager;

public class MainMenuController : MonoBehaviour
{

    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject controlsMenu;
    [SerializeField] private GameObject creditsMenu;

    [SerializeField] private string firstLevel = "";

    [Header("Navigation")]

    [SerializeField] private GameObject mainMenuOpenButton;
    [SerializeField] private GameObject settingsOpenButton, controlsOpenButton, creditsOpenButton;
    [SerializeField] private GameObject settingsCloseButton, controlsCloseButton, creditsCloseButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainMenu.SetActive(true);
        settingsMenu.SetActive(false);
        controlsMenu.SetActive(false);
        creditsMenu.SetActive(false);

        SetSelectedButton(mainMenuOpenButton);
    }

    public void OnStartButtonPress()
    {
        PlayButtonSound();

        SceneManager.LoadScene(firstLevel);
    }

    public void OnSettingsButtonPress()
    {
        PlayButtonSound();

        mainMenu.SetActive(false);
        settingsMenu.SetActive(true);
        controlsMenu.SetActive(false);
        creditsMenu.SetActive(false);

        SetSelectedButton(settingsOpenButton);
    }

    public void OnControlsButtonPress()
    {
        PlayButtonSound();

        mainMenu.SetActive(false);
        settingsMenu.SetActive(false);
        controlsMenu.SetActive(true);
        creditsMenu.SetActive(false);

        SetSelectedButton(controlsOpenButton);
    }

    public void OnCreditsButtonPress()
    {
        PlayButtonSound();

        mainMenu.SetActive(false);
        settingsMenu.SetActive(false);
        controlsMenu.SetActive(false);
        creditsMenu.SetActive(true);

        SetSelectedButton(creditsOpenButton);
    }

    public void OnEndButtonPress()
    {
        PlayButtonSound();

        StopAllCoroutines();
    }

    public void OnBackButtonPressSettings()
    {
        OnBackButtonPress();

        SetSelectedButton(settingsCloseButton);
    }
    
    public void OnBackButtonPressControls()
    {
        OnBackButtonPress();

        SetSelectedButton(controlsCloseButton);
    }
    
    public void OnBackButtonPressCredits()
    {
        OnBackButtonPress();

        SetSelectedButton(creditsCloseButton);
    }

    public void OnBackButtonPress()
    {
        PlayButtonSound();

        mainMenu.SetActive(true);
        settingsMenu.SetActive(false);
        controlsMenu.SetActive(false);
        creditsMenu.SetActive(false);
    }

    private void PlayButtonSound()
    {
        SoundManager.PlaySound(SoundType.BUTTON);
    }

    private void SetSelectedButton(GameObject go)
    {
        //Clear selected button in event system
        EventSystem.current.SetSelectedGameObject(null);
        //Set selected button in event system
        EventSystem.current.SetSelectedGameObject(go);
    }
}
