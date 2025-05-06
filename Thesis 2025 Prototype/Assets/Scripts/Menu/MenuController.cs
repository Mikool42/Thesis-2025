using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using SmallHedge.SoundManager;
using UnityEngine.EventSystems;


public class MenuController : MonoBehaviour
{
    public GameObject mainMenuUI;
    public GameObject settingsMenu;
    public GameObject controlsMenu;
    public GameObject levelsMenu;

    [Header("Navigation")]

    [SerializeField] private GameObject menuOpenButton;
    [SerializeField] private GameObject settingsOpenButton, controlsOpenButton, levelsOpenButton;
    [SerializeField] private GameObject settingsCloseButton, controlsCloseButton, levelsCloseButton;

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

        GameObject[] _players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject p in _players)
        { 
            p.GetComponent<PlayerMenuTrigger>().SwitchActionMapToMenu();
        }

        SetSelectedButton(menuOpenButton);
    }

    public void OnMenuExit()
    {
        Debug.Log("Im out!");
        mainMenuUI.SetActive(false);
        settingsMenu.SetActive(false);
        controlsMenu.SetActive(false);
        levelsMenu.SetActive(false);

        GameObject[] _players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject p in _players)
        {
            p.GetComponent<PlayerMenuTrigger>().SwitchActionMapToPlayer();
        }
    }

    public void EnterSettings()
    {
        mainMenuUI.SetActive(false);
        settingsMenu.SetActive(true);
        controlsMenu.SetActive(false);
        levelsMenu.SetActive(false);

        SetSelectedButton(settingsOpenButton);
    }

    public void ExitSettings()
    {
        OnMenuEnter();

        SetSelectedButton(settingsCloseButton);
    }

    public void EnterControls()
    {
        mainMenuUI.SetActive(false);
        settingsMenu.SetActive(false);
        controlsMenu.SetActive(true);
        levelsMenu.SetActive(false);

        SetSelectedButton(controlsOpenButton);
    }

    public void ExitControls()
    {
        OnMenuEnter();

        SetSelectedButton(controlsCloseButton);
    }

    public void EnterLevels()
    {
        mainMenuUI.SetActive(false);
        settingsMenu.SetActive(false);
        controlsMenu.SetActive(false);
        levelsMenu.SetActive(true);

        SetSelectedButton(levelsOpenButton);
    }

    public void ExitLevels()
    {
        OnMenuEnter();

        SetSelectedButton(levelsCloseButton);
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

    private void SetSelectedButton(GameObject go)
    {
        //Clear selected button in event system
        EventSystem.current.SetSelectedGameObject(null);
        //Set selected button in event system
        EventSystem.current.SetSelectedGameObject(go);
    }
}
