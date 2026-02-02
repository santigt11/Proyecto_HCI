using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Controlador principal de la sesión VR
/// Coordina el flujo completo: estímulos → interacción → métricas → IA → dificultad
/// </summary>
public class SesionVR : MonoBehaviour
{
    public static SesionVR Instance { get; private set; }
    
    [Header("Referencias de Componentes")]
    [SerializeField] private EstimuloManager estimuloManager;
    [SerializeField] private ArbolDecision arbolDecision;
    [SerializeField] private GestorDificultad gestorDificultad;
    [SerializeField] private InterfazRetroalimentacion interfazRetroalimentacion;
    [SerializeField] private ExportadorDatosCSV exportadorCSV;
    
    [Header("Configuración de Sesión")]
    [SerializeField] private bool iniciarAutomaticamente = true;
    [SerializeField] private float retardoEntreEstimulos = 1f;
    
    private Usuario usuario;
    private List<Metrica> metricasSesion;
    private bool sesionActiva = false;
    
    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    
    private void OnDestroy()
    {
        // Exportar datos automáticamente al salir del Play Mode
        if (sesionActiva && metricasSesion.Count > 0)
        {
            Debug.Log("[SesionVR] Saliendo del Play Mode - Exportando datos automáticamente...");
            DetenerSesion();
        }
    }
    
    private void Start()
    {
        // Inicializar usuario y métricas
        usuario = new Usuario("Usuario01");
        metricasSesion = new List<Metrica>();
        
        // Validar referencias
        if (!ValidarReferencias())
        {
            Debug.LogError("[SesionVR] Faltan referencias críticas. Revisa el Inspector.");
            return;
        }
        
        if (iniciarAutomaticamente)
        {
            IniciarSesion();
        }
    }
    
    /// <summary>
    /// Valida que todas las referencias necesarias estén asignadas
    /// </summary>
    private bool ValidarReferencias()
    {
        bool valido = true;
        
        if (estimuloManager == null)
        {
            Debug.LogError("[SesionVR] EstimuloManager no asignado!");
            valido = false;
        }
        
        if (arbolDecision == null)
        {
            Debug.LogError("[SesionVR] ArbolDecision no asignado!");
            valido = false;
        }
        
        if (gestorDificultad == null)
        {
            Debug.LogError("[SesionVR] GestorDificultad no asignado!");
            valido = false;
        }
        
        if (interfazRetroalimentacion == null)
        {
            Debug.LogError("[SesionVR] InterfazRetroalimentacion no asignada!");
            valido = false;
        }
        
        return valido;
    }
    
    /// <summary>
    /// Inicia la sesión VR
    /// </summary>
    public void IniciarSesion()
    {
        Debug.Log("[SesionVR] Iniciando sesión de entrenamiento de atención");
        sesionActiva = true;
        GenerarSiguienteEstimulo();
    }
    
    /// <summary>
    /// Detiene la sesión VR
    /// </summary>
    public void DetenerSesion()
    {
        Debug.Log("[SesionVR] Deteniendo sesión");
        sesionActiva = false;
        CancelInvoke(nameof(GenerarSiguienteEstimulo));
        estimuloManager.LimpiarEstimulos();
        
        // Exportar datos de la sesión a CSV
        if (exportadorCSV != null && metricasSesion.Count > 0)
        {
            exportadorCSV.ExportarSesion(usuario, metricasSesion, usuario.NivelAtencionActual);
            Debug.Log($"[SesionVR] Sesión exportada: {metricasSesion.Count} métricas");
        }
    }
    
    /// <summary>
    /// Se llama cuando un estímulo expira (desaparece sin ser clickeado)
    /// </summary>
    public void RegistrarExpiracion(TipoEstimulo tipo)
    {
        if (!sesionActiva) return;

        bool fueCorrecta;
        float tiempoReaccion;

        if (tipo == TipoEstimulo.Negro)
        {
            // Si era negro y NO se clickeó, es CORRECTO
            fueCorrecta = true;
            tiempoReaccion = 0f; // No contar tiempo para estímulos negros
            Debug.Log("[SesionVR] Estímulo NEGRO expiró correctamente (No interacción)");
        }
        else
        {
            // Si era blanco y NO se clickeó, es INCORRECTO (Miss)
            fueCorrecta = false;
            tiempoReaccion = gestorDificultad.ObtenerVidaUtilActual(); // Tiempo máximo como penalización
            Debug.Log("[SesionVR] Estímulo BLANCO expiró incorrectamente (Miss)");
        }

        ProcesarResultado(tiempoReaccion, fueCorrecta);
    }

    /// <summary>
    /// Registra una interacción del usuario con un estímulo
    /// </summary>
    public void RegistrarInteraccion(float tiempoReaccion, bool fueCorrecta)
    {
        if (!sesionActiva) return;
        
        Debug.Log($"[SesionVR] Interacción registrada - Tiempo: {tiempoReaccion:F2}s, Correcta: {fueCorrecta}");
        ProcesarResultado(tiempoReaccion, fueCorrecta);
    }

    /// <summary>
    /// Registra un click al vacío (no apuntaba a ningún estímulo)
    /// </summary>
    public void RegistrarClickVacio()
    {
        if (!sesionActiva) return;
        
        Debug.Log("[SesionVR] Click al vacío registrado como error");
        // Registrar como error con tiempo de reacción 0 (no hubo estímulo)
        ProcesarResultado(0f, false);
    }

    /// <summary>
    /// Método central para procesar resultados de interacciones o expiraciones
    /// </summary>
    private void ProcesarResultado(float tiempoReaccion, bool fueCorrecta)
    {
        // 1. Crear y almacenar métrica
        Metrica metrica = new Metrica(tiempoReaccion, fueCorrecta);
        metricasSesion.Add(metrica);
        usuario.AgregarMetrica(metrica);
        
        // 2. Mostrar retroalimentación inmediata
        interfazRetroalimentacion.MostrarResultadoInmediato(fueCorrecta);
        
        // 3. Calcular métricas agregadas (para mostrar en UI)
        float precision = CalcularPrecision();
        float tiempoPromedio = CalcularPromedioTiempoReaccion();
        int numeroErrores = CalcularNumeroErrores();
        
        // 4. Calcular métricas recientes (para IA - últimas 5 interacciones)
        float tiempoPromedioReciente = CalcularPromedioTiempoReaccionReciente(5);
        
        // 5. Clasificar nivel de atención con IA usando métricas recientes
        NivelAtencion nivelClasificado = arbolDecision.ClasificarAtencion(
            precision,
            tiempoPromedioReciente, // Usar tiempo reciente en lugar del promedio total
            numeroErrores
        );
        
        usuario.NivelAtencionActual = nivelClasificado;
        
        // 6. Mostrar métricas acumuladas (UI muestra el promedio total)
        interfazRetroalimentacion.ActualizarMetricas(
            precision,
            numeroErrores,
            tiempoPromedio, // UI muestra el promedio total histórico
            nivelClasificado
        );
        
        // 7. Ajustar dificultad basándose en el nivel de atención
        gestorDificultad.AjustarDificultad(nivelClasificado);
        
        // 8. Generar siguiente estímulo después de un retardo
        float intervalo = gestorDificultad.ObtenerIntervaloActual() + retardoEntreEstimulos;
        
        // Cancelar cualquier invocación previa para evitar múltiples estímulos
        CancelInvoke(nameof(GenerarSiguienteEstimulo));
        Invoke(nameof(GenerarSiguienteEstimulo), intervalo);
    }
    
    /// <summary>
    /// Genera el siguiente estímulo
    /// </summary>
    private void GenerarSiguienteEstimulo()
    {
        if (!sesionActiva) return;
        
        estimuloManager.GenerarEstimulo();
    }
    
    #region Cálculo de Métricas
    
    /// <summary>
    /// Calcula la precisión del usuario (aciertos / total)
    /// </summary>
    private float CalcularPrecision()
    {
        if (metricasSesion.Count == 0) return 0f;
        
        int aciertos = metricasSesion.Count(m => m.FueCorrecta);
        return (float)aciertos / metricasSesion.Count;
    }
    
    /// <summary>
    /// Calcula el promedio de tiempo de reacción (solo respuestas correctas)
    /// </summary>
    private float CalcularPromedioTiempoReaccion()
    {
        // Filtrar solo las métricas correctas con tiempo > 0 (excluir estímulos negros)
        var metricasCorrectas = metricasSesion
            .Where(m => m.FueCorrecta && m.TiempoReaccion > 0f)
            .ToList();
        
        if (metricasCorrectas.Count == 0) return 0f;
        
        return metricasCorrectas.Average(m => m.TiempoReaccion);
    }
    
    /// <summary>
    /// Calcula el promedio de tiempo de reacción de las últimas N interacciones correctas
    /// Usado para la IA para tener una ventana deslizante de rendimiento reciente
    /// </summary>
    private float CalcularPromedioTiempoReaccionReciente(int cantidadUltimas)
    {
        // Filtrar solo las métricas correctas con tiempo > 0 (excluir estímulos negros)
        var metricasCorrectas = metricasSesion
            .Where(m => m.FueCorrecta && m.TiempoReaccion > 0f)
            .ToList();
        
        if (metricasCorrectas.Count == 0) return 0f;
        
        // Tomar las últimas N métricas correctas
        var metricasRecientes = metricasCorrectas
            .Skip(Mathf.Max(0, metricasCorrectas.Count - cantidadUltimas))
            .ToList();
        
        if (metricasRecientes.Count == 0) return 0f;
        
        float promedio = metricasRecientes.Average(m => m.TiempoReaccion);
        
        Debug.Log($"[SesionVR] Tiempo promedio reciente ({metricasRecientes.Count} últimas): {promedio:F2}s");
        
        return promedio;
    }
    
    /// <summary>
    /// Calcula el número total de errores
    /// </summary>
    private int CalcularNumeroErrores()
    {
        return metricasSesion.Count(m => !m.FueCorrecta);
    }
    
    #endregion
    
    #region Métodos Públicos de Información
    
    /// <summary>
    /// Obtiene el usuario actual
    /// </summary>
    public Usuario ObtenerUsuario()
    {
        return usuario;
    }
    
    /// <summary>
    /// Obtiene todas las métricas de la sesión
    /// </summary>
    public List<Metrica> ObtenerMetricasSesion()
    {
        return new List<Metrica>(metricasSesion);
    }
    
    /// <summary>
    /// Verifica si la sesión está activa
    /// </summary>
    public bool EstaSesionActiva()
    {
        return sesionActiva;
    }
    
    #endregion
}
