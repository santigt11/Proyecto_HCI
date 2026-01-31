using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

/// <summary>
/// Componente que hace que un estímulo sea interactuable con XR Ray Interactors
/// Compatible con XR Interaction Toolkit 3.0+
/// </summary>
[RequireComponent(typeof(Collider))]
public class EstimuloInteractable : XRSimpleInteractable
{
    private Estimulo estimulo;

    protected override void Awake()
    {
        base.Awake();
        estimulo = GetComponent<Estimulo>();
        
        if (estimulo == null)
        {
            Debug.LogError("[EstimuloInteractable] No se encontró componente Estimulo en este GameObject");
        }

        // Asegurar que el collider no sea trigger para que funcione con raycast
        Collider col = GetComponent<Collider>();
        if (col != null && col.isTrigger)
        {
            Debug.LogWarning("[EstimuloInteractable] El Collider es trigger, cambiando a no-trigger para raycast");
            col.isTrigger = false;
        }
    }

    /// <summary>
    /// Se llama cuando el XRRayInteractor selecciona este objeto
    /// </summary>
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        
        if (estimulo != null)
        {
            Debug.Log($"[EstimuloInteractable] Seleccionado por {args.interactorObject}");
            estimulo.OnInteraccion();
        }
    }

    /// <summary>
    /// Se llama cuando el XRRayInteractor hace hover sobre este objeto
    /// </summary>
    protected override void OnHoverEntered(HoverEnterEventArgs args)
    {
        base.OnHoverEntered(args);
        // Opcional: Añadir feedback visual cuando el rayo apunta al estímulo
    }

    /// <summary>
    /// Se llama cuando el XRRayInteractor deja de hacer hover
    /// </summary>
    protected override void OnHoverExited(HoverExitEventArgs args)
    {
        base.OnHoverExited(args);
    }
}
