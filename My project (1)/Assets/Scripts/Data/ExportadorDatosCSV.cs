using UnityEngine;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Exporta datos de sesiones VR a formato CSV para análisis
/// Permite optimizar umbrales del árbol de decisión basándose en datos reales
/// </summary>
public class ExportadorDatosCSV : MonoBehaviour
{
    [Header("Configuración de Exportación")]
    [SerializeField] private string nombreArchivo = "datos_sesiones_vr.csv";
    [SerializeField] private bool exportarAutomaticamente = true;
    [SerializeField] private bool incluirMetricasIndividuales = true;
    
    private string rutaArchivo;
    private bool archivoCreado = false;
    
    private void Awake()
    {
        // Crear carpeta "DatosExportados" en el proyecto si no existe
        string carpetaDatos = Path.Combine(Application.dataPath, "..", "DatosExportados");
        if (!Directory.Exists(carpetaDatos))
        {
            Directory.CreateDirectory(carpetaDatos);
        }
        
        // Crear ruta del archivo en la carpeta del proyecto
        rutaArchivo = Path.Combine(carpetaDatos, nombreArchivo);
        Debug.Log($"[ExportadorCSV] Ruta de exportación: {rutaArchivo}");
    }
    
    /// <summary>
    /// Exporta los datos de una sesión completa al CSV
    /// </summary>
    public void ExportarSesion(Usuario usuario, List<Metrica> metricas, NivelAtencion nivelFinal)
    {
        if (!archivoCreado)
        {
            CrearArchivoCSV();
        }
        
        // Calcular estadísticas de la sesión
        var metricasCorrectas = metricas.Where(m => m.FueCorrecta && m.TiempoReaccion > 0f).ToList();
        var metricasIncorrectas = metricas.Where(m => !m.FueCorrecta).ToList();
        
        float precision = metricas.Count > 0 ? (float)metricas.Count(m => m.FueCorrecta) / metricas.Count : 0f;
        float tiempoPromedio = metricasCorrectas.Count > 0 ? metricasCorrectas.Average(m => m.TiempoReaccion) : 0f;
        int totalErrores = metricasIncorrectas.Count;
        int totalAciertos = metricasCorrectas.Count;
        int totalInteracciones = metricas.Count;
        
        // Calcular estadísticas adicionales
        float tiempoMinimo = metricasCorrectas.Count > 0 ? metricasCorrectas.Min(m => m.TiempoReaccion) : 0f;
        float tiempoMaximo = metricasCorrectas.Count > 0 ? metricasCorrectas.Max(m => m.TiempoReaccion) : 0f;
        float desviacionEstandar = CalcularDesviacionEstandar(metricasCorrectas);
        
        // Crear línea CSV con datos de la sesión
        StringBuilder linea = new StringBuilder();
        linea.Append($"{System.DateTime.Now:yyyy-MM-dd HH:mm:ss},");
        linea.Append($"{usuario.Id},");
        linea.Append($"{precision:F4},");
        linea.Append($"{tiempoPromedio:F4},");
        linea.Append($"{totalErrores},");
        linea.Append($"{totalAciertos},");
        linea.Append($"{totalInteracciones},");
        linea.Append($"{tiempoMinimo:F4},");
        linea.Append($"{tiempoMaximo:F4},");
        linea.Append($"{desviacionEstandar:F4},");
        linea.Append($"{nivelFinal}");
        
        // Escribir al archivo
        try
        {
            File.AppendAllText(rutaArchivo, linea.ToString() + "\n");
            Debug.Log($"[ExportadorCSV] Sesión exportada: {usuario.Id} - Nivel: {nivelFinal}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[ExportadorCSV] Error al exportar: {e.Message}");
        }
        
        // Exportar métricas individuales si está habilitado
        if (incluirMetricasIndividuales)
        {
            ExportarMetricasIndividuales(usuario.Id, metricas);
        }
    }
    
    /// <summary>
    /// Crea el archivo CSV con los encabezados
    /// </summary>
    private void CrearArchivoCSV()
    {
        StringBuilder encabezados = new StringBuilder();
        encabezados.Append("Fecha,");
        encabezados.Append("Usuario,");
        encabezados.Append("Precision,");
        encabezados.Append("TiempoPromedioReaccion,");
        encabezados.Append("TotalErrores,");
        encabezados.Append("TotalAciertos,");
        encabezados.Append("TotalInteracciones,");
        encabezados.Append("TiempoMinimo,");
        encabezados.Append("TiempoMaximo,");
        encabezados.Append("DesviacionEstandar,");
        encabezados.Append("NivelAtencionFinal");
        
        try
        {
            File.WriteAllText(rutaArchivo, encabezados.ToString() + "\n");
            archivoCreado = true;
            Debug.Log($"[ExportadorCSV] Archivo CSV creado: {rutaArchivo}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[ExportadorCSV] Error al crear archivo: {e.Message}");
        }
    }
    
    /// <summary>
    /// Exporta métricas individuales a un archivo separado
    /// </summary>
    private void ExportarMetricasIndividuales(string nombreUsuario, List<Metrica> metricas)
    {
        string carpetaDatos = Path.Combine(Application.dataPath, "..", "DatosExportados");
        string archivoMetricas = Path.Combine(
            carpetaDatos, 
            $"metricas_detalladas_{nombreUsuario}_{System.DateTime.Now:yyyyMMdd_HHmmss}.csv"
        );
        
        StringBuilder contenido = new StringBuilder();
        contenido.AppendLine("NumeroInteraccion,TiempoReaccion,FueCorrecta,Timestamp");
        
        for (int i = 0; i < metricas.Count; i++)
        {
            Metrica m = metricas[i];
            contenido.AppendLine($"{i + 1},{m.TiempoReaccion:F4},{m.FueCorrecta},{m.Timestamp:yyyy-MM-dd HH:mm:ss}");
        }
        
        try
        {
            File.WriteAllText(archivoMetricas, contenido.ToString());
            Debug.Log($"[ExportadorCSV] Métricas detalladas exportadas: {archivoMetricas}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[ExportadorCSV] Error al exportar métricas detalladas: {e.Message}");
        }
    }
    
    /// <summary>
    /// Calcula la desviación estándar de los tiempos de reacción
    /// </summary>
    private float CalcularDesviacionEstandar(List<Metrica> metricas)
    {
        if (metricas.Count == 0) return 0f;
        
        float promedio = metricas.Average(m => m.TiempoReaccion);
        float sumaCuadrados = metricas.Sum(m => Mathf.Pow(m.TiempoReaccion - promedio, 2));
        return Mathf.Sqrt(sumaCuadrados / metricas.Count);
    }
    
    /// <summary>
    /// Abre la carpeta donde se guardan los archivos CSV
    /// </summary>
    [ContextMenu("Abrir Carpeta de Datos")]
    public void AbrirCarpetaDatos()
    {
        string carpetaDatos = Path.Combine(Application.dataPath, "..", "DatosExportados");
        Application.OpenURL(carpetaDatos);
        Debug.Log($"[ExportadorCSV] Abriendo carpeta: {carpetaDatos}");
    }
    
    /// <summary>
    /// Muestra la ruta del archivo CSV en la consola
    /// </summary>
    [ContextMenu("Mostrar Ruta del Archivo")]
    public void MostrarRutaArchivo()
    {
        Debug.Log($"[ExportadorCSV] Ruta completa: {rutaArchivo}");
    }
    
    /// <summary>
    /// Reinicia el archivo CSV (útil para empezar una nueva recolección de datos)
    /// </summary>
    [ContextMenu("Reiniciar Archivo CSV")]
    public void ReiniciarArchivo()
    {
        archivoCreado = false;
        if (File.Exists(rutaArchivo))
        {
            File.Delete(rutaArchivo);
            Debug.Log("[ExportadorCSV] Archivo CSV eliminado. Se creará uno nuevo en la próxima exportación.");
        }
    }
}
