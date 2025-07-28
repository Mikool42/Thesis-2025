using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class CameraTakeoverController : MonoBehaviour
{

    //[SerializeField] private Camera _camera;
    [SerializeField] private CinemachineCamera playerGroupCamera;
    [SerializeField] private CinemachineCamera cinemachineCamera;
    [SerializeField] private CameraSplinePath splinePath;
    [SerializeField] private SplitScreenManager splitScreenManager;

    [SerializeField] private Transform targetTransform;

    [SerializeField] private float transitionTime = 5.0f;

    [SerializeField] private Collider boxColliderOne;
    [SerializeField] private Collider boxColliderTwo;

    private Transform _targetTransform;

    private GameObject playerOneEntered = null;
    private GameObject playerTwoEntered = null;

    private bool splineHasBeenTriggered = false;
    private bool splineHasBeenExited = false;

    private List<GameObject> colliderList = new List<GameObject>();

    //private void Awake()
    //{
    //    _targetTransform = targetTransform;
    //}

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cinemachineCamera.gameObject.SetActive(false);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Player") return;
        
        colliderList.Add(other.gameObject);

        if (colliderList.Count >= 2 && !splineHasBeenTriggered)
        {

            cinemachineCamera.gameObject.SetActive(true);
            splitScreenManager.Takeover(); //Makes sure theres only one screen and no split screen

            splinePath.StartTransitionToSpline();

            splineHasBeenTriggered = true;
            splineHasBeenExited = false;

            
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag != "Player") return;

        colliderList.Remove(other.gameObject);

        if (colliderList.Count == 0)
        {


            splinePath.StartTransitionFromSpline();
                
            splineHasBeenExited = true;
            splineHasBeenTriggered = false;

        }
    }

    public void StopTakeover()
    {
        cinemachineCamera.gameObject.SetActive(false);
        splitScreenManager.ReleaseTakeover(); //Releases control over split screen manager
    }

    private IEnumerator TransitionToTakeover(Transform targetTransform, Transform startTransform)
    {
        float _timer = transitionTime;

        float posXDiff = startTransform.position.x - targetTransform.position.x;
        float posYDiff = startTransform.position.y - targetTransform.position.y;
        float posZDiff = startTransform.position.z - targetTransform.position.z;

        float rotXDiff = startTransform.rotation.x - targetTransform.rotation.x;
        float rotYDiff = startTransform.rotation.y - targetTransform.rotation.y;
        float rotZDiff = startTransform.rotation.z - targetTransform.rotation.z;

        float prevPercentComplete = 0f;

        cinemachineCamera.transform.position = startTransform.position;
        cinemachineCamera.transform.rotation = startTransform.rotation;

        while (0f < _timer)
        {
            _timer -= Time.deltaTime;

            float percentComplete = _timer / transitionTime;

            //Rotate(float xAngle, float yAngle, float zAngle, Space relativeTo = Space.Self)
            //Translate(float x, float y, float z, Space relativeTo = Space.Self);

            float increment = percentComplete - prevPercentComplete;

            cinemachineCamera.transform.Rotate(increment * rotXDiff, increment * rotYDiff, increment * rotZDiff);
            cinemachineCamera.transform.Translate(increment * posXDiff, increment * posYDiff, increment * posZDiff);

            prevPercentComplete = percentComplete;

            yield return new WaitForFixedUpdate();
        }
    }
}
