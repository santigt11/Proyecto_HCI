using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

/// <summary>
/// Gestiona la retroalimentación háptica para los controladores VR
/// Singleton para acceso global desde cualquier script
/// </summary>
public class HapticFeedbackManager : MonoBehaviour
{
    public static HapticFeedbackManager Instance { get; private set; }

    [Header("Referencias XR")]
    [SerializeField] private XRRayInteractor leftHandRayInteractor;
    [SerializeField] private XRRayInteractor rightHandRayInteractor;

    [Header("Debug")]
    [SerializeField] private bool mostrarLogsDebug = true;

    private void Awake()
    {
        // Implementar patrón Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Envía retroalimentación háptica a los controladores VR
    /// </summary>
    /// <param name="amplitud">Intensidad de la vibración (0.0 a 1.0)</param>
    /// <param name="duracion">Duración de la vibración en segundos</param>
    public void SendHapticFeedback(float amplitud, float duracion)
    {
        if (mostrarLogsDebug)
        {
            Debug.Log($"[HapticFeedback] Enviando vibración - Amplitud: {amplitud}, Duración: {duracion}s");
        }

        // Intentar enviar a ambos controladores (el que esté activo responderá)
        bool feedbackEnviado = false;

        if (leftHandRayInteractor != null)
        {
            feedbackEnviado |= SendHapticImpulse(leftHandRayInteractor, amplitud, duracion);
        }

        if (rightHandRayInteractor != null)
        {
            feedbackEnviado |= SendHapticImpulse(rightHandRayInteractor, amplitud, duracion);
        }

        if (!feedbackEnviado && mostrarLogsDebug)
        {
            Debug.LogWarning("[HapticFeedback] No se pudo enviar retroalimentación háptica. Verifica que los XRRayInteractor estén asignados.");
        }
    }

    /// <summary>
    /// Envía un impulso háptico a un interactor específico
    /// </summary>
    private bool SendHapticImpulse(XRRayInteractor rayInteractor, float amplitud, float duracion)
    {
        if (rayInteractor == null) return false;

        // En XR Interaction Toolkit 3.0+, XRRayInteractor hereda de XRBaseControllerInteractor
        // que tiene el método SendHapticImpulse directamente
        if (rayInteractor is IXRInteractor interactor)
        {
            rayInteractor.SendHapticImpulse(amplitud, duracion);
            
            if (mostrarLogsDebug)
            {
                Debug.Log($"[HapticFeedback] Impulso enviado a {rayInteractor.name}");
            }
            
            return true;
        }

        return false;
    }

    /// <summary>
    /// Método de conveniencia para vibración de confirmación (estímulo correcto)
    /// </summary>
    public void VibracionCorrecta()
    {
        SendHapticFeedback(0.3f, 0.1f);
    }

    /// <summary>
    /// Método de conveniencia para vibración de error (estímulo incorrecto)
    /// </summary>
    public void VibracionIncorrecta()
    {
        SendHapticFeedback(0.8f, 0.18f);
    }

    #if UNITY_EDITOR
    private void OnValidate()
    {
        // Buscar automáticamente los ray interactors si no están asignados
        if (leftHandRayInteractor == null || rightHandRayInteractor == null)
        {
            var rayInteractors = FindObjectsByType<XRRayInteractor>(FindObjectsSortMode.None);
            
            foreach (var interactor in rayInteractors)
            {
                if (interactor.name.ToLower().Contains("left") && leftHandRayInteractor == null)
                {
                    leftHandRayInteractor = interactor;
                }
                else if (interactor.name.ToLower().Contains("right") && rightHandRayInteractor == null)
                {
                    rightHandRayInteractor = interactor;
                }
            }
        }
    }
    #endif
}
