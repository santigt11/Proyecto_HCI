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
    
    private void Start()
    {
        TiempoAparicion = Time.time;
    }
    
    /// <summary>
    /// Se llama cuando el usuario interactúa con este estímulo
    /// </summary>
    public void OnInteraccion()
    {
        float tiempoReaccion = Time.time - TiempoAparicion;
        bool esCorrecta = (Tipo == TipoEstimulo.Blanco);
        
        // Registrar la interacción en la sesión
        SesionVR.Instance.RegistrarInteraccion(tiempoReaccion, esCorrecta);
        
        // Destruir el estímulo
        Destroy(gameObject);
    }
}
