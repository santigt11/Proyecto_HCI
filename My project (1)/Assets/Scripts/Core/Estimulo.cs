using UnityEngine;

/// <summary>
/// Tipos de estímulos en el sistema
/// </summary>
public enum TipoEstimulo
{
    Blanco, // Usuario DEBE interactuar (correcto)
    Negro   // Usuario NO debe interactuar (incorrecto si interactúa)
}

/// <summary>
/// Representa un estímulo visual (círculo blanco o negro)
/// </summary>
public class Estimulo : MonoBehaviour
{
    public TipoEstimulo Tipo { get; set; }
    public float TiempoAparicion { get; private set; }
    public float VidaUtil { get; private set; } = 5f; // Valor por defecto
    
    private bool interactuado = false;

    /// <summary>
    /// Configura el estímulo con su tipo y tiempo de vida
    /// </summary>
    public void Configurar(TipoEstimulo tipo, float vidaUtil)
    {
        Tipo = tipo;
        VidaUtil = vidaUtil;
    }

    private void Start()
    {
        TiempoAparicion = Time.time;
    }

    private void Update()
    {
        if (interactuado) return;

        // Verificar si el estímulo ha expirado
        if (Time.time - TiempoAparicion > VidaUtil)
        {
            Expirar();
        }
    }

    /// <summary>
    /// Se llama cuando el estímulo desaparece sin interacción
    /// </summary>
    private void Expirar()
    {
        interactuado = true;
        
        // Notificar a la sesión que el estímulo expiró
        if (SesionVR.Instance != null)
        {
            SesionVR.Instance.RegistrarExpiracion(Tipo);
        }
        
        Destroy(gameObject);
    }

    /// <summary>
    /// Se llama cuando el usuario interactúa con este estímulo
    /// </summary>
    public void OnInteraccion()
    {
        if (interactuado) return;
        interactuado = true;

        float tiempoReaccion = Time.time - TiempoAparicion;
        bool esCorrecta = (Tipo == TipoEstimulo.Blanco);
        
        // Solo registrar tiempo de reacción para estímulos blancos
        // Los estímulos negros no requieren velocidad, solo evitarlos
        if (!esCorrecta)
        {
            tiempoReaccion = 0f; // No contar tiempo para estímulos negros
        }
        
        // Activar retroalimentación háptica según el tipo de estímulo
        if (HapticFeedbackManager.Instance != null)
        {
            if (esCorrecta)
            {
                // Vibración leve para respuesta correcta (blanco)
                HapticFeedbackManager.Instance.VibracionCorrecta();
            }
            else
            {
                // Vibración intensa para respuesta incorrecta (negro)
                HapticFeedbackManager.Instance.VibracionIncorrecta();
            }
        }
        
        // Registrar la interacción en la sesión
        SesionVR.Instance.RegistrarInteraccion(tiempoReaccion, esCorrecta);
        
        // Destruir el estímulo
        Destroy(gameObject);
    }

}
