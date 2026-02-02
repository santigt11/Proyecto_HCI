using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Componente para botones VR con efectos visuales de hover y click
/// </summary>
[RequireComponent(typeof(Button))]
[RequireComponent(typeof(Image))]
public class BotonVRInteractivo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [Header("Colores")]
    [SerializeField] private Color colorNormal = new Color(0.2f, 0.2f, 0.2f, 0.9f);
    [SerializeField] private Color colorHover = new Color(0.3f, 0.3f, 0.3f, 1f);
    [SerializeField] private Color colorPresionado = new Color(0.1f, 0.1f, 0.1f, 1f);
    
    [Header("Animación")]
    [SerializeField] private float velocidadTransicion = 10f;
    [SerializeField] private float escalaHover = 1.1f;
    [SerializeField] private float escalaPresionado = 0.95f;
    
    [Header("Audio (Opcional)")]
    [SerializeField] private AudioClip sonidoHover;
    [SerializeField] private AudioClip sonidoClick;
    
    private Image imagen;
    private Vector3 escalaOriginal;
    private Color colorObjetivo;
    private float escalaObjetivo = 1f;
    private AudioSource audioSource;
    
    private void Awake()
    {
        imagen = GetComponent<Image>();
        escalaOriginal = transform.localScale;
        colorObjetivo = colorNormal;
        
        // Configurar AudioSource si hay sonidos
        if (sonidoHover != null || sonidoClick != null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.spatialBlend = 0f; // 2D sound
        }
        
        // Establecer color inicial
        if (imagen != null)
        {
            imagen.color = colorNormal;
        }
    }
    
    private void Update()
    {
        // Animar color
        if (imagen != null)
        {
            imagen.color = Color.Lerp(imagen.color, colorObjetivo, Time.deltaTime * velocidadTransicion);
        }
        
        // Animar escala
        Vector3 escalaDeseada = escalaOriginal * escalaObjetivo;
        transform.localScale = Vector3.Lerp(transform.localScale, escalaDeseada, Time.deltaTime * velocidadTransicion);
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        colorObjetivo = colorHover;
        escalaObjetivo = escalaHover;
        
        // Reproducir sonido de hover
        if (audioSource != null && sonidoHover != null)
        {
            audioSource.PlayOneShot(sonidoHover);
        }
        
        Debug.Log($"[BotonVR] Hover en: {gameObject.name}");
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        colorObjetivo = colorNormal;
        escalaObjetivo = 1f;
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        colorObjetivo = colorPresionado;
        escalaObjetivo = escalaPresionado;
        
        // Reproducir sonido de click
        if (audioSource != null && sonidoClick != null)
        {
            audioSource.PlayOneShot(sonidoClick);
        }
        
        Debug.Log($"[BotonVR] Click en: {gameObject.name}");
    }
    
    public void OnPointerUp(PointerEventData eventData)
    {
        colorObjetivo = colorHover;
        escalaObjetivo = escalaHover;
    }
}
