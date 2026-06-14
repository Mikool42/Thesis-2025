using Unity.Cinemachine;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class PlayerManager : MonoBehaviour
{

    private PowerHUDScript _powerHUDScript;

    public GameObject playerFollowerOne;
    public GameObject playerFollowerTwo;

    public GameObject playerSpawnCube;

    public void OnPlayerJoined()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        GameObject[] cameras = GameObject.FindGameObjectsWithTag("Camera");

        if (players.Length == 1)
        {

            players[0].GetComponent<PlayerAbilityBehaviour>().SetPlayerAbility(PlayerAbilityBehaviour.AbilityType.PUSH);
            players[0].GetComponent<PlayerAbilityTargeting>().SetPlayerCamera(cameras[0].GetComponent<Camera>());
            playerFollowerOne.transform.SetParent(players[0].transform);

            if (playerSpawnCube != null)
            {
                players[0].transform.position = playerSpawnCube.transform.position;
            }

        }
        else if (players.Length == 2)
        {

            if (players[0].GetComponent<PlayerAbilityBehaviour>().GetPlayerAbility() == PlayerAbilityBehaviour.AbilityType.PUSH)
            {
                players[0].GetComponent<PlayerAbilityTargeting>().SetPlayerCamera(cameras[0].GetComponent<Camera>());
                playerFollowerOne.transform.SetParent(players[0].transform);

                players[1].GetComponent<PlayerAbilityBehaviour>().SetPlayerAbility(PlayerAbilityBehaviour.AbilityType.PULL);
                players[1].GetComponent<PlayerAbilityTargeting>().SetPlayerCamera(cameras[1].GetComponent<Camera>());
                playerFollowerTwo.transform.SetParent(players[1].transform);
            }
            else
            {
                players[0].GetComponent<PlayerAbilityTargeting>().SetPlayerCamera(cameras[0].GetComponent<Camera>());
                playerFollowerOne.transform.SetParent(players[0].transform);

                players[1].GetComponent<PlayerAbilityBehaviour>().SetPlayerAbility(PlayerAbilityBehaviour.AbilityType.PUSH);
                players[1].GetComponent<PlayerAbilityTargeting>().SetPlayerCamera(cameras[1].GetComponent<Camera>());
                playerFollowerTwo.transform.SetParent(players[1].transform);
            }

            if (playerSpawnCube != null)
            {
                players[1].transform.position = playerSpawnCube.transform.position;
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
