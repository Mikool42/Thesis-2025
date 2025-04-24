using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerMenuTrigger : MonoBehaviour
{

    private GameObject _menu;
    private MenuController menuController;

    public PlayerInput playerInput;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _menu = GameObject.FindGameObjectWithTag("MenuContainer");
        menuController = _menu.GetComponent<MenuController>();
    }

    public void OnMenuEnter()
    {
        if (menuController == null) { return; }

        menuController.OnMenuEnter();

        GameObject[] _players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject p in _players)
        { 
            if (p != gameObject)
            {
                p.GetComponent<PlayerMenuTrigger>().SwitchActionMapToMenu();
            }
        }

        // Disable gameplay inputs.
        SwitchActionMapToMenu();
    }

    public void OnMenuExit()
    {
        if (menuController == null) { return; }
        
        menuController.OnMenuExit();

        GameObject[] _players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject p in _players)
        {
            if (p != gameObject)
            {
                p.GetComponent<PlayerMenuTrigger>().SwitchActionMapToPlayer();
            }
        }

        // Reenable gameplay inputs.
        SwitchActionMapToPlayer();
    }

    public void SwitchActionMapToPlayer()
    {
        playerInput.SwitchCurrentActionMap("Player");
    }

    public void SwitchActionMapToMenu()
    {
        playerInput.SwitchCurrentActionMap("Menu");
    }
}
