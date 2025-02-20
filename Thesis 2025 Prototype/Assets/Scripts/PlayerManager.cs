using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    
    public void OnPlayerJoined()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        if (players.Length == 1)
        {
            players[0].GetComponent<PlayerAbilityBehaviour>().SetPlayerAbility(PlayerAbilityBehaviour.AbilityType.PUSH);
        }
        else if (players.Length == 2)
        {
            if (players[0].GetComponent<PlayerAbilityBehaviour>().GetPlayerAbility() == PlayerAbilityBehaviour.AbilityType.PUSH)
            {
                players[1].GetComponent<PlayerAbilityBehaviour>().SetPlayerAbility(PlayerAbilityBehaviour.AbilityType.PULL);
            }
            else
            {
                players[1].GetComponent<PlayerAbilityBehaviour>().SetPlayerAbility(PlayerAbilityBehaviour.AbilityType.PUSH);
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
            }
            else
            {
                players[0].GetComponent<PlayerAbilityBehaviour>().SetPlayerAbility(PlayerAbilityBehaviour.AbilityType.PUSH);
            }
        }
        else if (players.Length == 2)
        {
            if (players[0].GetComponent<PlayerAbilityBehaviour>().GetPlayerAbility() == PlayerAbilityBehaviour.AbilityType.PUSH)
            {
                players[0].GetComponent<PlayerAbilityBehaviour>().SetPlayerAbility(PlayerAbilityBehaviour.AbilityType.PULL);
                players[1].GetComponent<PlayerAbilityBehaviour>().SetPlayerAbility(PlayerAbilityBehaviour.AbilityType.PUSH);
            }
            else
            {
                players[0].GetComponent<PlayerAbilityBehaviour>().SetPlayerAbility(PlayerAbilityBehaviour.AbilityType.PUSH);
                players[1].GetComponent<PlayerAbilityBehaviour>().SetPlayerAbility(PlayerAbilityBehaviour.AbilityType.PULL);
            }
        }
        else
        {
            Debug.LogWarning("Player Count wrong, player manager will not work correctly");
        }
    }
}
