using UnityEngine;

/// <summary>
/// Dibuja un crosshair simple en el centro de la pantalla
/// </summary>
public class Crosshair : MonoBehaviour
{
    [Header("Configuración del Crosshair")]
    [SerializeField] private Color color = Color.white;
    [SerializeField] private float tamaño = 10f;
    [SerializeField] private float grosor = 2f;
    
    private void OnGUI()
    {
        // Calcular centro de la pantalla
        float centerX = Screen.width / 2f;
        float centerY = Screen.height / 2f;
        
        // Configurar color
        GUI.color = color;
        
        // Dibujar línea horizontal
        GUI.DrawTexture(
            new Rect(centerX - tamaño, centerY - grosor / 2f, tamaño * 2, grosor),
            Texture2D.whiteTexture
        );
        
        // Dibujar línea vertical
        GUI.DrawTexture(
            new Rect(centerX - grosor / 2f, centerY - tamaño, grosor, tamaño * 2),
            Texture2D.whiteTexture
        );
        
        // Punto central
        GUI.DrawTexture(
            new Rect(centerX - 2, centerY - 2, 4, 4),
            Texture2D.whiteTexture
        );
    }
}
