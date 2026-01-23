using UnityEngine;


/// <summary>
/// Maneja la interacción VR mediante raycast (mirada + clic)
/// Compatible con XR Interaction Toolkit
/// </summary>
public class VRInteractionHandler : MonoBehaviour
{
    [Header("Referencias XR")]
    [SerializeField] private UnityEngine.XR.Interaction.Toolkit.Interactors.XRRayInteractor rayInteractor;
    
    [Header("Configuración de Interacción")]
    [SerializeField] private LayerMask estimuloLayer;
    [SerializeField] private float maxDistanciaRaycast = 10f;
    
    [Header("Input")]
    [SerializeField] private string inputActionName = "Fire1";
    
    private void Update()
    {
        // Verificar input de interacción
        if (Input.GetButtonDown(inputActionName) || Input.GetMouseButtonDown(0))
        {
            IntentarInteractuar();
        }
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
        
        // Opción 2: Raycast manual desde la cámara (fallback)
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        
        if (Physics.Raycast(ray, out RaycastHit manualHit, maxDistanciaRaycast, estimuloLayer))
        {
            ProcesarHit(manualHit);
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
            Gizmos.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * maxDistanciaRaycast);
        }
    }
    #endif
}
