using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    private PowerHUDScript _powerHUDScript;

    public void OnPlayerJoined()
    {
        //FindingPowerHUDScript();

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        if (players.Length == 1)
        {
            //_powerHUDScript.AddPlayerToHud(players[0]);

            players[0].GetComponent<PlayerAbilityBehaviour>().SetPlayerAbility(PlayerAbilityBehaviour.AbilityType.PUSH);
            //_powerHUDScript.ChangeAbilityType(players[0], PlayerAbilityBehaviour.AbilityType.PUSH);

        }
        else if (players.Length == 2)
        {
            //_powerHUDScript.AddPlayerToHud(players[0]);
            //_powerHUDScript.AddPlayerToHud(players[1]);

            if (players[0].GetComponent<PlayerAbilityBehaviour>().GetPlayerAbility() == PlayerAbilityBehaviour.AbilityType.PUSH)
            {
                players[1].GetComponent<PlayerAbilityBehaviour>().SetPlayerAbility(PlayerAbilityBehaviour.AbilityType.PULL);
                //_powerHUDScript.ChangeAbilityType(players[1], PlayerAbilityBehaviour.AbilityType.PULL);
            }
            else
            {
                players[1].GetComponent<PlayerAbilityBehaviour>().SetPlayerAbility(PlayerAbilityBehaviour.AbilityType.PUSH);
                //_powerHUDScript.ChangeAbilityType(players[1], PlayerAbilityBehaviour.AbilityType.PUSH);
            }
        }
        else
        {
            Debug.LogWarning("Player Count wrong, player manager will not work correctly");
        }
    }

    public void SwitchPlayerAbility()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        if (players.Length == 1)
        {
            if (players[0].GetComponent<PlayerAbilityBehaviour>().GetPlayerAbility() == PlayerAbilityBehaviour.AbilityType.PUSH)
            {
                players[0].GetComponent<PlayerAbilityBehaviour>().SetPlayerAbility(PlayerAbilityBehaviour.AbilityType.PULL);
                //_powerHUDScript.ChangeAbilityType(players[0], PlayerAbilityBehaviour.AbilityType.PULL);
            }
            else
            {
                players[0].GetComponent<PlayerAbilityBehaviour>().SetPlayerAbility(PlayerAbilityBehaviour.AbilityType.PUSH);
                //_powerHUDScript.ChangeAbilityType(players[0], PlayerAbilityBehaviour.AbilityType.PUSH);
            }
        }
        else if (players.Length == 2)
        {
            if (players[0].GetComponent<PlayerAbilityBehaviour>().GetPlayerAbility() == PlayerAbilityBehaviour.AbilityType.PUSH)
            {
                players[0].GetComponent<PlayerAbilityBehaviour>().SetPlayerAbility(PlayerAbilityBehaviour.AbilityType.PULL);
                players[1].GetComponent<PlayerAbilityBehaviour>().SetPlayerAbility(PlayerAbilityBehaviour.AbilityType.PUSH);
                //_powerHUDScript.ChangeAbilityType(players[0], PlayerAbilityBehaviour.AbilityType.PULL);
                //_powerHUDScript.ChangeAbilityType(players[1], PlayerAbilityBehaviour.AbilityType.PUSH);
            }
            else
            {
                players[0].GetComponent<PlayerAbilityBehaviour>().SetPlayerAbility(PlayerAbilityBehaviour.AbilityType.PUSH);
                players[1].GetComponent<PlayerAbilityBehaviour>().SetPlayerAbility(PlayerAbilityBehaviour.AbilityType.PULL);
                //_powerHUDScript.ChangeAbilityType(players[0], PlayerAbilityBehaviour.AbilityType.PUSH);
                //_powerHUDScript.ChangeAbilityType(players[1], PlayerAbilityBehaviour.AbilityType.PULL);
            }
        }
        else
        {
            Debug.LogWarning("Player Count wrong, player manager will not work correctly");
        }
    }

    private void FindingPowerHUDScript()
    {
        GameObject _pp = GameObject.FindGameObjectWithTag("InGameUI");
        if (_pp != null)
        {
            _powerHUDScript = _pp.GetComponent<PowerHUDScript>();
        }
        else
        {
            Debug.LogWarning("power HUD Script not found");
        }
    }
}
