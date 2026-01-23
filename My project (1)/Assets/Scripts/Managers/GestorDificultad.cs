using UnityEngine;

/// <summary>
/// Gestiona la dificultad dinámica del sistema basada en el nivel de atención
/// </summary>
public class GestorDificultad : MonoBehaviour
{
    [Header("Parámetros de Dificultad")]
    public float velocidadEstimulo = 1f;
    public float intervaloGeneracion = 2f;
    public int maxEstimulosSimultaneos = 1;
    
    [Header("Configuración de Transición")]
    [SerializeField] private float suavidadTransicion = 0.3f;
    
    private float velocidadObjetivo;
    private float intervaloObjetivo;
    
    private void Start()
    {
        velocidadObjetivo = velocidadEstimulo;
        intervaloObjetivo = intervaloGeneracion;
    }
    
    /// <summary>
    /// Ajusta la dificultad basándose en el nivel de atención clasificado por la IA
    /// </summary>
    /// <param name="nivel">Nivel de atención actual del usuario</param>
    public void AjustarDificultad(NivelAtencion nivel)
    {
        switch (nivel)
        {
            case NivelAtencion.Bajo:
                // Dificultad reducida: más tiempo, menos estímulos
                velocidadObjetivo = 0.5f;
                intervaloObjetivo = 3f;
                maxEstimulosSimultaneos = 1;
                Debug.Log("[Dificultad] Ajustada a BAJA - Más tiempo para reaccionar");
                break;
                
            case NivelAtencion.Medio:
                // Dificultad balanceada
                velocidadObjetivo = 1f;
                intervaloObjetivo = 2f;
                maxEstimulosSimultaneos = 1;
                Debug.Log("[Dificultad] Ajustada a MEDIA - Parámetros balanceados");
                break;
                
            case NivelAtencion.Alto:
                // Dificultad aumentada: menos tiempo, más estímulos
                velocidadObjetivo = 1.5f;
                intervaloObjetivo = 1.5f;
                maxEstimulosSimultaneos = 2;
                Debug.Log("[Dificultad] Ajustada a ALTA - Mayor desafío");
                break;
        }
    }
    
    private void Update()
    {
        // Transición suave hacia los valores objetivo
        velocidadEstimulo = Mathf.Lerp(velocidadEstimulo, velocidadObjetivo, suavidadTransicion * Time.deltaTime);
        intervaloGeneracion = Mathf.Lerp(intervaloGeneracion, intervaloObjetivo, suavidadTransicion * Time.deltaTime);
    }
    
    /// <summary>
    /// Obtiene el intervalo actual de generación de estímulos
    /// </summary>
    public float ObtenerIntervaloActual()
    {
        return intervaloGeneracion;
    }
}
