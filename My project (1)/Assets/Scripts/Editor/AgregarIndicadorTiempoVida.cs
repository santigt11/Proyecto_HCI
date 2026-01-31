using UnityEngine;
using UnityEditor;

/// <summary>
/// Script ejecutable para agregar el indicador de tiempo de vida a los prefabs
/// Ejecuta este script desde: Assets → Create → Execute Script
/// </summary>
public class EjecutarAgregarIndicador
{
    [MenuItem("VR Attention/Execute: Add Lifetime Indicator Now")]
    public static void Ejecutar()
    {
        Debug.Log("[EjecutarAgregarIndicador] Iniciando...");
        
        // Cargar prefabs
        GameObject prefabBlanco = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/EstimuloBlanco.prefab");
        GameObject prefabNegro = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/EstimuloNegro.prefab");
        
        int agregados = 0;
        
        if (prefabBlanco != null)
        {
            if (AgregarIndicador(prefabBlanco, "EstimuloBlanco"))
                agregados++;
        }
        else
        {
            Debug.LogError("[EjecutarAgregarIndicador] No se encontró EstimuloBlanco.prefab");
        }
        
        if (prefabNegro != null)
        {
            if (AgregarIndicador(prefabNegro, "EstimuloNegro"))
                agregados++;
        }
        else
        {
            Debug.LogError("[EjecutarAgregarIndicador] No se encontró EstimuloNegro.prefab");
        }
        
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        
        string mensaje = $"✅ Indicador agregado a {agregados} prefab(s).\n\nPresiona Play para ver el círculo gris que se encoge alrededor de los estímulos.";
        EditorUtility.DisplayDialog("Completado", mensaje, "OK");
        
        Debug.Log($"[EjecutarAgregarIndicador] ✅ Completado - {agregados} prefabs actualizados");
    }
    
    private static bool AgregarIndicador(GameObject prefab, string nombre)
    {
        string path = AssetDatabase.GetAssetPath(prefab);
        GameObject prefabInstance = PrefabUtility.LoadPrefabContents(path);
        
        try
        {
            // Verificar si ya existe
            IndicadorTiempoVida existente = prefabInstance.GetComponentInChildren<IndicadorTiempoVida>();
            if (existente != null)
            {
                Debug.Log($"[EjecutarAgregarIndicador] {nombre} ya tiene el indicador, actualizando...");
                // Destruir el existente para recrearlo con nuevos valores
                GameObject.DestroyImmediate(existente.gameObject);
            }
            
            // Crear nuevo indicador
            GameObject indicadorObj = new GameObject("IndicadorTiempoVida");
            indicadorObj.transform.SetParent(prefabInstance.transform, false);
            indicadorObj.transform.localPosition = Vector3.zero;
            indicadorObj.transform.localRotation = Quaternion.identity;
            indicadorObj.transform.localScale = Vector3.one;
            
            // Agregar componente
            IndicadorTiempoVida indicador = indicadorObj.AddComponent<IndicadorTiempoVida>();
            
            // Guardar
            PrefabUtility.SaveAsPrefabAsset(prefabInstance, path);
            
            Debug.Log($"[EjecutarAgregarIndicador] ✅ {nombre} actualizado");
            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[EjecutarAgregarIndicador] Error en {nombre}: {e.Message}");
            return false;
        }
        finally
        {
            PrefabUtility.UnloadPrefabContents(prefabInstance);
        }
    }
}
