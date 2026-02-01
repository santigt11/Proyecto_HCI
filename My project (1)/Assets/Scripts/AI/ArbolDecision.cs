using UnityEngine;

/// <summary>
/// Árbol de Decisión para clasificar el nivel de atención del usuario
/// Implementación con estructura real de árbol usando nodos
/// </summary>
public class ArbolDecision : MonoBehaviour
{
    [Header("Umbrales de Clasificación")]
    [SerializeField] private float umbralPrecisionAlta = 0.8f;
    [SerializeField] private float umbralPrecisionMedia = 0.5f;
    [SerializeField] private float umbralTiempoRapido = 1.4f;
    [SerializeField] private int umbralErroresTolerables = 3;
    
    // Nodo raíz del árbol
    private NodoDecision nodoRaiz;
    
    private void Awake()
    {
        ConstruirArbol();
    }
    
    /// <summary>
    /// Construye la estructura del árbol de decisión
    /// </summary>
    private void ConstruirArbol()
    {
        // Nodos hoja (resultados finales)
        NodoHoja nodoAlto = new NodoHoja(NivelAtencion.Alto);
        NodoHoja nodoMedio1 = new NodoHoja(NivelAtencion.Medio);
        NodoHoja nodoMedio2 = new NodoHoja(NivelAtencion.Medio);
        NodoHoja nodoBajo1 = new NodoHoja(NivelAtencion.Bajo);
        NodoHoja nodoBajo2 = new NodoHoja(NivelAtencion.Bajo);
        
        // Nivel 3: Nodo de errores (para precisión media)
        NodoDecision nodoErrores = new NodoDecision(
            metrica => metrica.numeroErrores <= umbralErroresTolerables,
            nodoMedio2,  // <= 3 errores -> Medio
            nodoBajo1    // > 3 errores -> Bajo
        );
        
        // Nivel 2: Nodo de tiempo de reacción (para precisión alta)
        NodoDecision nodoTiempo = new NodoDecision(
            metrica => metrica.tiempoReaccionPromedio <= umbralTiempoRapido,
            nodoAlto,    // Tiempo rápido -> Alto
            nodoMedio1   // Tiempo lento -> Medio
        );
        
        // Nivel 2: Nodo de precisión media
        NodoDecision nodoPrecisionMedia = new NodoDecision(
            metrica => metrica.precision >= umbralPrecisionMedia,
            nodoErrores, // Precisión media -> evaluar errores
            nodoBajo2    // Precisión baja -> Bajo
        );
        
        // Nivel 1: Nodo raíz - Precisión alta
        nodoRaiz = new NodoDecision(
            metrica => metrica.precision >= umbralPrecisionAlta,
            nodoTiempo,          // Precisión alta -> evaluar tiempo
            nodoPrecisionMedia   // Precisión no alta -> evaluar precisión media
        );
        
        Debug.Log("[ArbolDecision] Árbol construido con estructura de nodos");
    }
    
    /// <summary>
    /// Clasifica el nivel de atención basado en las métricas del usuario
    /// Recorre el árbol desde la raíz hasta una hoja
    /// </summary>
    public NivelAtencion ClasificarAtencion(float precision, float tiempoReaccionPromedio, int numeroErrores)
    {
        MetricasClasificacion metricas = new MetricasClasificacion
        {
            precision = precision,
            tiempoReaccionPromedio = tiempoReaccionPromedio,
            numeroErrores = numeroErrores
        };
        
        NivelAtencion resultado = nodoRaiz.Evaluar(metricas);
        
        Debug.Log($"[ArbolDecision] Clasificación: {resultado} (P:{precision:F2}, T:{tiempoReaccionPromedio:F2}s, E:{numeroErrores})");
        
        return resultado;
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

/// <summary>
/// Estructura para pasar métricas entre nodos
/// </summary>
public struct MetricasClasificacion
{
    public float precision;
    public float tiempoReaccionPromedio;
    public int numeroErrores;
}

/// <summary>
/// Clase base abstracta para nodos del árbol
/// </summary>
public abstract class NodoArbol
{
    public abstract NivelAtencion Evaluar(MetricasClasificacion metricas);
}

/// <summary>
/// Nodo de decisión (nodo interno del árbol)
/// Evalúa una condición y delega a sus hijos
/// </summary>
public class NodoDecision : NodoArbol
{
    private System.Func<MetricasClasificacion, bool> condicion;
    private NodoArbol hijoVerdadero;
    private NodoArbol hijoFalso;
    
    public NodoDecision(System.Func<MetricasClasificacion, bool> condicion, NodoArbol hijoVerdadero, NodoArbol hijoFalso)
    {
        this.condicion = condicion;
        this.hijoVerdadero = hijoVerdadero;
        this.hijoFalso = hijoFalso;
    }
    
    public override NivelAtencion Evaluar(MetricasClasificacion metricas)
    {
        if (condicion(metricas))
        {
            return hijoVerdadero.Evaluar(metricas);
        }
        else
        {
            return hijoFalso.Evaluar(metricas);
        }
    }
}

/// <summary>
/// Nodo hoja (nodo terminal del árbol)
/// Retorna un resultado final
/// </summary>
public class NodoHoja : NodoArbol
{
    private NivelAtencion resultado;
    
    public NodoHoja(NivelAtencion resultado)
    {
        this.resultado = resultado;
    }
    
    public override NivelAtencion Evaluar(MetricasClasificacion metricas)
    {
        return resultado;
    }
}
