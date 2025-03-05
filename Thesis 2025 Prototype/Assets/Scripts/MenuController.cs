using UnityEngine;
using UnityEngine.InputSystem;

public class MenuController : MonoBehaviour
{

    public GameObject inGameUI;
    public GameObject mainMenuUI;

    internal enum State
    {
        InGame,
        InMenu,
    }

    internal State m_State;

    public void OnEnable()
    {
        // By default, hide menu and show game UI.
        inGameUI.SetActive(true);
        mainMenuUI.SetActive(false);

        m_State = State.InGame;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMenuToggle()
    {
        if(m_State == State.InGame)
        {
            Debug.Log("in menu enter in controller");
            inGameUI.SetActive(false);
            mainMenuUI.SetActive(true);

            m_State = State.InMenu;
        }
        else if (m_State == State.InMenu)
        {
            Debug.Log("in menu exit in controller");
            inGameUI.SetActive(true);
            mainMenuUI.SetActive(false);

            m_State = State.InGame;
        }
    }
}
