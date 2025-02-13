using UnityEngine;

public class PlayerAbilityTargeting : MonoBehaviour
{
    [Tooltip("The Target for the targeted ability")]
    [SerializeField] GameObject target = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        target = GameObject.FindGameObjectsWithTag("MovableObject")[0];
    }

    public GameObject GetTarget() 
    {
        if (target == null)
        {
            return GameObject.FindGameObjectsWithTag("MovableObject")[0];
        }
        return target;
    }
}
