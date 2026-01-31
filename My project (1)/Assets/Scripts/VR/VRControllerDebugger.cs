using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class VRControllerDebugger : MonoBehaviour
{
    // Opcional: Asigna esto en el inspector para ver los valores crudos
    public InputActionProperty positionAction;
    public InputActionProperty rotationAction;

    private Vector3 lastPosition;
    private Quaternion lastRotation;
    private TrackedPoseDriver poseDriver;

    void Start()
    {
        poseDriver = GetComponent<TrackedPoseDriver>();
        if (poseDriver == null)
        {
            Debug.LogWarning($"[Debugger] No TrackedPoseDriver found on {gameObject.name}. Movement might depend on another component.");
        }
        else
        {
             Debug.Log($"[Debugger] Found TrackedPoseDriver on {gameObject.name}");
        }

        lastPosition = transform.position;
        lastRotation = transform.rotation;
        
        Debug.Log($"[Debugger] Starting debug for {gameObject.name}. Initial Pos: {lastPosition}");
    }

    void Update()
    {
        // 1. Check Transform changes (Result of movement)
        bool moved = false;
        if (Vector3.Distance(transform.position, lastPosition) > 0.001f)
        {
            Debug.Log($"[Debugger] {gameObject.name} MOVED to {transform.position}");
            lastPosition = transform.position;
            moved = true;
        }

        if (Quaternion.Angle(transform.rotation, lastRotation) > 0.1f)
        {
            lastRotation = transform.rotation;
            moved = true;
        }

        // 2. Check Input Asset Values (Source of movement)
        // Read directly from the action if assigned
        if (positionAction.action != null)
        {
             Vector3 posInput = positionAction.action.ReadValue<Vector3>();
             
             if (!moved && posInput.magnitude > 0)
             {
                 // Input exists but object didn't move
                // Debug.LogWarning($"[Debugger] {gameObject.name} has INPUT {posInput} but Transform IS NOT MOVING!");
             }
        }
    }
}
