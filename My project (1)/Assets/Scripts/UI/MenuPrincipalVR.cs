using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Controlador del menú principal VR
/// Maneja la navegación entre menú y sesión de juego
/// </summary>
public class MenuPrincipalVR : MonoBehaviour
{
    [Header("Referencias UI")]
    [SerializeField] private GameObject menuCanvas;
    [SerializeField] private GameObject canvasMetricas;
    
    [Header("Detección de Grips para Volver al Menú")]
    [SerializeField] private InputActionProperty gripIzquierdo;
    [SerializeField] private InputActionProperty gripDerecho;
    [SerializeField] private float tiempoPresionRequerido = 2.0f;
    
    private float tiempoPresionando = 0f;
    private bool menuActivo = true;
    
    private void Start()
    {
        // Mostrar menú al inicio
        MostrarMenu();
        
        // Ocultar canvas de métricas al inicio
        if (canvasMetricas != null)
        {
            canvasMetricas.SetActive(false);
        }
        
        Debug.Log("[MenuPrincipalVR] Menú inicializado");
    }
    
    private void Update()
    {
        // Solo detectar grips si el menú NO está activo (durante la sesión)
        if (!menuActivo)
        {
            DetectarCombinacionGrips();
        }
    }
    
    /// <summary>
    /// Detecta si ambos grips están presionados simultáneamente
    /// </summary>
    private void DetectarCombinacionGrips()
    {
        bool gripIzqPresionado = gripIzquierdo.action?.ReadValue<float>() > 0.5f;
        bool gripDerPresionado = gripDerecho.action?.ReadValue<float>() > 0.5f;
        
        if (gripIzqPresionado && gripDerPresionado)
        {
            tiempoPresionando += Time.deltaTime;
            
            if (tiempoPresionando >= tiempoPresionRequerido)
            {
                Debug.Log("[MenuPrincipalVR] Combinación de grips detectada - Volviendo al menú");
                VolverAlMenu();
                tiempoPresionando = 0f;
            }
        }
        else
        {
            tiempoPresionando = 0f;
        }
    }
    
    /// <summary>
    /// Inicia la sesión de entrenamiento (llamado desde el botón)
    /// </summary>
    public void IniciarSesion()
    {
        Debug.Log("[MenuPrincipalVR] Iniciando sesión...");
        
        // Ocultar menú
        if (menuCanvas != null)
        {
            menuCanvas.SetActive(false);
        }
        
        // Mostrar canvas de métricas
        if (canvasMetricas != null)
        {
            canvasMetricas.SetActive(true);
        }
        
        // Iniciar sesión VR
        if (SesionVR.Instance != null)
        {
            SesionVR.Instance.IniciarSesion();
        }
        else
        {
            Debug.LogError("[MenuPrincipalVR] No se encontró SesionVR.Instance");
        }
        
        menuActivo = false;
    }
    
    /// <summary>
    /// Vuelve al menú principal (detiene la sesión)
    /// </summary>
    public void VolverAlMenu()
    {
        Debug.Log("[MenuPrincipalVR] Volviendo al menú...");
        
        // Detener sesión (esto exportará el CSV automáticamente)
        if (SesionVR.Instance != null)
        {
            SesionVR.Instance.DetenerSesion();
        }
        
        // Ocultar canvas de métricas
        if (canvasMetricas != null)
        {
            canvasMetricas.SetActive(false);
        }
        
        // Mostrar menú
        MostrarMenu();
    }
    
    /// <summary>
    /// Muestra el menú principal
    /// </summary>
    private void MostrarMenu()
    {
        if (menuCanvas != null)
        {
            menuCanvas.SetActive(true);
        }
        
        menuActivo = true;
        Debug.Log("[MenuPrincipalVR] Menú mostrado");
    }
    
    /// <summary>
    /// Cierra la aplicación (llamado desde el botón)
    /// </summary>
    public void SalirAplicacion()
    {
        Debug.Log("[MenuPrincipalVR] Saliendo de la aplicación...");
        
        // Detener sesión si está activa
        if (SesionVR.Instance != null && !menuActivo)
        {
            SesionVR.Instance.DetenerSesion();
        }
        
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}
