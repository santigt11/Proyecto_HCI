using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controlador del menú principal VR
/// Permite iniciar el juego y ajustar configuraciones
/// </summary>
public class MenuPrincipalVR : MonoBehaviour
{
    [Header("Referencias UI")]
    [SerializeField] private GameObject panelMenuPrincipal;
    [SerializeField] private GameObject panelConfiguracion;
    [SerializeField] private Button botonIniciarJuego;
    [SerializeField] private Button botonConfiguracion;
    [SerializeField] private Button botonVolverConfig;
    
    [Header("Configuración de Sensibilidad")]
    [SerializeField] private Slider sliderSensibilidad;
    [SerializeField] private Text textoSensibilidad;
    [SerializeField] private float sensibilidadMinima = 0.5f;
    [SerializeField] private float sensibilidadMaxima = 3f;
    [SerializeField] private float sensibilidadDefault = 1f;
    
    [Header("Referencias de Juego")]
    [SerializeField] private SesionVR sesionVR;
    [SerializeField] private GameObject canvasJuego;
    
    private float sensibilidadActual;
    
    private void Start()
    {
        // Configurar botones
        if (botonIniciarJuego != null)
            botonIniciarJuego.onClick.AddListener(IniciarJuego);
        
        if (botonConfiguracion != null)
            botonConfiguracion.onClick.AddListener(AbrirConfiguracion);
        
        if (botonVolverConfig != null)
            botonVolverConfig.onClick.AddListener(VolverAlMenuPrincipal);
        
        // Configurar slider de sensibilidad
        if (sliderSensibilidad != null)
        {
            sliderSensibilidad.minValue = sensibilidadMinima;
            sliderSensibilidad.maxValue = sensibilidadMaxima;
            sliderSensibilidad.value = sensibilidadDefault;
            sliderSensibilidad.onValueChanged.AddListener(OnSensibilidadCambiada);
        }
        
        // Cargar sensibilidad guardada
        sensibilidadActual = PlayerPrefs.GetFloat("Sensibilidad", sensibilidadDefault);
        if (sliderSensibilidad != null)
            sliderSensibilidad.value = sensibilidadActual;
        
        ActualizarTextoSensibilidad();
        
        // Mostrar menú principal al inicio
        MostrarMenuPrincipal();
    }
    
    /// <summary>
    /// Inicia el juego y oculta el menú
    /// </summary>
    public void IniciarJuego()
    {
        Debug.Log("[MenuPrincipal] Iniciando juego...");
        
        // Ocultar menú
        if (panelMenuPrincipal != null)
            panelMenuPrincipal.SetActive(false);
        
        if (panelConfiguracion != null)
            panelConfiguracion.SetActive(false);
        
        // Mostrar UI del juego
        if (canvasJuego != null)
            canvasJuego.SetActive(true);
        
        // Iniciar sesión VR
        if (sesionVR != null)
        {
            sesionVR.IniciarSesion();
        }
        else
        {
            Debug.LogError("[MenuPrincipal] SesionVR no asignada!");
        }
    }
    
    /// <summary>
    /// Abre el panel de configuración
    /// </summary>
    public void AbrirConfiguracion()
    {
        Debug.Log("[MenuPrincipal] Abriendo configuración...");
        
        if (panelMenuPrincipal != null)
            panelMenuPrincipal.SetActive(false);
        
        if (panelConfiguracion != null)
            panelConfiguracion.SetActive(true);
    }
    
    /// <summary>
    /// Vuelve al menú principal desde configuración
    /// </summary>
    public void VolverAlMenuPrincipal()
    {
        Debug.Log("[MenuPrincipal] Volviendo al menú principal...");
        
        if (panelConfiguracion != null)
            panelConfiguracion.SetActive(false);
        
        if (panelMenuPrincipal != null)
            panelMenuPrincipal.SetActive(true);
    }
    
    /// <summary>
    /// Muestra el menú principal (útil para volver desde el juego)
    /// </summary>
    public void MostrarMenuPrincipal()
    {
        if (panelMenuPrincipal != null)
            panelMenuPrincipal.SetActive(true);
        
        if (panelConfiguracion != null)
            panelConfiguracion.SetActive(false);
        
        if (canvasJuego != null)
            canvasJuego.SetActive(false);
    }
    
    /// <summary>
    /// Callback cuando cambia el slider de sensibilidad
    /// </summary>
    private void OnSensibilidadCambiada(float valor)
    {
        sensibilidadActual = valor;
        ActualizarTextoSensibilidad();
        
        // Guardar en PlayerPrefs
        PlayerPrefs.SetFloat("Sensibilidad", sensibilidadActual);
        PlayerPrefs.Save();
        
        Debug.Log($"[MenuPrincipal] Sensibilidad ajustada a: {sensibilidadActual:F2}");
    }
    
    /// <summary>
    /// Actualiza el texto que muestra el valor de sensibilidad
    /// </summary>
    private void ActualizarTextoSensibilidad()
    {
        if (textoSensibilidad != null)
        {
            textoSensibilidad.text = $"Sensibilidad: {sensibilidadActual:F2}x";
        }
    }
    
    /// <summary>
    /// Obtiene la sensibilidad actual configurada
    /// </summary>
    public float ObtenerSensibilidad()
    {
        return sensibilidadActual;
    }
}
