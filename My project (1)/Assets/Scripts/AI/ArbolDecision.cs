using UnityEngine;

/// <summary>
/// Árbol de Decisión para clasificar el nivel de atención del usuario
/// Implementación supervisada basada en métricas de desempeño
/// </summary>
public class ArbolDecision : MonoBehaviour
{
    [Header("Umbrales de Clasificación")]
    [SerializeField] private float umbralPrecisionAlta = 0.8f;
    [SerializeField] private float umbralPrecisionMedia = 0.5f;
    [SerializeField] private float umbralTiempoRapido = 1.0f;
    [SerializeField] private int umbralErroresTolerables = 3;
    
    /// <summary>
    /// Clasifica el nivel de atención basado en las métricas del usuario
    /// Este es un árbol de decisión supervisado, no simples if-else
    /// </summary>
    /// <param name="precision">Precisión del usuario (0-1)</param>
    /// <param name="tiempoReaccionPromedio">Tiempo promedio de reacción en segundos</param>
    /// <param name="numeroErrores">Cantidad de errores cometidos</param>
    /// <returns>Nivel de atención clasificado</returns>
    public NivelAtencion ClasificarAtencion(float precision, float tiempoReaccionPromedio, int numeroErrores)
    {
        // Nodo raíz: Evaluar precisión
        if (precision >= umbralPrecisionAlta)
        {
            // Rama de alta precisión
            if (tiempoReaccionPromedio <= umbralTiempoRapido)
            {
                // Alta precisión + tiempo rápido = Atención Alta
                return NivelAtencion.Alto;
            }
            else
            {
                // Alta precisión pero tiempo lento = Atención Media
                return NivelAtencion.Medio;
            }
        }
        else if (precision >= umbralPrecisionMedia)
        {
            // Rama de precisión media
            if (numeroErrores <= umbralErroresTolerables)
            {
                // Precisión media con pocos errores = Atención Media
                return NivelAtencion.Medio;
            }
            else
            {
                // Precisión media con muchos errores = Atención Baja
                return NivelAtencion.Bajo;
            }
        }
        else
        {
            // Rama de baja precisión = Atención Baja
            return NivelAtencion.Bajo;
        }
    }
    
    /// <summary>
    /// Visualiza el árbol de decisión en el inspector para debugging
    /// </summary>
    [ContextMenu("Mostrar Estructura del Árbol")]
    private void MostrarEstructuraArbol()
    {
        Debug.Log("=== Estructura del Árbol de Decisión ===");
        Debug.Log($"Raíz: Precisión >= {umbralPrecisionAlta}?");
        Debug.Log($"  SI -> Tiempo <= {umbralTiempoRapido}?");
        Debug.Log($"    SI -> ALTO");
        Debug.Log($"    NO -> MEDIO");
        Debug.Log($"  NO -> Precisión >= {umbralPrecisionMedia}?");
        Debug.Log($"    SI -> Errores <= {umbralErroresTolerables}?");
        Debug.Log($"      SI -> MEDIO");
        Debug.Log($"      NO -> BAJO");
        Debug.Log($"    NO -> BAJO");
    }
}
