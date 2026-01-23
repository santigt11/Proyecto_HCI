using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Maneja la interacción VR mediante raycast (mirada + clic)
/// Compatible con XR Interaction Toolkit y nuevo Input System
/// Soporta modo Desktop para pruebas sin VR
/// </summary>
public class VRInteractionHandler : MonoBehaviour
{
    [Header("Referencias XR")]
    [SerializeField] private UnityEngine.XR.Interaction.Toolkit.Interactors.XRRayInteractor rayInteractor;
    
    [Header("Configuración de Interacción")]
    [SerializeField] private LayerMask estimuloLayer;
    [SerializeField] private float maxDistanciaRaycast = 10f;
    
    [Header("Modo Desktop (para pruebas sin VR)")]
    [SerializeField] private bool usarModoDesktop = true;
    [SerializeField] private float sensibilidadRaton = 2f;
    
    private Vector2 rotacion = Vector2.zero;
    
    private void Start()
    {
        // Si estamos en modo desktop, configurar el cursor
        if (usarModoDesktop)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
    
    private void Update()
    {
        // Modo Desktop: Controlar cámara con ratón
        if (usarModoDesktop)
        {
            ControlarCamaraConRaton();
        }
        
        // Verificar input de interacción (compatible con nuevo Input System)
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            IntentarInteractuar();
        }
        
        // Permitir desbloquear cursor con ESC
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        
        // Bloquear cursor al hacer clic
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame && Cursor.lockState == CursorLockMode.None)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
    
    /// <summary>
    /// Controla la rotación de la cámara con el ratón (modo desktop)
    /// </summary>
    private void ControlarCamaraConRaton()
    {
        if (Mouse.current == null) return;
        
        Vector2 mouseDelta = Mouse.current.delta.ReadValue();
        
        rotacion.x += mouseDelta.x * sensibilidadRaton;
        rotacion.y -= mouseDelta.y * sensibilidadRaton;
        rotacion.y = Mathf.Clamp(rotacion.y, -90f, 90f);
        
        transform.rotation = Quaternion.Euler(rotacion.y, rotacion.x, 0f);
    }
    
    /// <summary>
    /// Intenta interactuar con un estímulo mediante raycast
    /// </summary>
    private void IntentarInteractuar()
    {
        // Opción 1: Usar XR Ray Interactor si está disponible
        if (rayInteractor != null && rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
        {
            ProcesarHit(hit);
            return;
        }
        
        // Opción 2: Raycast manual desde la cámara (fallback para desktop)
        Ray ray = new Ray(transform.position, transform.forward);
        
        if (Physics.Raycast(ray, out RaycastHit manualHit, maxDistanciaRaycast, estimuloLayer))
        {
            ProcesarHit(manualHit);
        }
        else
        {
            Debug.Log("[VRInteraction] No se detectó ningún estímulo");
        }
    }
    
    /// <summary>
    /// Procesa el hit del raycast y verifica si es un estímulo
    /// </summary>
    private void ProcesarHit(RaycastHit hit)
    {
        Estimulo estimulo = hit.collider.GetComponent<Estimulo>();
        
        if (estimulo != null)
        {
            Debug.Log($"[VRInteraction] Interacción con estímulo {estimulo.Tipo}");
            estimulo.OnInteraccion();
        }
    }
    
    #if UNITY_EDITOR
    /// <summary>
    /// Visualiza el raycast en el editor
    /// </summary>
    private void OnDrawGizmos()
    {
        if (Camera.main != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawRay(transform.position, transform.forward * maxDistanciaRaycast);
        }
    }
    #endif
}
