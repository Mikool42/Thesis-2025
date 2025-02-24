using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class PlayerAbilityTargeting : MonoBehaviour
{
    [Tooltip("The Target for the targeted ability")]
    [SerializeField] GameObject target = null;

    [Tooltip("The delay for looping through all movable objects and finding which ones are targettable")]
    [SerializeField] float targetFindingDelay = 0.2f;

    [Tooltip("The distance from the player that they can find targets")]
    [SerializeField] float targettingRadius = 5f;


    private Camera cam;

    private List<GameObject> viableTargets = new List<GameObject>();

    void Start()
    {
        cam = Camera.main;

        target = GameObject.FindGameObjectsWithTag("MovableObject")[0];

        StartCoroutine(CheckObjects(targetFindingDelay));
    }

    public void OnTargetLeft()
    {
        target.GetComponent<MovableObjectTargetColorSwitch>().SetAsTarget(false);

        if (viableTargets.Count == 0) {  return; }

        int indexOfTargetInSortedList = viableTargets.IndexOf(target);
        if (indexOfTargetInSortedList == -1) { indexOfTargetInSortedList = 0; }

        if (indexOfTargetInSortedList == 0 )
        {
            target = viableTargets[viableTargets.Count -1]; // if the target is the firstobject set it to the last object
        }
        else
        {
            target = viableTargets[indexOfTargetInSortedList - 1]; // set the target as the next target below
        }
        target.GetComponent<MovableObjectTargetColorSwitch>().SetAsTarget(true);
    }
    
    public void OnTargetRight()
    {
        target.GetComponent<MovableObjectTargetColorSwitch>().SetAsTarget(false);

        if (viableTargets.Count == 0) {  return; }

        int indexOfTargetInSortedList = viableTargets.IndexOf(target);
        if (indexOfTargetInSortedList == -1) { indexOfTargetInSortedList = 0; }

        if (indexOfTargetInSortedList == viableTargets.Count - 1)
        {
            target = viableTargets[0]; // if the target is the last object set it to the first object
        }
        else
        {
            target = viableTargets[indexOfTargetInSortedList + 1]; // set the target as the next target above
        }
        target.GetComponent<MovableObjectTargetColorSwitch>().SetAsTarget(true);
    }

    public GameObject GetTarget() 
    {
        if (target == null)
        {
            return GameObject.FindGameObjectsWithTag("MovableObject")[0];
        }
        return target;
    }
    
    public float GetTargettingRadius() 
    {
        return targettingRadius;
    }

    private IEnumerator CheckObjects(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);

            GameObject[] movableObjects = GameObject.FindGameObjectsWithTag("MovableObject");
            viableTargets.Clear();

            foreach (GameObject movObj in movableObjects)
            {
                Vector3 viewPos = cam.WorldToViewportPoint(movObj.transform.position);
                if (Vector3.Distance(transform.position, movObj.transform.position) <= targettingRadius && viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1 && viewPos.z > 0)
                {
                    viableTargets.Add(movObj);
                }
            }

            viableTargets = SortByPosX(viableTargets);
        }
    }

    private List<GameObject> SortByPosX(List<GameObject> unsortedList)
    {
        List<GameObject> sortedList = new List<GameObject> ();
        sortedList = unsortedList.OrderBy(_object => _object.transform.position.x).ToList();
        return sortedList;
    }


    /// <summary>
    /// ///////////////////delete this when done only ussed to debug
    /// </summary>
    /// <param name="list"></param>
    private void PrintNamesOfObjectsInList(List<GameObject> list)
    {
        foreach (GameObject obj in list)
        {
            Debug.Log(obj.name);
        }
    }
    
    private void PrintXPosOfObjectsInList(List<GameObject> list)
    {
        foreach (GameObject obj in list)
        {
            Debug.Log(obj.transform.position.x);
        }
    }
    /////////////////////////////////////////
}
