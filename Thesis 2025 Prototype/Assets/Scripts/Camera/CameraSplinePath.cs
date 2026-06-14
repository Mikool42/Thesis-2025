using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class CameraSplinePath : MonoBehaviour
{
    public SplineContainer container;

    [SerializeField]
    float speed = 0.01f;

    [SerializeField] GameObject lookAtTarget;
    [SerializeField] private CameraTakeoverController ctc;

    SplinePath cameraTrack;

    float prevFrac = 0;

    private bool Running = false;

    private bool transitionTo = true;

    void Start()
    {
        cameraTrack = new SplinePath(new[]
        {
                new SplineSlice<Spline>(container.Splines[0], new SplineRange(0, 6),
                    container.transform.localToWorldMatrix)
            });
    }

    void Update()
    {
        if (!Running) return; 

        if (transitionTo)
        {
            TransitionToSpline();
        }
        else
        {
            TransitionFromSpline();
        }
    }

    private void TransitionToSpline()
    {
        float currFrac = math.frac(speed * Time.time);

        if (currFrac < 0.98)
        {
            cameraTrack.Evaluate(currFrac, out var pos, out var right, out var up);
            //Vector3 forward = Vector3.Cross(right, up);
            transform.position = pos;
            transform.LookAt(lookAtTarget.transform.position);
        }
        else
        {
            cameraTrack.Evaluate(1f, out var pos, out var right, out var up);
            //Vector3 forward = Vector3.Cross(right, up);
            transform.position = pos;
            transform.LookAt(lookAtTarget.transform.position);

            Running = false;

            Debug.Log("not running ");
        }
    }

    private void TransitionFromSpline()
    {
        float currFrac = math.frac(speed * Time.time);

        if (currFrac < 0.98)
        {
            cameraTrack.Evaluate(0.9f - currFrac, out var pos, out var right, out var up);
            //Vector3 forward = Vector3.Cross(right, up);
            transform.position = pos;
            transform.LookAt(lookAtTarget.transform.position);
        }
        else
        {
            cameraTrack.Evaluate(0.1f, out var pos, out var right, out var up);
            //Vector3 forward = Vector3.Cross(right, up);
            transform.position = pos;
            transform.LookAt(lookAtTarget.transform.position);

            Running = false;
         
            ctc.StopTakeover();
        }
    }

    public void StartTransitionToSpline()
    {
        Running = true;
        transitionTo = true;
    }
    
    public void StartTransitionFromSpline()
    {
        Running = true;
        transitionTo = false;
    }
}

