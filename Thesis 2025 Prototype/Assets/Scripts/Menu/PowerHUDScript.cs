using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;


public class PowerHUDScript : MonoBehaviour
{
    private List<GameObject> _players = new List<GameObject>();

    [Header("General")]
    [Tooltip("True if player one has PULL ability, false otherwise. (Should not be edited via inspector)")]
    [SerializeField] bool playerOnePulls = true;

    [Header("Player one")]
    [Tooltip("Indicates if the PULL or PUSH ability is currently chosen")]
    [SerializeField] PlayerAbilityBehaviour.AbilityType P1CurrentAbility = PlayerAbilityBehaviour.AbilityType.PULL;
    [Tooltip("Indicates the level of power for the targeted ability currently being used")]
    [SerializeField] PlayerAbilityBehaviour.ForceLevel P1TargetAbilityLevel = PlayerAbilityBehaviour.ForceLevel.L1;
    [Tooltip("Indicates the level of power for the aoe ability currently being used")]
    [SerializeField] PlayerAbilityBehaviour.ForceLevel P1AoeAbilityLevel = PlayerAbilityBehaviour.ForceLevel.L1;
    [Tooltip("Reference to the target ability image used for player one")]
    [SerializeField] Image p1ImageTarget;
    [Tooltip("Reference to the aoe ability image used for player one")]
    [SerializeField] Image p1ImageAoe;
    [Tooltip("List of sprites to be used for player one pull ability")]
    [SerializeField] Sprite[] p1Sprites;

    [Header("Player two")]
    [Tooltip("Indicates if the PULL or PUSH ability is currently chosen")]
    [SerializeField] PlayerAbilityBehaviour.AbilityType P2CurrentAbility = PlayerAbilityBehaviour.AbilityType.PUSH;
    [Tooltip("Indicates the level of power for the targeted ability currently being used")]
    [SerializeField] PlayerAbilityBehaviour.ForceLevel P2TargetAbilityLevel = PlayerAbilityBehaviour.ForceLevel.L1;
    [Tooltip("Indicates the level of power for the aoe ability currently being used")]
    [SerializeField] PlayerAbilityBehaviour.ForceLevel P2AoeAbilityLevel = PlayerAbilityBehaviour.ForceLevel.L1;
    [Tooltip("Reference to the target ability image used for player two")]
    [SerializeField] Image p2ImageTarget;
    [Tooltip("Reference to the aoe ability image used for player two")]
    [SerializeField] Image p2ImageAoe;
    [Tooltip("List of sprites to be used for player two pull ability")]
    [SerializeField] Sprite[] p2Sprites;

    void Start()
    {
        p1ImageTarget.gameObject.SetActive(false);
        p1ImageAoe.gameObject.SetActive(false);
        p2ImageTarget.gameObject.SetActive(false);
        p2ImageAoe.gameObject.SetActive(false);
    }

    public void ChangeAbilityType(GameObject player, PlayerAbilityBehaviour.AbilityType abilityType)
    {
        if (_players[0] == player)
        {
            P1CurrentAbility = abilityType;
        }
        else if (_players[1] == player)
        {
            P2CurrentAbility = abilityType;
        }
        else
        {
            Debug.LogWarning("Player not found error");
        }

        if (P1CurrentAbility == PlayerAbilityBehaviour.AbilityType.PULL)
        {
            playerOnePulls = true;
        }
        else
        {
            playerOnePulls = false;
        }

        UpdateHUD();
    }

    public void ChangeAbilityPowerLevel(GameObject player, PlayerAbilityBehaviour.ForceLevel forceLevel, bool _isAOE = false)
    {
        if (_players[0] == player)
        {
            if (!_isAOE)
            {
                P1TargetAbilityLevel = forceLevel;
            }
            else
            {
                P1AoeAbilityLevel = forceLevel;
            }
        }
        else if (_players[1] == player)
        {
            if (!_isAOE)
            {
                P2TargetAbilityLevel = forceLevel;
            }
            else
            {
                P2AoeAbilityLevel = forceLevel;
            }
        }
        else
        {
            Debug.LogWarning("Player not found error");
        }

        UpdateHUD();
    }

    public void AddPlayerToHud(GameObject playerToAdd)
    {
        if (_players.Count == 0)
        {
            _players.Add(playerToAdd);

            p1ImageTarget.gameObject.SetActive(true);
            p1ImageAoe.gameObject.SetActive(true);
        }
        else if (_players.Count == 1)
        {
            if (_players[0] == playerToAdd)
            {
                return;
            }
            _players.Add(playerToAdd);

            p2ImageTarget.gameObject.SetActive(true);
            p2ImageAoe.gameObject.SetActive(true);
        }

        UpdateHUD();
    }

    private void UpdateHUD()
    {
        if (playerOnePulls)
        {
            UpdatePlayerAbility("L_Pull_ST_", p1ImageTarget, 1, 1);
            UpdatePlayerAbility("L_Pull_AOE_", p1ImageAoe, -1, 1);

            UpdatePlayerAbility("R_Push_ST_", p2ImageTarget, 1, 2);
            UpdatePlayerAbility("R_Push_AOE_", p2ImageAoe, -1, 2);
        }
        else
        {
            UpdatePlayerAbility("L_Push_ST_", p1ImageTarget, 1, 1);
            UpdatePlayerAbility("L_Push_AOE_", p1ImageAoe, -1, 1);

            UpdatePlayerAbility("R_Pull_ST_", p2ImageTarget, 1, 2);
            UpdatePlayerAbility("R_Pull_AOE_", p2ImageAoe, -1, 2);
        }
    }

    private void UpdatePlayerAbility(string startOfSpriteName, Image _image, int AoeOrSt, int playerNr)
    {
        string spriteName = startOfSpriteName;
        if (AoeOrSt == 1)
        {
            if (playerNr == 1)
            {
                spriteName = spriteName + GetIntOfForceLevel(P1TargetAbilityLevel).ToString();
            }
            else if (playerNr == 2)
            {
                spriteName = spriteName + GetIntOfForceLevel(P2TargetAbilityLevel).ToString();
            }
            else
            {
                Debug.LogWarning("playerNr not set to either 1 of 2");
            }
        }
        else if (AoeOrSt == -1) 
        {
            if (playerNr == 1)
            {
                spriteName = spriteName + GetIntOfForceLevel(P1AoeAbilityLevel).ToString();
            }
            else if (playerNr == 2)
            {
                spriteName = spriteName + GetIntOfForceLevel(P2AoeAbilityLevel).ToString();
            }
            else
            {
                Debug.LogWarning("playerNr not set to either 1 of 2");
            }
        }
        else
        {
            Debug.LogWarning("AoeOrSt not set to either -1 of 1");
        }

        if (playerNr == 1)
        {
            foreach (Sprite sp in p1Sprites)
            {
                if (sp.name == spriteName)
                {
                    _image.sprite = sp;
                }
            }
        }
        else if (playerNr == 2)
        {
            foreach (Sprite sp in p2Sprites)
            {
                if (sp.name == spriteName)
                {
                    _image.sprite = sp;
                }
            }
        }
        else
        {
            Debug.LogWarning("playerNr not set to either 1 of 2");
        }
    }

    private int GetIntOfForceLevel(PlayerAbilityBehaviour.ForceLevel fl)
    {
        if (fl == PlayerAbilityBehaviour.ForceLevel.L1)
        {
            return 1;
        }
        else if (fl == PlayerAbilityBehaviour.ForceLevel.L2)
        {
            return 2;
        }
        else if (fl == PlayerAbilityBehaviour.ForceLevel.L3)
        {
            return 3;
        }
        else
        {
            return 1;
        }
    }
}
