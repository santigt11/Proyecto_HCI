using UnityEngine;
using UnityEngine.InputSystem;

public class InputTest : MonoBehaviour
{

    public InputActionProperty testAction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bool value = testAction.action.IsPressed();
        Debug.Log("Value: " + value);
    }
}
