using UnityEngine;

public class PressurePlateController : MonoBehaviour
{
    private GameObject button;
    private Animation buttonAnim;

    [SerializeField] string TriggerTag = "MovableObject";
    [SerializeField] AutomaticTrigger TargetToTrigger;

    [SerializeField] bool toggleTrigger = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        button = this.gameObject.transform.GetChild(0).gameObject;
        buttonAnim = button.GetComponent<Animation>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == TriggerTag)
        {
            buttonAnim.Play("Button_Down");
            TargetToTrigger.TriggerFirstAnimation();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == TriggerTag && !toggleTrigger)
        {
            buttonAnim.Play("Button_Up");
            TargetToTrigger.TriggerSecondAnimation();
        }
    }
}
