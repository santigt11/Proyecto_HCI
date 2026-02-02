using UnityEngine;

/// <summary>
/// Script de prueba para verificar la exportación de CSV
/// Agrega este componente temporalmente para probar la exportación
/// </summary>
public class PruebaExportacionCSV : MonoBehaviour
{
    private void Update()
    {
        // Presiona F9 para detener la sesión y exportar datos
        if (Input.GetKeyDown(KeyCode.F9))
        {
            Debug.Log("[PruebaExportacion] Deteniendo sesión y exportando datos...");
            
            if (SesionVR.Instance != null)
            {
                SesionVR.Instance.DetenerSesion();
                Debug.Log("[PruebaExportacion] ✅ Sesión detenida. Verifica la carpeta DatosExportados/");
            }
            else
            {
                Debug.LogError("[PruebaExportacion] ❌ No se encontró SesionVR.Instance");
            }
        }
        
        // Presiona F10 para iniciar una nueva sesión
        if (Input.GetKeyDown(KeyCode.F10))
        {
            Debug.Log("[PruebaExportacion] Iniciando nueva sesión...");
            
            if (SesionVR.Instance != null)
            {
                SesionVR.Instance.IniciarSesion();
                Debug.Log("[PruebaExportacion] ✅ Sesión iniciada");
            }
            else
            {
                Debug.LogError("[PruebaExportacion] ❌ No se encontró SesionVR.Instance");
            }
        }
    }
}
