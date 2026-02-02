using UnityEngine;

/// <summary>
/// Script de diagnóstico para verificar la configuración del sistema
/// Agrega este componente al GameManager y revisa la Console
/// </summary>
public class DiagnosticoSistema : MonoBehaviour
{
    [ContextMenu("Verificar Configuración")]
    public void VerificarConfiguracion()
    {
        Debug.Log("=== DIAGNÓSTICO DEL SISTEMA ===");
        
        // Verificar SesionVR
        SesionVR sesionVR = GetComponent<SesionVR>();
        if (sesionVR != null)
        {
            Debug.Log("✅ SesionVR encontrado");
            
            // Verificar referencias usando reflection
            var tipo = typeof(SesionVR);
            var campos = tipo.GetFields(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            
            foreach (var campo in campos)
            {
                if (campo.FieldType.IsSubclassOf(typeof(MonoBehaviour)) || campo.FieldType == typeof(MonoBehaviour))
                {
                    var valor = campo.GetValue(sesionVR);
                    if (valor == null)
                    {
                        Debug.LogWarning($"⚠️ Campo '{campo.Name}' no está asignado en SesionVR");
                    }
                    else
                    {
                        Debug.Log($"✅ Campo '{campo.Name}' asignado correctamente");
                    }
                }
            }
        }
        else
        {
            Debug.LogError("❌ SesionVR NO encontrado. Agrega el componente SesionVR al GameManager");
        }
        
        // Verificar ExportadorDatosCSV
        ExportadorDatosCSV exportador = GetComponent<ExportadorDatosCSV>();
        if (exportador != null)
        {
            Debug.Log("✅ ExportadorDatosCSV encontrado");
        }
        else
        {
            Debug.LogWarning("⚠️ ExportadorDatosCSV NO encontrado. Agrega el componente al GameManager");
        }
        
        // Verificar ArbolDecision
        ArbolDecision arbol = GetComponent<ArbolDecision>();
        if (arbol != null)
        {
            Debug.Log("✅ ArbolDecision encontrado");
        }
        else
        {
            Debug.LogWarning("⚠️ ArbolDecision NO encontrado. Puede estar en otro GameObject");
        }
        
        // Verificar otros componentes en la escena
        Debug.Log("\n=== COMPONENTES EN LA ESCENA ===");
        
        EstimuloManager estimuloMgr = FindObjectOfType<EstimuloManager>();
        Debug.Log(estimuloMgr != null ? 
            $"✅ EstimuloManager encontrado en: {estimuloMgr.gameObject.name}" : 
            "❌ EstimuloManager NO encontrado en la escena");
        
        GestorDificultad gestorDif = FindObjectOfType<GestorDificultad>();
        Debug.Log(gestorDif != null ? 
            $"✅ GestorDificultad encontrado en: {gestorDif.gameObject.name}" : 
            "❌ GestorDificultad NO encontrado en la escena");
        
        InterfazRetroalimentacion interfaz = FindObjectOfType<InterfazRetroalimentacion>();
        Debug.Log(interfaz != null ? 
            $"✅ InterfazRetroalimentacion encontrado en: {interfaz.gameObject.name}" : 
            "❌ InterfazRetroalimentacion NO encontrado en la escena");
        
        Debug.Log("\n=== FIN DEL DIAGNÓSTICO ===");
    }
    
    private void Start()
    {
        // Ejecutar diagnóstico automáticamente al iniciar
        Invoke(nameof(VerificarConfiguracion), 0.5f);
    }
}
