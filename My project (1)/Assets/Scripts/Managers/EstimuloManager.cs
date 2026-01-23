using UnityEngine;

/// <summary>
/// Gestiona la generación de estímulos en el entorno VR
/// </summary>
public class EstimuloManager : MonoBehaviour
{
    [Header("Referencias de Prefabs")]
    [SerializeField] private GameObject estimuloBlancoPrefab;
    [SerializeField] private GameObject estimuloNegroPrefab;
    
    [Header("Configuración de Spawn")]
    [SerializeField] private Transform spawnArea;
    [SerializeField] private float spawnRadius = 3f;
    [SerializeField] private float alturaMinima = 1f;
    [SerializeField] private float alturaMaxima = 3f;
    
    [Header("Probabilidades")]
    [SerializeField] [Range(0f, 1f)] private float probabilidadBlanco = 0.5f;
    
    /// <summary>
    /// Genera un nuevo estímulo en una posición aleatoria
    /// </summary>
    public void GenerarEstimulo()
    {
        // Determinar tipo de estímulo
        TipoEstimulo tipo = (Random.value <= probabilidadBlanco) ? TipoEstimulo.Blanco : TipoEstimulo.Negro;
        
        // Generar posición aleatoria
        Vector3 randomPos = spawnArea.position + Random.insideUnitSphere * spawnRadius;
        randomPos.y = Mathf.Clamp(randomPos.y, alturaMinima, alturaMaxima);
        
        // Seleccionar prefab según tipo
        GameObject prefab = (tipo == TipoEstimulo.Blanco) ? estimuloBlancoPrefab : estimuloNegroPrefab;
        
        if (prefab == null)
        {
            Debug.LogError($"[EstimuloManager] Prefab para estímulo {tipo} no asignado!");
            return;
        }
        
        // Instanciar estímulo
        GameObject estimuloObj = Instantiate(prefab, randomPos, Quaternion.identity);
        
        // Configurar componente Estimulo
        Estimulo estimulo = estimuloObj.GetComponent<Estimulo>();
        if (estimulo != null)
        {
            estimulo.Tipo = tipo;
        }
        else
        {
            Debug.LogError("[EstimuloManager] El prefab no tiene componente Estimulo!");
        }
        
        // Hacer que el estímulo mire hacia la cámara
        if (Camera.main != null)
        {
            estimuloObj.transform.LookAt(Camera.main.transform);
        }
        
        Debug.Log($"[EstimuloManager] Generado estímulo {tipo} en posición {randomPos}");
    }
    
    /// <summary>
    /// Destruye todos los estímulos activos en la escena
    /// </summary>
    public void LimpiarEstimulos()
    {
        Estimulo[] estimulos = FindObjectsOfType<Estimulo>();
        foreach (Estimulo estimulo in estimulos)
        {
            Destroy(estimulo.gameObject);
        }
        Debug.Log($"[EstimuloManager] Limpiados {estimulos.Length} estímulos");
    }
}
