using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using SmallHedge.SoundManager;

public class MenuController : MonoBehaviour
{
    public GameObject mainMenuUI;
    public GameObject settingsMenu;
    public GameObject controlsMenu;
    public GameObject levelsMenu;

    public void OnEnable()
    {
        OnMenuExit();
    }

    public void OnMenuEnter()
    {
        Debug.Log("Im in!");
        mainMenuUI.SetActive(true);
        settingsMenu.SetActive(false);
        controlsMenu.SetActive(false);
        levelsMenu.SetActive(false);
    }

    public void OnMenuExit()
    {
        Debug.Log("Im out!");
        mainMenuUI.SetActive(false);
        settingsMenu.SetActive(false);
        controlsMenu.SetActive(false);
        levelsMenu.SetActive(false);
    }

    public void EnterSettings()
    {
        mainMenuUI.SetActive(false);
        settingsMenu.SetActive(true);
        controlsMenu.SetActive(false);
        levelsMenu.SetActive(false);
    }

    public void ExitSettings()
    {
        OnMenuEnter();
    }

    public void EnterControls()
    {
        mainMenuUI.SetActive(false);
        settingsMenu.SetActive(false);
        controlsMenu.SetActive(true);
        levelsMenu.SetActive(false);
    }

    public void ExitControls()
    {
        OnMenuEnter();
    }

    public void EnterLevels()
    {
        mainMenuUI.SetActive(false);
        settingsMenu.SetActive(false);
        controlsMenu.SetActive(false);
        levelsMenu.SetActive(true);
    }

    public void ExitLevels()
    {
        OnMenuEnter();
    }

    public void SwitchLevel(int level)
    {
        if(level < 0)
        {
            Debug.LogError("Level less than 0, not allowed level starts at 1");
        }

        SceneManager.LoadScene(sceneBuildIndex:level);
    }

    public void ReloadCurrentScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        Debug.Log(currentSceneName);
        SceneManager.LoadScene(currentSceneName);
    }

    public void OnEndButtonPress()
    {
        PlayButtonSound();

        StopAllCoroutines();
    }

    private void PlayButtonSound()
    {
        SoundManager.PlaySound(SoundType.BUTTON);
    }
}
