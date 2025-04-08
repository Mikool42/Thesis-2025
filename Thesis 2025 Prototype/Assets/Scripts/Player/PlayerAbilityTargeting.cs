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

    [Tooltip("The minnimum distance from the player that they can find targets")]
    [SerializeField] float minTargettingRadius = 1f;

    [Tooltip("The reference for the lazer lines line renderer")]
    [SerializeField] private LineRenderer lr;

    [Tooltip("The line prefab to be used for AOE targets")]
    [SerializeField] private GameObject AOELazerPrefab;

    [Tooltip("The reference for the AOE lazer container object")]
    [SerializeField] private GameObject aoeLazerContainer;

    private List<GameObject> aoeLazers = new List<GameObject>();
    private List<GameObject> aoeLazersActive = new List<GameObject>();

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
        InstantiateAoeLazers();
        RenderLineOnTarget();
        StartCoroutine(CheckObjects(targetFindingDelay));
    }

    void Update()
    {

        if (target != null)
        {
            float _dist = Vector3.Distance(transform.position, target.transform.position);
            if (_dist > targettingRadius || _dist < minTargettingRadius)
            {
                OnTargetOutOfRange();
            }
        }

        if (target != null && (prevTargetPos - target.transform.position).sqrMagnitude < 0.01)
            return;

        RenderLineOnTarget();
        AddLinesToAOETargets();
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

        lr.startWidth = 0.2f;
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
                float dist = Vector3.Distance(transform.position, movObj.transform.position);
                if (dist <= targettingRadius && dist >= minTargettingRadius && viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1 && viewPos.z > 0)
                {
                    viableTargets.Add(movObj);
                }
            }

            viableTargets = SortByPosX(viableTargets);
        }
    }

    private void AddLinesToAOETargets()
    {
        // Clear out and disable the active lazers
        for (int i = 0; i < aoeLazersActive.Count; i++)
        {
            aoeLazersActive[i].GetComponent<LineRenderer>().enabled = false;
        }
        aoeLazersActive.Clear();

        // Only add lazers on the aoe targets and not the main one
        List<GameObject> targetsMinusMainTarget = new List<GameObject>(viableTargets);
        if (viableTargets.Contains(target))
        {
            targetsMinusMainTarget.Remove(target);
        }

        // Activate the lazers, aim them and add them to the active list
        for (int i = 0; i < aoeLazers.Count; i++)
        {
            if (i >= targetsMinusMainTarget.Count) return;

            GameObject _target = aoeLazers[i];
            aoeLazersActive.Add(_target);

            LineRenderer _targetLR = _target.GetComponent<LineRenderer>();
            _targetLR.enabled = true;

            var points = new Vector3[2];
            points[0] = transform.position;
            points[1] = targetsMinusMainTarget[i].transform.position;
            _targetLR.SetPositions(points);

            _targetLR.startWidth = 0.2f;
            _targetLR.endWidth = lineThickness;
        }
        

    }

    public List<GameObject> GetAoeTargetsList()
    {
        return viableTargets;
    }

    private void InstantiateAoeLazers()
    {
        for (int i = 0; i < 10;i++)
        {
            GameObject newLazer = Instantiate(AOELazerPrefab);
            newLazer.transform.parent = aoeLazerContainer.transform;
            newLazer.GetComponent<LineRenderer>().enabled = false;
            aoeLazers.Add(newLazer);
        }
    }

    private List<GameObject> SortByPosX(List<GameObject> unsortedList)
    {
        List<GameObject> sortedList = new List<GameObject> ();
        sortedList = unsortedList.OrderBy(_object => _object.transform.position.x).ToList();
        return sortedList;
    }
}
