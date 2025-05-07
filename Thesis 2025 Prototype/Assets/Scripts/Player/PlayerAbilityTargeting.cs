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

    [Tooltip("The Color the line should become when player is using push ability")]
    [SerializeField] private Color pushLazerColor;
    
    [Tooltip("The Color the line should become when player is using pull ability")]
    [SerializeField] private Color pullLazerColor;
    
    [Tooltip("The Color the line should become on single target when the object is within min radius")]
    [SerializeField] private Color minLazerColor;

    /*[Tooltip("The line prefab to be used for AOE targets")]
    [SerializeField] private GameObject AOELazerPrefab;

    [Tooltip("The reference for the AOE lazer container object")]
    [SerializeField] private GameObject aoeLazerContainer;*/

    [Tooltip("The particle system prefab to be used for AOE Push")]
    [SerializeField] private GameObject PushParticlePrefab;

    [Tooltip("The particle system prefab to be used for AOE Pull")]
    [SerializeField] private GameObject PullParticlePrefab;

    private List<GameObject> aoeLazers = new List<GameObject>();
    private List<GameObject> aoeLazersActive = new List<GameObject>();

    private GameObject pushParticle = null;
    private GameObject pullParticle = null;
    private bool AOEStarted = false;

    private Camera cam;

    private float lineThickness = 1.0f;

    private Vector3 prevTargetPos;
    private bool isTooClose = false;

    private List<GameObject> viableTargets = new List<GameObject>();

    private PlayerAbilityBehaviour pab;

    void Start()
    {
        cam = Camera.main;
        pab = GetComponent<PlayerAbilityBehaviour>();

        //lr = GetComponent<LineRenderer>();
        SetLazerColorAccordingToAbility(pab.GetPlayerAbility());
        
        lr.enabled = false;

        //target = GameObject.FindGameObjectsWithTag("MovableObject")[0];
        //prevTargetPos = target.transform.position;
        //InstantiateAoeLazers();
        RenderLineOnTarget();
        StartCoroutine(CheckObjects(targetFindingDelay));
        InstantiateParticles();
    }

    void Update()
    {

        if (target != null)
        {
            float _dist = Vector3.Distance(transform.position, target.transform.position);
            if (_dist > targettingRadius)
            {
                OnTargetOutOfRange();
            }
            else if (_dist < minTargettingRadius)
            {
                OnTargetLessThanMinRange();
            }
            else
            {
                OnTargetIsInRange();
            }
        }

        if (target != null && (prevTargetPos - target.transform.position).sqrMagnitude < 0.01)
            return;

        RenderLineOnTarget();
        //AddLinesToAOETargets();
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
    
    private void OnTargetLessThanMinRange()
    {
        isTooClose = true;
    }

    private void OnTargetIsInRange()
    {
        isTooClose = false;
    }

    public void RenderLineOnTarget()
    {
        if (target == null)
        {
            lr.enabled = false;
            return;
        }

        lr.enabled = true;

        if (isTooClose) SetLazerMaterialBool(lr, 1);
        else SetLazerMaterialBool(lr, 0);

        GameObject _tar = target;
        MovableObjectTargetColorSwitch mov = target.GetComponent<MovableObjectTargetColorSwitch>();
        if (mov.IsSeperateIndicator()) _tar = mov.GetSeperateIndicator();

        var points = new Vector3[2];
        points[0] = transform.position;
        points[1] = _tar.transform.position;
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
        if (isTooClose) return null;
        return target;
    }
    
    public Vector2 GetTargettingRadius() 
    {
        return new Vector2(minTargettingRadius, targettingRadius);
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
                if (dist <= targettingRadius && viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1 && viewPos.z > 0)
                {
                    viableTargets.Add(movObj);
                }
            }

            viableTargets = SortByPosX(viableTargets);
        }
    }

    // De-commisioned
   /* private void AddLinesToAOETargets()
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

            LineRenderer _targetLR = _target.GetComponent<LineRenderer>();
            _targetLR.enabled = true;

            float dist = Vector3.Distance(transform.position, targetsMinusMainTarget[i].transform.position);
            if (dist <= minTargettingRadius) SetLazerMaterialBool(_targetLR, 1);
            else
            {
                aoeLazersActive.Add(_target);
                SetLazerMaterialBool(_targetLR, 0);
            }

            GameObject _tar = targetsMinusMainTarget[i];
            MovableObjectTargetColorSwitch mov = _tar.GetComponent<MovableObjectTargetColorSwitch>();
            if (mov.IsSeperateIndicator()) _tar = mov.GetSeperateIndicator();

            var points = new Vector3[2];
            points[0] = transform.position;
            points[1] = _tar.transform.position;
            _targetLR.SetPositions(points);

            _targetLR.startWidth = 0.2f;
            _targetLR.endWidth = lineThickness;
        }
        

    }*/

    public List<GameObject> GetAoeTargetsList()
    {
        return viableTargets;
    }

    // De-commisioned
    /*private void InstantiateAoeLazers()
    {
        for (int i = 0; i < 10;i++)
        {
            GameObject newLazer = Instantiate(AOELazerPrefab);
            newLazer.transform.parent = aoeLazerContainer.transform;
            newLazer.GetComponent<LineRenderer>().enabled = false;
            aoeLazers.Add(newLazer);
        }
    }*/

    private void InstantiateParticles()
    {
        pushParticle = Instantiate(PushParticlePrefab);
        pullParticle = Instantiate(PullParticlePrefab);

        pushParticle.transform.parent = transform;
        pullParticle.transform.parent = transform;
    }

    public void OnAOEStart(PlayerAbilityBehaviour.AbilityType _abilityType)
    {
        if (!AOEStarted && _abilityType == PlayerAbilityBehaviour.AbilityType.PULL)
        {
            pullParticle.GetComponent<ParticleSystem>().Play();
            AOEStarted = true;
        }
        else if (!AOEStarted && _abilityType == PlayerAbilityBehaviour.AbilityType.PUSH)
        {
            pushParticle.GetComponent<ParticleSystem>().Play();
            AOEStarted = true;
        }
    }

    public void OnAOEStop()
    {
        pullParticle.GetComponent<ParticleSystem>().Stop();
        pushParticle.GetComponent<ParticleSystem>().Stop();
        AOEStarted = false;
    }

    private List<GameObject> SortByPosX(List<GameObject> unsortedList)
    {
        List<GameObject> sortedList = new List<GameObject> ();
        sortedList = unsortedList.OrderBy(_object => _object.transform.position.x).ToList();
        return sortedList;
    }

    private void SetLazerMaterialBool(LineRenderer _lr, int _bool) //false = 0, true = 1
    {
        _lr.material.SetInt("_TriggerHighlight", _bool);
    }
    
    public void SetLazerColorAccordingToAbility(PlayerAbilityBehaviour.AbilityType _abilityType)
    {
        if (_abilityType == PlayerAbilityBehaviour.AbilityType.PULL) SetLazerMaterialColor(lr, pullLazerColor);
        else SetLazerMaterialColor(lr, pushLazerColor);
    }
    
    private void SetLazerMaterialColor(LineRenderer _lr, Color _color) //false = 0, true = 1
    {
        _lr.material.SetColor("_BaseColor", _color);
    }
}
