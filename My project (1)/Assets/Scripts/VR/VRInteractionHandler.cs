using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

/// <summary>
/// Maneja la interacción VR mediante raycast explícito verificando acciones de Input System
/// Compatible con XR Interaction Toolkit 3.0+
/// </summary>
public class VRInteractionHandler : MonoBehaviour
{
    [Header("Left Hand Refs")]
    [SerializeField] private XRRayInteractor leftRayInteractor;
    [SerializeField] private InputActionProperty leftSelectInput;

    [Header("Right Hand Refs")]
    [SerializeField] private XRRayInteractor rightRayInteractor;
    [SerializeField] private InputActionProperty rightSelectInput;

    [Header("Configuración")]
    [SerializeField] private LayerMask estimuloLayer;

    private void Start()
    {
        // Asegurarse de que el cursor esté libre en VR, o bloqueado si se desea
        // En VR generalmente no usamos Cursor.lockState, pero limpiamos config previa
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void Update()
    {
        // Verificar Mano Izquierda
        CheckHandInteraction(leftRayInteractor, leftSelectInput, "LeftHand");

        // Verificar Mano Derecha
        CheckHandInteraction(rightRayInteractor, rightSelectInput, "RightHand");
    }

    private bool leftWasPressed = false;
    private bool rightWasPressed = false;

    private void CheckHandInteraction(XRRayInteractor interactor, InputActionProperty inputAction, string handName)
    {
        // 1. Verificar si el interactor y la acción son válidos
        if (interactor == null || inputAction.action == null) return;

        // Determinar qué estado usar según la mano
        bool wasPressed = (handName == "LeftHand") ? leftWasPressed : rightWasPressed;

        // 2. Verificar si el botón está presionado
        bool isPressed = inputAction.action.IsPressed();

        // 3. Si está presionado, verificar si apunta a algo
        if (isPressed && !wasPressed)
        {
            // Actualizar estado
            if (handName == "LeftHand")
                leftWasPressed = true;
            else
                rightWasPressed = true;
            
            if (interactor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
            {
                // Verificar si es un estímulo
                Estimulo estimulo = hit.collider.GetComponent<Estimulo>();
                if (estimulo != null)
                {
                    Debug.Log($"[VRInteraction] {handName} interactuó con {estimulo.Tipo}");
                    estimulo.OnInteraccion();
                }
                else
                {
                    Debug.Log($"[VRInteraction] {handName} clickeó en {hit.collider.name} (no es un estímulo)");
                    // Registrar click al vacío (objeto que no es estímulo)
                    RegistrarClickVacio();
                }
            }
            else
            {
                Debug.Log($"[VRInteraction] {handName} clickeó al vacío");
                // Registrar click al vacío (sin hit)
                RegistrarClickVacio();
            }
        }
        else if (!isPressed)
        {
            // Resetear estado
            if (handName == "LeftHand")
                leftWasPressed = false;
            else
                rightWasPressed = false;
        }
    }

    private void RegistrarClickVacio()
    {
        // Buscar SesionVR y registrar el miss
        SesionVR sesion = FindFirstObjectByType<SesionVR>();
        if (sesion != null)
        {
            sesion.RegistrarClickVacio();
        }
    }
}
