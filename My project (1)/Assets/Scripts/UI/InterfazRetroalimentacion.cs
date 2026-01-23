using UnityEngine;
using TMPro;

/// <summary>
/// Interfaz de retroalimentación visual para el usuario
/// Muestra resultados inmediatos y métricas acumuladas
/// </summary>
public class InterfazRetroalimentacion : MonoBehaviour
{
    [Header("Retroalimentación Inmediata")]
    [SerializeField] private GameObject feedbackCorrecto;
    [SerializeField] private GameObject feedbackIncorrecto;
    [SerializeField] private float tiempoMostrarFeedback = 0.5f;
    
    [Header("Métricas Acumuladas")]
    [SerializeField] private TextMeshProUGUI textoPrecision;
    [SerializeField] private TextMeshProUGUI textoErrores;
    [SerializeField] private TextMeshProUGUI textoTiempoPromedio;
    [SerializeField] private TextMeshProUGUI textoNivelAtencion;
    
    [Header("Colores de Nivel de Atención")]
    [SerializeField] private Color colorAlto = Color.green;
    [SerializeField] private Color colorMedio = Color.yellow;
    [SerializeField] private Color colorBajo = Color.red;
    
    /// <summary>
    /// Muestra retroalimentación inmediata sobre si la interacción fue correcta
    /// </summary>
    public void MostrarResultadoInmediato(bool fueCorrecta)
    {
        if (fueCorrecta)
        {
            if (feedbackCorrecto != null)
            {
                feedbackCorrecto.SetActive(true);
                Invoke(nameof(OcultarFeedbackCorrecto), tiempoMostrarFeedback);
            }
        }
        else
        {
            if (feedbackIncorrecto != null)
            {
                feedbackIncorrecto.SetActive(true);
                Invoke(nameof(OcultarFeedbackIncorrecto), tiempoMostrarFeedback);
            }
        }
    }
    
    private void OcultarFeedbackCorrecto()
    {
        if (feedbackCorrecto != null)
            feedbackCorrecto.SetActive(false);
    }
    
    private void OcultarFeedbackIncorrecto()
    {
        if (feedbackIncorrecto != null)
            feedbackIncorrecto.SetActive(false);
    }
    
    /// <summary>
    /// Actualiza las métricas acumuladas mostradas al usuario
    /// </summary>
    public void ActualizarMetricas(float precision, int errores, float tiempoPromedio, NivelAtencion nivel)
    {
        // Actualizar textos
        if (textoPrecision != null)
            textoPrecision.text = $"Precisión: {(precision * 100):F1}%";
        
        if (textoErrores != null)
            textoErrores.text = $"Errores: {errores}";
        
        if (textoTiempoPromedio != null)
            textoTiempoPromedio.text = $"Tiempo Promedio: {tiempoPromedio:F2}s";
        
        if (textoNivelAtencion != null)
        {
            textoNivelAtencion.text = $"Nivel de Atención: {nivel}";
            
            // Aplicar color según nivel
            Color color = nivel switch
            {
                NivelAtencion.Alto => colorAlto,
                NivelAtencion.Medio => colorMedio,
                NivelAtencion.Bajo => colorBajo,
                _ => Color.white
            };
            textoNivelAtencion.color = color;
        }
    }
    
    /// <summary>
    /// Oculta todos los elementos de feedback
    /// </summary>
    public void OcultarTodoFeedback()
    {
        if (feedbackCorrecto != null)
            feedbackCorrecto.SetActive(false);
        
        if (feedbackIncorrecto != null)
            feedbackIncorrecto.SetActive(false);
    }
}
