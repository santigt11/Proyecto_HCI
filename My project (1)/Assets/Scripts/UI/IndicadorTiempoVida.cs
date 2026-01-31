using UnityEngine;

/// <summary>
/// Indicador visual que muestra el tiempo de vida restante del estímulo
/// Usa un círculo que se escala conforme pasa el tiempo
/// </summary>
public class IndicadorTiempoVida : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private Color colorIndicador = new Color(0.7f, 0.7f, 0.7f, 0.6f);
    [SerializeField] private float tamañoInicial = 0.5f;
    
    private Estimulo estimulo;
    private SpriteRenderer spriteRenderer;
    private GameObject circulo;
    
    private void Start()
    {
        // Obtener referencia al estímulo padre
        estimulo = GetComponentInParent<Estimulo>();
        if (estimulo == null)
        {
            Debug.LogError("[IndicadorTiempoVida] No se encontró componente Estimulo en el padre");
            enabled = false;
            return;
        }
        
        CrearCirculoVisual();
    }
    
    private void CrearCirculoVisual()
    {
        // Crear un círculo usando un sprite
        circulo = new GameObject("CirculoIndicador");
        circulo.transform.SetParent(transform, false);
        circulo.transform.localPosition = Vector3.zero;
        
        // Agregar SpriteRenderer
        spriteRenderer = circulo.AddComponent<SpriteRenderer>();
        
        // Crear textura circular
        Texture2D textura = CrearTexturaCirculo(128);
        Sprite sprite = Sprite.Create(
            textura,
            new Rect(0, 0, textura.width, textura.height),
            new Vector2(0.5f, 0.5f),
            100f
        );
        
        spriteRenderer.sprite = sprite;
        spriteRenderer.color = colorIndicador;
        
        // Configurar escala inicial
        circulo.transform.localScale = Vector3.one * tamañoInicial;
        
        // Hacer que el sprite esté detrás del estímulo
        spriteRenderer.sortingOrder = -1;
    }
    
    private Texture2D CrearTexturaCirculo(int resolucion)
    {
        Texture2D textura = new Texture2D(resolucion, resolucion);
        Color[] pixeles = new Color[resolucion * resolucion];
        
        float centro = resolucion / 2f;
        float radio = resolucion / 2f - 2f; // Dejar un pequeño borde
        
        for (int y = 0; y < resolucion; y++)
        {
            for (int x = 0; x < resolucion; x++)
            {
                float distancia = Vector2.Distance(new Vector2(x, y), new Vector2(centro, centro));
                
                // Crear un anillo (círculo hueco)
                if (distancia <= radio && distancia >= radio - 4f)
                {
                    pixeles[y * resolucion + x] = Color.white;
                }
                else
                {
                    pixeles[y * resolucion + x] = Color.clear;
                }
            }
        }
        
        textura.SetPixels(pixeles);
        textura.Apply();
        textura.filterMode = FilterMode.Bilinear;
        
        return textura;
    }
    
    private void Update()
    {
        if (estimulo == null || circulo == null) return;
        
        // Hacer que el indicador siempre mire a la cámara
        if (Camera.main != null)
        {
            circulo.transform.rotation = Camera.main.transform.rotation;
        }
        
        // Calcular progreso (0 = inicio, 1 = expirado)
        float tiempoTranscurrido = Time.time - estimulo.TiempoAparicion;
        float progreso = Mathf.Clamp01(tiempoTranscurrido / estimulo.VidaUtil);
        
        // Escalar el círculo (de 100% a 0%)
        float escalaActual = (1f - progreso) * tamañoInicial;
        circulo.transform.localScale = Vector3.one * escalaActual;
        
        // Opcional: Hacer más transparente conforme se acaba el tiempo
        Color color = colorIndicador;
        color.a = Mathf.Lerp(0.6f, 0.2f, progreso);
        spriteRenderer.color = color;
    }
    
    private void OnDestroy()
    {
        if (circulo != null)
        {
            Destroy(circulo);
        }
    }
}
