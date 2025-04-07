using UnityEngine;
using System.Collections;

public class DisappearingPlatform : MonoBehaviour
{
    [SerializeField] bool Disappears = false;

    [Tooltip("The delay until the platfome disappears")]
    [SerializeField] float platformDisappearingDuration = 2f;
    
    [Tooltip("The smoothness of the platforms transparancy")]
    [SerializeField] int rateOfDisappearing = 20;

    private bool disappearing = false;

    private void OnTriggerEnter(Collider collision)
    {
        if (!Disappears || disappearing || collision.isTrigger) { return; }

        string objTag = collision.gameObject.tag;

        if (objTag == "Player")
        {
            disappearing = true;
            StartCoroutine(StartDisappearing(platformDisappearingDuration));
        }
    }


    private IEnumerator StartDisappearing(float delay)
    {
        MeshRenderer rend = GetComponent<MeshRenderer>();

        Color colorStart = rend.material.color;
        Color colorEnd = new Color(colorStart.r, colorStart.g, colorStart.b, 0f);

        float timeStep = delay / (float) rateOfDisappearing;

        int counter = 1;
        float lerp = 0;
        while (counter < rateOfDisappearing)
        {
            yield return new WaitForSeconds(timeStep);

            lerp = counter / (float) rateOfDisappearing; 

            Color tmpColor = Color.Lerp(colorStart, colorEnd, lerp);
            Debug.Log(tmpColor);
            rend.material.color = tmpColor;
            
            counter++;
        }

        this.transform.parent.gameObject.SetActive(false);
    }
}
