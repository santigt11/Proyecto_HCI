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
    [SerializeField] private float alturaMinima = 11f;
    [SerializeField] private float alturaMaxima = 14f;
    
    [Header("Probabilidades")]
    [SerializeField] [Range(0f, 1f)] private float probabilidadBlanco = 0.65f;
    
    [Header("Audio")]
    [SerializeField] private AudioClip sonidoEstimuloBlanco;
    [SerializeField] private AudioClip sonidoEstimuloNegro;
    [Tooltip("Distancia mínima donde el sonido está a volumen máximo")]
    [SerializeField] private float minDistance = 1f;
    [Tooltip("Distancia máxima donde el sonido se escucha")]
    [SerializeField] private float maxDistance = 10f;
    
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
            // Obtener tiempo de vida actual del gestor de dificultad
            float vidaUtil = 4f;
            if (SesionVR.Instance != null && SesionVR.Instance.GetComponent<GestorDificultad>() != null)
            {
                vidaUtil = SesionVR.Instance.GetComponent<GestorDificultad>().ObtenerVidaUtilActual();
            }

            estimulo.Configurar(tipo, vidaUtil);
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
        
        // Agregar y configurar AudioSource 3D en el estímulo
        ConfigurarAudio3D(estimuloObj, tipo);
        
        Debug.Log($"[EstimuloManager] Generado estímulo {tipo} con vida útil {estimulo?.VidaUtil:F1}s en posición {randomPos}");
    }
    
    /// <summary>
    /// Configura un AudioSource 3D en el estímulo y reproduce el sonido correspondiente
    /// </summary>
    private void ConfigurarAudio3D(GameObject estimuloObj, TipoEstimulo tipo)
    {
        // Seleccionar el clip según el tipo
        AudioClip clip = (tipo == TipoEstimulo.Blanco) ? sonidoEstimuloBlanco : sonidoEstimuloNegro;
        
        if (clip == null) return; // No hay sonido asignado
        
        // Agregar AudioSource al estímulo
        AudioSource audioSource = estimuloObj.AddComponent<AudioSource>();
        
        // Configurar como audio 3D espacial
        audioSource.clip = clip;
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1f; // 1 = 3D completo, 0 = 2D
        audioSource.minDistance = minDistance;
        audioSource.maxDistance = maxDistance;
        audioSource.rolloffMode = AudioRolloffMode.Linear;
        audioSource.dopplerLevel = 0f; // Sin efecto Doppler para estímulos estáticos
        
        // Reproducir el sonido
        audioSource.Play();
    }
    
    /// <summary>
    /// Destruye todos los estímulos activos en la escena
    /// </summary>
    public void LimpiarEstimulos()
    {
        Estimulo[] estimulos = FindObjectsByType<Estimulo>(FindObjectsSortMode.None);
        foreach (Estimulo estimulo in estimulos)
        {
            Destroy(estimulo.gameObject);
        }
        Debug.Log($"[EstimuloManager] Limpiados {estimulos.Length} estímulos");
    }
}
