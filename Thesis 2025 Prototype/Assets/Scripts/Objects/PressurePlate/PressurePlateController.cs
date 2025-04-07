using UnityEngine;

public class PressurePlateController : MonoBehaviour
{
    private GameObject button;
    private Animation buttonAnim;

    [SerializeField] string TriggerTag = "MovableObject";
    [SerializeField] AutomaticTrigger TargetToTrigger;

    [SerializeField] bool toggleTrigger = false;

    private bool hasBeenToggled = false;
    private bool canBePressed = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        button = this.gameObject.transform.GetChild(0).gameObject;
        buttonAnim = button.GetComponent<Animation>();
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == TriggerTag && canBePressed && !hasBeenToggled)
        {
            buttonAnim.Play("Button_Down");
            TargetToTrigger.TriggerButtonDown();
            hasBeenToggled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == TriggerTag && !toggleTrigger)
        {
            buttonAnim.Play("Button_Up");
            TargetToTrigger.TriggerButtonUp();
            hasBeenToggled = false;
        }
    }

    public void UnToggle()
    {
        buttonAnim.Play("Button_Up");
        hasBeenToggled = false;
    }

    public void Lock()
    {
        canBePressed = false;
    }

    public void Unlock()
    {
        canBePressed = true;
    }
}
