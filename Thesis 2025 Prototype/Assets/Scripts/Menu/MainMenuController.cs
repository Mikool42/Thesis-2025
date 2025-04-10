using UnityEngine;
using UnityEngine.SceneManagement;
using SmallHedge.SoundManager;

public class MainMenuController : MonoBehaviour
{

    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject controlsMenu;
    [SerializeField] private GameObject creditsMenu;

    [SerializeField] private string firstLevel = "";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainMenu.SetActive(true);
        settingsMenu.SetActive(false);
        controlsMenu.SetActive(false);
        creditsMenu.SetActive(false);
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
    }

    public void OnControlsButtonPress()
    {
        PlayButtonSound();

        mainMenu.SetActive(false);
        settingsMenu.SetActive(false);
        controlsMenu.SetActive(true);
        creditsMenu.SetActive(false);
    }

    public void OnCreditsButtonPress()
    {
        PlayButtonSound();

        mainMenu.SetActive(false);
        settingsMenu.SetActive(false);
        controlsMenu.SetActive(false);
        creditsMenu.SetActive(true);
    }

    public void OnEndButtonPress()
    {
        PlayButtonSound();

        StopAllCoroutines();
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
}
