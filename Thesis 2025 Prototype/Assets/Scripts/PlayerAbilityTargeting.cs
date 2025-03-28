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

    [Tooltip("The reference for the lazer lines line renderer")]
    [SerializeField] private LineRenderer lr;

    private Camera cam;

    private float lineThickness = 1.0f;

    private Vector3 prevTargetPos;

    private List<GameObject> viableTargets = new List<GameObject>();

    private PlayerAbilityBehaviour pab;

    void Start()
    {
        cam = Camera.main;
        pab = GetComponent<PlayerAbilityBehaviour>();

        //lr = GetComponent<LineRenderer>();
        lr.enabled = false;

        //target = GameObject.FindGameObjectsWithTag("MovableObject")[0];
        //prevTargetPos = target.transform.position;
        RenderLineOnTarget();
        StartCoroutine(CheckObjects(targetFindingDelay));
    }

    void Update()
    {
        if (target != null && Vector3.Distance(transform.position, target.transform.position) > targettingRadius)
            OnTargetOutOfRange();

        if (target != null && (prevTargetPos - target.transform.position).sqrMagnitude < 0.01)
            return;

        RenderLineOnTarget();
    }

    public void OnTargetLeft()
    {
        if (target != null)
        {
            target.GetComponent<MovableObjectTargetColorSwitch>().SetAsTarget(false, pab.GetPlayerAbility());
        }

        if (viableTargets.Count == 0) 
        {
            target = null;
            return; 
        }

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
        target.GetComponent<MovableObjectTargetColorSwitch>().SetAsTarget(true, pab.GetPlayerAbility());

        RenderLineOnTarget();
    }
    
    public void OnTargetRight()
    {
        if (target != null)
        {
            target.GetComponent<MovableObjectTargetColorSwitch>().SetAsTarget(false, pab.GetPlayerAbility());
        }

        if (viableTargets.Count == 0)
        {
            target = null;
            return;
        }

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
        target.GetComponent<MovableObjectTargetColorSwitch>().SetAsTarget(true, pab.GetPlayerAbility());

        RenderLineOnTarget();
    }

    private void OnTargetOutOfRange()
    {
        if (target != null)
        {
            target.GetComponent<MovableObjectTargetColorSwitch>().SetAsTarget(false, pab.GetPlayerAbility());
        }

        target = null;
    }

    public void RenderLineOnTarget()
    {
        if (target == null)
        {
            lr.enabled = false;
            return;
        }

        lr.enabled = true;
        var points = new Vector3[2];
        points[0] = transform.position;
        points[1] = target.transform.position;
        lr.SetPositions(points);
        ChangeLineThickness(lineThickness);
    }

    public void ChangeLineThickness(float thickness)
    {
        lineThickness = thickness;

        if (lr == null) { return; }

        lr.startWidth = lineThickness;
        lr.endWidth = lineThickness;
    }

    public GameObject GetTarget() 
    {
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
}
